using System;
using System.Reflection;
using Xamarin.Forms;

namespace PingPong.Behavior
{
    public class AnimatedPropertyChanger<T>
    {
        private T _intermediateValue;
        private T _animationStartValue;
        private bool _isPropertyAnimated;
        private View _view;
        private uint _animationLength;
        private IAnimationValueConverter _extrapolationConverter;
        private PropertyInfo _propertyInfo;


        private readonly string _animationName;

        public AnimatedPropertyChanger(View view, string propertyName, IAnimationValueConverter extrapolationConverter, uint length = 250)
        {
            _animationName = Guid.NewGuid().ToString();
            _animationLength = length;
            _extrapolationConverter = extrapolationConverter;
            _view = view;
            _propertyInfo = _view.GetType().GetProperty(propertyName);
        }

        public T OldValue;
        public T TargetValue;

        public void PropertyChanging()
        {
            var value = (T)_propertyInfo.GetValue(_view);
            OldValue = value;
        }

        public void PropertyChanged()
        {
            var value = (T)_propertyInfo.GetValue(_view);
            if (!_isPropertyAnimated)
            {
                TargetValue = value;
                StartAnimation();
            }
            else if (!_extrapolationConverter.IsValueEqual(value, _intermediateValue))
            {
                _view.AbortAnimation(_animationName);
                OldValue = _intermediateValue;
                TargetValue = value;

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

                _intermediateValue = (T)actualValue;
                _propertyInfo.SetValue(_view, _intermediateValue);
            });

            anim.Commit(_view, _animationName, length: _animationLength, finished: (d, b) => _isPropertyAnimated = false);
        }
    }
}
