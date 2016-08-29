using System;
using System.Linq.Expressions;

namespace WebsiteForAds.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDateTime { get; set; }

        public TimeSpan? Duration { get; set; }

        public string Author { get; set; }

        public string Location { get; set; }

        public static Expression<Func<Post, PostViewModel>> ViewModel
        {
            get
            {
                return p => new PostViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    StartDateTime = p.Date,
                    Duration = p.Duration,
                    Location = p.Location,
                    Author = p.Author.FullName
                };
            }
        }
    }
}