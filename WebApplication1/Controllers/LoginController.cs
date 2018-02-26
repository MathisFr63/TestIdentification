using System.Linq;
<<<<<<< HEAD
using System.Net.Mail;
using System.Web;
=======
>>>>>>> b79922169b30c2efb1c34df6d7e268f373cfb92f
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.DAL;
using WebApplication1.Models.Account;
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
            if (ModelState.IsValid)
            {
                var utilisateur = db.Authentifier(viewModel.Utilisateur.ID, viewModel.motDePasse);
                if (utilisateur != null)
                {
                    FormsAuthentication.SetAuthCookie(utilisateur.ID.ToString(), false);
                    return Redirect("/");
                }
                ViewBag.erreur = "Adresse e-mail et/ou mot de passe incorrect(s)";
                ModelState.AddModelError("Utilisateur.Mail", "Adresse e-mail et/ou mot de passe incorrect(s)");
            }
            return View("Index", viewModel);
        }

        // Méthode permettant grâce à l'accès par l'url d'accéder à la page d'inscription.
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        // Méthode permettant à l'utilisateur de s'inscrire après avoir rempli toutes les cases correctement.
        public ActionResult CreateUser(UtilisateurViewModelConnection vm)
        {
            if (ModelState.IsValid)
            {
                if (db.Utilisateurs.Count(u => u.ID == vm.Utilisateur.ID) == 0)
                {
                    string id = db.AjouterUtilisateur(vm.Utilisateur.ID, vm.motDePasse, vm.Utilisateur.Nom, vm.Utilisateur.Prénom, TypeUtilisateur.Enregistré, vm.Utilisateur.Question, vm.Utilisateur.Réponse);
                    FormsAuthentication.SetAuthCookie(id, false);
                    return Redirect("/");
                }
                ModelState.AddModelError("Utilisateur.ID", "Cette adresse e-mail est déjà utilisée");
            }
            return View(vm);
        }

        // Méthode permettant à l'utilisateur de se déconnecter.
        public ActionResult Deconnexion()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

<<<<<<< HEAD
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public ActionResult RecoverMDP(Utilisateur utilisateur)
=======
        // Méthode permettant grâce à l'accès par l'url d'accéder à la page permettant de récupérer un mot de passe oublié.
        public ActionResult RecoverMDP()
>>>>>>> b79922169b30c2efb1c34df6d7e268f373cfb92f
        {
            return View();
        }

        // Méthode permettant de continuer la récupération du mot de passe après avoir rentrer l'identifiant du compte.
        public ActionResult RecoverMDPAfterLogin(Utilisateur utilisateur)
        {
<<<<<<< HEAD
            //Utilisateur user = db.Utilisateurs.FirstOrDefault(u => u.Mail == utilisateur.Mail);
            /*if (user != null)
=======
            var user = db.Utilisateurs.FirstOrDefault(u => u.ID == utilisateur.ID);
            if (user != null)
>>>>>>> b79922169b30c2efb1c34df6d7e268f373cfb92f
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
        // Méthode permettant de vérifier si la réponse est correcte et d'afficher le mot de passe si c'est le cas.
        public ActionResult VérifierRéponse(Utilisateur utilisateur)
        {
<<<<<<< HEAD
            Utilisateur user = db.Utilisateurs.FirstOrDefault(u => u.Mail == utilisateur.Mail);
            if (user.codeRecup == utilisateur.codeRecup)
                
                return View("AfficherMotDePasse", new UtilisateurViewModel {Utilisateur=user });
=======
            var user = db.Utilisateurs.FirstOrDefault(u => u.ID == utilisateur.ID);
            if (user != null)
                if (user.Réponse == utilisateur.Réponse)
                    return View("AfficherMotDePasse", user);
>>>>>>> b79922169b30c2efb1c34df6d7e268f373cfb92f

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