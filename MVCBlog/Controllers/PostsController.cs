using WebsiteForAds.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebsiteForAds.Extensions;
using Microsoft.AspNet.Identity;
using WebsiteForAds.Web.InputModels;

namespace WebsiteForAds.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index(string searchBy, string search)
        {
            if (searchBy == "Body")
            {
                return View(db.Posts.Where(x => x.Body.Contains(search) || search == null).ToList());
            }
            else
            {
                return View(db.Posts.Where(x => x.Title.Contains(search) || search == null).ToList());
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
        public ActionResult Create([Bind(Include = "Id,Title,Body,Date,Image")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                this.AddNotification("Публикацията беше създадена успешно!",NotificationType.SUCCESS);
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
        [Authorize(Roles = "Administrators")]
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
        [Authorize(Roles = "Administrators")]
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

        public ActionResult MineAdvertisments()
        {
            string currentUserId = this.User.Identity.GetUserId();
            var advertisments = this.db.Posts
                .Where(e => e.AuthorId == currentUserId)
                .OrderBy(e => e.Date)
                .Select(PostViewModel.ViewModel);
            
            return View(advertisments);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(CommentInputModel comment)
        {
            if (comment != null && this.ModelState.IsValid)
            {
                comment.UserId = this.User.Identity.GetUserId();
                var comment = Mapper.Map<Comment>(model);
                this.Data.Comments.Add(comment);
                this.Data.SaveChanges();

                var commentViewModel = this.Data.Comments
                    .All()
                    .Where(x => x.Id == comment.Id)
                    .Project()
                    .To<CommentViewModel>()
                    .FirstOrDefault();
                return this.PartialView("DisplayTemplates/CommentViewModel", commentViewModel);
            }

            return this.Json(this.ModelState);
        }
    }
}
