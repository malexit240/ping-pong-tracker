using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Xamarin.Forms;

namespace PingPong.Behavior
{
    public class AnimatedPropertyChangedBehavior : Behavior<View>
    {
        private View _view;

        private Dictionary<string, AnimatedPropertyChanger<object>> _animatedPropertyChangers = new Dictionary<string, AnimatedPropertyChanger<object>>();

        #region -- Public Properties --

        public static readonly BindableProperty TrackedProperitesProperty = BindableProperty.Create(
            propertyName: nameof(TrackedProperites),
            returnType: typeof(string),
            declaringType: typeof(AnimatedPropertyChangedBehavior),
            propertyChanged: OnTrackedPropertiesPropertyChanged);

        public string TrackedProperites
        {
            get => (string)GetValue(TrackedProperitesProperty);
            set => SetValue(TrackedProperitesProperty, value);
        }

        #endregion

        #region -- Overrides --

        protected override void OnAttachedTo(View bindable)
        {
            _view = bindable;

            UpdateTrackedProperties();

            _view.PropertyChanging += OnViewPropertyChanging;
            _view.PropertyChanged += OnViewPropertyChanged;
        }

        protected override void OnDetachingFrom(View bindable)
        {
            _view.PropertyChanging -= OnViewPropertyChanging;
            _view.PropertyChanged -= OnViewPropertyChanged;

            _view = null;

            _animatedPropertyChangers.Clear();

            _animatedPropertyChangers = null;
        }

        #endregion

        #region -- Private Helpers --

        private static void OnTrackedPropertiesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as AnimatedPropertyChangedBehavior)?.UpdateTrackedProperties();
        }

        private void UpdateTrackedProperties()
        {
            if (_view != null)
            {
                var viewType = _view.GetType();

                foreach (var propertyName in TrackedProperites.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    try
                    {
                        var propetyInfo = viewType.GetProperty(propertyName);

                        if (!_animatedPropertyChangers.ContainsKey(propertyName))
                        {
                            _animatedPropertyChangers.Add(propertyName,
                                new AnimatedPropertyChanger<object>(_view, propertyName, AnimationValueConvertersContainer.Get(propetyInfo.PropertyType)));
                        }
                    }
                    catch (AmbiguousMatchException ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                }
            }
        }

        private void OnViewPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_animatedPropertyChangers.TryGetValue(e.PropertyName, out var animatedProperty))
            {
                animatedProperty.PropertyChanged();
            }

        }

        private void OnViewPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (_animatedPropertyChangers.TryGetValue(e.PropertyName, out var animatedProperty))
            {
                animatedProperty.PropertyChanging();
            }
        }

        #endregion
    }
}
