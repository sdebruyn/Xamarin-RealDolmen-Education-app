using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EducationApp.Extensions;
using GalaSoft.MvvmLight;

namespace EducationApp.Models
{
    public class Session : ObservableObject, IComparable<Session>
    {
        private int _courseId;
        private DateTime? _firstStartTime;
        private int _id;
        private Location _location;
        private string _preparation;

        public Session()
        {
            SessionSchedules = new ObservableCollection<SessionSchedule>();
            SessionSchedules.CollectionChanged += DescriptionUpdated;
            SessionSchedules.CollectionChanged += (sender, args) =>
            {
                _firstStartTime = null;
                RaisePropertyChanged(nameof(FirstStartTime));
            };
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
        ///     Sets and gets the CourseId property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int CourseId
        {
            get { return _courseId; }
            set { Set(ref _courseId, value); }
        }

        /// <summary>
        ///     Sets and gets the Preparation property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Preparation
        {
            get { return _preparation; }
            set { Set(ref _preparation, value); }
        }

        /// <summary>
        ///     Sets and gets the Location property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Location Location
        {
            get { return _location; }
            set
            {
                Set(ref _location, value);
                RaisePropertyChanged(nameof(Description));
            }
        }

        public string Description => ToString();

        public DateTime? FirstStartTime
            =>
                _firstStartTime ??
                (_firstStartTime = new SortedSet<SessionSchedule>(SessionSchedules).FirstOrDefault()?.StartTime);

        public ObservableCollection<SessionSchedule> SessionSchedules { get; set; }

        public int CompareTo(Session other)
        {
            var result = 0;
            if (result == 0 && FirstStartTime != null && other.FirstStartTime != null)
            {
                result = FirstStartTime.Value.CompareTo(other.FirstStartTime.Value);
            }

            if (result == 0 && Location != null && other.Location != null)
            {
                result = string.Compare(Location.Name, other.Location.Name, StringComparison.Ordinal);
            }

            if (result == 0)
            {
                result = string.Compare(Preparation, other.Preparation, StringComparison.Ordinal);
            }

            return result;
        }

        private void DescriptionUpdated(object s, EventArgs e) => RaisePropertyChanged(nameof(Description));

        public override string ToString()
        {
            var desc = new List<string>();
            if (Location != null && Location.ToString().IsNotNullOrWhiteSpace())
            {
                desc.Add(Location.ToString());
            }

            if (FirstStartTime != null)
            {
                desc.Add(FirstStartTime.Value.ToString("d"));
            }

            return string.Join(" - ", desc);
        }
    }
}