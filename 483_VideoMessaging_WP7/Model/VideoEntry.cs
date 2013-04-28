using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO; 
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace _483_VideoMessaging_WP7.Model
{
    [XmlRoot("entry")]
    public class VideoEntry
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("videofilename")]
        public string VideoFileName { get; set; }       // name of the video file 

        [XmlElement("videofile")]
        public IsolatedStorageFileStream VideoFile { get; set; }

        [XmlElement("thumbnailfilename")]
        public string ThumbnailFileName { get; set; }       // name of the image file 

        [XmlElement("thumbnailimage")]
        public WriteableBitmap ThumbnailImage { get; set; }

        [XmlElement("thumbnailbitmap")]
        public BitmapImage ThumbnailBitmap { get; set; }

        [XmlElement("DateTaken")]
        public string DateTaken { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        public VideoEntry(string title, string videofilename, IsolatedStorageFileStream videofile,
            string thumbnailfilename, WriteableBitmap thumbnailimage , BitmapImage thumbnailbitmap,string datetaken, string description)
        {
            this.Title = title;
            this.VideoFileName = videofilename;
            this.VideoFile = videofile;
            this.ThumbnailFileName = thumbnailfilename;
            this.ThumbnailImage = thumbnailimage;
            this.ThumbnailBitmap = thumbnailbitmap; 
            this.DateTaken = datetaken;
            this.Description = description; 
        }

        public VideoEntry()
        {
            Title = "";
            VideoFileName = "";
            ThumbnailFileName = "";
            DateTaken = "";
            Description = ""; 
        }

        public List<VideoEntry> ConvertSerializedVideos(List<SerializedVideoEntry> SerializedVideos)
        {
            List<VideoEntry> list = new List<VideoEntry>();
            foreach (SerializedVideoEntry video in SerializedVideos)
            {
                IsolatedStorageFile videoFileISF = null; 
                IsolatedStorageFile imageFile;
                WriteableBitmap thumbnail;
                IsolatedStorageFileStream videoFile = null; // new IsolatedStorageFileStream(video.VideoFileName, System.IO.FileMode.Open, videoFileISF);

                BitmapImage bitmap = new BitmapImage(new Uri(video.ThumbnailFileName, UriKind.Relative));
                thumbnail = new WriteableBitmap(bitmap);

                var newVideoEntry = new VideoEntry(video.Title, video.VideoFileName, videoFile, video.ThumbnailFileName, thumbnail, bitmap, video.DateTaken, video.Description);

                list.Add(newVideoEntry); 
            }

            return list; 
        }
    }
}
