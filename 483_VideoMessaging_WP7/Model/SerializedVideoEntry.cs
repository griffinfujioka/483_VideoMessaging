using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace _483_VideoMessaging_WP7.Model
{
    [XmlRoot("entry")]
    public class SerializedVideoEntry
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("videofilename")]
        public string VideoFileName { get; set; }       // name of the video file 

        [XmlElement("thumbnailfilename")]
        public string ThumbnailFileName { get; set; }       // name of the image file 

        [XmlElement("DateTaken")]
        public string DateTaken { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        public SerializedVideoEntry(string title, string videofilename,
            string thumbnailfilename, string datetaken, string description)
        {
            this.Title = title;
            this.VideoFileName = videofilename;
            this.ThumbnailFileName = thumbnailfilename;
            this.DateTaken = datetaken;
            this.Description = description; 
        }

        public SerializedVideoEntry()
        {
            Title = "";
            VideoFileName = "";
            ThumbnailFileName = "";
            DateTaken = "";
            Description = ""; 
        }

        public SerializedVideoEntry(VideoEntry video)
        {
            this.Title = video.Title;
            this.VideoFileName = video.VideoFileName;
            this.ThumbnailFileName = video.ThumbnailFileName;
            this.DateTaken = video.DateTaken;
            this.Description = video.Description; 
            
        }
    }
}
