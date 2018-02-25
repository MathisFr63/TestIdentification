using System.Linq;
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

        // Méthode permettant grâce à l'accès par l'url d'accéder à la page permettant de récupérer un mot de passe oublié.
        public ActionResult RecoverMDP()
        {
            return View();
        }

        // Méthode permettant de continuer la récupération du mot de passe après avoir rentrer l'identifiant du compte.
        public ActionResult RecoverMDPAfterLogin(Utilisateur utilisateur)
        {
            var user = db.Utilisateurs.FirstOrDefault(u => u.ID == utilisateur.ID);
            if (user != null)
            {
                user.Réponse = "";
                return View(user);
            }
            ViewBag.erreur = "Adresse e-mail inconnue";
            return View("RecoverMDP");
        }

        [HttpPost]
        // Méthode permettant de vérifier si la réponse est correcte et d'afficher le mot de passe si c'est le cas.
        public ActionResult VérifierRéponse(Utilisateur utilisateur)
        {
            var user = db.Utilisateurs.FirstOrDefault(u => u.ID == utilisateur.ID);
            if (user != null)
                if (user.Réponse == utilisateur.Réponse)
                    return View("AfficherMotDePasse", user);

            return View("RecoverMDP");
        }
    }
}