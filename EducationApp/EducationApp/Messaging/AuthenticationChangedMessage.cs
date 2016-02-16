using EducationApp.Models;
using GalaSoft.MvvmLight.Messaging;

namespace EducationApp.Messaging
{
    public class AuthenticationChangedMessage : MessageBase
    {
        public AuthenticationChangedMessage(Identity newIdentity)
        {
            NewIdentity = newIdentity;
        }

        public Identity NewIdentity { get; }
    }
}