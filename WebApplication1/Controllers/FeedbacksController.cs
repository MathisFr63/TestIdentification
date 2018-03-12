using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using PagedList;
using WebApplication1.DAL;
using WebApplication1.Models.Account;
using Rotativa;

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
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            if (user.Type != TypeUtilisateur.Administrateur && user.Type != TypeUtilisateur.SA)
                return RedirectToAction("BadUserTypeError", "Home");

            var param = db.Parametres.Find(user.ParametreID);

            var listTrie = new List<Feedback>();

            //var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            //var ListFeedbacks = db.Feedbacks.Where(feedback => feedback.UtilisateurID == user.ID).ToList();
            var ListFeedbacks = db.Feedbacks.ToList();

            if (searchstring != null)
                page = 1;
            else
                searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;

            int pageSize = param.NbElementPage;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchstring))
            {
                return View(ListFeedbacks.Where(s => s.Subject.ToUpper().Contains(searchstring.ToUpper())).ToPagedList(pageNumber, pageSize));
            }
            else
                return View(ListFeedbacks.ToPagedList(pageNumber, pageSize));
        }


        // GET: Feedbacks/Create
        // Méthode permettant à l'utilisateur d'ajouter un nouveau devis parmis sa liste grâce à l'accès par l'url.
        public ActionResult Create()
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            if (user != null)
            {
                ViewBag.Param = db.Parametres.Find(user.ParametreID).DefaultTextFeedback.Replace("\r\n", "_");
                return View(new Feedback(user.ID, user.Nom + " " + user.Prénom));
            }
            return View(new Feedback());
        }

        // POST: Feedbacks/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        // Méthode permettant à l'utilisateur d'ajouter le devis qu'il vient de créer sur la page create (get) si le model est valide.
        public ActionResult Create(Feedback feedback)
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new System.Net.NetworkCredential("afiacrocus@gmail.com", "projetTut1718"),
                EnableSsl = true
            };

            MailMessage mail = new MailMessage
            {
                From = new MailAddress(feedback.UtilisateurID, "Feedback Easybill"),
                IsBodyHtml = true,
                Subject = feedback.userName + ": " + feedback.Subject,
                Body = feedback.Comment,
                Priority = MailPriority.High
            };
            mail.To.Add("afiacrocus@gmail.com");
            smtp.Send(mail);

            MailMessage mail2 = new MailMessage
            {
                From = new MailAddress("afiacrocus@gmail.com", "Résumé Feedback Easybill"),
                IsBodyHtml = true,
                Subject = "Votre Feedback: " + feedback.Subject,
                Body = "Le commentaire suivant a été transmis à notre équipe et sera traiter dès que possible : <br/><br/>" + feedback.Comment + "<br/><br/>Nous vous remercions pour votre commentaire. <br/>Cordialement, l'équipe d'EasyBill.",
                Priority = MailPriority.High
            };
            mail2.To.Add(feedback.UtilisateurID);
            smtp.Send(mail2);

            feedback.UtilisateurID = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).ID;
            db.Feedbacks.Add(feedback);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Feedbacks/Details/5
        // Méthode permettant grâce à l'accès par l'url d'afficher les détails du feedback sélectionné.
        public ActionResult Details(int? id)
        {
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            if (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)
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
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            if (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)
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
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            if (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)
                return RedirectToAction("BadUserTypeError", "Home");
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var feedback = db.Feedbacks.Find(id);

            if (feedback == null) return HttpNotFound();

            feedback.IsResolved = !feedback.IsResolved;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Print(int id)
        {
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            if (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)
                return RedirectToAction("BadUserTypeError", "Home");
            //if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var feedback = db.Feedbacks.Find(id);

            if (feedback == null) return HttpNotFound();

            return new ViewAsPdf("FeedbackToPdf", feedback);
        }

        public ActionResult FeedBackToPdf(int? id)
        {
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            if (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)
                return RedirectToAction("BadUserTypeError", "Home");
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var feedback = db.Feedbacks.Find(id);

            if (feedback == null) return HttpNotFound();

            return View(feedback);
        }

        //Methode permettant de créer un pdf à partir d'une vue html de la liste des devis
        public ActionResult PrintList(string sortOrder, String searchstring, string currentFilter, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            if (user.Type != TypeUtilisateur.Administrateur && user.Type != TypeUtilisateur.SA)
                return RedirectToAction("BadUserTypeError", "Home");

            var param = db.Parametres.Find(user.ParametreID);
            var listTrie = new List<Feedback>();

            //var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            //var ListFeedbacks = db.Feedbacks.Where(feedback => feedback.UtilisateurID == user.ID).ToList();
            var ListFeedbacks = db.Feedbacks.ToList();

            if (searchstring != null)
                page = 1;
            else
                searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;

            int pageSize = param.NbElementPage;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchstring))
            {
                return View(ListFeedbacks.Where(s => s.Subject.ToUpper().Contains(searchstring.ToUpper())).ToPagedList(pageNumber, pageSize));
            }
            else
                return new ViewAsPdf("ListToPdf", ListFeedbacks.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ListToPdf(string sortOrder, String searchstring, string currentFilter, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            if (user.Type != TypeUtilisateur.Administrateur && user.Type != TypeUtilisateur.SA)
                return RedirectToAction("BadUserTypeError", "Home");

            var param = db.Parametres.Find(user.ParametreID);
            var listTrie = new List<Feedback>();

            //var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            //var ListFeedbacks = db.Feedbacks.Where(feedback => feedback.UtilisateurID == user.ID).ToList();
            var ListFeedbacks = db.Feedbacks.ToList();

            if (searchstring != null)
                page = 1;
            else
                searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;

            int pageSize = param.NbElementPage;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchstring))
            {
                return View(ListFeedbacks.Where(s => s.Subject.ToUpper().Contains(searchstring.ToUpper())).ToPagedList(pageNumber, pageSize));
            }
            else
                return View(ListFeedbacks.ToPagedList(pageNumber, pageSize));
        }
    }
}
