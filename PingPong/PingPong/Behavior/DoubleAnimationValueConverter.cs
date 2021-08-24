namespace PingPong.Behavior
{
    public class DoubleAnimationValueConverter : IAnimationValueConverter
    {
        public object GetValue(double value, object StartValue, object EndValue)
        {
            return (double)StartValue + value * ((double)EndValue - (double)StartValue);
        }

        public bool IsValueEqual(object first, object second) => (double)first == (double)second;
    }
}
