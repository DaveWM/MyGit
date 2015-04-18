using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ObjectBuilder2;
using MyGit.ViewModels.MainPage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyGit.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void RepoChoiceBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            // this is completely retarded, but apparently comboboxes don't update their bindings until they're opened
            // well done xaml
            var combo = sender as ComboBox;
            combo.IsDropDownOpen = true;
            combo.IsDropDownOpen = false;
        }

        private void IssueTypeChoiceBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            var combo = sender as ComboBox;
            combo.IsDropDownOpen = true;
            combo.IsDropDownOpen = false;
        }
    }
}
