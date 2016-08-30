using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebsiteForAds.Models
{
    public class Post
    {
        public Post()
        {
            this.IsPublic = true;
            this.Date = DateTime.Now;
            this.Comments = new HashSet<Comment>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Заглваие:")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Съдържание:")]
        public string Body { get; set; }

        public TimeSpan? Duration { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public ApplicationUser Author { get; set; }

        public string AuthorId { get; set; }

        public string Location { get; set; }

        public bool IsPublic { get; set; }

        //[Required]
        [Display(Name = "Снимка:")]
        public byte[] Picture { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}