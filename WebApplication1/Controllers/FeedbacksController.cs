using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using WebApplication1.DAL;
using WebApplication1.Models.Account;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Controller permettant la gestion des feedbacks des utilisateurs (affichage, modification de l'état, suppression).
    /// </summary>
    [Authorize]
    public class FeedbacksController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Feedbacks
        // Méthode permettant grâce à l'accès par l'url d'afficher la liste des feedbacks des utilisateurs de l'application.
        public ActionResult Index(String searchstring, string currentFilter, int? page)
        {
            if (db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type != TypeUtilisateur.Administrateur)
                return RedirectToAction("BadUserTypeError", "Home");
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
                return View(ListFeedbacks.Where(s => s.Subject.ToUpper().Contains(searchstring.ToUpper())).ToPagedList(pageNumber, pageSize));
            }
            else
                return View(ListFeedbacks.ToPagedList(pageNumber, pageSize));
        }

        // GET: Feedbacks/Details/5
        // Méthode permettant grâce à l'accès par l'url d'afficher les détails du feedback sélectionné.
        public ActionResult Details(int? id)
        {
            if (db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type != TypeUtilisateur.Administrateur)
                return RedirectToAction("BadUserTypeError", "Home");
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var feedback = db.Feedbacks.Find(id);

            if (feedback == null) return HttpNotFound();

            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        // Méthode permettat grâce à l'accès par l'url d'afficher les détails du feedback sélectionné afin de vérifier si l'administrateur souhaite le supprimer.
        public ActionResult Delete(int? id)
        {
            if (db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type != TypeUtilisateur.Administrateur)
                return RedirectToAction("BadUserTypeError", "Home");
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var feedback = db.Feedbacks.Find(id);

            if (feedback == null) return HttpNotFound();

            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        // Méthode appelée lorsque l'administrateur clique sur supprimer après avoir vérifier qu'il souhaite supprimer le feedback sélectionné.
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

        // Méthode permettant de modifier l'état du feedback sélectionné afin de préciser si le feedback est résolu ou en cours.
        public ActionResult Check(int? id)
        {
            if (db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type != TypeUtilisateur.Administrateur)
                return RedirectToAction("BadUserTypeError", "Home");
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var feedback = db.Feedbacks.Find(id);

            if (feedback == null) return HttpNotFound();

            feedback.IsResolved = !feedback.IsResolved;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
