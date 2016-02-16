using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;

namespace EducationApp.Models
{
    public class Category : ObservableObject, IEquatable<Category>, IDetailLeveled, IComparable<Category>
    {
        private int _id;
        private string _name;
        private int? _parentCategoryId;
        private byte _presentationOrder;

        public Category()
        {
            Courses = new ObservableCollection<Course>();
            Subcategories = new ObservableCollection<Category>();

            // ReSharper disable ExplicitCallerInfoArgument
            Courses.CollectionChanged += (sender, args) => RaisePropertyChanged(nameof(Courses));
            Subcategories.CollectionChanged += (sender, args) => RaisePropertyChanged(nameof(Subcategories));
            // ReSharper restore ExplicitCallerInfoArgument
        }

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
        ///     Sets and gets the ParentCategoryId property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int? ParentCategoryId
        {
            get { return _parentCategoryId; }
            set { Set(ref _parentCategoryId, value); }
        }

        /// <summary>
        ///     Sets and gets the PresentationOrder property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public byte PresentationOrder
        {
            get { return _presentationOrder; }
            set { Set(ref _presentationOrder, value); }
        }

        public ObservableCollection<Category> Subcategories { get; set; }

        public ObservableCollection<Course> Courses { get; set; }

        /// <summary>
        ///     Sets and gets the Name property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        public int CompareTo(Category other) => Equals(other) ? 0 : PresentationOrder.CompareTo(other.PresentationOrder);

        /// <summary>
        ///     Level of detailed information in this object (as we usually get data shaped objects and those are cached).
        ///     0: no details
        ///     1: subcategories
        ///     2: courses
        /// </summary>
        /// <returns>A byte with the level of detailed information in this object.</returns>
        public byte GetDetailLevel()
        {
            if (Courses.Any())
            {
                return 2;
            }
            if (Subcategories.Any())
            {
                return 1;
            }
            return 0;
        }

        public bool Equals(Category other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Id != 0;
        }

        public override string ToString() => $"{Name} ({Id})";

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Category) obj);
        }

        public override int GetHashCode() => Id == 0 ? base.GetHashCode() : Id;
    }
}