using GalaSoft.MvvmLight.Messaging;

namespace EducationApp.Messaging
{
    public class SubmittingMessage: MessageBase
    {
        public bool Submitting { get; }

        public SubmittingMessage(bool submitting)
        {
            Submitting = submitting;
        }
    }
}