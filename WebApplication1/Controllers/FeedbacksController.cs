using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using WebApplication1.DAL;
using WebApplication1.Models.Compte;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class FeedbacksController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Feedbacks
        public ActionResult Index(String searchstring, string currentFilter, int? page)
        {
            List<Feedback> listTrie = new List<Feedback>();

            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var ListFeedbacks = db.Feedbacks.Where(feedback => feedback.UtilisateurID == user.ID).ToList();

            if (searchstring != null)
                page = 1;
            else
                searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchstring))
            {
                return View(ListFeedbacks.Where(s => s.Name.ToUpper().Contains(searchstring.ToUpper())).ToPagedList(pageNumber, pageSize));
            }
            else
                return View(ListFeedbacks.ToPagedList(pageNumber, pageSize));
        }

        // GET: Feedbacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var feedback = db.Feedbacks.Find(id);

            if (feedback == null) return HttpNotFound();

            return View(feedback);
        }

        // GET: Feedbacks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Feedbacks/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.UtilisateurID = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).ID;
                db.Feedbacks.Add(feedback);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(feedback);
        }

        // GET: Feedbacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var feedback = db.Feedbacks.Find(id);

            if (feedback == null) return HttpNotFound();

            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int id)
        {
            var feedback = db.Feedbacks.Find(id);
            if (TryUpdateModel(feedback, "", new string[] { "Name", "Email", "Comment" }))
            {
                try
                {   
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Impossible d'enregistrer les modifications. Réessayez et si le problème persiste, consultez votre administrateur système.");
                }
            }
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var feedback = db.Feedbacks.Find(id);

            if (feedback == null) return HttpNotFound();

            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Feedbacks.Remove(db.Feedbacks.Find(id));
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
