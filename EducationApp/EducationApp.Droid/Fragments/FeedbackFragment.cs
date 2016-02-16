using System.Collections.Generic;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using EducationApp.ViewModels;
using EducationApp.ViewModels.Utilities;
using GalaSoft.MvvmLight.Helpers;

namespace EducationApp.Droid.Fragments
{
    public class FeedbackFragment : BindingFragment
    {
        private LinearLayout _feedbackForm;


        private Button _sendFeedbackButton;
        private AppCompatSeekBar _slClassRoom;
        private AppCompatSeekBar _slConcepts;
        private AppCompatSeekBar _slContent;
        private AppCompatSeekBar _slGlobal;
        private AppCompatSeekBar _slGoalAttained;
        private AppCompatSeekBar _slInDepth;
        private AppCompatSeekBar _slInstructorInteraction;
        private AppCompatSeekBar _slInstructorKnowledge;
        private AppCompatSeekBar _slInstructorPresentation;
        private AppCompatSeekBar _slMaterials;
        private AppCompatSeekBar _slOrganization;
        private AppCompatSeekBar _slWellPaced;
        private AppCompatSeekBar _slWellStructured;
        private TextView _tvPleaseLogin;

        public Button SendFeedbackButton => _sendFeedbackButton
                                            ??
                                            (_sendFeedbackButton =
                                                Activity.FindViewById<Button>(Resource.Id.SendFeedbackButton));

        public TextView TvPleaseLogin => _tvPleaseLogin
                                         ??
                                         (_tvPleaseLogin = Activity.FindViewById<TextView>(Resource.Id.TvPleaseLogin));

        public LinearLayout FeedbackForm => _feedbackForm
                                            ??
                                            (_feedbackForm =
                                                Activity.FindViewById<LinearLayout>(Resource.Id.FeedbackForm));

        public AppCompatSeekBar SlClassRoom
            => _slClassRoom ?? (_slClassRoom = Activity.FindViewById<AppCompatSeekBar>(Resource.Id.SlClassRoom));

        public AppCompatSeekBar SlContent
            => _slContent ?? (_slContent = Activity.FindViewById<AppCompatSeekBar>(Resource.Id.SlContent));

        public AppCompatSeekBar SlConcepts
            => _slConcepts ?? (_slConcepts = Activity.FindViewById<AppCompatSeekBar>(Resource.Id.SlConcepts));

        public AppCompatSeekBar SlMaterials
            => _slMaterials ?? (_slMaterials = Activity.FindViewById<AppCompatSeekBar>(Resource.Id.SlMaterials));

        public AppCompatSeekBar SlInDepth
            => _slInDepth ?? (_slInDepth = Activity.FindViewById<AppCompatSeekBar>(Resource.Id.SlInDepth));

        public AppCompatSeekBar SlInstructorInteraction
            =>
                _slInstructorInteraction ??
                (_slInstructorInteraction = Activity.FindViewById<AppCompatSeekBar>(Resource.Id.SlInstructorInteraction))
            ;

        public AppCompatSeekBar SlInstructorKnowledge
            =>
                _slInstructorKnowledge ??
                (_slInstructorKnowledge = Activity.FindViewById<AppCompatSeekBar>(Resource.Id.SlInstructorKnowledge));

        public AppCompatSeekBar SlInstructorPresentation
            =>
                _slInstructorPresentation ??
                (_slInstructorPresentation =
                    Activity.FindViewById<AppCompatSeekBar>(Resource.Id.SlInstructorPresentation));

        public AppCompatSeekBar SlOrganization
            =>
                _slOrganization ??
                (_slOrganization = Activity.FindViewById<AppCompatSeekBar>(Resource.Id.SlOrganization));

        public AppCompatSeekBar SlWellPaced
            => _slWellPaced ?? (_slWellPaced = Activity.FindViewById<AppCompatSeekBar>(Resource.Id.SlWellPaced));

        public AppCompatSeekBar SlWellStructured
            =>
                _slWellStructured ??
                (_slWellStructured = Activity.FindViewById<AppCompatSeekBar>(Resource.Id.SlWellStructured));

        public AppCompatSeekBar SlGoalAttained
            =>
                _slGoalAttained ??
                (_slGoalAttained = Activity.FindViewById<AppCompatSeekBar>(Resource.Id.SlGoalAttained));

        public AppCompatSeekBar SlGlobal
            => _slGlobal ?? (_slGlobal = Activity.FindViewById<AppCompatSeekBar>(Resource.Id.SlGlobal));


        private SessionViewModel Vm => App.Locator.SessionViewModel;

        protected override ViewModelBase GetViewModel() => Vm;

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            SlClassRoom.ProgressChanged +=
                (sender, args) => Vm.ScoredFeedback.Classroom = (sender as AppCompatSeekBar)?.Progress;
            SlContent.ProgressChanged +=
                (sender, args) => Vm.ScoredFeedback.Content = (sender as AppCompatSeekBar)?.Progress;
            SlConcepts.ProgressChanged +=
                (sender, args) => Vm.ScoredFeedback.ConceptsAndPractice = (sender as AppCompatSeekBar)?.Progress;
            SlMaterials.ProgressChanged +=
                (sender, args) => Vm.ScoredFeedback.CourseMaterials = (sender as AppCompatSeekBar)?.Progress;
            SlInDepth.ProgressChanged +=
                (sender, args) => Vm.ScoredFeedback.InDepth = (sender as AppCompatSeekBar)?.Progress;
            SlInstructorInteraction.ProgressChanged +=
                (sender, args) => Vm.ScoredFeedback.InstructorInteraction = (sender as AppCompatSeekBar)?.Progress;
            SlInstructorPresentation.ProgressChanged +=
                (sender, args) => Vm.ScoredFeedback.InstructorPresentation = (sender as AppCompatSeekBar)?.Progress;
            SlInstructorKnowledge.ProgressChanged +=
                (sender, args) => Vm.ScoredFeedback.InstructorKnowledge = (sender as AppCompatSeekBar)?.Progress;
            SlOrganization.ProgressChanged +=
                (sender, args) => Vm.ScoredFeedback.Organization = (sender as AppCompatSeekBar)?.Progress;
            SlWellPaced.ProgressChanged +=
                (sender, args) => Vm.ScoredFeedback.WellPaced = (sender as AppCompatSeekBar)?.Progress;
            SlWellStructured.ProgressChanged +=
                (sender, args) => Vm.ScoredFeedback.WellStructured = (sender as AppCompatSeekBar)?.Progress;
            SlGoalAttained.ProgressChanged +=
                (sender, args) => Vm.ScoredFeedback.GoalAttained = (sender as AppCompatSeekBar)?.Progress;
            SlGlobal.ProgressChanged +=
                (sender, args) => Vm.ScoredFeedback.Global = (sender as AppCompatSeekBar)?.Progress;

            SendFeedbackButton.SetCommand("Click", Vm.SendFeedbackCommand);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            => inflater.Inflate(Resource.Layout.FeedbackFragment, container, false);

        protected override IEnumerable<Binding> SetBindings() => new List<Binding>
        {
            App.Locator.SessionViewModel.SetBinding(() => App.Locator.SessionViewModel.Identity)
                .WhenSourceChanges(IdentityChanged)
        };

        private void IdentityChanged()
        {
            if (Vm.Identity == null)
            {
                FeedbackForm.Visibility = ViewStates.Gone;
                TvPleaseLogin.Visibility = ViewStates.Visible;
            }
            else
            {
                FeedbackForm.Visibility = ViewStates.Visible;
                TvPleaseLogin.Visibility = ViewStates.Gone;
            }
        }
    }
}