using System.Collections.Generic;
using System.ComponentModel;
using Android.OS;
using Android.Views;
using Android.Widget;
using EducationApp.Droid.Extensions;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;

namespace EducationApp.Droid.Fragments
{
    public class SubscribeFragment : BindingFragment, AdapterView.IOnItemLongClickListener
    {
        private Button _addParticipantButton;


        private ListView _lvParticipants;
        private Button _subscribeButton;


        private LinearLayout _subscribeForm;
        private EditText _tvCity;
        private EditText _tvCompany;
        private EditText _tvCountry;
        private EditText _tvEmail;
        private EditText _tvFax;
        private EditText _tvFirstName;
        private EditText _tvJobTitle;
        private EditText _tvLastName;
        private EditText _tvNumber;
        private EditText _tvParticipantEmail;
        private EditText _tvParticipantFirstName;
        private EditText _tvParticipantLastName;
        private EditText _tvPhone;


        private TextView _tvPleaseLogin;
        private EditText _tvRemarks;
        private EditText _tvStreet;
        private EditText _tvVAT;
        private EditText _tvZIP;

        public LinearLayout SubscribeForm => _subscribeForm
                                             ??
                                             (_subscribeForm =
                                                 Activity.FindViewById<LinearLayout>(Resource.Id.SubscribeForm));

        public TextView TvPleaseLogin => _tvPleaseLogin
                                         ??
                                         (_tvPleaseLogin = Activity.FindViewById<TextView>(Resource.Id.TvPleaseLogin));

        public EditText TvFirstName
            => _tvFirstName ?? (_tvFirstName = Activity.FindViewById<EditText>(Resource.Id.TvFirstName));

        public EditText TvLastName
            => _tvLastName ?? (_tvLastName = Activity.FindViewById<EditText>(Resource.Id.TvLastName));

        public EditText TvJobTitle
            => _tvJobTitle ?? (_tvJobTitle = Activity.FindViewById<EditText>(Resource.Id.TvJobTitle));

        public EditText TvCompany => _tvCompany ?? (_tvCompany = Activity.FindViewById<EditText>(Resource.Id.TvCompany))
            ;

        public EditText TvStreet => _tvStreet ?? (_tvStreet = Activity.FindViewById<EditText>(Resource.Id.TvStreet));
        public EditText TvNumber => _tvNumber ?? (_tvNumber = Activity.FindViewById<EditText>(Resource.Id.TvNumber));
        public EditText TvCity => _tvCity ?? (_tvCity = Activity.FindViewById<EditText>(Resource.Id.TvCity));
        public EditText TvZIP => _tvZIP ?? (_tvZIP = Activity.FindViewById<EditText>(Resource.Id.TvZIP));

        public EditText TvCountry => _tvCountry ?? (_tvCountry = Activity.FindViewById<EditText>(Resource.Id.TvCountry))
            ;

        public EditText TvEmail => _tvEmail ?? (_tvEmail = Activity.FindViewById<EditText>(Resource.Id.TvEmail));
        public EditText TvPhone => _tvPhone ?? (_tvPhone = Activity.FindViewById<EditText>(Resource.Id.TvPhone));
        public EditText TvFax => _tvFax ?? (_tvFax = Activity.FindViewById<EditText>(Resource.Id.TvFax));
        public EditText TvVAT => _tvVAT ?? (_tvVAT = Activity.FindViewById<EditText>(Resource.Id.TvVAT));

        public EditText TvRemarks => _tvRemarks ?? (_tvRemarks = Activity.FindViewById<EditText>(Resource.Id.TvRemarks))
            ;

        public EditText TvParticipantFirstName
            =>
                _tvParticipantFirstName ??
                (_tvParticipantFirstName = Activity.FindViewById<EditText>(Resource.Id.TvParticipantFirstName));

        public EditText TvParticipantLastName
            =>
                _tvParticipantLastName ??
                (_tvParticipantLastName = Activity.FindViewById<EditText>(Resource.Id.TvParticipantLastName));

        public EditText TvParticipantEmail
            =>
                _tvParticipantEmail ??
                (_tvParticipantEmail = Activity.FindViewById<EditText>(Resource.Id.TvParticipantEmail));

        public Button AddParticipantButton
            =>
                _addParticipantButton ??
                (_addParticipantButton = Activity.FindViewById<Button>(Resource.Id.AddParticipantButton));

        public Button SubscribeButton
            => _subscribeButton ?? (_subscribeButton = Activity.FindViewById<Button>(Resource.Id.SubscribeButton));

        public ListView LvParticipants => _lvParticipants
                                          ??
                                          (_lvParticipants = Activity.FindViewById<ListView>(Resource.Id.LvParticipants))
            ;

