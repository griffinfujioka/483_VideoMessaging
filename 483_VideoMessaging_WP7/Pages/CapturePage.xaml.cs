using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.IO.IsolatedStorage;
using Microsoft.Devices;
using System.IO;

namespace _483_VideoMessaging_WP7.Pages
{
    public partial class CapturePage : PhoneApplicationPage
    {
        private VideoBrush videoRecorderBrush;  // Viewfinder for capturing video

        // Source and device for capturing video.
        private CaptureSource captureSource;
        private VideoCaptureDevice videoCaptureDevice;
        private PhotoCamera photo_camera; 

        // File details for storing the recording.   
        private IsolatedStorageFileStream isoVideoFile;
        private FileSink fileSink;
        private string isoVideoFileName = "CameraMovie.mp4";

        public CapturePage()
        {
            InitializeComponent();

            captureSource = new CaptureSource();
            fileSink = new FileSink();
            



        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            videoCaptureDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();
           
          
            // Add eventhandlers for captureSource
            //// Initialize the camera if it exists on the device.
            if (videoCaptureDevice != null)
            {
                // Create the VideoBrush for the viewfinder.
                videoRecorderBrush = new VideoBrush();

                videoRecorderBrush.SetSource(captureSource);
                videoRecorderBrush.RelativeTransform
    = new CompositeTransform() { CenterX = 0.5, CenterY = 0.5, Rotation = 90 };

                // Display the viewfinder image on the rectangle.
                viewfinderRectangle.Fill = videoRecorderBrush;
                

                // Start video capture and display it on the viewfinder.
                captureSource.Start();
            }
            else
            {
                // Disable buttons when the camera is not supported by the device.
            }

            // Start the recording 
            // Connect fileSink to captureSource.
            if (captureSource.VideoCaptureDevice != null
                && captureSource.State == CaptureState.Started)
            {
                captureSource.Stop();
                // Connect the input and output of fileSink.
                fileSink.CaptureSource = captureSource;
                fileSink.IsolatedStorageFileName = isoVideoFileName;
               
            }

            // Begin recording.
            if (captureSource.VideoCaptureDevice != null
                && captureSource.State == CaptureState.Stopped)
            {
                captureSource.Start();
            }


            base.OnNavigatedTo(e);
        }

        private void okBtn_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Stop recording.
                if (captureSource.VideoCaptureDevice != null
                && captureSource.State == CaptureState.Started)
                {
                    captureSource.Stop();

                    // Disconnect fileSink.
                    fileSink.CaptureSource = null;
                    fileSink.IsolatedStorageFileName = null;


                    StartVideoPreview();
                }
            }
            // If stop fails, display an error.
            catch (Exception ex)
            {
      
            }
            //NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative)); 
        }

        // Set the recording state: display the video on the viewfinder.
        private void StartVideoPreview()
        {
            try
            {
                // Display the video on the viewfinder.
                if (captureSource.VideoCaptureDevice != null
                && captureSource.State == CaptureState.Stopped)
                {
                    // Add captureSource to videoBrush.
                    videoRecorderBrush.SetSource(captureSource);

                    // Add videoBrush to the visual tree.
                    viewfinderRectangle.Fill = videoRecorderBrush;

                    captureSource.Start();

                   
                }
            }
            // If preview fails, display an error.
            catch (Exception e)
            {
              
            }
        }

        private void playBtn_Click_1(object sender, EventArgs e)
        {
            
            // Start video playback when the file stream exists.
            if (isoVideoFile != null)
            {
                VideoPlayer.Play();
            }
            // Start the video for the first time.
            else
            {
                // Stop the capture source.
                captureSource.Stop();

                // Remove VideoBrush from the tree.
                viewfinderRectangle.Fill = null;

                // Create the file stream and attach it to the MediaElement.
                isoVideoFile = new IsolatedStorageFileStream(isoVideoFileName,
                                        FileMode.Open, FileAccess.Read,
                                        IsolatedStorageFile.GetUserStoreForApplication());

                VideoPlayer.SetSource(isoVideoFile);

                // Add an event handler for the end of playback.
                VideoPlayer.MediaEnded += new RoutedEventHandler(VideoPlayerMediaEnded);

                // Start video playback.
                VideoPlayer.Play();
            }
        }

        // Display the viewfinder when playback ends.
        public void VideoPlayerMediaEnded(object sender, RoutedEventArgs e)
        {
            // Remove the playback objects.
            DisposeVideoPlayer();

            StartVideoPreview();
        }

        private void DisposeVideoPlayer()
        {
            if (VideoPlayer != null)
            {
                // Stop the VideoPlayer MediaElement.
                VideoPlayer.Stop();

                // Remove playback objects.
                VideoPlayer.Source = null;
                isoVideoFile = null;

                // Remove the event handler.
                VideoPlayer.MediaEnded -= VideoPlayerMediaEnded;
            }
        }

    }
}