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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using _483_VideoMessaging_WP7.Model;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace _483_VideoMessaging_WP7
{
    public partial class App : Application
    {
        private static MainViewModel viewModel = null;
        public static ObservableCollection<VideoEntry> UsersVideos { get; set; }
        public static ObservableCollection<SerializedVideoEntry> SerializedUserVideos { get; set; }
        public static WriteableBitmap thumbnail; 
       
        public static int video_counter; 

        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static MainViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (viewModel == null)
                    viewModel = new MainViewModel();

                return viewModel;
            }
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        public void OpenAndReadFile()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            if (!storage.FileExists("SavedVideos.xml"))
                return; 

            var fileList = storage.GetFileNames(); 

            try
            {

                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!myIsolatedStorage.FileExists("SavedVideos.xml"))
                        return; 

                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("SavedVideos.xml", FileMode.Open))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<SerializedVideoEntry>));
                        List<SerializedVideoEntry> data = (List<SerializedVideoEntry>)serializer.Deserialize(stream);
                        App.SerializedUserVideos = new ObservableCollection<SerializedVideoEntry>(data); 
                        

                    }
                }

                foreach (SerializedVideoEntry video in App.SerializedUserVideos)
                {

                    IsolatedStorageFileStream videoFile = null; // new IsolatedStorageFileStream(video.VideoFileName, System.IO.FileMode.Open, videoFileISF);


                    //BitmapImage bitmap = new BitmapImage(new Uri(video.ThumbnailFileName, UriKind.Relative));
                    //bitmap.CreateOptions = BitmapCreateOptions.None;
                    //bitmap.ImageOpened += (s, e) =>
                    //{
                    //    WriteableBitmap wbm = new WriteableBitmap((BitmapImage)s);
                    //    thumbnail = new WriteableBitmap((BitmapImage)s);
                    //};

                    //Uri uri = new Uri("Image.jpg", UriKind.Relative);
                    //StreamResourceInfo resourceInfo = Application.GetResourceStream(uri);
                    //BitmapImage img = new BitmapImage();
                    //img.SetSource(resourceInfo.Stream);
                    //WriteableBitmap wbm = new WriteableBitmap(img);

                    BitmapImage bi = new BitmapImage();
                    using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("Image.jpg", FileMode.Open, FileAccess.Read))
                        {
                            bi.SetSource(fileStream);
                        }
                    }



                    var newVideoEntry = new VideoEntry(video.Title, video.VideoFileName, videoFile, video.ThumbnailFileName, null, bi, video.DateTaken, video.Description);

                    App.UsersVideos.Add(newVideoEntry);
                }


            }
            catch(Exception ex)
            {
                //add some code here
            }


        }

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            video_counter = 1; 
            UsersVideos = new ObservableCollection<VideoEntry>();
            SerializedUserVideos = new ObservableCollection<SerializedVideoEntry>(); 
            OpenAndReadFile();

            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            // Ensure that application state is restored appropriately
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            // Ensure that required application state is persisted here.

            // Write to the Isolated Storage
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;

            foreach (VideoEntry video in App.UsersVideos)
            {
                var newSerializedVideoEntry = new SerializedVideoEntry(video);
                SerializedUserVideos.Add(newSerializedVideoEntry); 
            }

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("SavedVideos.xml", FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<SerializedVideoEntry>));
                    using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                    {

                        serializer.Serialize(xmlWriter, SerializedUserVideos); 
                    }
                }
            }
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}