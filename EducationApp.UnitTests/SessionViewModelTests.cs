using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using EducationApp.Exceptions;
using EducationApp.Messaging;
using EducationApp.Models;
using EducationApp.Services;
using EducationApp.Services.Fakes;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EducationApp.UnitTests
{
    [TestClass]
    public class SessionViewModelTests
    {
        private AuthenticationChangedMessage _authMessage;
        private Mock<IContactService> _fakeContactService;
        private Mock<IDialogService> _fakeDialogService;
        private Participant _fakeParticipant;
        private Session _fakeSession;
        private Identity _identity;
        private ViewModelLocator _locator;
        private ContainerBuilder _registrations;
        private SessionViewModel _vm => _locator.SessionViewModel;

        [TestInitialize]
        public void Initialize()
        {
            _registrations = new ContainerBuilder();
            _fakeContactService = new Mock<IContactService>(MockBehavior.Strict);
            _fakeDialogService = new Mock<IDialogService>();
            _fakeSession = FakeCourseService.GenerateFakeSession(0);

            _identity = new Identity();
            _identity.AddClaims(new Claim(Claim.GivenNameName, Claim.GivenNameName),
                new Claim(Claim.FamilyNameName, Claim.FamilyNameName), new Claim(Claim.EmailName, Claim.EmailName));
            _authMessage = new AuthenticationChangedMessage(_identity);
        }

        [TestMethod]
        public void Initialize_Initialized_WithId()
        {
            // Arrange
            FinishRegistrations();

            // Assert
            Assert.IsNotNull(_vm);
            Assert.IsNotNull(_vm.ContactForm);
            Assert.IsNotNull(_vm.Subscription);
            Assert.IsNotNull(_vm.ScoredFeedback);
        }

        [TestMethod]
        public void SetParameter_ParameterSet_ValidSession()
        {
            // Arrange
            ViewModelLocator.RegisterServices(_registrations, true);
            _locator = new ViewModelLocator();

            // Act
            Assert.AreEqual(null, _vm.Session);
            _vm.SetParameter(_fakeSession);

            // Assert
            Assert.AreEqual(_fakeSession, _vm.Session);
        }

        [TestMethod]
        public void SetParameter_ParameterNotSet_SetObject()
        {
            // Arrange
            ViewModelLocator.RegisterServices(_registrations, true);
            _locator = new ViewModelLocator();

            // Act
            Assert.AreEqual(null, _vm.Session);
            _vm.SetParameter(new object());

            // Assert
            Assert.AreEqual(null, _vm.Session);
        }

        [TestMethod]
        public async Task IdentitySet_IdentitySet_SetIdentity()
        {
            // Arrange
            var fakeIdentity = new Identity();
            var authService = new Mock<IAuthenticationService>(MockBehavior.Strict);
            authService.Setup(authS => authS.GetIdentityAsync()).ReturnsAsync(fakeIdentity);
            _registrations.RegisterInstance(authService.Object);
            FinishRegistrations();

            // Act
            await _vm.ActivateAsync().ConfigureAwait(true);

            // Assert
            Assert.IsNotNull(_vm.Identity);
        }


        [TestMethod]
        public void ReceiveIdentityMessage_IdentityUpdated_WithMessage()
        {
            // Arrange
            FinishRegistrations();

            // Act
            var firstIdentity = _vm.Identity;
            SendAuthMessage();

            // Assert
            Assert.IsNotNull(_vm.Identity);
            Assert.AreNotEqual(firstIdentity, _vm.Identity);
        }


        [TestMethod]
        public void SendContactMessage_MessageSent_UsingCommand()
        {
            // Arrange
            const string fakeSubject = "Confirm your membership of the Dark Side";
            const string fakeMessage =
                "Dear Anakin, please click this link to confirm your registration for the Dark Side of the Force. With kind regards, Senator Palpatine";

            _fakeContactService.Setup(fcs => fcs.SendContactFormAsync(_fakeSession.Id, It.IsAny<ContactForm>()))
                .Returns(Task.Run(() => { }));

            FinishRegistrations();
            SendAuthMessage();

            // Act
            Assert.IsNotNull(_vm.ContactForm);
            Assert.IsNotNull(_vm.Identity);

            _vm.ContactForm.Subject = fakeSubject;
            _vm.ContactForm.Body = fakeMessage;

            _vm.SendContactFormCommand.Execute(null);
            Thread.Sleep(new TimeSpan(0, 0, 3));

            // Assert
            Assert.AreEqual(fakeSubject, _vm.ContactForm.Subject);
            Assert.AreEqual(fakeMessage, _vm.ContactForm.Body);
            _fakeContactService.Verify(fcs => fcs.SendContactFormAsync(_fakeSession.Id, It.IsAny<ContactForm>()),
                Times.Once);
        }


        [TestMethod]
        public void SendFeedback_MessageSent_UsingCommand()
        {
            // Arrange
            _fakeContactService.Setup(fcs => fcs.SendFeedbackAsync(_fakeSession.Id, It.IsAny<ScoredFeedback>()))
                .Returns(Task.Run(
                    () => { }));
            FinishRegistrations();

            SendAuthMessage();

            // Act
            Assert.IsNotNull(_vm.ScoredFeedback);
            Assert.IsNotNull(_vm.Identity);

            _vm.SendFeedbackCommand.Execute(null);
            Thread.Sleep(new TimeSpan(0, 0, 3));

            // Assert
            _fakeContactService.Verify(fcs => fcs.SendFeedbackAsync(_fakeSession.Id, It.IsAny<ScoredFeedback>()),
                Times.Once);
        }


        [TestMethod]
        public void SendFeedback_ErrorMessage_ServiceThrowingException()
        {
            // Arrange
            _fakeContactService.Setup(fcs => fcs.SendFeedbackAsync(_fakeSession.Id, It.IsAny<ScoredFeedback>()))
                .Throws<ConnectionException>();
            _fakeDialogService.Setup(fds => fds.ShowError(It.IsNotNull<string>(), It.IsAny<string>(), null, null))
                .Returns(Task.Run(() => { }));
            FinishRegistrations();

            SendAuthMessage();

            // Act
            Assert.IsNotNull(_vm.ScoredFeedback);
            Assert.IsNotNull(_vm.Identity);

            _vm.SendFeedbackCommand.Execute(null);
            Thread.Sleep(new TimeSpan(0, 0, 3));

            // Assert
            _fakeDialogService.Verify(fds => fds.ShowError(It.IsNotNull<string>(), It.IsAny<string>(), null, null));
        }

        [TestMethod]
        public void SendSubscription_SubscriptionSent_UsingCommand()
        {
            // Arrange
            _fakeContactService.Setup(fcs => fcs.SendSubscriptionAsync(_fakeSession.Id, It.IsAny<Subscription>()))
                .Returns(Task.Run(
                    () => { }));

            FinishRegistrations();
            SendAuthMessage();

            // Act
            Assert.IsNotNull(_vm.Subscription);
            Assert.IsNotNull(_vm.Identity);

            _vm.Subscription.LastName = "Vader";
            _vm.Subscription.FirstName = "Darth";
            _vm.Subscription.Company = "Dark Side Operations";
            _vm.Subscription.City = "Galactic City";
            _vm.Subscription.Email = "ani1977@darkmail.com";
            _vm.Subscription.Phone = "+3221234560";
            _vm.Subscription.Country = "Coruscant";
            _vm.Subscription.Street = "Senate";
            _vm.Subscription.Number = "123";
            _vm.Subscription.ZIPCode = "123";
            _vm.Subscription.Participants.Add(_fakeParticipant);
            Assert.IsTrue(_vm.Subscription.IsValid);

            _vm.SendSubscriptionCommand.Execute(null);
            Thread.Sleep(new TimeSpan(0, 0, 3));

            // Assert
            _fakeContactService.Verify(fcs => fcs.SendSubscriptionAsync(_fakeSession.Id, It.IsAny<Subscription>()),
                Times.Once);
        }

        private void FinishRegistrations()
        {
            _registrations.RegisterInstance(_fakeContactService.Object).AsImplementedInterfaces();
            _registrations.RegisterInstance(_fakeDialogService.Object).AsImplementedInterfaces();
            ViewModelLocator.RegisterServices(_registrations, true);
            _locator = new ViewModelLocator();
            _vm.Session = _fakeSession;

            _fakeParticipant = new Participant
            {
                Email = "ani77@darkmail.com",
                FirstName = "Darth",
                LastName = "Vader"
            };
        }

        private void SendAuthMessage()
        {
            Messenger.Default.Send(_authMessage);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _registrations = null;
            _identity = null;
            _fakeContactService = null;
            _fakeDialogService = null;
            _fakeParticipant = null;
            _locator = null;
            _fakeSession = null;
            ServiceLocator.SetLocatorProvider(null);
        }
    }
}