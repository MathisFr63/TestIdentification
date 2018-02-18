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
    [Authorize]
    public class UtilisateursController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Utilisateurs
        [Authorize]
        public ActionResult Index()
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            if (user.Type == TypeUtilisateur.Administrateur)
                return View(db.Utilisateurs.ToList());
            return RedirectToAction("BadUserTypeError", "Home");
        }

        public ActionResult Connection()
        {
            return View();
        }

        public ActionResult SeConnecter([Bind(Include = "Mail, MotDePasse")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                Utilisateur UtilisateurCourant = db.Utilisateurs.Contains(utilisateur) ? utilisateur : null;
            }
            return RedirectToAction("Index");
        }

        // GET: Utilisateurs/Details/5
        public ActionResult Details(string id)
        {
            if (db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type != TypeUtilisateur.Administrateur)
                return RedirectToAction("BadUserTypeError", "Home");

            Utilisateur utilisateur = db.Utilisateurs.Find(id.Replace('~', '.'));

            if (utilisateur == null)
                return HttpNotFound();

            return View(utilisateur);
        }

        // GET: Utilisateurs/Create
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
        public ActionResult Create(UtilisateurViewModel vm)
        {
            //if (db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type != TypeUtilisateur.Administrateur)
                //return RedirectToAction("BadUserTypeError", "Home");
            if (ModelState.IsValid)
            {
                db.AjouterUtilisateur(vm.Utilisateur.ID, vm.motDePasse, vm.Utilisateur.Nom, vm.Utilisateur.Prénom, vm.Utilisateur.Type, vm.Utilisateur.Question, vm.Utilisateur.Réponse);
                return RedirectToAction("Index");
            }

            return View(vm.Utilisateur);
        }

        // GET: Utilisateurs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type != TypeUtilisateur.Administrateur)
                return RedirectToAction("BadUserTypeError", "Home");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Utilisateur utilisateur = db.Utilisateurs.Find(id);

            if (utilisateur == null)
                return HttpNotFound();

            return View(utilisateur);
        }

        // POST: Utilisateurs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int id)
        {
            var utilisateur = db.Utilisateurs.Find(id);
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
        public ActionResult Delete(int? id)
        {
            if (db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type != TypeUtilisateur.Administrateur)
                return RedirectToAction("BadUserTypeError", "Home");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Utilisateur utilisateur = db.Utilisateurs.Find(id);

            if (utilisateur == null)
                return HttpNotFound();

            return View(utilisateur);
        }

        // POST: Utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Utilisateurs.Remove(db.Utilisateurs.Find(id));
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
