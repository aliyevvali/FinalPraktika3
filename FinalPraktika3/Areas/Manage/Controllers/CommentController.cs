using FinalPraktika3.DAL;
using FinalPraktika3.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinalPraktika3.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CommentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _ev;

        public CommentController(AppDbContext context, IWebHostEnvironment ev)
        {
            _context = context;
            _ev = ev;

        }
        public IActionResult Index()
        {

            return View(_context.Comments.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Comment comment)
        {
            if (comment.Photo != null)
            {
                string fileName = Guid.NewGuid().ToString() + comment.Photo.FileName;
                string path = Path.Combine(_ev.WebRootPath, "assets", "imgs", "user", fileName);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    comment.Photo.CopyTo(stream);
                }
                comment.Image = fileName;
            }
            
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            Comment comment = _context.Comments.FirstOrDefault(x => x.Id == id);
            if (comment == null) return NotFound();
            return View(comment);
        }
        [HttpPost]
        public IActionResult Edit(Comment comment)
        {
            var existComment = _context.Comments.FirstOrDefault(x => x.Id == comment.Id);
            if (existComment == null) return NotFound();
            string newFileName = null;

            if (comment.Photo != null)
            {
                if (comment.Photo.ContentType != "image/jpeg" && comment.Photo.ContentType != "image/png" && comment.Photo.ContentType != "image/webp")
                {
                    ModelState.AddModelError("ImageFile", "Image can be only .jpeg or .png");
                    return View();
                }
                if (comment.Photo.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "Image size must be lower than 2mb");
                    return View();
                }
                string fileName = comment.Photo.FileName;
                if (fileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }
                newFileName = Guid.NewGuid().ToString() + fileName;

                string path = Path.Combine(_ev.WebRootPath, "assets", "imgs", newFileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    comment.Photo.CopyTo(stream);
                }
            }
            if (newFileName != null)
            {
                string deletePath = Path.Combine(_ev.WebRootPath, "assets", "images", existComment.Image);

                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }

                existComment.Image = newFileName;
            }

            existComment.FullName = comment.FullName;
            existComment.FeedBack = comment.FeedBack;
            existComment.Posission = comment.Posission;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                Comment comment = _context.Comments.Find(id);
                if (comment == null) return NotFound();
                if (comment.Image != null)
                {
                    DeleteFile(comment.Image);

                }
                _context.Comments.Remove(comment);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return BadRequest();

        }
        public void DeleteFile(string fileName)
        {
            string path = Path.Combine(_ev.WebRootPath, "assets", "imgs", "user");
            System.IO.File.Delete(Path.Combine(path, fileName));
        }


    }
}
