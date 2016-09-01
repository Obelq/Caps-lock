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
            if (searchBy == "Body")
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
        public ActionResult Create([Bind(Include = "Id,Title,Body,Date,Picture,AuthorId,Author")] Post post)
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

        [Authorize]
        public ActionResult Edit(int? id)
        {
            var userId = this.User.Identity.GetUserId();
            var user = this.db.Users.Where(u => u.Id == userId).First();

            Post post = db.Posts.Find(id);


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (post == null)
            {
                return HttpNotFound();
            }

            if (user.Id == post.AuthorId || user.Email == "admin@gmail.com")
            {
                return View(post);
            }
            else
            {
                this.AddNotification("Обявата, която искате да редактирате, не е ваша!", NotificationType.ERROR);
                return RedirectToAction("Index");
            }           
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,Date,Picture")] Post post)
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
            var userId = this.User.Identity.GetUserId();
            var user = this.db.Users.Where(u => u.Id == userId).First();

            Post post = db.Posts.Find(id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (post == null)
            {
                return HttpNotFound();
            }

            if (user.Id == post.AuthorId || user.Email == "admin@gmail.com")
            {
                return View(post);
            }
            else
            {
                this.AddNotification("Обявата, която искате да изтриете, не е ваша!", NotificationType.ERROR);
                return RedirectToAction("Index");
            }
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
        
    }
}
