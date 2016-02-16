using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using EducationApp.Models;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;

namespace EducationApp.Droid.Fragments
{
    public class SessionDetailsFragment : BindingFragment
    {
        private ListView _lvSessionSchedules;
        private TextView _tvLocation;
        private TextView _tvPreparation;

        public TextView TvLocation => _tvLocation
                                      ?? (_tvLocation = Activity.FindViewById<TextView>(Resource.Id.TvLocation));

        public ListView LvSessionSchedules => _lvSessionSchedules
                                              ??
                                              (_lvSessionSchedules =
                                                  Activity.FindViewById<ListView>(Resource.Id.LvSessionSchedules));

        public TextView TvPreparation => _tvPreparation
                                         ??
                                         (_tvPreparation = Activity.FindViewById<TextView>(Resource.Id.TvPreparation));

        protected override ViewModelBase GetViewModel() => App.Locator.SessionViewModel;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            => inflater.Inflate(Resource.Layout.SessionDetailsFragment, container, false);

        protected override IEnumerable<Binding> SetBindings() => new List<Binding>
        {
            App.Locator.SessionViewModel.SetBinding(() => App.Locator.SessionViewModel.Session.Location.Name, this,
                () => TvLocation.Text),
            App.Locator.SessionViewModel.SetBinding(() => App.Locator.SessionViewModel.Session.Preparation, this,
                () => TvPreparation.Text)
        };

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            LvSessionSchedules.Adapter =
                App.Locator.SessionViewModel.Session.SessionSchedules.GetAdapter(GetSessionScheduleTemplate);
        }

        private View GetSessionScheduleTemplate(int arg1, SessionSchedule schedule, View arg3)
        {
            var view = arg3 ?? Activity.LayoutInflater.Inflate(Resource.Layout.SessionScheduleListItem, null);

            var tvStarTime = view.FindViewById<TextView>(Resource.Id.TvStartTime);
            var tvEndTime = view.FindViewById<TextView>(Resource.Id.TvEndTime);
            var tvClassRoom = view.FindViewById<TextView>(Resource.Id.TvClassRoom);
            var tvInstructor = view.FindViewById<TextView>(Resource.Id.TvInstructor);

            tvStarTime.Text = schedule.StartTime?.ToString("U") ?? "";
            tvEndTime.Text = schedule.EndTime?.ToString("U") ?? "";
            tvClassRoom.Text = schedule.ClassRoom.Name;
            tvInstructor.Text = schedule.Instructor.FirstName + " " + schedule.Instructor.LastName;

            return view;
        }
    }
}