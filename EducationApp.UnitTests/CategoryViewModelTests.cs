using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CategoryViewModelTests
    {
        private Mock<ICategoryService> _categoryService;
        private Category _fakeCategory;
        private ViewModelLocator _locator;
        private ContainerBuilder _registrations;
        private CategoryViewModel Vm => _locator.CategoryViewModel;

        [TestInitialize]
        public void Initialize()
        {
            var random = new Random();

            _fakeCategory = FakeCategoryService.GetFakeCategory(1, random);
            for (byte i = 0; i < 5; i++)
            {
                var fakeSub = FakeCategoryService.GetFakeCategory((byte) (i + 5), random);
                fakeSub.ParentCategoryId = _fakeCategory.Id;
                _fakeCategory.Subcategories.Add(fakeSub);
            }

            _registrations = new ContainerBuilder();

            _categoryService = new Mock<ICategoryService>();
            _categoryService.Setup(cs => cs.GetAllCategoriesAsync()).ReturnsAsync(new List<Category>
            {
                _fakeCategory
            });
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _registrations = null;
            _fakeCategory = null;
            _locator = null;
            ServiceLocator.SetLocatorProvider(null);
        }

        [TestMethod]
        public void SetParameter_ParameterSet_ValidCategory()
        {
            // Arrange
            FinishRegistrations();
            Vm.Category = null;

            // Act
            Assert.IsNull(Vm.Category);
            Vm.SetParameter(_fakeCategory);

            // Assert
            Assert.AreEqual(_fakeCategory, Vm.Category);
        }


        [TestMethod]
        public void SetParameter_ParameterNotSet_SetObject()
        {
            // Arrange
            FinishRegistrations();
            Vm.Category = null;

            // Act
            Assert.IsNull(Vm.Category);
            Vm.SetParameter(new object());

            // Assert
            Assert.IsNull(Vm.Category);
        }


        [TestMethod]
        public void ShowDetails_Navigated_ValidCategory()
        {
            // Arrange
            var ns = new Mock<INavigationService>();
            ns.Setup(
                n => n.NavigateTo(Constants.Pages.SubcategoryDetailsKey, _fakeCategory.Subcategories.FirstOrDefault()));
            _registrations.RegisterInstance(ns.Object);
            FinishRegistrations();

            // Act
            Vm.ShowDetailsCommand.Execute(_fakeCategory.Subcategories.FirstOrDefault());

            // Assert
            ns.Verify(
                n => n.NavigateTo(Constants.Pages.SubcategoryDetailsKey, _fakeCategory.Subcategories.FirstOrDefault()),
                Times.Once);
        }

        [TestMethod]
        public void ShowDetails_Navigated_InvalidCategory()
        {
            // Arrange
            var ns = new Mock<INavigationService>();
            ns.Setup(
                n => n.NavigateTo(Constants.Pages.SubcategoryDetailsKey, It.IsAny<Category>()));
            _registrations.RegisterInstance(ns.Object);
            FinishRegistrations();

            // Act
            Vm.ShowDetailsCommand.Execute(new object());

            // Assert
            ns.Verify(n => n.NavigateTo(Constants.Pages.SubcategoryDetailsKey, It.IsAny<Category>()), Times.Never);
        }

        [TestMethod]
        public async Task LoadDetails_NoExceptions()
        {
            _categoryService.Setup(cs => cs.GetCategoryDetailsAsync(_fakeCategory.Id)).ReturnsAsync(_fakeCategory);
            FinishRegistrations();
            Assert.IsNotNull(Vm);

            await Vm.ActivateAsync().ConfigureAwait(true);
            Assert.AreEqual(_fakeCategory, Vm.Category);
        }

        [TestMethod]
        public async Task LoadDetails_DataSourceException_Dialog()
        {
            var dialogService = new Mock<IDialogService>();
            dialogService.Setup(ds => ds.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            _registrations.RegisterInstance(dialogService.Object).AsImplementedInterfaces();

            _categoryService.Setup(cs => cs.GetCategoryDetailsAsync(_fakeCategory.Id))
                .ThrowsAsync(new DataSourceException(null));
            FinishRegistrations();
            Assert.IsNotNull(Vm);

            try
            {
                await Vm.ActivateAsync().ConfigureAwait(true);
            }
            catch (Exception)
            {
            }

            dialogService.Verify(ds => ds.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task LoadDetails_ConnectionException_Dialog()
        {
            var dialogService = new Mock<IDialogService>();
            dialogService.Setup(ds => ds.ShowMessageBox(It.IsNotNull<string>(), It.IsNotNull<string>())).Verifiable();
            _registrations.RegisterInstance(dialogService.Object).AsImplementedInterfaces();

            _categoryService.Setup(cs => cs.GetCategoryDetailsAsync(_fakeCategory.Id))
                .ThrowsAsync(new ConnectionException());
            FinishRegistrations();
            Assert.IsNotNull(Vm);

            try
            {
                await Vm.ActivateAsync().ConfigureAwait(true);
            }
            catch (Exception)
            {
            }

            dialogService.Verify(ds => ds.ShowMessageBox(It.IsNotNull<string>(), It.IsNotNull<string>()), Times.Once);
        }

        private void FinishRegistrations()
        {
            _registrations.RegisterInstance(_categoryService.Object).AsImplementedInterfaces();
            ViewModelLocator.RegisterServices(_registrations, true);
            _locator = new ViewModelLocator();
            Vm.Category = _fakeCategory;
        }
    }
}