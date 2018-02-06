using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.DAL;
using WebApplication1.Models.Compte;
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

        public ActionResult RecoverMDP()
        {
            return View();
        }

        public ActionResult RecoverMDPAfterLogin(Utilisateur utilisateur)
        {
            Utilisateur user = db.Utilisateurs.FirstOrDefault(u => u.Mail == utilisateur.Mail);
            if (user != null)
            {
                user.Réponse = "";
                return View(user);
            }
            ViewBag.erreur = "Adresse e-mail inconnue";
            return View("RecoverMDP");
        }

        [HttpPost]
        public ActionResult VérifierRéponse(Utilisateur utilisateur)
        {
            Utilisateur user = db.Utilisateurs.FirstOrDefault(u => u.Mail == utilisateur.Mail);
            if (user != null)
                if (user.Réponse == utilisateur.Réponse)
                    return View("AfficherMotDePasse", user);

            return View("RecoverMDP");
        }
    }
}