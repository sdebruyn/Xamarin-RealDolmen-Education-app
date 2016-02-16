using System;
using GalaSoft.MvvmLight;

namespace EducationApp.Models
{
    public class SessionSchedule : ObservableObject, IComparable<SessionSchedule>
    {
        private ClassRoom _classRoom;
        private DateTime? _endTime;
        private int? _id;
        private Instructor _instructor;
        private int? _sessionId;
        private DateTime? _startTime;

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
        ///     Sets and gets the SessionId property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int? SessionId
        {
            get { return _sessionId; }
            set { Set(ref _sessionId, value); }
        }

        /// <summary>
        ///     Sets and gets the StartTime property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public DateTime? StartTime
        {
            get { return _startTime; }
            set { Set(ref _startTime, value); }
        }

        /// <summary>
        ///     Sets and gets the EndTime property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public DateTime? EndTime
        {
            get { return _endTime; }
            set { Set(ref _endTime, value); }
        }

        /// <summary>
        ///     Sets and gets the Instructor property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Instructor Instructor
        {
            get { return _instructor; }
            set { Set(ref _instructor, value); }
        }

        /// <summary>
        ///     Sets and gets the ClassRoom property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public ClassRoom ClassRoom
        {
            get { return _classRoom; }
            set { Set(ref _classRoom, value); }
        }

        public int CompareTo(SessionSchedule other)
        {
            if (StartTime.HasValue && other.StartTime.HasValue)
            {
                return StartTime.Value.CompareTo(other.StartTime.Value);
            }

            if (EndTime.HasValue && other.EndTime.HasValue)
            {
                return EndTime.Value.CompareTo(other.EndTime.Value);
            }

            if (ClassRoom != null && other.ClassRoom != null)
            {
                return ClassRoom.CompareTo(other.ClassRoom);
            }

            return 0;
        }
    }
}