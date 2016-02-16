using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;

namespace EducationApp.Droid.Fragments
{
    public class ContactFragment : BindingFragment
    {
        private LinearLayout _contactForm;
        private EditText _etMessage;
        private EditText _etSubject;
        private Button _sendButton;
        private TextView _tvPleaseLogin;

        public Button SendMessageButton => _sendButton
                                           ??
                                           (_sendButton = Activity.FindViewById<Button>(Resource.Id.SendMessageButton));

        public EditText EtMessage => _etMessage
                                     ?? (_etMessage = Activity.FindViewById<EditText>(Resource.Id.EtMessage));

        public EditText EtSubject => _etSubject
                                     ?? (_etSubject = Activity.FindViewById<EditText>(Resource.Id.EtSubject));

        public LinearLayout ContactForm => _contactForm
                                           ??
                                           (_contactForm = Activity.FindViewById<LinearLayout>(Resource.Id.ContactForm))
            ;

        public TextView TvPleaseLogin => _tvPleaseLogin
                                         ??
                                         (_tvPleaseLogin = Activity.FindViewById<TextView>(Resource.Id.TvPleaseLogin));


        private SessionViewModel Vm => App.Locator.SessionViewModel;

        protected override ViewModelBase GetViewModel() => Vm;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            => inflater.Inflate(Resource.Layout.ContactFragment, container, false);

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            SendMessageButton.SetCommand("Click", Vm.SendContactFormCommand);
        }

        protected override IEnumerable<Binding> SetBindings() => new List<Binding>
        {
            this.SetBinding(() => EtSubject.Text, App.Locator.SessionViewModel,
                () => App.Locator.SessionViewModel.ContactForm.Subject, BindingMode.TwoWay),
            this.SetBinding(() => EtMessage.Text, App.Locator.SessionViewModel,
                () => App.Locator.SessionViewModel.ContactForm.Body, BindingMode.TwoWay),
            App.Locator.SessionViewModel.SetBinding(() => App.Locator.SessionViewModel.Identity)
                .WhenSourceChanges(IdentityChanged)
        };

        private void IdentityChanged()
        {
            if (Vm.Identity == null)
            {
                ContactForm.Visibility = ViewStates.Gone;
                TvPleaseLogin.Visibility = ViewStates.Visible;
            }
            else
            {
                ContactForm.Visibility = ViewStates.Visible;
                TvPleaseLogin.Visibility = ViewStates.Gone;
            }
        }
    }
}