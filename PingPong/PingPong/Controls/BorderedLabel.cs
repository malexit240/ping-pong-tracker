using Xamarin.Forms;

namespace PingPong.Controls
{
    public class BorderedLabel : Label
    {
        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(
                    propertyName: nameof(BorderWidth),
                    returnType: typeof(double),
                    declaringType: typeof(BorderedLabel));

        public double BorderWidth
        {
            get => (double)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }
    }
}
