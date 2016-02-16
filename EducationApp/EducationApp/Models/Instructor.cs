using System;
using GalaSoft.MvvmLight;

namespace EducationApp.Models
{
    public class Instructor : ObservableObject, IComparable<Instructor>
    {
        private string _email;
        private string _employer;
        private string _employerDepartment;
        private string _firstName;
        private string _gender;
        private int? _id;
        private bool? _isActive;
        private string _language;
        private string _lastName;

        /// <summary>
        ///     Sets and gets the Id property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int? Id
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        /// <summary>
        ///     Sets and gets the FirstName property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set { Set(ref _firstName, value); }
        }

        /// <summary>
        ///     Sets and gets the LastName property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set { Set(ref _lastName, value); }
        }

        /// <summary>
        ///     Sets and gets the Email property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }

        /// <summary>
        ///     Sets and gets the Gender property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Gender
        {
            get { return _gender; }
            set { Set(ref _gender, value); }
        }

        /// <summary>
        ///     Sets and gets the Language property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Language
        {
            get { return _language; }
            set { Set(ref _language, value); }
        }

        /// <summary>
        ///     Sets and gets the Employer property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Employer
        {
            get { return _employer; }
            set { Set(ref _employer, value); }
        }

        /// <summary>
        ///     Sets and gets the IsActive property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool? IsActive
        {
            get { return _isActive; }
            set { Set(ref _isActive, value); }
        }

        /// <summary>
        ///     Sets and gets the EmployerDepartment property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string EmployerDepartment
        {
            get { return _employerDepartment; }
            set { Set(ref _employerDepartment, value); }
        }

        public int CompareTo(Instructor other) => string.Compare(LastName, other.LastName, StringComparison.Ordinal);
    }
}