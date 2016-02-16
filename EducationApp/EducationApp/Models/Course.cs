using System;
using System.Collections.ObjectModel;
using System.Linq;
using EducationApp.Extensions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace EducationApp.Models
{
    public class Course : ObservableObject, IEquatable<Course>, IDetailLeveled, IComparable<Course>
    {
        private string _contentUrl;
        private double? _duration;
        private int _id;
        private bool? _inCatalog;
        private Instructor _instructor;
        private bool? _isActive;
        private bool _isNew;
        private decimal? _price;
        private string _publisher;
        private DateTime? _startDate;
        private string _title;

        public Course()
        {
            Descriptions = new ObservableCollection<Description>();
            Sessions = new ObservableCollection<Session>();

            Descriptions.CollectionChanged += (sender, args) =>
            {
                RaisePropertyChanged(nameof(Description));
                RaisePropertyChanged(nameof(ExternalUrl));
                Messenger.Default.Send(new PropertyChangedMessage<Description>(this, null, Description,
                    nameof(Description)));
            };
            Sessions.CollectionChanged += (sender, args) => RaisePropertyChanged(nameof(Sessions));
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
        ///     Sets and gets the Title property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        /// <summary>
        ///     Sets and gets the Duration property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public double? Duration
        {
            get { return _duration; }
            set { Set(ref _duration, value); }
        }

        /// <summary>
        ///     Sets and gets the Price property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public decimal? Price
        {
            get { return _price; }
            set { Set(ref _price, value); }
        }

        /// <summary>
        ///     Sets and gets the Publisher property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Publisher
        {
            get { return _publisher; }
            set { Set(ref _publisher, value); }
        }

        /// <summary>
        ///     Sets and gets the StartDate property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public DateTime? StartDate
        {
            get { return _startDate; }
            set { Set(ref _startDate, value); }
        }

        /// <summary>
        ///     Sets and gets the InCatalog property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool? InCatalog
        {
            get { return _inCatalog; }
            set { Set(ref _inCatalog, value); }
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
        ///     Sets and gets the IsNew property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsNew
        {
            get { return _isNew; }
            set { Set(ref _isNew, value); }
        }

        /// <summary>
        ///     Sets and gets the ContentUrl property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string ContentUrl
        {
            get { return _contentUrl; }
            set { Set(ref _contentUrl, value); }
        }

        public ObservableCollection<Description> Descriptions { get; set; }
        public ObservableCollection<Session> Sessions { get; set; }

        public Description Description => Descriptions.FirstOrDefault();
        public string ExternalUrl => Description?.ExternalUrl ?? ContentUrl;

        /// <summary>
        ///     Sets and gets the Instructor property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Instructor Instructor
        {
            get { return _instructor; }
            set { Set(ref _instructor, value); }
        }

        public int CompareTo(Course other)
            => Equals(other) ? 0 : string.Compare(Title, other.Title, StringComparison.Ordinal);

        /// <summary>
        ///     Returns the level of details in this object.
        ///     0: no details at all
        ///     1: title
        ///     2: instructor & descriptions
        /// </summary>
        /// <returns>A byte with the level of details.</returns>
        public byte GetDetailLevel()
        {
            if (Sessions.Any())
            {
                return 3;
            }
            if (Instructor != null && Descriptions.Any())
            {
                return 2;
            }
            if (Title.IsNotNullOrWhiteSpace())
            {
                return 1;
            }
            return 0;
        }

        public bool Equals(Course other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (Id != 0 && Id != other.Id) return false;
            return GetDetailLevel() == other.GetDetailLevel();
        }

        public override string ToString()
            => $"Id: {Id}, Title: {Title}, InCatalog: {InCatalog}, IsActive: {IsActive}, IsNew: {IsNew}";

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Course) obj);
        }

        public void CleanNullProperties()
        {
            if (Instructor != null && Instructor.Id == null)
            {
                Instructor = null;
            }
            var toRemove = Descriptions.Where(description => description.CourseId == null).ToList();
            toRemove.ForEach(d => Descriptions.Remove(d));
        }

        public override int GetHashCode() => Id == 0 ? base.GetHashCode() : Id;
    }
}