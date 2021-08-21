using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PingPong.Controls;
using PingPong.Droid;
using Android.Runtime;
using System;
using Android.Views;
using System.Collections.Generic;
using Xamarin.Essentials;

[assembly: ExportRenderer(typeof(ClickableContentView), typeof(ClickableContentViewRenderer))]
namespace PingPong.Droid
{
    [Preserve(AllMembers = true)]
    public class ClickableContentViewRenderer : VisualElementRenderer<ClickableContentView>
    {
        public ClickableContentViewRenderer(Android.Content.Context context)
            : base(context)
        {
        }

        public static void Init()
        {
            var temp = DateTime.Now;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ClickableContentView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.OnInvalidate -= HandleInvalidate;
            }

            if (e.NewElement != null)
            {
                e.NewElement.OnInvalidate += HandleInvalidate;
            }

            this.SetBackgroundColor(Android.Graphics.Color.Transparent);

            Invalidate();
        }

        protected override void UpdateBackgroundColor()
        {
            // Do NOT call update background here.
            // base.UpdateBackgroundColor();
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ClickableContentView.BackgroundColorProperty.PropertyName)
            {
                Element.Invalidate();
            }
            else if (e.PropertyName == ClickableContentView.IsVisibleProperty.PropertyName)
            {
                Element.Invalidate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Element != null)
            {
                Element.OnInvalidate -= HandleInvalidate;
            }

            base.Dispose(disposing);
        }

        #region -- Touch Handling --

        public override bool OnTouchEvent(MotionEvent e)
        {
            var scale = Element.Width / Width / DeviceDisplay.MainDisplayInfo.Density;
            bool result = false;

            var touchInfo = new List<Point>();
            for (var i = 0; i < e.PointerCount; i++)
            {
                var coord = new MotionEvent.PointerCoords();
                e.GetPointerCoords(i, coord);
                touchInfo.Add(new Point(coord.X * scale, coord.Y * scale));
            }

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    if (Element != null)
                    {
                        result = Element.TouchesBegan(touchInfo);
                    }

                    break;

                case MotionEventActions.Move:
                    if (Element != null)
                    {
                        result = Element.TouchesMoved(touchInfo);
                    }

                    break;

                case MotionEventActions.Up:
                    if (Element != null)
                    {
                        result = Element.TouchesEnded(touchInfo);
                    }

                    break;

                case MotionEventActions.Cancel:
                    if (Element != null)
                    {
                        result = Element.TouchesCancelled(touchInfo);
                    }

                    break;
            }

            return result;
        }

        #endregion

        #region -- Private Members --
        private void HandleInvalidate(object sender, EventArgs args)
        {
            Invalidate();
        }

        protected Size GetScreenSize()
        {
            var metrics = Context.Resources.DisplayMetrics;
            return new Size(metrics.WidthPixels, metrics.HeightPixels);
        }

        #endregion
    }
}