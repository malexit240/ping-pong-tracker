using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PingPong.Controls;
using PingPong.Droid;
using Android.Content;
using System.ComponentModel;
using Android.Graphics;
using Xamarin.Essentials;

[assembly: ExportRenderer(typeof(BorderedLabel), typeof(BorderedLabelRenderer))]
namespace PingPong.Droid
{
    public class BorderedLabelRenderer : LabelRenderer
    {
        public BorderedLabelRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control != null && Element != null)
            {
                this.Control.SetTextColor(Element.TextColor.ToAndroid());
                this.Control.Paint.StrokeWidth = (float)((Element as BorderedLabel).BorderWidth * DeviceDisplay.MainDisplayInfo.Density);
                this.Control.Paint.SetStyle(Paint.Style.Stroke);
            }
        }
    }
}