using System;

namespace Upsaleslab.Templates.App.Models
{
    public class Preview
    {
        public PreviewSize Video { get; private set; }

        public PreviewSize Image { get; private set; }

        public Preview(PreviewSize video, PreviewSize image)
        {
            Video = video;
            Image = image;
        }
    }

    public class PreviewSize
    {
        public Uri Full { get; private set; }

        public Uri Thumbnail { get; private set; }

        public PreviewSize(Uri full, Uri thumbnail)
        {
            Full = full;
            Thumbnail = thumbnail;
        }
    }
}