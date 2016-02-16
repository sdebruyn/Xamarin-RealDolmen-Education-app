using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using static EducationApp.Extensions.StringExtensions;

namespace EducationApp.Models
{
    public class ContactForm : ObservableObject
    {
        private string _body;
        private string _subject;

        public string Subject
        {
            get { return _subject; }
            set
            {
                var old = IsValid;
                Set(ref _subject, value);
                ValidChanged(old);
            }
        }

        public string Body
        {
            get { return _body; }
            set
            {
                var old = IsValid;
                Set(ref _body, value);
                ValidChanged(old);
            }
        }

        [JsonIgnore]
        public bool IsValid => AreNotNullOrWhiteSpace(Body, Subject);

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