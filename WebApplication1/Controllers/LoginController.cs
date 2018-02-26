using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.DAL;
using WebApplication1.Models.Account;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        public ActionResult Index()
        {
            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = HttpContext.User.Identity.IsAuthenticated };
            if (viewModel.Authentifie)
                viewModel.Utilisateur = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(UtilisateurViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Utilisateur utilisateur = db.Authentifier(viewModel.Utilisateur.Mail, viewModel.motDePasse);
                if (utilisateur != null)
                {
                    FormsAuthentication.SetAuthCookie(utilisateur.ID.ToString(), false);
                    return Redirect("/");
                }
                ModelState.AddModelError("Utilisateur.Mail", "Adresse e-mail et/ou mot de passe incorrect(s)");
            }
            return View("Index", viewModel);
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(UtilisateurViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (db.Utilisateurs.Count(u => u.Mail == vm.Utilisateur.Mail) == 0)
                {
                    int id = db.AjouterUtilisateur(vm.Utilisateur.Mail, vm.motDePasse, vm.Utilisateur.Nom, vm.Utilisateur.Prénom, TypeUtilisateur.Enregistré, vm.Utilisateur.Question, vm.Utilisateur.Réponse);
                    FormsAuthentication.SetAuthCookie(id.ToString(), false);
                    return Redirect("/");
                }
                ModelState.AddModelError("Mail", "Cette adresse e-mail est déjà utilisée");
            }
            return View(vm.Utilisateur);
        }

        public ActionResult Deconnexion()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public ActionResult RecoverMDP(Utilisateur utilisateur)
        {
            return View();
        }

        public ActionResult RecoverMDPAfterLogin(Utilisateur utilisateur)
        {
            //Utilisateur user = db.Utilisateurs.FirstOrDefault(u => u.Mail == utilisateur.Mail);
            /*if (user != null)
            {
                user.Réponse = "";
                return View(user);
            }*/
            Utilisateur user = db.Utilisateurs.FirstOrDefault(u => u.Mail == utilisateur.Mail);
            user.codeRecup = RandomString(6);
            db.SaveChanges();

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new System.Net.NetworkCredential("afiacrocus@gmail.com", "projetTut1718"),
                EnableSsl = true
            };

            MailMessage mail = new MailMessage
            {
                From = new MailAddress(user.Mail, "Recuperation de mot de passe"),
                IsBodyHtml = true,
                Subject = "Recuperation de mot de passe",
                Body = "Voici votre code de récupération :<p/>" + user.codeRecup + "<p/>Merci d'utiliser Easy Bill. A bientot !",
                Priority = MailPriority.High
            };
            mail.To.Add(user.Mail);
            smtp.Send(mail);
            ViewBag.erreur = "mail envoyé";
            //ViewBag.erreur = "Adresse e-mail inconnue";
            return View(utilisateur);
        }

        [HttpPost]
        public ActionResult VérifierRéponse(Utilisateur utilisateur)
        {
            Utilisateur user = db.Utilisateurs.FirstOrDefault(u => u.Mail == utilisateur.Mail);
            if (user.codeRecup == utilisateur.codeRecup)
                
                return View("AfficherMotDePasse", new UtilisateurViewModel {Utilisateur=user });

            return View("RecoverMDP");
        }

        
        public ActionResult ChangePassword(UtilisateurViewModel m)
        {
            Utilisateur user = db.Utilisateurs.FirstOrDefault(u => u.Mail == m.Utilisateur.Mail);
            user.MotDePasse = m.motDePasse.GetHashCode();
            ViewBag.Message = "Votre mot de passe a bien été changé. Pour revenir à l'acceuil, cliquez sur le bouton ci-dessous.";
            return View();
        }
    }
}