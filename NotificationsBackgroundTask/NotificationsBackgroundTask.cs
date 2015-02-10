using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using Octokit;

namespace NotificationsBackgroundTask
{
    public sealed class NotificationsBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            var settings = ApplicationData.Current.LocalSettings;
            var token = settings.Values["token"] as string;
            if (!settings.Values.ContainsKey("notificationsCount"))
            {
                settings.Values["notificationsCount"] = 0;
            }
            if (token != null)
            {
                var client = new GitHubClient(new ProductHeaderValue("MyGit"));
                client.Credentials = new Credentials(token);

                var notifications = await client.Notification.GetAllForCurrent(new NotificationsRequest
                {
                    All = false
                });

                var badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeGlyph);
                var badge = badgeXml.SelectSingleNode("/badge") as XmlElement;
                badge.SetAttribute("value", notifications.Count.ToString());
                BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(new BadgeNotification(badgeXml));

                if (notifications.Count <= (int)settings.Values["notificationsCount"])
                {
                    settings.Values["notificationsCount"] = notifications.Count;
                    deferral.Complete();
                    return;
                }

                var template = ToastTemplateType.ToastImageAndText01;
                var xml = ToastNotificationManager.GetTemplateContent(template);
                var toastTextElements = xml.GetElementsByTagName("text");
                toastTextElements[0].AppendChild(xml.CreateTextNode(String.Format("{0} GitHub Notifications", notifications.Count)));

                XmlElement audioNode = xml.CreateElement("audio");
                audioNode.SetAttribute("silent", "true");
                xml.SelectSingleNode("/toast").AppendChild(audioNode);

                var toast = new ToastNotification(xml);
                ToastNotificationManager.CreateToastNotifier().Show(toast);

                settings.Values["notificationsCount"] = notifications.Count;
            }
            deferral.Complete();
        }
    }
}
