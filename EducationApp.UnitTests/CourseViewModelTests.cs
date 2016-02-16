using System;
using System.Threading.Tasks;
using Autofac;
using EducationApp.Exceptions;
using EducationApp.Models;
using EducationApp.Services;
using EducationApp.Services.Fakes;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EducationApp.UnitTests
{
    [TestClass]
    public class CourseViewModelTests
    {
        private Mock<ICourseService> _courseService;
        private Course _detailedFakeCourse;
        private Course _fakeCourse;
        private ViewModelLocator _locator;
        private ContainerBuilder _registrations;
        private CourseViewModel _vm => _locator.CourseViewModel;

        [TestInitialize]
        public void Initialize()
        {
            _registrations = new ContainerBuilder();
            _courseService = new Mock<ICourseService>(MockBehavior.Loose);
            _detailedFakeCourse = FakeCourseService.GenerateFakeCourse();
            _detailedFakeCourse.Descriptions.Add(new Description());
            _detailedFakeCourse.Instructor = new Instructor();
            _fakeCourse = new Course
            {
                Title = _detailedFakeCourse.Title,
                Id = _detailedFakeCourse.Id
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _fakeCourse = null;
            _detailedFakeCourse = null;
            _courseService = null;
            _registrations = null;
            _locator = null;
        }

        private void FinishRegistrations()
        {
            _registrations.RegisterInstance(_courseService.Object).AsImplementedInterfaces();
            ViewModelLocator.RegisterServices(_registrations, true);
            _locator = new ViewModelLocator();
            _vm.Course = _fakeCourse;
        }


        [TestMethod]
        public void Initialize_WithCourse_FakeCourse()
        {
            // Arrange
            FinishRegistrations();

            // Assert
            Assert.IsNotNull(_vm);
            Assert.IsInstanceOfType(_vm, typeof (CourseViewModel));
            Assert.AreEqual(_fakeCourse, _vm.Course);
        }


        [TestMethod]
        public void ActivateViewModel_Succeeded_WorkingFakeService()
        {
            // Arrange
            _courseService.Setup(cs => cs.GetCourseDetailsAsync(_fakeCourse.Id)).ReturnsAsync(_detailedFakeCourse);
            FinishRegistrations();

            // Act
            var courseBefore = _vm.Course;
            _vm.ActivateAsync().Wait();
            var courseAfter = _vm.Course;

            // Assert
            Assert.IsNotNull(courseBefore);
            Assert.IsNotNull(courseAfter);
            Assert.AreEqual(_fakeCourse, courseBefore);
            Assert.AreNotEqual(_fakeCourse, courseAfter);
            Assert.AreEqual(_detailedFakeCourse, courseAfter);
            _courseService.Verify(cs => cs.GetCourseDetailsAsync(_fakeCourse.Id), Times.Once);
        }

        [TestMethod]
        public void SetParameter_ParameterSet_ValidCourse()
        {
            // Arrange
            FinishRegistrations();
            _vm.Course = null;

            // Act
            Assert.IsNull(_vm.Course);
            _vm.SetParameter(_fakeCourse);

            // Assert
            Assert.AreEqual(_fakeCourse, _vm.Course);
        }


        [TestMethod]
        public void SendInstructorEmail_EmailSent_MockedEmailServiceCanSendMail()
        {
            // Arrange
            var platformService = new Mock<IPlatformActionService>();
            platformService.Setup(ps => ps.CanSendMail()).Returns(true);
            platformService.Setup(ps => ps.SendEmail(It.IsNotNull<string>(), It.IsNotNull<string>()));

            _registrations.RegisterInstance(platformService.Object).AsImplementedInterfaces();
            FinishRegistrations();

            // Act
            Assert.IsFalse(_vm.SendInstructorEmailCommand.CanExecute(null));
            _vm.Course.Instructor = new Instructor
            {
                Email = "test@realdolmen.com"
            };
            Assert.IsTrue(_vm.SendInstructorEmailCommand.CanExecute(null));
            _vm.SendInstructorEmailCommand.Execute(null);

            // Assert
            platformService.Verify(ps => ps.CanSendMail(), Times.AtLeastOnce);
            platformService.Verify(ps => ps.SendEmail(It.IsNotNull<string>(), It.IsNotNull<string>()), Times.AtLeastOnce);
        }


        [TestMethod]
        public void ActiveDescription_NotChanged_NoDescription()
        {
            // Arrange
            _fakeCourse.Descriptions.Clear();
            FinishRegistrations();

            // Act
            Assert.IsNull(_vm.Course?.Description);
            Assert.IsNull(_vm.ActiveDescription);
            _vm.SwitchDescriptionCommand.Execute(null);

            // Assert
            Assert.IsNull(_vm.ActiveDescription);
        }

        [TestMethod]
        public void ActiveDescription_Changed_ValidDescription()
        {
            // Arrange
            _fakeCourse.Descriptions.Clear();
            var fakeDescription = new Description
            {
                ShortDescription = "short",
                LongDescription = "long"
            };
            _fakeCourse.Descriptions.Add(fakeDescription);
            FinishRegistrations();

            // Act
            var first = _vm.ActiveDescription;
            Assert.IsNotNull(first);
            Assert.IsNotNull(_vm.ActiveDescription);
            _vm.SwitchDescriptionCommand.Execute(null);
            var second = _vm.ActiveDescription;

            // Assert
            Assert.AreNotEqual(first, second);
            Assert.AreEqual(fakeDescription.ShortDescription, first);
            Assert.AreEqual(fakeDescription.LongDescription, second);
        }

        [TestMethod]
        public void OpenBrowserCommand_BrowserOpenend_MockedService()
        {
            // Arrange
            var platformService = new Mock<IPlatformActionService>();
            platformService.Setup(ps => ps.OpenBrowserAsync(It.IsNotNull<string>())).Returns(Task.Run(() => { }));

            _registrations.RegisterInstance(platformService.Object).AsImplementedInterfaces();
            FinishRegistrations();

            // Act
            Assert.IsFalse(_vm.OpenBrowserCommand.CanExecute(null));
            _vm.Course.ContentUrl = "http://education.realdolmen.com";
            Assert.IsTrue(_vm.OpenBrowserCommand.CanExecute(null));
            _vm.OpenBrowserCommand.Execute(null);

            // Assert
            platformService.Verify(ps => ps.OpenBrowserAsync(It.IsNotNull<string>()), Times.Once);
        }

        [TestMethod]
        public void SendInstructorEmail_EmailSent_MockedEmailServiceCanNotSendMail()
        {
            // Arrange
            var platformService = new Mock<IPlatformActionService>();
            platformService.Setup(ps => ps.CanSendMail()).Returns(false);
            platformService.Setup(ps => ps.SendEmail(It.IsNotNull<string>(), It.IsNotNull<string>()));

            _registrations.RegisterInstance(platformService.Object).AsImplementedInterfaces();
            FinishRegistrations();

            // Act
            Assert.IsFalse(_vm.SendInstructorEmailCommand.CanExecute(null));
            _vm.Course.Instructor = new Instructor
            {
                Email = "test@realdolmen.com"
            };
            Assert.IsFalse(_vm.SendInstructorEmailCommand.CanExecute(null));
            _vm.SendInstructorEmailCommand.Execute(null);

            // Assert
            platformService.Verify(ps => ps.CanSendMail(), Times.AtLeastOnce);
            platformService.Verify(ps => ps.SendEmail(It.IsNotNull<string>(), It.IsNotNull<string>()), Times.Never);
        }

        [TestMethod]
        public void SetParameter_ParameterNotSet_SetObject()
        {
            // Arrange
            FinishRegistrations();
            _vm.Course = null;

            // Act
            Assert.IsNull(_vm.Course);
            _vm.SetParameter(new object());

            // Assert
            Assert.IsNull(_vm.Course);
        }


        [TestMethod]
        public void ActivateViewModel_Dialog_ConnectionExceptionThrown()
        {
            // Arrange
            var dialogService = new Mock<IDialogService>(MockBehavior.Strict);
            dialogService.Setup(ds => ds.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>()));
            _registrations.RegisterInstance(dialogService.Object).AsImplementedInterfaces();

            _courseService.Setup(cs => cs.GetCourseDetailsAsync(_fakeCourse.Id)).ThrowsAsync(new ConnectionException());
            FinishRegistrations();

            // Act
            var courseBefore = _vm.Course;
            try
            {
                _vm.ActivateAsync().Wait();
            }
            catch (Exception)
            {
            }

            // Assert
            Assert.IsNotNull(courseBefore);
            _courseService.Verify(cs => cs.GetCourseDetailsAsync(_fakeCourse.Id));
            dialogService.Verify(ds => ds.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>()));
        }

        [TestMethod]
        public void ActivateViewModel_Dialog_DataSourceExceptionThrown()
        {
            // Arrange
            var dialogService = new Mock<IDialogService>(MockBehavior.Strict);
            dialogService.Setup(ds => ds.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>()));
            _registrations.RegisterInstance(dialogService.Object).AsImplementedInterfaces();

            _courseService.Setup(cs => cs.GetCourseDetailsAsync(_fakeCourse.Id))
                .ThrowsAsync(new DataSourceException(new Exception()));
            FinishRegistrations();

            // Act
            var courseBefore = _vm.Course;
            try
            {
                _vm.ActivateAsync().Wait();
            }
            catch (Exception)
            {
            }

            // Assert
            Assert.IsNotNull(courseBefore);
            _courseService.Verify(cs => cs.GetCourseDetailsAsync(_fakeCourse.Id));
            dialogService.Verify(ds => ds.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}