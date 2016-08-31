using WebsiteForAds.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebsiteForAds.Extensions;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;

namespace WebsiteForAds.Controllers
{
    
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index(string searchBy, string search, int? page)
        {
            if (searchBy == "Title")
            {
                return View(db.Posts.Where(x => x.Body.Contains(search) || search == null).OrderByDescending(p => p.Date).ToList().ToPagedList(page ?? 1, 10));
            }
            else
            {
                return View(db.Posts.Where(x => x.Title.Contains(search) || search == null).OrderByDescending(p => p.Date).ToList().ToPagedList(page ?? 1, 10));
            }
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Body,Date,Image,AuthorId,Author")] Post post)
        {
            if (ModelState.IsValid)
            {
                var userId = this.User.Identity.GetUserId();
                post.AuthorId = userId;
                var user = this.db.Users.Where(u => u.Id == userId).First();
                post.AuthorEmail = user.Email;
                db.Posts.Add(post);
                db.SaveChanges();
                this.AddNotification("Обявата беше създадена успешно!",NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }

            return View(post);
        }

        //public ActionResult CreateComment([Bind(Include = "Id,Body,Date,Author")] Comment comment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Comments.Add(comment);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //
        //    return View(comment);
        //}

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,Date,Image")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                this.AddNotification("Публикацията беше редактирана успешно!", NotificationType.SUCCESS);
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            this.AddNotification("Публикацията беше изтрита успешно!", NotificationType.SUCCESS);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment([Bind(Include = "Body, PostId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.AuthorId = this.User.Identity.GetUserId();
                db.Comments.Add(comment);
                db.SaveChanges();
                this.AddNotification("Коментарът беше създаден успешно!", NotificationType.SUCCESS);
                return RedirectToAction("Details/" + comment.PostId);
            }

            return RedirectToAction("Details/" + comment.PostId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Comments()
        {
                string currentUserId = this.User.Identity.GetUserId();
                var comments = this.db.Comments
                    .Where(e => e.AuthorId == currentUserId)
                    .OrderBy(e => e.Date)
                    .Select(CommentViewModel.ViewModel);

                return View(new AllCommentsViewModel() { Comments = comments });
            }
    }
}
