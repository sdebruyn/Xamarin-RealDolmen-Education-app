using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using EducationApp.Exceptions;
using EducationApp.Models;
using EducationApp.Services;
using EducationApp.Services.Fakes;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EducationApp.UnitTests
{
    [TestClass]
    public class SubcategoryViewModelTests
    {
        private Mock<ICourseService> _courseService;
        private Category _fakeCategory;
        private ICollection<Course> _fakeCourseList;
        private ViewModelLocator _locator;
        private ContainerBuilder _registrations;
        private SubcategoryViewModel _vm => _locator.SubcategoryViewModel;


        [TestInitialize]
        public void Initialize()
        {
            var random = new Random();
            _fakeCategory = FakeCategoryService.GetFakeCategory(1, random);
            _registrations = new ContainerBuilder();
            _courseService = new Mock<ICourseService>();

            var courseList = new List<Course>();
            for (var i = 0; i < 5; i++)
            {
                courseList.Add(FakeCourseService.GenerateFakeCourse(random));
            }
            courseList.Sort();
            _fakeCourseList = courseList;
        }

        [TestCleanup]
        public void Cleanup()
        {
            _courseService = null;
            _fakeCategory = null;
            _registrations = null;
            _fakeCourseList = null;
            _locator = null;
            ServiceLocator.SetLocatorProvider(null);
        }

        private void FinishRegistrations()
        {
            _registrations.RegisterInstance(_courseService.Object).AsImplementedInterfaces();
            ViewModelLocator.RegisterServices(_registrations, true);
            _locator = new ViewModelLocator();
            _vm.Category = _fakeCategory;
        }

        [TestMethod]
        public async Task Activate_CorrectCourses_CourseServiceSetup()
        {
            // Arrange
            _courseService.Setup(cs => cs.GetCategoryCoursesAsync(_fakeCategory.Id, false))
                .ReturnsAsync(_fakeCourseList);
            FinishRegistrations();

            // Act
            Assert.IsNotNull(_vm);
            Assert.IsNotNull(_vm.Category);
            Assert.AreEqual(_fakeCategory, _vm.Category);
            await _vm.ActivateAsync().ConfigureAwait(true);

            // Assert
            AssertExtensions.AreEqual(_fakeCourseList, _vm.Category.Courses);
        }


        [TestMethod]
        public void SetParameter_ParameterSet_ValidCategory()
        {
            // Arrange
            FinishRegistrations();
            _vm.Category = null;

            // Act
            Assert.IsNull(_vm.Category);
            _vm.SetParameter(_fakeCategory);

            // Assert
            Assert.AreEqual(_fakeCategory, _vm.Category);
        }


        [TestMethod]
        public void SetParameter_ParameterNotSet_SetObject()
        {
            // Arrange
            FinishRegistrations();
            _vm.Category = null;

            // Act
            Assert.IsNull(_vm.Category);
            _vm.SetParameter(new object());

            // Assert
            Assert.IsNull(_vm.Category);
        }

        [TestMethod]
        public void Activate_Dialog_ConnectionException()
        {
            // Arrange
            _courseService.Setup(cs => cs.GetCategoryCoursesAsync(_fakeCategory.Id, false))
                .ThrowsAsync(new ConnectionException());

            var dialogService = new Mock<IDialogService>(MockBehavior.Strict);
            dialogService.Setup(ds => ds.ShowMessageBox(It.IsNotNull<string>(), It.IsAny<string>()))
                .Returns(Task.Run(() => { }));
            _registrations.RegisterInstance(dialogService.Object).AsImplementedInterfaces();

            FinishRegistrations();

            // Act
            Assert.IsNotNull(_vm);
            Assert.IsNotNull(_vm.Category);
            Assert.AreEqual(_fakeCategory, _vm.Category);
            _vm.ActivateAsync().Wait();

            // Assert
            dialogService.Verify(ds => ds.ShowMessageBox(It.IsNotNull<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task Activate_Dialog_DataSourceException()
        {
            // Arrange
            _courseService.Setup(cs => cs.GetCategoryCoursesAsync(_fakeCategory.Id, false))
                .ThrowsAsync(new DataSourceException(new Exception()));

            var dialogService = new Mock<IDialogService>(MockBehavior.Strict);
            dialogService.Setup(ds => ds.ShowMessageBox(It.IsNotNull<string>(), It.IsAny<string>()))
                .Returns(Task.Run(() => { }));
            _registrations.RegisterInstance(dialogService.Object).AsImplementedInterfaces();

            FinishRegistrations();

            // Act
            Assert.IsNotNull(_vm);
            Assert.IsNotNull(_vm.Category);
            Assert.AreEqual(_fakeCategory, _vm.Category);
            await _vm.ActivateAsync().ConfigureAwait(true);

            // Assert
            dialogService.Verify(ds => ds.ShowMessageBox(It.IsNotNull<string>(), It.IsAny<string>()), Times.Once);
        }


        [TestMethod]
        public void ShowCourseDetails_NavigationServiceCalled_ValidCourse()
        {
            // Arrange
            var course = FakeCourseService.GenerateFakeCourse();
            var ns = new Mock<INavigationService>(MockBehavior.Loose);
            _registrations.RegisterInstance(ns.Object).AsImplementedInterfaces();
            FinishRegistrations();

            // Act
            _vm.ShowCourseDetailsCommand.Execute(course);

            // Assert
            ns.Verify(n => n.NavigateTo(Constants.Pages.CourseDetailsKey, It.IsAny<object>()), Times.Once);
        }

        [TestMethod]
        public void ShowCourseDetails_NavigationServiceNotCalled_InvalidCourse()
        {
            // Arrange
            var ns = new Mock<INavigationService>(MockBehavior.Loose);
            _registrations.RegisterInstance(ns.Object).AsImplementedInterfaces();
            FinishRegistrations();

            // Act
            _vm.ShowCourseDetailsCommand.Execute(null);

            // Assert
            ns.Verify(n => n.NavigateTo(Constants.Pages.CourseDetailsKey, It.IsAny<object>()), Times.Never);
        }
    }
}