using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using EducationApp.Models;
using EducationApp.Services;
using EducationApp.ViewModels;
using GalaSoft.MvvmLight.Helpers;
using Microsoft.Practices.ServiceLocation;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace EducationApp.Droid.Fragments
{
    public class SessionsFragment : AppCompatDialogFragment, IDialogInterfaceOnClickListener
    {
        private CourseViewModel Vm => App.Locator.CourseViewModel;

        public void OnClick(IDialogInterface dialog, int which)
        {
            var session = Vm.Course.Sessions[which];
            Vm.ViewSessionCommand.Execute(session);
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var loc = ServiceLocator.Current.GetInstance<ILocalizedStringProvider>();

            var builder = new AlertDialog.Builder(Activity);
            builder.SetTitle(loc.GetLocalizedString(Localized.Sessions_Label));
            builder.SetAdapter(App.Locator.CourseViewModel.Course.Sessions.GetAdapter(GetSessionTemplate), this);

            return builder.Create();
        }

        private View GetSessionTemplate(int arg1, Session arg2, View arg3)
        {
            var view = arg3 ?? Activity.LayoutInflater.Inflate(Resource.Layout.TextListItem, null);
            view.FindViewById<TextView>(Resource.Id.TvText).Text = arg2.Description;
            return view;
        }
    }
}