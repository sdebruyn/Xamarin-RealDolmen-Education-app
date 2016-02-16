using System;
using System.Collections.Generic;
using EducationApp.Models;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;

namespace EducationApp.iOS.ViewControllers
{
    internal partial class CategoryViewController : BaseViewController
    {
        private ObservableTableViewController<Category> _categoryListController;

        public CategoryViewController(IntPtr handle) : base(handle)
        {
        }

        private CategoryViewModel Vm => Application.Locator.CategoryViewModel;

        protected override ViewModelBase GetViewModel() => Vm;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _categoryListController = Vm.Category.Subcategories.GetController(CreateDefaultTableCell, BindCategoryCell);
            _categoryListController.TableView = SubcategoryTable;
            _categoryListController.SelectionChanged += SelectCategory;
        }

        protected override IEnumerable<Binding> SetBindings() => new List<Binding>
        {
            Application.Locator.CategoryViewModel.SetBinding(() => Application.Locator.CategoryViewModel.IsLoading)
                .WhenSourceChanges(LoadingChanged),
            Application.Locator.CategoryViewModel.SetBinding(() => Application.Locator.CategoryViewModel.Category.Name,
                this, () => Title, BindingMode.OneWay)
        };

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            _categoryListController.SelectionChanged -= SelectCategory;
            ReleaseDesignerOutlets();
        }

        private void SelectCategory(object sender, EventArgs e)
        {
            Vm.ShowDetailsCommand.Execute(_categoryListController.SelectedItem);
        }
    }
}