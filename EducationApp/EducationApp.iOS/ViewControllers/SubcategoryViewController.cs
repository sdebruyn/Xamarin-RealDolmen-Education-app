using System;
using System.Collections.Generic;
using EducationApp.Models;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;

namespace EducationApp.iOS.ViewControllers
{
    partial class SubcategoryViewController : BaseViewController
    {
        private ObservableTableViewController<Course> _courseListController;

        public SubcategoryViewController(IntPtr handle) : base(handle)
        {
        }

        private SubcategoryViewModel Vm => Application.Locator.SubcategoryViewModel;

        protected override ViewModelBase GetViewModel() => Vm;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _courseListController = Vm.Category.Courses.GetController(CreateDefaultTableCell, BindCourseCell);
            _courseListController.TableView = CoursesTable;
            _courseListController.SelectionChanged += SelectCourse;
        }

        protected override IEnumerable<Binding> SetBindings() => new List<Binding>
        {
            Application.Locator.SubcategoryViewModel.SetBinding(() => Application.Locator.SubcategoryViewModel.IsLoading)
                .WhenSourceChanges(LoadingChanged),
            Application.Locator.SubcategoryViewModel.SetBinding(
                () => Application.Locator.SubcategoryViewModel.Category.Name, this, () => Title)
        };

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            _courseListController.SelectionChanged -= SelectCourse;
            _courseListController.Dispose();
            _courseListController = null;

            ReleaseDesignerOutlets();
        }

        private void SelectCourse(object sender, EventArgs e)
        {
            Vm.ShowCourseDetailsCommand.Execute(_courseListController.SelectedItem);
        }
    }
}