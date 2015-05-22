using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyGit.Behaviours
{
    public class WebViewBehaviour : DependencyObject
    {
        public static DependencyProperty Html = DependencyProperty.RegisterAttached("Html", typeof(string), typeof(WebViewBehaviour), new PropertyMetadata(String.Empty,
            (w, e) =>
            {
                var webview = w as WebView;
                webview.NavigateToString((e.NewValue as string) ?? String.Empty);
            }));

        public static string GetHtml(WebView webView)
        {
            return webView.InvokeScriptAsync("eval", new string[] { "document.documentElement.outerHTML;" }).GetResults();
        }

        public static void SetHtml(WebView webView, string html)
        {
            webView.NavigateToString(html);
        }
    }
}
