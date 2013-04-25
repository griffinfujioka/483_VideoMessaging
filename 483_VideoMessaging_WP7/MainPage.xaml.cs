using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using System.Windows.Navigation;

namespace _483_VideoMessaging_WP7
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            SecondListBox.ItemsSource = App.UsersVideos;

            //var bounds = Application.Current.RootVisual.RenderSize;
            //viewfinderRectangle.Width = bounds.Width;
            //viewfinderRectangle.Height = bounds.Height;
        }

        private void captureBtn_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/CapturePage.xaml", UriKind.Relative)); 

            


        }

        private void FirstListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            switch (FirstListBox.SelectedIndex)
            {
                // Record a new video 
                case 0:                         
                    NavigationService.Navigate(new Uri("/Pages/CapturePage.xaml", UriKind.Relative)); 
                    break; 
                // Share a video 
                case 1:
                    break; 
                default: break; 
            }
        }
    }
}