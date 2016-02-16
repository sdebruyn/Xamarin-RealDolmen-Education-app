using System;
using GalaSoft.MvvmLight;

namespace EducationApp.Models
{
    public class ClassRoom : ObservableObject, IComparable<ClassRoom>
    {
        private int _id;
        private string _name;

        /// <summary>
        ///     Sets and gets the Id property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        /// <summary>
        ///     Sets and gets the Name property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        public int CompareTo(ClassRoom other) => string.Compare(Name, other.Name, StringComparison.Ordinal);

        public override string ToString() => Name;
    }
}