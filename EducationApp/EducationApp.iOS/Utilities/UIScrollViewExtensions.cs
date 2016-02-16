using System;
using CoreGraphics;
using UIKit;

// ReSharper disable InconsistentNaming

namespace EducationApp.iOS.Utilities
{
    public static class UIScrollViewExtensions
    {
        public static void SetUIScrollViewHeight(this UIScrollView scrollView, nfloat preferedWidth = default(nfloat))
        {
            nfloat width = 0;
            nfloat height = 0;

            foreach (var view in scrollView.Subviews)
            {
                if (view.Frame.GetMaxX() > width)
                {
                    width = view.Frame.GetMaxX();
                }
                if (view.Frame.GetMaxY() > height)
                {
                    height = view.Frame.GetMaxY();
                }
            }

            if (preferedWidth != default(nfloat))
                width = preferedWidth;
            scrollView.ContentSize = new CGSize(width, height);
        }
    }
}