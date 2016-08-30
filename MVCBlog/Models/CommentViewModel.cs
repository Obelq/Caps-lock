namespace WebsiteForAds.Models
{
    using System;
    using System.Linq.Expressions;
    public class CommentViewModel
    {
        public int Id { get; set; }
   
        public string Content { get; set; }
   
        public string Username { get; set; }

        public static Expression<Func<Comment, CommentViewModel>> ViewModel
        {
            get
            {
                return c => new CommentViewModel()
                {
                    Content = c.Body,
                    Username = c.Author.ToString()
                };
            }
        }
    }
}
