namespace WebsiteForAds.Models
{
    using System;
    using System.Linq.Expressions;
    public class CommentViewModel
    {
        public int Id { get; set; }
   
        public string Body { get; set; }
   
        public string Username { get; set; }

        public static Expression<Func<Comment, CommentViewModel>> ViewModel
        {
            get
            {
                return c => new CommentViewModel()
                {
                    Body = c.Body,
                    Username = c.Author.UserName
                };
            }
        }
    }
}
