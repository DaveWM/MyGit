using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Octokit;

namespace MyGit.Templates
{
    class IssueHistoryTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Comment { get; set; }
        public DataTemplate Event { get; set; }
        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item.GetType() == typeof (IssueComment))
            {
                return Comment;
            }
            else if (item.GetType() == typeof (EventInfo))
            {
                return Event;
            }

            return base.SelectTemplateCore(item);
        }
    }
}
