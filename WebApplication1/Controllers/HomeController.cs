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

        private void ChargerDevis(Utilisateur user, Parametre param)
        {
            var ListDevis = db.Devis.Where(devis => devis.UtilisateurID == user.ID).ToList();
            ListDevis.ForEach(devis => devis.Produits = db.DonneeProduit.Where(DP => DP.DevisID == devis.ID).ToList());
            ListDevis = ListDevis.OrderByDescending(s => s.Date).ToList();

            ViewBag.listeDevis = ListDevis.Take(param.TailleHistorique);
            ViewBag.NbDevis = ListDevis.Count();
            ViewBag.NbDevisRecent = ListDevis.Where(d => d.Date.AddDays(param.NbJourStat) > DateTime.Today).Count();

            // % de devis concrétisés
            if (ViewBag.NbDevis > 0)
                ViewBag.DevisConcret = ViewBag.NbFactures * 100 / ViewBag.NbDevis;
            else
                ViewBag.DevisConcret = 0;
        }

        private IEnumerable<Facture> ChargerFacture(Utilisateur user, Parametre param)
        {
            var ListFactures = db.Factures.Where(facture => facture.UtilisateurID == user.ID).ToList();
            ListFactures.ForEach(facture => facture.Produits = db.DonneeProduit.Where(DP => DP.FactureID == facture.ID).ToList());
            ListFactures = ListFactures.OrderByDescending(s => s.Date).ToList();

            ViewBag.listeFactures = ListFactures.Take(param.TailleHistorique);
            ViewBag.NbFactures = ListFactures.Count();
            ViewBag.NbProdSold = ListFactures.Sum(f => f.Produits.Sum(p => p.Quantite));
            ViewBag.CA = ListFactures.Sum(f => f.Produits.Sum(p => p.PrixHT * p.Quantite));

            var a = ListFactures.Where(d => d.Date.AddDays(param.NbJourStat) > DateTime.Today);

            ViewBag.CAduMois = a.Sum(f => f.Produits.Sum(p => p.PrixHT * p.Quantite));

            return a;
        }

        private void ChargerProduit(Utilisateur user, Parametre param)
        {
            var ListProduits = db.Produits.Where(produit => produit.UtilisateurID == user.ID).ToList();
            ListProduits.Reverse();
            ViewBag.listeProduits = ListProduits.Take(param.TailleHistorique);
            ViewBag.NbProduits = ListProduits.Count();
        }

        private void ProduitLePlusVendu(IEnumerable<Facture> listFacturesRecentes)
        {
            // calcul du produit le plus vendu du mois
            var dico = new Dictionary<string, int>();
            foreach (var facture in listFacturesRecentes)
            {
                foreach (var dp in facture.Produits)
                {
                    if (dico.ContainsKey(dp.Nom))
                        dico[dp.Nom] += dp.Quantite;
                    else
                        dico.Add(dp.Nom, dp.Quantite);
                }
            }
            if (dico.Count > 0)
                ViewBag.ProduitPlusVendu = dico.OrderByDescending(p => p.Value).First().Key;
        }

        // Méthode permettant l'affichage de la page d'accueil.
        public ActionResult Index()
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
  
            if (user != null)
            {
                if (user.Type == TypeUtilisateur.EnAttente)
                {
                    return Redirect("/Home/Attente");
                }
                var param = db.Parametres.Find(user.ParametreID);
                ViewBag.Stats = param.NbJourStat;

                var listFacturesRecentes = ChargerFacture(user, param);

                ChargerDevis(user, param);

                ChargerProduit(user, param);

                ProduitLePlusVendu(listFacturesRecentes);
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
            if (user != null)
            {
                ViewBag.Param = db.Parametres.Find(user.ParametreID).DefaultTextFeedback.Replace("\r\n", "_");
                return View(new Feedback(user.ID, user.Nom + " " + user.Prénom));
            }

            return View(new Feedback());
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

        public ActionResult Attente()
        {
            return View();
        }
    }
}
