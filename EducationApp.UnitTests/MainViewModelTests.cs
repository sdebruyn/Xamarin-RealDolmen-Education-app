using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using EducationApp.Exceptions;
using EducationApp.Extensions;
using EducationApp.Messaging;
using EducationApp.Models;
using EducationApp.Services;
using EducationApp.Services.Fakes;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EducationApp.UnitTests
{
    [TestClass]
    public class MainViewModelTests
    {
        private Mock<IDialogService> _dialogService;
        private ViewModelLocator _locator;
        private Mock<INavigationService> _navigationService;
        private ContainerBuilder _registrations;

        [TestInitialize]
        public void Initialize()
        {
            _registrations = new ContainerBuilder();

            _dialogService = new Mock<IDialogService>();
            _navigationService = new Mock<INavigationService>();

            _registrations.RegisterInstance(_dialogService.Object).AsImplementedInterfaces();
            _registrations.RegisterInstance(_navigationService.Object).AsImplementedInterfaces();
            _registrations.RegisterType<FakeDispatcherHelper>().AsImplementedInterfaces();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _registrations = null;
            _dialogService = null;
            _navigationService = null;
        }

        [TestMethod]
        public void Search_ResultsUpdated()
        {
            var fakeCourse = FakeCourseService.GenerateFakeCourse();
            const string testTitle = "test title";
            fakeCourse.Title = testTitle;

            var courseService = new Mock<ICourseService>();
            courseService.Setup(ics => ics.SearchCoursesAsync(testTitle))
                .ReturnsAsync(new List<Course>
                {
                    fakeCourse
                });

            _registrations.RegisterInstance(courseService.Object).AsImplementedInterfaces();
            FinishRegistrations();

            var vm = _locator.MainViewModel;
            Assert.IsNotNull(vm);
            Assert.IsNotNull(vm.FoundCourses);

            Assert.IsTrue(vm.SearchValue.IsNullOrEmpty());
            vm.SearchValue = testTitle;

            Thread.Sleep(new TimeSpan(0, 0, 3));

            var foundCourse = vm.FoundCourses.FirstOrDefault();
            Assert.IsNotNull(foundCourse);
            Assert.AreEqual(fakeCourse, foundCourse);
        }

        [TestMethod]
        public void SearchTwice_ResultsUpdated()
        {
            var fakeCourse1 = FakeCourseService.GenerateFakeCourse();
            var fakeCourse2 = FakeCourseService.GenerateFakeCourse();
            const string testTitle1 = "test title 1";
            const string testTitle2 = "test title 2";
            fakeCourse1.Title = testTitle1;
            fakeCourse2.Title = testTitle2;

            var courseService = new Mock<ICourseService>();
            courseService.Setup(ics => ics.SearchCoursesAsync(testTitle1))
                .ReturnsAsync(new List<Course>
                {
                    fakeCourse1
                });
            courseService.Setup(ics => ics.SearchCoursesAsync(testTitle2))
                .ReturnsAsync(new List<Course>
                {
                    fakeCourse2
                });

            _registrations.RegisterInstance(courseService.Object).AsImplementedInterfaces();
            FinishRegistrations();

            var vm = _locator.MainViewModel;
            Assert.IsNotNull(vm);
            Assert.IsNotNull(vm.FoundCourses);
            Assert.IsTrue(vm.SearchValue.IsNullOrEmpty());

            vm.SearchValue = testTitle1;
            vm.SearchValue = testTitle2;

            Thread.Sleep(5000);

            var foundCourse = vm.FoundCourses.FirstOrDefault();
            Assert.IsNotNull(foundCourse);
            Assert.AreEqual(fakeCourse2, foundCourse);
        }

        [TestMethod]
        public async Task ShowDetails_Navigated()
        {
            var fakeCategory = FakeCategoryService.GetFakeCategory(1);

            var categoryService = new Mock<ICategoryService>();
            categoryService.Setup(cs => cs.GetAllCategoriesAsync()).ReturnsAsync(new List<Category>
            {
                fakeCategory
            });
            _navigationService.Setup(ns => ns.NavigateTo(Constants.Pages.CategoryDetailsKey, fakeCategory)).Verifiable();

            _registrations.RegisterInstance(categoryService.Object).AsImplementedInterfaces();
            FinishRegistrations();

            var vm = _locator.MainViewModel;
            Assert.IsNotNull(vm);
            Assert.IsNotNull(vm.Categories);

            await vm.ActivateAsync().ConfigureAwait(true);

            Assert.IsTrue(vm.Categories.Count > 0);
            Assert.AreEqual(fakeCategory, vm.Categories.First());

            vm.ShowDetailsCommand.Execute(fakeCategory);
            _navigationService.Verify(nv => nv.NavigateTo(Constants.Pages.CategoryDetailsKey, fakeCategory), Times.Once);
        }

        [TestMethod]
        public void ShowDetails_NotNavigated()
        {
            var navigationService = new Mock<INavigationService>(MockBehavior.Strict);

            _registrations.RegisterInstance(navigationService.Object).AsImplementedInterfaces();
            FinishRegistrations();

            var vm = _locator.MainViewModel;
            Assert.IsNotNull(vm);

            vm.ShowDetailsCommand.Execute(null);
            Assert.IsNotNull(vm.ShowDetailsCommand);
        }

        [TestMethod]
        public void Login_CalledLogin()
        {
            var viewRef = new object();
            var authenticationService = new Mock<IAuthenticationService>();
            authenticationService.Setup(a => a.LoginAsync(viewRef)).Verifiable();

            _registrations.RegisterInstance(authenticationService.Object).AsImplementedInterfaces();
            FinishRegistrations();

            var vm = _locator.MainViewModel;
            Assert.IsNotNull(vm);

            vm.LoginCommand.Execute(viewRef);
            authenticationService.Verify(a => a.LoginAsync(viewRef), Times.Once);
        }

        [TestMethod]
        public void Logout_CalledLogout()
        {
            var viewRef = new object();
            var authenticationService = new Mock<IAuthenticationService>();
            authenticationService.Setup(a => a.LogoutAsync()).Verifiable();

            _registrations.RegisterInstance(authenticationService.Object).AsImplementedInterfaces();
            FinishRegistrations();

            var vm = _locator.MainViewModel;
            Assert.IsNotNull(vm);

            vm.LogoutCommand.Execute(viewRef);
            authenticationService.Verify(a => a.LogoutAsync(), Times.Once);
        }

        [TestMethod]
        public void Identity_UpdateFromMessage()
        {
            var fakeIdentity = new Identity();
            fakeIdentity.AddClaims(new Claim(Claim.FamilyNameName, Claim.FamilyNameName),
                new Claim(Claim.EmailName, Claim.EmailName), new Claim(Claim.GivenNameName, Claim.GivenNameName));

            FinishRegistrations();

            var vm = _locator.MainViewModel;
            Assert.IsNotNull(vm);
            Assert.IsNull(vm.Identity);

            Messenger.Default.Send(new AuthenticationChangedMessage(fakeIdentity));

            Assert.IsNotNull(vm.Identity);
            Assert.AreEqual(fakeIdentity, vm.Identity);
        }

        [TestMethod]
        public async Task LoadCategories_Dialog_DataSourceException()
        {
            var categoryService = new Mock<ICategoryService>(MockBehavior.Strict);
            categoryService.Setup(cs => cs.GetAllCategoriesAsync()).ThrowsAsync(new DataSourceException(null));
            _registrations.RegisterInstance(categoryService.Object);

            FinishRegistrations();

            var vm = _locator.MainViewModel;
            Assert.IsNotNull(vm);

            await vm.ActivateAsync().ConfigureAwait(true);

            _dialogService.Verify(
                d =>
                    d.ShowMessageBox(It.IsAny<string>(),
                        It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task LoadCategories_Dialog_Exception()
        {
            var categoryService = new Mock<ICategoryService>(MockBehavior.Strict);
            categoryService.Setup(cs => cs.GetAllCategoriesAsync()).ThrowsAsync(new ConnectionException());
            _registrations.RegisterInstance(categoryService.Object);

            FinishRegistrations();

            var vm = _locator.MainViewModel;
            Assert.IsNotNull(vm);

            await vm.ActivateAsync().ConfigureAwait(true);

            _dialogService.Verify(
                d =>
                    d.ShowMessageBox(It.IsAny<string>(),
                        It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void SearchValue_SearchStatus_Updates()
        {
            FinishRegistrations();

            var vm = _locator.MainViewModel;
            Assert.IsNotNull(vm);
            Assert.AreEqual(SearchStatus.Inactive, vm.SearchStatus);
            Assert.IsTrue(vm.SearchValue.IsNullOrEmpty());

            vm.SearchValue = "test";
            Assert.AreNotEqual(SearchStatus.Inactive, vm.SearchStatus);
        }

        private void FinishRegistrations()
        {
            ViewModelLocator.RegisterServices(_registrations, true);
            _locator = new ViewModelLocator();
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
            var vm = _locator.MainViewModel;
            vm.ShowCourseDetailsCommand.Execute(course);

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
            var vm = _locator.MainViewModel;
            vm.ShowCourseDetailsCommand.Execute(null);

            // Assert
            ns.Verify(n => n.NavigateTo(Constants.Pages.CourseDetailsKey, It.IsAny<object>()), Times.Never);
        }
    }
}