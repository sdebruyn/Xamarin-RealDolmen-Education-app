using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace EducationApp.iOS.ViewControllers
{
    public partial class CourseViewController : UIPageViewController
    {
        private CoursePagesDataSource _dataSource;

        public CourseViewController(IntPtr handle)
            : base(handle)
        {
        }

        public CoursePagesDataSource CoursePagesDataSource
            => _dataSource ?? (_dataSource = new CoursePagesDataSource(Storyboard));

        public override void ViewDidLoad()
        {
            DataSource = CoursePagesDataSource;
            SetViewControllers(new[] {CoursePagesDataSource.GetControllers().First()},
                UIPageViewControllerNavigationDirection.Forward,
                true, null);

            base.ViewDidLoad();
        }
    }

    public class CoursePagesDataSource : UIPageViewControllerDataSource
    {
        private readonly List<UIViewController> _pages;

        public CoursePagesDataSource(UIStoryboard storyboard)
        {
            _pages = new List<UIViewController>
            {
                storyboard.InstantiateViewController(nameof(InformationViewController)),
                storyboard.InstantiateViewController(nameof(DescriptionViewController)),
                storyboard.InstantiateViewController(nameof(InstructorViewController))
            };
        }

        public override nint GetPresentationCount(UIPageViewController pageViewController) => _pages.Count;

        public override nint GetPresentationIndex(UIPageViewController pageViewController) => 0;

        public override UIViewController GetPreviousViewController(UIPageViewController pageViewController,
            UIViewController referenceViewController)
        {
            var current = _pages.FindIndex(vc => vc.GetType() == referenceViewController.GetType());
            var previous = current - 1;
            if (previous < 0)
            {
                previous = _pages.Count - 1;
            }
            return _pages[previous];
        }

        public UIViewController[] GetControllers() => _pages.ToArray();

        public override UIViewController GetNextViewController(UIPageViewController pageViewController,
            UIViewController referenceViewController)
        {
            var current = _pages.FindIndex(vc => vc.GetType() == referenceViewController.GetType());
            var next = current + 1;
            if (next >= _pages.Count)
            {
                next = 0;
            }
            return _pages[next];
        }
    }
}