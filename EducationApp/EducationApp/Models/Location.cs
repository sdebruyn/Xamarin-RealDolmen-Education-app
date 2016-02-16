using System.Collections.ObjectModel;
using EducationApp.Extensions;
using GalaSoft.MvvmLight;

namespace EducationApp.Models
{
    public class Location : ObservableObject
    {
        private int? _id;
        private string _name;

        public Location()
        {
            ClassRooms = new ObservableCollection<ClassRoom>();
        }

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
        ///     Sets and gets the Name property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        public ObservableCollection<ClassRoom> ClassRooms { get; set; }

        public override string ToString() => Name.IsNotNullOrWhiteSpace() ? Name : Id.ToString();
    }
}