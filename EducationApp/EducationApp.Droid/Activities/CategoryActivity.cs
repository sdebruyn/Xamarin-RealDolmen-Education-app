using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using AnimatedLoadingViews;
using EducationApp.Droid.Extensions;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace EducationApp.Droid.Activities
{
    [Activity]
    public class CategoryActivity : ActivityBase, AdapterView.IOnItemClickListener
    {
        private Toolbar _categoryToolbar;

        private bool _isLoading;
        private AnimatedCircleLoadingView _loadingView;
        private ListView _subcategoryListView;
        public CategoryViewModel Vm => App.Locator.CategoryViewModel;

        public ListView SubcategoryListView
            => _subcategoryListView ?? (_subcategoryListView = FindViewById<ListView>(Resource.Id.SubcategoryList));

        public AnimatedCircleLoadingView LoadingView
            =>
                _loadingView ??
                (_loadingView = FindViewById<AnimatedCircleLoadingView>(Resource.Id.CategoryCircleLoadingView));

        public Toolbar CategoryToolbar
            => _categoryToolbar ?? (_categoryToolbar = FindViewById<Toolbar>(Resource.Id.CategoryToolbar));

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            if (Vm?.Category != null)
            {
                var category = Vm.Category.Subcategories[position];
                Vm.ShowDetailsCommand.Execute(category);
            }
        }

        public void LoadingChanged()
        {
            if (Vm.IsLoading == _isLoading)
            {
                return;
            }
            _isLoading = Vm.IsLoading;

            if (_isLoading)
            {
                LoadingView.Visibility = ViewStates.Visible;
                LoadingView.StartIndeterminate();
            }
            else
            {
                LoadingView.StopOk();
                LoadingView.Visibility = ViewStates.Gone;
            }
        }

        private void SetAdapter()
        {
            if (Vm?.Category?.Subcategories == null)
            {
                SubcategoryListView.Adapter = null;
                return;
            }

            SubcategoryListView.Adapter =
                Vm.Category.Subcategories.GetAdapter(
                    ((i, cat, v) =>
                        this.GetObjectAdapter((c => c.Name), cat, v, Resource.Layout.CategoryListItem,
                            Resource.Id.CategoryListItemName)));
        }

        protected override ViewModelBase GetViewModel() => Vm;

        protected override IEnumerable<Binding> SetBindings() => new List<Binding>
        {
            App.Locator.CategoryViewModel.SetBinding(() => App.Locator.CategoryViewModel.IsLoading)
                .WhenSourceChanges(LoadingChanged),
            App.Locator.CategoryViewModel.SetBinding(() => App.Locator.CategoryViewModel.Category.Name, this,
                () => CategoryToolbar.Title, BindingMode.OneWay),
            App.Locator.CategoryViewModel.SetBinding(() => App.Locator.CategoryViewModel.Category.Subcategories)
                .WhenSourceChanges(SetAdapter)
        };

        protected override void SetupView()
        {
            SetContentView(Resource.Layout.CategoryActivity);
            SetSupportActionBar(CategoryToolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SubcategoryListView.OnItemClickListener = this;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ServiceLocator.Current.GetInstance<INavigationService>().GoBack();
            return true;
        }
    }
}