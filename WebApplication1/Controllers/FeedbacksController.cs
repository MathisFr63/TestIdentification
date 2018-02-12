using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using WebApplication1.DAL;
using WebApplication1.Models.Account;

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

            //var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            //var ListFeedbacks = db.Feedbacks.Where(feedback => feedback.UtilisateurID == user.ID).ToList();
            var ListFeedbacks = db.Feedbacks.ToList();

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

        public ActionResult Check(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var feedback = db.Feedbacks.Find(id);

            if (feedback == null) return HttpNotFound();
            
            if (feedback.Etat == "En cours") feedback.Etat = "Résolu";
            else feedback.Etat = "En cours";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
