using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using static EducationApp.Extensions.StringExtensions;

namespace EducationApp.Models
{
    public class Subscription : ObservableObject
    {
        private string _city;
        private string _company;
        private string _country;
        private string _email;
        private string _fax;
        private string _firstName;
        private string _jobTitle;
        private string _lastName;
        private string _number;
        private string _phone;
        private string _remarks;
        private string _street;
        private string _vat;
        private string _zipCode;

        public Subscription()
        {
            Participants = new ObservableCollection<Participant>();
            Participants.CollectionChanged +=
                (sender, args) => ValidChanged(args?.OldItems != null && args.OldItems.Count > 0 && IsValid);
        }

        public string Email
        {
            get { return _email; }
            set
            {
                var old = IsValid;
                Set(ref _email, value);
                ValidChanged(old);
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                var old = IsValid;
                Set(ref _firstName, value);
                ValidChanged(old);
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                var old = IsValid;
                Set(ref _lastName, value);
                ValidChanged(old);
            }
        }

        public string Phone
        {
            get { return _phone; }
            set
            {
                var old = IsValid;
                Set(ref _phone, value);
                ValidChanged(old);
            }
        }

        public string JobTitle
        {
            get { return _jobTitle; }
            set { Set(ref _jobTitle, value); }
        }

        public string Company
        {
            get { return _company; }
            set
            {
                var old = IsValid;
                Set(ref _company, value);
                ValidChanged(old);
            }
        }

        public string VAT
        {
            get { return _vat; }
            set { Set(ref _vat, value); }
        }

        public string Fax
        {
            get { return _fax; }
            set
            {
                var old = IsValid;
                Set(ref _fax, value);
                ValidChanged(old);
            }
        }

        public string Street
        {
            get { return _street; }
            set
            {
                var old = IsValid;
                Set(ref _street, value);
                ValidChanged(old);
            }
        }

        public string Number
        {
            get { return _number; }
            set
            {
                var old = IsValid;
                Set(ref _number, value);
                ValidChanged(old);
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                var old = IsValid;
                Set(ref _city, value);
                ValidChanged(old);
            }
        }

        public string ZIPCode
        {
            get { return _zipCode; }
            set
            {
                var old = IsValid;
                Set(ref _zipCode, value);
                ValidChanged(old);
            }
        }

        public string Country
        {
            get { return _country; }
            set
            {
                var old = IsValid;
                Set(ref _country, value);
                ValidChanged(old);
            }
        }

        public string Remarks
        {
            get { return _remarks; }
            set { Set(ref _remarks, value); }
        }

        public ObservableCollection<Participant> Participants { get; }

        [JsonIgnore]
        public bool IsValid
            =>
                AreNotNullOrWhiteSpace(Email, FirstName, LastName, Phone, Street, Number, City, ZIPCode, Country) &&
                Participants.All(p => p.IsValid) && Participants.Any() &&
                Regex.Match(Email, @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$", RegexOptions.IgnoreCase).Success;

        private void ValidChanged(bool old)
        {
            if (old == IsValid)
                return;

            RaisePropertyChanged(nameof(IsValid));
            var message = new PropertyChangedMessage<bool>(this, old, IsValid, nameof(IsValid));
            Messenger.Default.Send(message);
        }
    }
}