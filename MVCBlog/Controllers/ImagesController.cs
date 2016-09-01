using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using WebsiteForAds.Models;
using System.IO;
using Microsoft.AspNet.Identity;

namespace WebsiteForAds.Controllers
{
    public class ImagesController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var imagesModel = new Image();
            var currUserId = this.User.Identity.GetUserId();
            var user = this.db.Users.Where(u => u.Id == currUserId).FirstOrDefault();
            Directory.CreateDirectory(Server.MapPath("~/Upload_Files/" + user.UserName + "/"));
            var imageFiles = Directory.GetFiles(Server.MapPath("~/Upload_Files/" + user.UserName + "/"));
            foreach (var item in imageFiles)
            {
                imagesModel.ImageList.Add(Path.GetFileName(item));
            }
            return View(imagesModel);
        }
        [HttpGet]
        public ActionResult UploadImage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadImageMethod()
        {
            if (Request.Files.Count != 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];
                    int fileSize = file.ContentLength;
                    string fileName = file.FileName;

                    var currUserId = this.User.Identity.GetUserId();
                    var user = this.db.Users.Where(u => u.Id == currUserId).FirstOrDefault();


                    file.SaveAs(Server.MapPath("~/Upload_Files/" + user.UserName + "/" + fileName));
                    Image imageGallery = new Image();
                    imageGallery.ID = Guid.NewGuid();
                    imageGallery.Name = fileName;
                    imageGallery.ImagePath = "~/Upload_Files/" + user.UserName + "/" + fileName;
                    db.Images.Add(imageGallery);
                    db.SaveChanges();
                }
                return Content("Success");
            }
            return Content("failed");
        }
    }
}
