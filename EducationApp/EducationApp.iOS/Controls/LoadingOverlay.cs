using CoreGraphics;
using UIKit;

namespace EducationApp.iOS.Controls
{
    public sealed class LoadingOverlay : UIView
    {
        public LoadingOverlay(CGRect frame) : base(frame)
        {
            // configurable bits
            BackgroundColor = UIColor.Black;
            Alpha = 0.75f;
            AutoresizingMask = UIViewAutoresizing.All;

            // derive the center x and y
            var centerX = Frame.Width/2;
            var centerY = Frame.Height/2;

            // create the activity spinner, center it horizontall and put it 5 points above center x
            var activitySpinner1 = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
            activitySpinner1.Frame = new CGRect(
                centerX - activitySpinner1.Frame.Width/2,
                centerY - activitySpinner1.Frame.Height - 20,
                activitySpinner1.Frame.Width,
                activitySpinner1.Frame.Height);
            activitySpinner1.AutoresizingMask = UIViewAutoresizing.All;
            AddSubview(activitySpinner1);
            activitySpinner1.StartAnimating();
        }

        /// <summary>
        ///     Fades out the control and then removes it from the super view
        /// </summary>
        public void Hide()
        {
            Animate(0.5, () => { Alpha = 0; }, RemoveFromSuperview);
        }
    }
}