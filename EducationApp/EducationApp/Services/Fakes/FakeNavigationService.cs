using GalaSoft.MvvmLight.Views;

namespace EducationApp.Services.Fakes
{
    public class FakeNavigationService : INavigationService
    {
        public void GoBack()
        {
            //NOP
        }

        public void NavigateTo(string pageKey)
        {
            //NOP
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            //NOP
        }

        public string CurrentPageKey => "Fake";
    }
}