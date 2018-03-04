using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Account;
using WebApplication1.Models.Papiers;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Controller permettant l'accès aux pages d'accueil
    /// </summary>
    public class HomeController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // Méthode permettant l'affichage de la page d'accueil.
        public ActionResult Index()
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            if (user != null)
            {
                var ListDevis = db.Devis.Where(devis => devis.UtilisateurID == user.ID).ToList();
                ListDevis.ForEach(devis => devis.Produits = db.DonneeProduit.Where(DP => DP.DevisID == devis.ID).ToList());
                ListDevis = ListDevis.OrderByDescending(s => s.Date).ToList();
                ViewBag.listeDevis = ListDevis.Take(6);
                ViewBag.NbDevis = ListDevis.Count();

                var ListFactures = db.Factures.Where(facture => facture.UtilisateurID == user.ID).ToList();
                ListFactures.ForEach(facture => facture.Produits = db.DonneeProduit.Where(DP => DP.FactureID == facture.ID).ToList());
                ListFactures = ListFactures.OrderByDescending(s => s.Date).ToList();

                ViewBag.listeFactures = ListFactures.Take(6);
                ViewBag.NbFactures = ListFactures.Count();

                var ListProduits = db.Produits.Where(produit => produit.UtilisateurID == user.ID).ToList();
                ListProduits.Reverse();
                ViewBag.listeProduits = ListProduits.Take(6);
                ViewBag.NbProduits = ListProduits.Count();
                
                ViewBag.NbProdSold = ListFactures.Sum(f => f.Produits.Sum(p => p.Quantite));

                ViewBag.CA = ListFactures.Sum(f => f.Produits.Sum(p => p.PrixHT * p.Quantite));
            }
            
            return View();
        }

        // Méthode permettant l'affichage de la page "À propos".
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        // Méthode permettant l'afichage de la page contact
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            return View(user != null ? new Feedback(user.ID, user.Nom + " " + user.Prénom) : new Feedback());
        }

        [HttpPost]
        [ValidateInput(false)]
        // Méthode permettant l'accès à la page de contact afin d'envoyer un feedback.
        public ActionResult Contact(Feedback feedback)
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

            return RedirectToAction("FeedbackSent");
        }

        // Méthode appelée lorsque l'envoi du feedback a réussi.
        public ActionResult FeedbackSent()
        {
            ViewBag.Message = "Votre message a bien été envoyé. Pour revenir à l'acceuil, cliquez sur le bouton ci-dessous.";
            return View();
        }

        // Méthode appelée lorsque l'utilisateur n'a pas le bon type (s'il n'est pas administrateur par exemple) afin d'afficher une page d'erreur lui précisant.
        public ActionResult BadUserTypeError(string message)
        {
            ViewBag.errorMessage = message;
            return View();
        }
    }
}
