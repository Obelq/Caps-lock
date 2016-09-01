using System;
using System.Linq.Expressions;

namespace WebsiteForAds.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public string Role { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string Picture { get; set; }

        public DateTime Date { get; set; }

        public string AuthorId { get; set; }

        public static Expression<Func<Post, PostViewModel>> ViewModel
        {
            get
            {
                return p => new PostViewModel()
                {
                    Id = p.Id,
                    Body = p.Body,
                    Title = p.Title,
                    Date = p.Date,
                    AuthorId = p.AuthorId,
                    Picture = p.Picture
                };
            }
        }
    }
}