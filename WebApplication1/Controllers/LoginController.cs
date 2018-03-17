using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;

using WebApplication1.DAL;
using WebApplication1.Models.Account;
using WebApplication1.Models.Entite;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Controller permettant la connexion et la déconnexion de l'utilisateur de l'application.
    /// </summary>
    public class LoginController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // Méthode permettant grâce à l'accès par l'url d'accéder à la page de connexion de l'application.
        public ActionResult Index()
        {
            var viewModel = new UtilisateurViewModelConnection { Authentifie = HttpContext.User.Identity.IsAuthenticated };

            if (viewModel.Authentifie) viewModel.Utilisateur = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);

            return View(viewModel);
        }

        [HttpPost]
        // Méthode permettant à l'utilisateur de se connecter après avoir rentrer son identifiant et son mot de passe
        public ActionResult Index(UtilisateurViewModel viewModel)
        {
            var utilisateur = db.Authentifier(viewModel.Utilisateur.ID, viewModel.motDePasse);
            if (utilisateur != null)
            {
                FormsAuthentication.SetAuthCookie(utilisateur.ID.ToString(), false);
                if(utilisateur.Type == TypeUtilisateur.EnAttente)
                {
                    return Redirect("/Home/Attente");
                }
                return Redirect("/");
            }
            ViewBag.erreur = "Adresse e-mail et/ou mot de passe incorrect(s)";
            ModelState.AddModelError("Utilisateur.ID", "Adresse e-mail et/ou mot de passe incorrect(s)");
            
            return View("Index", viewModel);
        }

        // Méthode permettant grâce à l'accès par l'url d'accéder à la page d'inscription.
        public ActionResult CreateUser()
        {
            var userVMC = new UtilisateurViewModelConnection { Utilisateur = new Utilisateur() };
            userVMC.Utilisateur.Telephones = new List<Telephone>();
            return View(userVMC);
        }

        [HttpPost]
        // Méthode permettant à l'utilisateur de s'inscrire après avoir rempli toutes les cases correctement.
        public ActionResult CreateUser(UtilisateurViewModelConnection vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            if (db.Utilisateurs.Count(u => u.ID == vm.Utilisateur.ID) != 0)
            {
                ModelState.AddModelError("Utilisateur.ID", "Cette adresse e-mail est déjà utilisée");
                return View(vm);
            }

            int i=1;
            var telephones = new List<Telephone>();
                    
            while ( i < Request.Form.AllKeys.Length)
            {
                if (Request.Form.AllKeys[i].Contains("prefixe"))
                {
                    telephones.Add(new Telephone()
                    {
                        Numéro = Request.Form.GetValues(Request.Form.AllKeys[i + 1])[0],
                        Préfixe = Request.Form.GetValues(Request.Form.AllKeys[i])[0],
                        UtilisateurID = vm.Utilisateur.ID
                    });
                    i++;
                }
                i++;
            }

            string id = db.AjouterUtilisateur(vm.Utilisateur.ID, vm.motDePasse, vm.Utilisateur.Nom, vm.Utilisateur.Prénom, TypeUtilisateur.EnAttente, telephones, vm.Lieu, vm.Utilisateur.Civilite, vm.Utilisateur.otherInfo);

            FormsAuthentication.SetAuthCookie(id, false);
            return Redirect("/");
        }
        
        // Méthode permettant à l'utilisateur de se déconnecter.
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


        // Méthode permettant grâce à l'accès par l'url d'accéder à la page permettant de récupérer un mot de passe oublié.
        public ActionResult RecoverMDP()
        {
            return View();
        }

        // Méthode permettant de continuer la récupération du mot de passe après avoir rentrer l'identifiant du compte.
        public ActionResult RecoverMDPAfterLogin(Utilisateur utilisateur)
        {
            var user = db.Utilisateurs.FirstOrDefault(u => u.ID == utilisateur.ID);
            user.codeRecup = RandomString(6);
            db.SaveChanges();

            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new System.Net.NetworkCredential("afiacrocus@gmail.com", "projetTut1718"),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress(user.ID, "Recuperation de mot de passe"),
                IsBodyHtml = true,
                Subject = "Recuperation de mot de passe",
                Body = "Voici votre code de récupération :<p/>" + user.codeRecup + "<p/>Merci d'utiliser Easy Bill. A bientot !",
                Priority = MailPriority.High
            };

            mail.To.Add(user.ID);
            smtp.Send(mail);
            ViewBag.erreur = "mail envoyé";

            return View(utilisateur);
        }

        [HttpPost]
        // Méthode permettant de vérifier si la réponse est correcte et d'afficher le mot de passe si c'est le cas.
        public ActionResult VérifierRéponse(Utilisateur utilisateur)
        {
            var user = db.Utilisateurs.FirstOrDefault(u => u.ID == utilisateur.ID);

            if (user.codeRecup == utilisateur.codeRecup)                
                return View("AfficherMotDePasse", new UtilisateurViewModelConnection {Utilisateur=user });

            return View("RecoverMDP");
        }

        
        public ActionResult ChangePassword(UtilisateurViewModelConnection m)
        {
            if (!ModelState.IsValid)
               return View("AfficherMotDePasse", m);

            var user = db.Utilisateurs.FirstOrDefault(u => u.ID == m.Utilisateur.ID);
            user.MotDePasse = m.motDePasse.GetHashCode();
            db.SaveChanges();
            ViewBag.Message = "Votre mot de passe a bien été changé. Pour revenir à l'acceuil, cliquez sur le bouton ci-dessous.";
            return View();
        }
    }
}