using UIKit;

namespace EducationApp.iOS.Utilities
{
    public static class Theme
    {
        public static void SetAppearance()
        {
            UINavigationBar.Appearance.BarTintColor = RDRed;
            UINavigationBar.Appearance.TintColor = UIColor.White;

            var textAttributes = new UITextAttributes
            {
                TextColor = UIColor.White
            };
            UINavigationBar.Appearance.SetTitleTextAttributes(textAttributes);

            UITabBar.Appearance.BackgroundColor = UIColor.White;
            UITabBar.Appearance.BarTintColor = UIColor.White;
            UITabBar.Appearance.TintColor = RDBlue;

            UITableView.Appearance.BackgroundColor = UIColor.White;
            UITableView.Appearance.TintColor = RDBlue;
            UITableView.Appearance.SeparatorColor = RDBlue;

            UIPageControl.Appearance.BackgroundColor = UIColor.White;
            UIPageControl.Appearance.CurrentPageIndicatorTintColor = RDBlue;
            UIPageControl.Appearance.PageIndicatorTintColor = UIColor.LightGray;

            UIButton.Appearance.TintColor = RDBlue;
        }

        // ReSharper disable InconsistentNaming
        public static readonly UIColor RDRed = UIColor.FromRGB(226, 0, 26);
        public static readonly UIColor RDBlue = UIColor.FromRGB(1, 137, 180);
        // ReSharper restore InconsistentNaming
    }
}