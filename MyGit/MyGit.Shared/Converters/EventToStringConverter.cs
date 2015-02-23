using System;
using Windows.UI.Xaml.Data;
using Octokit;

namespace MyGit.Converters
{
    class EventToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var evt = value as EventInfo;
            var user = evt.Actor == null ? "Unknown user" : evt.Actor.Login;

            string actionString;

            switch (evt.Event)
            {
                case EventInfoState.Assigned:
                    actionString = "was assigned this issue";
                    break;
                case EventInfoState.Closed:
                    actionString = "closed this issue";
                    break;
                case EventInfoState.Demilestoned:
                    actionString = "removed this issue from a milestone";
                    break;
                case EventInfoState.HeadRefDeleted:
                    actionString = "deleted the branch for this PR";
                    break;
                case EventInfoState.HeadRefRestored:
                    actionString = "restored the branch for this PR";
                    break;
                case EventInfoState.Labeled:
                    actionString = string.Format("added the label '{0}'", evt.Label.Name);
                    break;
                case EventInfoState.Locked:
                    actionString = "locked this issue";
                    break;
                case EventInfoState.Mentioned:
                    actionString = "was mentioned";
                    break;
                case EventInfoState.Merged:
                    actionString = "merged this PR";
                    break;
                case EventInfoState.Milestoned:
                    actionString = "added this issue to a milestone";
                    break;
                case EventInfoState.Referenced:
                    actionString = "referenced in a commit";
                    break;
                case EventInfoState.Renamed:
                    actionString = "renamed this issue";
                    break;
                case EventInfoState.Reopened:
                    actionString = "re-opened this issue";
                    break;
                case EventInfoState.Subscribed:
                    actionString = "subscribed to this issue";
                    break;
                case EventInfoState.Unassigned:
                    actionString = "was unassigned from this issue";
                    break;
                case EventInfoState.Unlabeled:
                    actionString = string.Format("removed the label '{0}'", evt.Label);
                    break;
                case EventInfoState.Unlocked:
                    actionString = "unlocked this issue";
                    break;
                default:
                    actionString = "did something";
                    break;
            }

            return string.Format("{0} {1}", user, actionString);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
