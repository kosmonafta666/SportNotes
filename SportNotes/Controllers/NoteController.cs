namespace SportNotes.Controllers
{
    using Microsoft.AspNet.Identity;
    using SportNotes.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class NoteController : Controller
    {
        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(CreateNoteModel model)
        {
            var db = new SportNotesDbContext();

            if (ModelState.IsValid)
            {
                var note = new Note()
                {
                    Date = DateTime.UtcNow,
                    Content = model.Content,
                    Comment = model.Comment,
                    OwnerId = this.User.Identity.GetUserId()
                };

                db.Notes.Add(note);
                db.SaveChanges();

                return RedirectToAction("Details", "Note", new { id = note.Id});
            }

            return View(model);
        }
        
        [Authorize]
        [HttpGet]
        public ActionResult Details(int id)
        {
            var db = new SportNotesDbContext();

            var note = db.Notes
                .Where(x => x.Id == id)
                .FirstOrDefault();

            return View(note);
        }

        [Authorize]
        [HttpGet]
        public ActionResult UserNotes(int page = 1)
        {
            var db = new SportNotesDbContext();
            var pageSize = 4;
            var owner = this.User.Identity.GetUserId();

            //extract all notes from current user;
            var userNotes = db.Notes
                .Where(x => x.OwnerId == owner)
                .OrderByDescending(x => x.Date)
                .Select(x => new UserNote
                {
                    Id = x.Id,
                    Date = x.Date,
                    Summary = x.Content.Substring(0, 5) + "..."
                })
                .ToList();

            //take pagesize note from all
            var pageNotes = userNotes
                .OrderByDescending(x => x.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            if (userNotes == null || pageNotes == null)
            {
                return HttpNotFound();
            }

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.UserNotes = userNotes.Count;

            return View(pageNotes);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var db = new SportNotesDbContext();

            var note = db.Notes
                .Where(x => x.Id == id)
                .FirstOrDefault();

            db.Notes.Remove(note);
            db.SaveChanges();

            return RedirectToAction("UserNotes", "Note");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new SportNotesDbContext();

            var note = db.Notes
                .Where(x => x.Id == id)
                .FirstOrDefault();

            var editNote = new EditNoteModel()
            {
                Id = note.Id,
                Date = note.Date,
                Content = note.Content,
                Comment = note.Comment
            };

            return View(editNote);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(EditNoteModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new SportNotesDbContext())
                {
                    var note = db.Notes.Find(model.Id);

                    if (note == null)
                    {
                        return HttpNotFound();
                    }

                    note.Content = model.Content;
                    note.Comment = model.Comment;

                    db.SaveChanges();
                } 

                return RedirectToAction("Details", new { id = model.Id });
            }

            return View(model);
        }
    }
}