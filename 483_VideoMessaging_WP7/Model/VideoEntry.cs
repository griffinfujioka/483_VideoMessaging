using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace _483_VideoMessaging_WP7.Model
{
    public class VideoEntry
    {
        public string Title { get; set; }
        public string VideoFileName { get; set; }       // name of the video file 
        public IsolatedStorageFileStream VideoFile { get; set; }
        public string ThumbnailFileName { get; set; }       // name of the image file 
        public WriteableBitmap ThumbnailImage { get; set; }
        public string DateTaken { get; set; }
        public string Description { get; set; }

        public VideoEntry(string title, string videofilename, IsolatedStorageFileStream videofile,
            string thumbnailfilename, WriteableBitmap thumbnailimage, string datetaken, string description)
        {
            this.Title = title;
            this.VideoFileName = videofilename;
            this.VideoFile = videofile;
            this.ThumbnailFileName = thumbnailfilename;
            this.ThumbnailImage = thumbnailimage;
            this.DateTaken = datetaken;
            this.Description = description; 
        }
    }
}
