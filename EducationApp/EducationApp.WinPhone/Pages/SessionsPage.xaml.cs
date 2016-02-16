using Windows.UI.Xaml.Navigation;
using EducationApp.ViewModels;

namespace EducationApp.WinPhone.Pages
{
    public sealed partial class SessionsPage
    {
        public SessionsPage()
        {
            InitializeComponent();
            DelayedActivation = true;
            DelayedParameter = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            (DataContext as CourseViewModel)?.UpdateSessionsCommand.Execute(null);
        }
    }
}