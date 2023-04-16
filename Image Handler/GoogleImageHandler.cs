using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CustomProject.Image_Handler
{
    public class GoogleImageHandler
    {
        public Dictionary<int, string> images = new Dictionary<int, string>();
        public static readonly GoogleImageHandler Instance = new GoogleImageHandler();

        static GoogleImageHandler() { }

        public string GetImageAtId(int id)
        {
            var imageURL = images.TryGetValue(id, out var image);
            return image;
        }
    }
}
