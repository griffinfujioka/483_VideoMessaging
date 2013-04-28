using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.IO.IsolatedStorage;
using Microsoft.Devices;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Media;
using _483_VideoMessaging_WP7.Model;
using ExifLib;


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
        private WriteableBitmap thumbnail;
        private FileSink fileSink;
        private string isoVideoFileName = "CameraMovie.mp4";
        private string isoImageFileName;

        ExifLib.ExifOrientation _orientation;
        int _width;
        int _height;
        int _angle;
        Stream capturedImage;


        public CapturePage()
        {
            InitializeComponent();

            captureSource = new CaptureSource();
            fileSink = new FileSink();




        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            captureSource.Stop();
            isoVideoFile.Close(); 

            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
         
            videoCaptureDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();

            statusTxtBlock.Text = "Recording...";


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

                    statusTxtBlock.Text = "Click Play button to preview";
                    okBtn.IsEnabled = false;


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

            statusTxtBlock.Text = "Playing video...";
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
                statusTxtBlock.Text = "Click to play";
            }

        }

        // Display the viewfinder when playback ends.
        public void VideoPlayerMediaEnded(object sender, RoutedEventArgs e)
        {
            // Remove the playback objects.
            DisposeVideoPlayer();

            StartVideoPreview();
        }


        public void CaptureImageCompleted(object sender, CaptureImageCompletedEventArgs e)
        {
            // Figure out the orientation from the EXIF data 
            //e.Result.Position = 0;
            //JpegInfo info = ExifReader.ReadJpeg(isoVideoFile, isoImageFileName);

            //_width = info.Width;
            //_height = info.Height;
            //_orientation = info.Orientation;

            //switch (info.Orientation)
            //{
            //    case ExifOrientation.TopLeft:
            //    case ExifOrientation.Undefined:
            //        _angle = 0;
            //        break;
            //    case ExifOrientation.TopRight:
            //        _angle = 90;
            //        break;
            //    case ExifOrientation.BottomRight:
            //        _angle = 180;
            //        break;
            //    case ExifOrientation.BottomLeft:
            //        _angle = 270;
            //        break;
            //}
            //if (_angle > 0d)
            //{
            //    capturedImage = RotateStream(isoVideoFile, _angle);
            //}
            //else
            //{
            //    capturedImage = isoVideoFile;
            //}

            //// Give the thumbnail image a unique name 
            //isoImageFileName = DateTime.Now.Date + "_image.jpg";

            //using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            //{
            //    IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(isoImageFileName);
            //    BitmapImage bitmap = new BitmapImage();
            //    bitmap.SetSource(capturedImage);

            //    WriteableBitmap wb = new WriteableBitmap(bitmap);
            //    wb.SaveJpeg(fileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);

            //    fileStream.Close();
            //}

            var thumbnail = e.Result;
            var myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication(); 
            // Give the thumbnail image a unique name 
            //isoImageFileName = DateTime.Now.Date + "_image.jpg";
            var filename = titleTxtBox.Text + "_image.jpg";
            IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(filename);
            // Encode WriteableBitmap object to a JPEG stream.
            Extensions.SaveJpeg(thumbnail, fileStream, thumbnail.PixelWidth, thumbnail.PixelHeight, 90, 85);
            BitmapImage bmp = new BitmapImage();
            using (MemoryStream ms = new MemoryStream())
            {
                thumbnail.SaveJpeg(ms, thumbnail.PixelWidth, thumbnail.PixelHeight, 0, 100);
                
                bmp.SetSource(ms);

            }

            

            fileStream.Close();

            var title = titleTxtBox.Text;
            var description = descriptionTxtBox.Text;


            var video = new VideoEntry(title, isoVideoFileName, isoVideoFile, filename, thumbnail, bmp, DateTime.Now.Date.ToShortDateString(), description);

            App.UsersVideos.Add(video);

        }

        private Stream RotateStream(Stream stream, int angle)
        {
            stream.Position = 0;
            if (angle % 90 != 0 || angle < 0) throw new ArgumentException();
            if (angle % 360 == 0) return stream;

            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(stream);
            WriteableBitmap wbSource = new WriteableBitmap(bitmap);

            WriteableBitmap wbTarget = null;
            if (angle % 180 == 0)
            {
                wbTarget = new WriteableBitmap(wbSource.PixelWidth, wbSource.PixelHeight);
            }
            else
            {
                wbTarget = new WriteableBitmap(wbSource.PixelHeight, wbSource.PixelWidth);
            }

            for (int x = 0; x < wbSource.PixelWidth; x++)
            {
                for (int y = 0; y < wbSource.PixelHeight; y++)
                {
                    switch (angle % 360)
                    {
                        case 90:
                            wbTarget.Pixels[(wbSource.PixelHeight - y - 1) + x * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 180:
                            wbTarget.Pixels[(wbSource.PixelWidth - x - 1) + (wbSource.PixelHeight - y - 1) * wbSource.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 270:
                            wbTarget.Pixels[y + (wbSource.PixelWidth - x - 1) * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                    }
                }
            }
            MemoryStream targetStream = new MemoryStream();
            wbTarget.SaveJpeg(targetStream, wbTarget.PixelWidth, wbTarget.PixelHeight, 0, 100);
            return targetStream;
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

        private void saveBtn_Click_1(object sender, EventArgs e)
        {
            // Get a thumbnail image from the video
            captureSource.CaptureImageAsync();
            captureSource.CaptureImageCompleted += new EventHandler<CaptureImageCompletedEventArgs>(CaptureImageCompleted);

            // Stop the capture source.
            captureSource.Stop();


            // Create the file stream and attach it to the MediaElement.

            isoVideoFile = new IsolatedStorageFileStream(isoVideoFileName,
                                    FileMode.Open, FileAccess.Read,
                                    IsolatedStorageFile.GetUserStoreForApplication());

            if (NavigationService.CanGoBack)
                NavigationService.GoBack();

        }



    }
}