using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Octokit;

namespace MyGit.Templates
{
    public class ActivityPayloadTemplateSelector : DataTemplateSelector
    {
        public string Type { get; set; }
        public static DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(string), typeof(Activity), new PropertyMetadata(""));
        public DataTemplate Default { get; set; }
        public DataTemplate Issue { get; set; }
        public DataTemplate IssueComment { get; set; }
        public DataTemplate Push { get; set; }
        public DataTemplate Watch { get; set; }
        public DataTemplate Fork { get; set; }
        public DataTemplate PullRequest {get;set;}
        public DataTemplate PullRequestComment { get; set; }
        protected override DataTemplate SelectTemplateCore(object item)
        {
            var activity = item as Activity;
            if (activity == null)
            {
                return base.SelectTemplateCore(item);
            }
            switch (activity.Type)
            {
                case "IssuesEvent":
                    return Issue;
                case "IssueCommentEvent":
                    return IssueComment;
                case "PushEvent":
                    return Push;
                case "WatchEvent":
                    return Watch;
                case "ForkEvent":
                    return Fork;
                case "PullRequestEvent":
                    return PullRequest;
                case "PullRequestReviewCommentEvent":
                    return PullRequestComment;
                default:
                    return Default;
            }
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplateCore(item);
        }
    }
}
