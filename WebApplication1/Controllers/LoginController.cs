<<<<<<< .mine
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.DAL;
using WebApplication1.Models.Compte;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        public ActionResult Index()
        {
            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = HttpContext.User.Identity.IsAuthenticated };
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                viewModel.Utilisateur = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(UtilisateurViewModel viewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Utilisateur utilisateur = db.Authentifier(viewModel.Utilisateur.Identifiant, viewModel.Utilisateur.MotDePasse);
                if (utilisateur != null)
                {
                    FormsAuthentication.SetAuthCookie(utilisateur.ID.ToString(), false);
                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return Redirect("/");
                }
                ModelState.AddModelError("Utilisateur.Prenom", "Prénom et/ou mot de passe incorrect(s)");
            }
            return View(viewModel);
        }

        public ActionResult CreerCompte()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreerCompte(Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                int id = db.AjouterUtilisateur(utilisateur.Identifiant, utilisateur.MotDePasse);
                FormsAuthentication.SetAuthCookie(id.ToString(), false);
                return Redirect("/");
            }
            return View(utilisateur);
        }

        public ActionResult Deconnexion()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}
/*

    var message = new MailMessage(new MailAddress("afiacrocus@gmail.com"), new MailAddress("afiacrocus@gmail.com"))
    {
        Subject = "Mon sujet",
        Body = "Message de mon email",
        IsBodyHtml = true
    };

    var smtp = new SmtpClient("smtp.gmail.com", 587)
    {
        Credentials = new NetworkCredential("afiacrocus@gmail.com", "projetTut1718"),
        EnableSsl = false
    };
    smtp.Send(message);

    web.config
    <system.net>
        <mailSettings>
            <smtp deliveryMethod="SpecifiedPickupDirectory">
                <specifiedPickupDirectory pickupDirectoryLocation="C:\Mails\"/>
            </smtp>
        </mailSettings>
    </system.net>
*/
||||||| .r29
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.DAL;
using WebApplication1.Models.Compte;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        public ActionResult Index()
        {
            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = HttpContext.User.Identity.IsAuthenticated };
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                viewModel.Utilisateur = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(UtilisateurViewModel viewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Utilisateur utilisateur = db.Authentifier(viewModel.Utilisateur.Identifiant, viewModel.Utilisateur.MotDePasse);
                if (utilisateur != null)
                {
                    FormsAuthentication.SetAuthCookie(utilisateur.ID.ToString(), false);
                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return Redirect("/");
                }
                ModelState.AddModelError("Utilisateur.Prenom", "Prénom et/ou mot de passe incorrect(s)");
            }
            return View(viewModel);
        }

        public ActionResult CreerCompte()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreerCompte(Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                int id = db.AjouterUtilisateur(utilisateur.Identifiant, utilisateur.MotDePasse);
                FormsAuthentication.SetAuthCookie(id.ToString(), false);
                return Redirect("/");
            }
            return View(utilisateur);
        }

        public ActionResult Deconnexion()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.DAL;
using WebApplication1.Models.Compte;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        public ActionResult Index()
        {
            UtilisateurViewModel viewModel = new UtilisateurViewModel { Authentifie = HttpContext.User.Identity.IsAuthenticated };
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                viewModel.Utilisateur = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(UtilisateurViewModel viewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Utilisateur utilisateur = db.Authentifier(viewModel.Utilisateur.Identifiant, viewModel.Utilisateur.MotDePasse);
                if (utilisateur != null)
                {
                    FormsAuthentication.SetAuthCookie(utilisateur.ID.ToString(), false);
                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return Redirect("/");
                }
                ModelState.AddModelError("Utilisateur.Prenom", "Prénom et/ou mot de passe incorrect(s)");
            }
            return View(viewModel);
        }

        public ActionResult CreerCompte()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreerCompte(Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                int id = db.AjouterUtilisateur(utilisateur.Identifiant, utilisateur.MotDePasse, utilisateur.Nom, utilisateur.Prénom, utilisateur.Mail, utilisateur.Type, utilisateur.Question, utilisateur.Réponse);
                FormsAuthentication.SetAuthCookie(id.ToString(), false);
                return Redirect("/");
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
            {
                if (user.Réponse == utilisateur.Réponse)
                    return View("AfficherMotDePasse", user);
            }
            return View("RecoverMDP");
        }
    }
}>>>>>>> .r33
