using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using EducationApp.Services;
using EducationApp.ViewModels;
using Microsoft.Practices.ServiceLocation;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace EducationApp.Droid.Fragments
{
    public class LanguageFragment : AppCompatDialogFragment, IDialogInterfaceOnClickListener
    {
        private CourseViewModel Vm => App.Locator.CourseViewModel;

        public void OnClick(IDialogInterface dialog, int which)
        {
            var lang = Vm.Languages[which];
            Vm.ChangeLanguageCommand.Execute(lang);
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var loc = ServiceLocator.Current.GetInstance<ILocalizedStringProvider>();

            var builder = new AlertDialog.Builder(Activity);
            builder.SetTitle(loc.GetLocalizedString(Localized.ChangeLanguage_Label));
            builder.SetAdapter(new LanguageAdapter(Activity), this);

            return builder.Create();
        }

        public class LanguageAdapter : BaseAdapter<LanguageCode>
        {
            private readonly Activity _activity;
            private readonly ILocalizedStringProvider _localizedStringProvider;

            public LanguageAdapter(Activity activity)
            {
                _activity = activity;
                _localizedStringProvider = ServiceLocator.Current.GetInstance<ILocalizedStringProvider>();
            }

            private IList<LanguageCode> Languages => App.Locator.CourseViewModel.Languages;

            public override int Count => Languages.Count;

            public override LanguageCode this[int position] => Languages[position];

            public override long GetItemId(int position) => position;

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                var view = convertView ?? _activity.LayoutInflater.Inflate(Resource.Layout.TextListItem, parent, false);
                var tv = view.FindViewById<TextView>(Resource.Id.TvText);
                tv.Text = _localizedStringProvider.GetLocalizedString(Languages[position]);
                return view;
            }
        }
    }
}