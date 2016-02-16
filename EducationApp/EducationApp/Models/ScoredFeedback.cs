using GalaSoft.MvvmLight;

namespace EducationApp.Models
{
    public class ScoredFeedback : ObservableObject
    {
        private int? _classroom;
        private int? _conceptsAndPractice;
        private int? _content;
        private int? _courseMaterials;
        private int? _global;
        private int? _goalAttained;
        private int? _inDepth;
        private int? _instructorInteraction;
        private int? _instructorKnowledge;
        private int? _instructorPresentation;
        private int? _organization;
        private int? _wellPaced;
        private int? _wellStructured;

        public int? Content
        {
            get { return _content; }
            set { Set(ref _content, value); }
        }

        public int? InDepth
        {
            get { return _inDepth; }
            set { Set(ref _inDepth, value); }
        }

        public int? ConceptsAndPractice
        {
            get { return _conceptsAndPractice; }
            set { Set(ref _conceptsAndPractice, value); }
        }

        public int? WellPaced
        {
            get { return _wellPaced; }
            set { Set(ref _wellPaced, value); }
        }

        public int? InstructorKnowledge
        {
            get { return _instructorKnowledge; }
            set { Set(ref _instructorKnowledge, value); }
        }

        public int? InstructorPresentation
        {
            get { return _instructorPresentation; }
            set { Set(ref _instructorPresentation, value); }
        }

        public int? InstructorInteraction
        {
            get { return _instructorInteraction; }
            set { Set(ref _instructorInteraction, value); }
        }

        public int? WellStructured
        {
            get { return _wellStructured; }
            set { Set(ref _wellStructured, value); }
        }

        public int? Classroom
        {
            get { return _classroom; }
            set { Set(ref _classroom, value); }
        }

        public int? Organization
        {
            get { return _organization; }
            set { Set(ref _organization, value); }
        }

        public int? CourseMaterials
        {
            get { return _courseMaterials; }
            set { Set(ref _courseMaterials, value); }
        }

        public int? GoalAttained
        {
            get { return _goalAttained; }
            set { Set(ref _goalAttained, value); }
        }

        public int? Global
        {
            get { return _global; }
            set { Set(ref _global, value); }
        }
    }
}