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
                Utilisateur utilisateur = db.Authentifier(viewModel.Utilisateur.Identifiant, viewModel.Utilisateur.MotDePasse);
                if (utilisateur != null)
                {
                    FormsAuthentication.SetAuthCookie(utilisateur.ID.ToString(), false);
                    return Redirect("/");
                }
                ModelState.AddModelError("Utilisateur.Identifiant", "Identifiant et/ou mot de passe incorrect(s)");
            }
            return View("Index", viewModel);
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                if (db.Utilisateurs.Count(u => u.Identifiant == utilisateur.Identifiant) == 0)
                {
                    utilisateur.Type = TypeUtilisateur.Enregistré;
                    int id = db.AjouterUtilisateur(utilisateur.Identifiant, utilisateur.MotDePasse, utilisateur.Nom, utilisateur.Prénom, utilisateur.Mail, utilisateur.Type, utilisateur.Question, utilisateur.Réponse);
                    FormsAuthentication.SetAuthCookie(id.ToString(), false);
                    return Redirect("/");
                }
                ModelState.AddModelError("Identifiant", "Cet identifiant est déjà utilisé");
            }
            return View(utilisateur);
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
            Utilisateur user = db.Utilisateurs.FirstOrDefault(u => u.Identifiant == utilisateur.Identifiant);
            if (user != null)
            {
                user.Réponse = "";
                return View(user);
            }
            ViewBag.erreur = "Identifiant inconnu";
            return View("RecoverMDP");
        }

        [HttpPost]
        public ActionResult VérifierRéponse(Utilisateur utilisateur)
        {
            Utilisateur user = db.Utilisateurs.FirstOrDefault(u => u.Identifiant == utilisateur.Identifiant);
            if (user != null)
                if (user.Réponse == utilisateur.Réponse)
                    return View("AfficherMotDePasse", user);

            return View("RecoverMDP");
        }
    }
}