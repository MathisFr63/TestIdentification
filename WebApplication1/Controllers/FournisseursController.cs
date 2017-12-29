using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Entreprise;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class FournisseursController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Fournisseurs
        public ActionResult Index()
        {
            db.UtilisateurCourant = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            return View(db.UtilisateurCourant.Fournisseurs.ToList());
        }

        // GET: Fournisseurs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fournisseur fournisseur = db.Fournisseurs.Find(id);
            if (fournisseur == null)
            {
                return HttpNotFound();
            }
            return View(fournisseur);
        }

        // GET: Fournisseurs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fournisseurs/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID, Nom, SiteWeb, Mail, Commentaire")] Fournisseur fournisseur)
        {
            if (ModelState.IsValid)
            {
                if (int.TryParse(HttpContext.User.Identity.Name, out int tmp))
                {
                    fournisseur.UtilisateurID = tmp;
                    db.Fournisseurs.Add(fournisseur);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Create");
            }

            return View(fournisseur);
        }

        // GET: Fournisseurs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fournisseur fournisseur = db.Fournisseurs.Find(id);
            if (fournisseur == null)
            {
                return HttpNotFound();
            }
            return View(fournisseur);
        }

        // POST: Fournisseurs/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "ID, Nom, SiteWeb, Mail, Commentaire")] Fournisseur fournisseur)
        {
            if (ModelState.IsValid)
            {
                Fournisseur u = db.Fournisseurs.Find(id);
                //db.Entry(fournisseur).State = EntityState.Modified;
                u.Nom = fournisseur.Nom;
                u.SiteWeb = fournisseur.SiteWeb;
                u.Mail = fournisseur.Mail;
                u.Commentaire = fournisseur.Commentaire;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fournisseur);
        }

        // GET: Fournisseurs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fournisseur fournisseur = db.Fournisseurs.Find(id);
            if (fournisseur == null)
            {
                return HttpNotFound();
            }
            return View(fournisseur);
        }

        // POST: Fournisseurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fournisseur fournisseur = db.Fournisseurs.Find(id);
            db.Fournisseurs.Remove(fournisseur);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
