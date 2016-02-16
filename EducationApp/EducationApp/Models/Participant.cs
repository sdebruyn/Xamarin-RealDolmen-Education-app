using System;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using static EducationApp.Extensions.StringExtensions;

namespace EducationApp.Models
{
    public class Participant : ObservableObject, IEquatable<Participant>, IEquatable<object>
    {
        private string _email;
        private string _firstName;
        private string _lastName;

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

        public bool IsValid
            =>
                AreNotNullOrWhiteSpace(FirstName, LastName, Email) &&
                Regex.Match(Email, @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$", RegexOptions.IgnoreCase).Success;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Participant) obj);
        }

        public bool Equals(Participant other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Email, other.Email) && string.Equals(FirstName, other.FirstName) &&
                   string.Equals(LastName, other.LastName);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Email?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (FirstName?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ (LastName?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

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