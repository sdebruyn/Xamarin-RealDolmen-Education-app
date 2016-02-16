using GalaSoft.MvvmLight;

namespace EducationApp.Models
{
    public class Description : ObservableObject
    {
        private string _audience;
        private string _courseContent;
        private int? _courseId;
        private string _externalUrl;
        private string _language;
        private string _longDescription;
        private string _materials;
        private string _methods;
        private string _objectives;
        private string _platforms;
        private string _prerequisites;
        private string _shortDescription;

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
        ///     Sets and gets the Audience property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Audience
        {
            get { return _audience; }
            set { Set(ref _audience, value); }
        }

        /// <summary>
        ///     Sets and gets the Prerequisites property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Prerequisites
        {
            get { return _prerequisites; }
            set { Set(ref _prerequisites, value); }
        }

        /// <summary>
        ///     Sets and gets the Objectives property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Objectives
        {
            get { return _objectives; }
            set { Set(ref _objectives, value); }
        }

        /// <summary>
        ///     Sets and gets the Methods property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Methods
        {
            get { return _methods; }
            set { Set(ref _methods, value); }
        }

        /// <summary>
        ///     Sets and gets the Materials property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Materials
        {
            get { return _materials; }
            set { Set(ref _materials, value); }
        }

        /// <summary>
        ///     Sets and gets the ShortDescription property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string ShortDescription
        {
            get { return _shortDescription; }
            set { Set(ref _shortDescription, value); }
        }

        /// <summary>
        ///     Sets and gets the LongDescription property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string LongDescription
        {
            get { return _longDescription; }
            set { Set(ref _longDescription, value); }
        }

        /// <summary>
        ///     Sets and gets the Platforms property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Platforms
        {
            get { return _platforms; }
            set { Set(ref _platforms, value); }
        }

        /// <summary>
        ///     Sets and gets the CourseContent property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string CourseContent
        {
            get { return _courseContent; }
            set { Set(ref _courseContent, value); }
        }

        /// <summary>
        ///     Sets and gets the ExternalUrl property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string ExternalUrl
        {
            get { return _externalUrl; }
            set { Set(ref _externalUrl, value); }
        }

        /// <summary>
        ///     Sets and gets the CourseId property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int? CourseId
        {
            get { return _courseId; }
            set { Set(ref _courseId, value); }
        }
    }
}