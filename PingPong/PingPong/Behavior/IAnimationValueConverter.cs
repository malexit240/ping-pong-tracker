namespace PingPong.Behavior
{
    public interface IAnimationValueConverter
    {
        public object GetValue(double value, object StartValue, object EndValue);

        public bool IsValueEqual(object first, object second);
    }
}
