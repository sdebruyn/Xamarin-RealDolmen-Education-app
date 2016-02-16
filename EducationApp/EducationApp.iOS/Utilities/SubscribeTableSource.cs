using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CoreGraphics;
using EducationApp.Models;
using EducationApp.Services;
using EducationApp.ViewModels;
using Foundation;
using GalaSoft.MvvmLight.Helpers;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace EducationApp.iOS.Utilities
{
    /// <remarks>
    ///     Section 0: company info input fields
    ///     Section 1: particpants
    ///     Section 2: participant info input fields
    /// </remarks>
    internal class SubscribeTableSource : UITableViewSource
    {
        private const string ParticipantCellReuseIdentifier = "participant";
        private const int AmountOfSections = 3;

        private static readonly ILocalizedStringProvider LocalizedStringProvider =
            ServiceLocator.Current.GetInstance<ILocalizedStringProvider>();

        private static readonly IList<Tuple<string, Expression<Func<string>>>> CompanyInputCellDefinitions = new List
            <Tuple<string, Expression<Func<string>>>>
        {
            new Tuple<string, Expression<Func<string>>>(
                LocalizedStringProvider.GetLocalizedString(Localized.FirstName_Text),
                () => Application.Locator.SessionViewModel.Subscription.FirstName),
            new Tuple<string, Expression<Func<string>>>(
                LocalizedStringProvider.GetLocalizedString(Localized.LastName_Text),
                () => Application.Locator.SessionViewModel.Subscription.LastName),
            new Tuple<string, Expression<Func<string>>>(
                LocalizedStringProvider.GetLocalizedString(Localized.JobTitle_Text),
                () => Application.Locator.SessionViewModel.Subscription.JobTitle),
            new Tuple<string, Expression<Func<string>>>(
                LocalizedStringProvider.GetLocalizedString(Localized.Company_Text),
                () => Application.Locator.SessionViewModel.Subscription.Company),
            new Tuple<string, Expression<Func<string>>>(
                LocalizedStringProvider.GetLocalizedString(Localized.Street_Text),
                () => Application.Locator.SessionViewModel.Subscription.Street),
            new Tuple<string, Expression<Func<string>>>(
                LocalizedStringProvider.GetLocalizedString(Localized.Number_Text),
                () => Application.Locator.SessionViewModel.Subscription.Number),
            new Tuple<string, Expression<Func<string>>>(
                LocalizedStringProvider.GetLocalizedString(Localized.City_Text),
                () => Application.Locator.SessionViewModel.Subscription.City),
            new Tuple<string, Expression<Func<string>>>(LocalizedStringProvider.GetLocalizedString(Localized.ZIP_Text),
                () => Application.Locator.SessionViewModel.Subscription.ZIPCode),
            new Tuple<string, Expression<Func<string>>>(
                LocalizedStringProvider.GetLocalizedString(Localized.Country_Text),
                () => Application.Locator.SessionViewModel.Subscription.Country),
            new Tuple<string, Expression<Func<string>>>(
                LocalizedStringProvider.GetLocalizedString(Localized.Email_Text),
                () => Application.Locator.SessionViewModel.Subscription.Email),
            new Tuple<string, Expression<Func<string>>>(
                LocalizedStringProvider.GetLocalizedString(Localized.Phone_Text),
                () => Application.Locator.SessionViewModel.Subscription.Phone),
            new Tuple<string, Expression<Func<string>>>(LocalizedStringProvider.GetLocalizedString(Localized.Fax_Text),
                () => Application.Locator.SessionViewModel.Subscription.Fax),
            new Tuple<string, Expression<Func<string>>>(LocalizedStringProvider.GetLocalizedString(Localized.VAT_Text),
                () => Application.Locator.SessionViewModel.Subscription.VAT),
            new Tuple<string, Expression<Func<string>>>(
                LocalizedStringProvider.GetLocalizedString(Localized.Remarks_Text),
                () => Application.Locator.SessionViewModel.Subscription.Remarks)
        };

        private static readonly IList<Tuple<string, Expression<Func<string>>>> ParticipantInputCellDefinitions = new List
            <Tuple<string, Expression<Func<string>>>>
        {
            new Tuple<string, Expression<Func<string>>>(
                LocalizedStringProvider.GetLocalizedString(Localized.FirstName_Text),
                () => Application.Locator.SessionViewModel.ParticipantToAdd.FirstName),
            new Tuple<string, Expression<Func<string>>>(
                LocalizedStringProvider.GetLocalizedString(Localized.LastName_Text),
                () => Application.Locator.SessionViewModel.ParticipantToAdd.LastName),
            new Tuple<string, Expression<Func<string>>>(
                LocalizedStringProvider.GetLocalizedString(Localized.Email_Text),
                () => Application.Locator.SessionViewModel.ParticipantToAdd.Email)
        };

        private readonly IList<Binding> _bindings;
        private readonly IDictionary<string, UITableViewCell> _tableCells;

        private readonly UIGestureRecognizer _tapGestureRecognizer;

        public SubscribeTableSource(UIView view)
        {
            _tableCells = new Dictionary<string, UITableViewCell>();
            _bindings = new List<Binding>();

            _tapGestureRecognizer = new UIGestureRecognizer();
            _tapGestureRecognizer.AddTarget(() => view.EndEditing(true));
        }

        private SessionViewModel Vm => Application.Locator.SessionViewModel;

        private UITableViewCell ConstructBindingCell(string description, Expression<Func<string>> propertyExpression)
        {
            var cell = new UITableViewCell(UITableViewCellStyle.Value1, "notreusable");

            cell.TextLabel.Text = description;
            cell.TextLabel.Font = UIFont.BoldSystemFontOfSize(14);
            cell.Accessory = UITableViewCellAccessory.None;
            cell.DetailTextLabel.Hidden = true;

            var textField = new UITextField(new CGRect(110, 10, 185, 30))
            {
                AdjustsFontSizeToFitWidth = true,
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextAlignment = UITextAlignment.Right
            };

            cell.ContentView.AddSubview(textField);
            _bindings.Add(textField.SetBinding(() => textField.Text, Application.Locator.SessionViewModel,
                propertyExpression, BindingMode.TwoWay)
                .UpdateSourceTrigger("EditingDidEnd")
                .UpdateTargetTrigger(UpdateTriggerMode.PropertyChanged));

            return cell;
        }

        private UITableViewCell ConstructParticipantCell(Participant participant, UITableView tableView)
        {
            var cell = tableView.DequeueReusableCell(ParticipantCellReuseIdentifier) ??
                       new UITableViewCell(UITableViewCellStyle.Subtitle, ParticipantCellReuseIdentifier);
            cell.TextLabel.Text = $"{participant.FirstName} {participant.LastName}";
            cell.DetailTextLabel.Text = participant.Email;
            var gestureRecognizer =
                new UILongPressGestureRecognizer(() => Vm.Subscription.Participants.Remove(participant));
            cell.AddGestureRecognizer(gestureRecognizer);
            return cell;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var key = $"{indexPath.Section}-{indexPath.Row}";
            if (!_tableCells.ContainsKey(key))
            {
                Tuple<string, Expression<Func<string>>> definition;
                switch (indexPath.Section)
                {
                    case 0:
                        definition = CompanyInputCellDefinitions[indexPath.Row];
                        _tableCells[key] = ConstructBindingCell(definition.Item1, definition.Item2);
                        break;
                    case 1:
                        _tableCells[key] =
                            ConstructParticipantCell(
                                Vm.Subscription.Participants[indexPath.Row], tableView);
                        break;
                    case 2:
                        definition = ParticipantInputCellDefinitions[indexPath.Row];
                        _tableCells[key] = ConstructBindingCell(definition.Item1, definition.Item2);
                        break;
                }
            }
            var cell = _tableCells[key];
            cell.AddGestureRecognizer(_tapGestureRecognizer);
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            switch (section)
            {
                case 0:
                    return CompanyInputCellDefinitions.Count;
                case 1:
                    return Vm.Subscription.Participants.Count;
                case 2:
                    return ParticipantInputCellDefinitions.Count;
                default:
                    return 0;
            }
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            switch (section)
            {
                case 0:
                    return LocalizedStringProvider.GetLocalizedString(Localized.CompanyInfo);
                case 1:
                    return LocalizedStringProvider.GetLocalizedString(Localized.Participants_Text);
                case 2:
                    return LocalizedStringProvider.GetLocalizedString(Localized.AddParticipant_Content);
                default:
                    return base.TitleForHeader(tableView, section);
            }
        }

        public override nint NumberOfSections(UITableView tableView) => AmountOfSections;
    }
}