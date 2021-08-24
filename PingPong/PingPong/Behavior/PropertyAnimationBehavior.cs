using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PingPong.Behavior
{
    public interface IExtrapolationConverter<T>
    {
        public T GetValue(double value, T StartValue, T EndValue);

        public bool IsValueEqual(T first, T second);
    }

    public class DoubleExtraPolationConverter : IExtrapolationConverter<double>
    {
        public double GetValue(double value, double StartValue, double EndValue)
        {
            return StartValue + value * (EndValue - StartValue);
        }

        public bool IsValueEqual(double first, double second) => first == second;
    }

    public class ColumnDefinitionCollectionExtraPolationConverter : IExtrapolationConverter<ColumnDefinitionCollection>
    {
        public ColumnDefinitionCollection GetValue(double value, ColumnDefinitionCollection StartValue, ColumnDefinitionCollection EndValue)
        {
            var newCollection = new ColumnDefinitionCollection();

            var index = 0;
            foreach (var collumnDefinition in EndValue)
            {
                if (index < StartValue.Count)
                {
                    var oldWidth = StartValue[index].Width.Value;
                    var newWidth = collumnDefinition.Width.Value;

                    newCollection.Add(new ColumnDefinition() { Width = new GridLength(oldWidth + value * (newWidth - oldWidth), type: collumnDefinition.Width.GridUnitType) });
                    index++;
                }
                else
                {
                    newCollection.Add(new ColumnDefinition() { Width = new GridLength(collumnDefinition.Width.Value, type: collumnDefinition.Width.GridUnitType) });
                }
            }

            return newCollection;
        }

        public bool IsValueEqual(ColumnDefinitionCollection first, ColumnDefinitionCollection second)
        {
            bool isEqual = first.Count == second.Count;

            if (isEqual)
            {
                for (int i = 0; i < first.Count && isEqual; i++)
                {
                    isEqual = first[i].Width.Value == second[i].Width.Value && first[i].Width.GridUnitType == second[i].Width.GridUnitType;
                }
            }

            return isEqual;
        }
    }

    public class AnimatedPropertyChanged<T>
    {
        private T _intermediateValue;
        private T _animationStartValue;
        private bool _isPropertyAnimated;
        private View _view;
        private uint _animationLength;
        private Action<T> _animationFuction;
        private IExtrapolationConverter<T> _extrapolationConverter;


        private readonly string _animationName;

        public AnimatedPropertyChanged(View view, Action<T> animationFuction, IExtrapolationConverter<T> extrapolationConverter, uint length = 250)
        {
            _animationName = Guid.NewGuid().ToString();
            _animationLength = length;
            _extrapolationConverter = extrapolationConverter;
            _view = view;
            _animationFuction = animationFuction;
        }

        public T OldValue;
        public T TargetValue;

        public void PropertyChanging(T value)
        {
            OldValue = value;
        }

        public void PropertyChanged(T Value)
        {
            if (!_isPropertyAnimated)
            {
                TargetValue = Value;
                StartAnimation();
            }
            else if (!_extrapolationConverter.IsValueEqual(Value, _intermediateValue))
            {
                _view.AbortAnimation(_animationName);
                OldValue = _intermediateValue;
                TargetValue = Value;

                StartAnimation();
            }
        }

        private void StartAnimation()
        {
            _isPropertyAnimated = true;

            _animationStartValue = OldValue;

            var anim = new Animation(value =>
            {
                var actualValue = _extrapolationConverter.GetValue(value, _animationStartValue, TargetValue);

                _intermediateValue = actualValue;

                _animationFuction(actualValue);
            });

            anim.Commit(_view, _animationName, length: _animationLength, finished: (d, b) => _isPropertyAnimated = false);
        }
    }

    public class PropertyAnimationBehavior : Behavior<View>
    {
        private Grid _view;

        private Dictionary<string, AnimatedPropertyChanged<object>> _animatedProperties;

        private AnimatedPropertyChanged<ColumnDefinitionCollection> columnDefinitionsProperty;

        //public static readonly BindableProperty TrackedProperitesProperty = BindableProperty.Create(
        //    propertyName: nameof(TrackedProperites),
        //    returnType: typeof(string),
        //    declaringType: typeof(PropertyAnimationBehavior),
        //    defaultValue: "ColumnDefinitions",
        //    propertyChanged: OnTrackedPropertiesPropertyChanged);

        //private static IExtrapolationConverter GetConverterByType()
        //{
        //}

        //private static void OnTrackedPropertiesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    var behavior = bindable as PropertyAnimationBehavior;

        //    var viewType = behavior._view.GetType();

        //    foreach (var propertyName in (newValue as string).Split(',', StringSplitOptions.RemoveEmptyEntries))
        //    {
        //        var propetyInfo = viewType.GetProperty(propertyName);

        //        if (!behavior._animatedProperties.ContainsKey(propertyName))
        //        {
        //            behavior._animatedProperties.Add(propertyName, new AnimatedPropertyChanged<object>())
        //        }
        //    }
        //}

        //public string TrackedProperites
        //{
        //    get => (string)GetValue(TrackedProperitesProperty);
        //    set => SetValue(TrackedProperitesProperty, value);
        //}

        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);

            _view = bindable as Grid;
            _animatedProperties = new Dictionary<string, AnimatedPropertyChanged<object>>();

            columnDefinitionsProperty = new AnimatedPropertyChanged<ColumnDefinitionCollection>(_view, v => _view.ColumnDefinitions = v, new ColumnDefinitionCollectionExtraPolationConverter(), 300);

            _view.PropertyChanging += Bindable_PropertyChanging;
            _view.PropertyChanged += Bindable_PropertyChanged;
        }

        private void Bindable_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_view.ColumnDefinitions):
                    columnDefinitionsProperty.PropertyChanged(_view.ColumnDefinitions);
                    break;
                default:
                    break;
            }

        }

        private async void Bindable_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_view.ColumnDefinitions):
                    columnDefinitionsProperty.PropertyChanging(_view.ColumnDefinitions);
                    break;
                default:
                    break;
            }
        }
    }
}
