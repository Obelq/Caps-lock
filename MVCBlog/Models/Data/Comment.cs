using System;
using System.ComponentModel.DataAnnotations;

namespace WebsiteForAds.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Date = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }
        
        [Required]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
        
        public DateTime Date { get; set; }

        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }
    }
}

