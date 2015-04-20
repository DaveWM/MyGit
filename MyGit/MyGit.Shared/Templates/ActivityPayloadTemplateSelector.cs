using System.Diagnostics;
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
        public DataTemplate IssueComment { get; set; }
        protected override DataTemplate SelectTemplateCore(object item)
        {
            var activity = item as Activity;
            if (activity == null)
            {
                return base.SelectTemplateCore(item);
            }
            switch (activity.Type)
            {
                case "IssueCommentEvent":
                    return IssueComment;
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
