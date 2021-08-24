using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PingPong.Behavior
{
    public static class AnimationValueConvertersContainer
    {
        private static Dictionary<Type, IAnimationValueConverter> _converters;

        static AnimationValueConvertersContainer()
        {
            _converters = new Dictionary<Type, IAnimationValueConverter>();

            _converters.Add(typeof(double), new DoubleAnimationValueConverter());
            _converters.Add(typeof(ColumnDefinitionCollection), new ColumnDefinitionCollectionAnimationConverter());
        }

        public static IAnimationValueConverter Get(Type type)
        {
            return _converters[type];
        }
    }
}
