using System.Collections.Generic;
using System.ComponentModel;
using Android.OS;
using Android.Views;
using Android.Widget;
using EducationApp.Droid.Extensions;
using EducationApp.Models;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;

namespace EducationApp.Droid.Fragments
{
    public sealed partial class CategoryListFragment : FragmentBase, AdapterView.IOnItemClickListener
    {
        private Dictionary<View, SearchStatus> _searchViews;

        private MainViewModel Vm => App.Locator.MainViewModel;

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            if (parent.Id == CategoryListView.Id)
            {
                var category = Vm.Categories[position];
                Vm.ShowDetailsCommand.Execute(category);
            }
            if (parent.Id == SearchResultListView.Id)
            {
                var course = Vm.FoundCourses[position];
                Vm.ShowCourseDetailsCommand.Execute(course);
            }
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

        private void SearchStatusUpdate()
        {
            foreach (var pair in _searchViews)
            {
                pair.Key.Visibility = pair.Value == Vm.SearchStatus ? ViewStates.Visible : ViewStates.Gone;
            }
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            CategoryListView.Adapter =
                Vm.Categories.GetAdapter(
                    (i, cat, v) =>
                        this.GetObjectAdapter((c => c.Name), cat, v, Resource.Layout.CategoryListItem,
                            Resource.Id.CategoryListItemName));
            CategoryListView.OnItemClickListener = this;

            SearchResultListView.Adapter =
                Vm.FoundCourses.GetAdapter(
                    (i, course, v) =>
                        this.GetObjectAdapter((c => c.Title), course, v, Resource.Layout.CourseListItem,
                            Resource.Id.CourseListItemName));
            SearchResultListView.OnItemClickListener = this;

            _searchViews = new Dictionary<View, SearchStatus>
            {
                {TvSearching, SearchStatus.Searching},
                {TvSearchFaulted, SearchStatus.Faulted},
                {TvNoCoursesFound, SearchStatus.NoResults},
                {CategoryListView, SearchStatus.Inactive},
                {SearchResultListView, SearchStatus.ResultsAvailable}
            };

            Vm.PropertyChanged += VmPropertyChanged;
            SearchStatusUpdate();
        }

        private void VmPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Vm.SearchStatus):
                    SearchStatusUpdate();
                    break;
                case nameof(Vm.IsLoading):
                    LoadingChanged();
                    break;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            => inflater.Inflate(Resource.Layout.CategoryListFragment, container, false);
    }
}