        private SessionViewModel Vm => App.Locator.SessionViewModel;

        public bool OnItemLongClick(AdapterView parent, View view, int position, long id)
        {
            var participant = Vm.Subscription.Participants[position];
            return Vm.Subscription.Participants.Remove(participant);
        }

        protected override ViewModelBase GetViewModel() => Vm;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            => inflater.Inflate(Resource.Layout.SubscribeFragment, container, false);

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            TvFirstName.TextChanged += (sender, args) => Vm.Subscription.FirstName = (sender as EditText)?.Text;
            TvLastName.TextChanged += (sender, args) => Vm.Subscription.LastName = (sender as EditText)?.Text;
            TvJobTitle.TextChanged += (sender, args) => Vm.Subscription.JobTitle = (sender as EditText)?.Text;
            TvCompany.TextChanged += (sender, args) => Vm.Subscription.Company = (sender as EditText)?.Text;
            TvStreet.TextChanged += (sender, args) => Vm.Subscription.Street = (sender as EditText)?.Text;
            TvNumber.TextChanged += (sender, args) => Vm.Subscription.Number = (sender as EditText)?.Text;
            TvCity.TextChanged += (sender, args) => Vm.Subscription.City = (sender as EditText)?.Text;
            TvZIP.TextChanged += (sender, args) => Vm.Subscription.ZIPCode = (sender as EditText)?.Text;
            TvCountry.TextChanged += (sender, args) => Vm.Subscription.Country = (sender as EditText)?.Text;
            TvEmail.TextChanged += (sender, args) => Vm.Subscription.Email = (sender as EditText)?.Text;
            TvPhone.TextChanged += (sender, args) => Vm.Subscription.Phone = (sender as EditText)?.Text;
            TvFax.TextChanged += (sender, args) => Vm.Subscription.Fax = (sender as EditText)?.Text;
            TvVAT.TextChanged += (sender, args) => Vm.Subscription.VAT = (sender as EditText)?.Text;
            TvRemarks.TextChanged += (sender, args) => Vm.Subscription.Remarks = (sender as EditText)?.Text;
            TvParticipantFirstName.TextChanged +=
                (sender, args) => Vm.ParticipantToAdd.FirstName = (sender as EditText)?.Text;
            TvParticipantLastName.TextChanged +=
                (sender, args) => Vm.ParticipantToAdd.LastName = (sender as EditText)?.Text;
            TvParticipantEmail.TextChanged += (sender, args) => Vm.ParticipantToAdd.Email = (sender as EditText)?.Text;

            Vm.ParticipantToAdd.PropertyChanged += UpdateParticipant;
            Vm.PropertyChanged += UpdateParticipant;

            AddParticipantButton.SetCommand("Click", Vm.AddParticipantCommand);
            SubscribeButton.SetCommand("Click", Vm.SendSubscriptionCommand);

            LvParticipants.Adapter = Vm.Subscription.Participants.GetAdapter(
                (i, participant, v) =>
                    this.GetObjectAdapter(p => $"{p.FirstName} {p.LastName} ({p.Email})", participant, v,
                        Resource.Layout.ParticipantListItem, Resource.Id.TvParticipantDescription));
            LvParticipants.OnItemLongClickListener = this;
        }

        private void UpdateParticipant(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FirstName" || e.PropertyName == "ParticipantToAdd")
            {
                if (TvParticipantFirstName.Text != Vm.ParticipantToAdd.FirstName)
                {
                    TvParticipantFirstName.Text = Vm.ParticipantToAdd.FirstName;
                }
            }
            if (e.PropertyName == "LastName" || e.PropertyName == "ParticipantToAdd")
            {
                if (TvParticipantLastName.Text != Vm.ParticipantToAdd.LastName)
                {
                    TvParticipantLastName.Text = Vm.ParticipantToAdd.LastName;
                }
            }
            if (e.PropertyName == "Email" || e.PropertyName == "ParticipantToAdd")
            {
                if (TvParticipantEmail.Text != Vm.ParticipantToAdd.Email)
                {
                    TvParticipantEmail.Text = Vm.ParticipantToAdd.Email;
                }
            }
        }

        protected override IEnumerable<Binding> SetBindings() => new List<Binding>
        {
            App.Locator.SessionViewModel.SetBinding(() => App.Locator.SessionViewModel.Identity)
                .WhenSourceChanges(IdentityChanged)
        };

        private void IdentityChanged()
        {
            if (Vm.Identity == null)
            {
                SubscribeForm.Visibility = ViewStates.Gone;
                TvPleaseLogin.Visibility = ViewStates.Visible;
            }
            else
            {
                SubscribeForm.Visibility = ViewStates.Visible;
                TvPleaseLogin.Visibility = ViewStates.Gone;
            }
        }
    }
}