using System.ComponentModel;
using Android.App;
using Android.Views;
using Android.Widget;
using EducationApp.Droid.Extensions;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace EducationApp.Droid.Activities
{
    [Activity]
    public sealed partial class CategoryActivity : ActivityBase, AdapterView.IOnItemClickListener
    {
        private CategoryViewModel Vm => App.Locator.CategoryViewModel;

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            Vm.ShowDetailsCommand.Execute(Vm.Category?.Subcategories[position]);
        }

        private void LoadingChanged()
        {
            if (Vm.IsLoading)
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

        protected override ViewModelBase GetViewModel() => Vm;

        protected override void SetupView()
        {
            SetContentView(Resource.Layout.CategoryActivity);

            CategoryToolbar.Title = Vm.Category.Name;
            SetSupportActionBar(CategoryToolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            SubcategoryListView.Adapter =
                Vm.Category.Subcategories.GetAdapter(
                    ((i, cat, v) =>
                        this.GetObjectAdapter((c => c.Name), cat, v, Resource.Layout.CategoryListItem,
                            Resource.Id.CategoryListItemName)));
            SubcategoryListView.OnItemClickListener = this;

            Vm.PropertyChanged += VmPropertyChanged;
        }

        private void VmPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Vm.IsLoading))
            {
                LoadingChanged();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Vm.PropertyChanged -= VmPropertyChanged;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            ServiceLocator.Current.GetInstance<INavigationService>().GoBack();
            return true;
        }
    }
}