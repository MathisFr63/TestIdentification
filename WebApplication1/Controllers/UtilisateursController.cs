using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Account;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Controller permettant la gestion des utilisateurs de l'application (affichage, ajout, modification, suppression).
    /// </summary>
    [Authorize]
    public class UtilisateursController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Utilisateurs
        [Authorize]
        // Méthode permettant d'afficher la liste des utilisateurs de l'application si l'utilisateur courant est un administrateur.
        public ActionResult Index()
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            if (user.Type == TypeUtilisateur.Administrateur)
                return View(db.Utilisateurs.ToList());
            return RedirectToAction("BadUserTypeError", "Home");
        }

        // Méthode permettant à l'utilisateur d'accèder à la page de connexion.
        public ActionResult Connection()
        {
            return View();
        }

        // Méthode permettant à l'utilisateur de se connecter après avoir rentré son identifiant et son mot de passe.
        public ActionResult SeConnecter([Bind(Include = "Mail, MotDePasse")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                Utilisateur UtilisateurCourant = db.Utilisateurs.Contains(utilisateur) ? utilisateur : null;
            }
            return RedirectToAction("Index");
        }

        // GET: Utilisateurs/Details/5
        // Méthode permettant grâce à l'accès par l'url d'afficher les données de l'utilisateur sélectionné et dont l'id est passé dans l'url si l'utilisateur courant est un administrateur.
        public ActionResult Details(string id)
        {
            if (db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type != TypeUtilisateur.Administrateur && HttpContext.User.Identity.Name != id.Replace("~","."))
                return RedirectToAction("BadUserTypeError", "Home");

            Utilisateur utilisateur = db.Utilisateurs.Find(id.Replace('~', '.'));

            if (utilisateur == null)
                return HttpNotFound();

            return View(utilisateur);
        }

        // GET: Utilisateurs/Create
        // Méthode permettant grâce à l'accès par l'url d'accéder à la page de création d'un utilisateur si l'utilisateur courant est un administrateur.
        public ActionResult Create()
        {
            if (db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type != TypeUtilisateur.Administrateur)
                return RedirectToAction("BadUserTypeError", "Home");
            return View();
        }

        // POST: Utilisateurs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Méthode permettant à l'administrateur de créer un utilisateur après avoir instancié les données sur la page de création.
        public ActionResult Create(UtilisateurViewModelConnection vm)
        {
            //if (db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type != TypeUtilisateur.Administrateur)
            //return RedirectToAction("BadUserTypeError", "Home");
            if (ModelState.IsValid)
            {
                if (db.Utilisateurs.Count(u => u.ID == vm.Utilisateur.ID) == 0)
                {
                    db.AjouterUtilisateur(vm.Utilisateur.ID, vm.motDePasse, vm.Utilisateur.Nom, vm.Utilisateur.Prénom, TypeUtilisateur.Enregistré, vm.Utilisateur.Question, vm.Utilisateur.Réponse);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Utilisateur.ID", "Cette adresse e-mail est déjà utilisée");
            }
            return View(vm);
        }

        // GET: Utilisateurs/Edit/5
        // Méthode permettant à un administrateur d'accéder à la page de modification des données de l'utilisateur sélectionné dont l'id est passé dans l'url.
        public ActionResult Edit(string id)
        {
            if (db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type != TypeUtilisateur.Administrateur)
                return RedirectToAction("BadUserTypeError", "Home");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Utilisateur utilisateur = db.Utilisateurs.Find(id.Replace('~', '.'));

            if (utilisateur == null)
                return HttpNotFound();

            return View(utilisateur);
        }

        // POST: Utilisateurs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        // Méthode permettant à un administrateur de modifier les données de l'utilisateur sélectionné et dont l'id est passé dans l'url après avoir modifié les valeurs sur la page de modification.
        public ActionResult EditPost(string id)
        {
            var utilisateur = db.Utilisateurs.Find(id.Replace('~', '.'));
            if (TryUpdateModel(utilisateur, "", new string[] { "Mail", "Nom", "Prénom", "Type" }))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Impossible d'enregistrer les modifications. Réessayez et si le problème persiste, consultez votre administrateur système.");
                }
            }
            return View(utilisateur);
        }

        // GET: Utilisateurs/Delete/5
        // Méthode permettant d'afficher les détails de l'utilisateur sélectionné et dont l'id est passé dans l'url afin de vérifier qu'il veut le supprimer.
        public ActionResult Delete(string id)
        {
            if (db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type != TypeUtilisateur.Administrateur)
                return RedirectToAction("BadUserTypeError", "Home");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Utilisateur utilisateur = db.Utilisateurs.Find(id.Replace('~', '.'));

            if (utilisateur == null)
                return HttpNotFound();

            return View(utilisateur);
        }

        // POST: Utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // Méthode appelée lorsque l'utilisateur souhaite réelement supprimer l'utilisateur sélectionné (lui même s'il n'est pas administrateur).
        public ActionResult DeleteConfirmed(string id)
        {
            db.Utilisateurs.Remove(db.Utilisateurs.Find(id.Replace('~', '.')));
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
