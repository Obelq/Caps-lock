using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WebsiteForAds.Models
{
    public class Image
    {
        public Image()
        {
            ImageList = new List<string>();
        }
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public List<string> ImageList { get; set; }
    }
}