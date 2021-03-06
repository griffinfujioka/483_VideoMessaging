﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using _483_VideoMessaging.Resources;
using Microsoft.Devices;
using System.Windows.Media;
using System.IO.IsolatedStorage;        // For camera


namespace _483_VideoMessaging
{
    public partial class MainPage : PhoneApplicationPage
    {
        private VideoBrush videoRecorderBrush;  // Viewfinder for capturing video
        // Source and device for capturing video.
        private CaptureSource captureSource;
        private VideoCaptureDevice videoCaptureDevice;
        // File details for storing the recording.   
        private IsolatedStorageFileStream isoVideoFile;
        private FileSink fileSink;
        private string isoVideoFileName = "CameraMovie.mp4";

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            
            captureSource = new CaptureSource();
            fileSink = new FileSink(); 

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

            //var bounds = Application.Current.RootVisual.RenderSize;
            //viewfinderRectangle.Width = bounds.Width;
            //viewfinderRectangle.Height = bounds.Height;
        }

        private void captureBtn_Click_1(object sender, EventArgs e)
        {
            videoCaptureDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();
            
            // Initialize the camera if it exists on the device.
            if (videoCaptureDevice != null)
            {
                // Create the VideoBrush for the viewfinder.
                videoRecorderBrush = new VideoBrush();
                videoRecorderBrush.SetSource(captureSource);
                // Display the viewfinder image on the rectangle.
                viewfinderRectangle.Fill = videoRecorderBrush;
                // Start video capture and display it on the viewfinder.
                captureSource.Start();
            }
            else
            {
                // Disable buttons when the camera is not supported by the device.
            }

            // Connect fileSink to captureSource.
            if (captureSource.VideoCaptureDevice != null
                && captureSource.State == CaptureState.Started)
            {
                captureSource.Stop();
                // Connect the input and output of fileSink.
                fileSink.CaptureSource = captureSource;
                fileSink.IsolatedStorageFileName = isoVideoFileName;
            }

            captureSource.Start();


        }


        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}