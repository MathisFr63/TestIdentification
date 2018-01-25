using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Entite;
using PagedList;
using System.Data.Entity.Infrastructure;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class FournisseursController : Controller
    {
        /*        
        private ApplicationContext db = new ApplicationContext();

        // GET: Fournisseurs
        public ActionResult Index(String searchstring, string currentFilter, int? page)
        {
            db.UtilisateurCourant = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            List<Entreprise> listTrie = new List<Entreprise>();

            if (searchstring != null)
                page = 1;
            else
                searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchstring))
            {
                foreach (Entreprise e in db.UtilisateurCourant.Entreprises.Where(e => e.Type == TypeEntreprise.Fournisseur))
                    if (e.NomEntreprise.ToUpper().Contains(searchstring.ToUpper()))
                        listTrie.Add(e);

                return View(listTrie.ToPagedList(pageNumber, pageSize));
            }
            return View(db.UtilisateurCourant.Entreprises.Where(e => e.Type == TypeEntreprise.Fournisseur).ToPagedList(pageNumber, pageSize));
        }

        // GET: Fournisseurs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entreprise entreprise = db.Entreprises.Find(id);
            if (entreprise == null)
            {
                return HttpNotFound();
            }
            return View(entreprise);
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
        public ActionResult Create([Bind(Include = "ID, NomEntreprise, NomContact, Mail")] Entreprise entreprise)
        {
            if (ModelState.IsValid)
            {
                if (int.TryParse(HttpContext.User.Identity.Name, out int tmp))
                {
                    entreprise.UtilisateurID = tmp;
                    entreprise.Type = TypeEntreprise.Fournisseur;
                    db.Entreprises.Add(entreprise);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Create");
            }

            return View(entreprise);
        }

        // GET: Fournisseurs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Entreprise entreprise = db.Entreprises.Find(id);

            if (entreprise == null)
                return HttpNotFound();
            
            return View(entreprise);
        }

        // POST: Fournisseurs/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int id)
        {
            var fournisseur = db.Entreprises.Find(id);
            if (TryUpdateModel(fournisseur, "", new string[] { "NomEntreprise", "NomContact", "Mail" }))
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
            return View(fournisseur);
        }

        // GET: Fournisseurs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Entreprise entreprise = db.Entreprises.Find(id);

            if (entreprise == null)
                return HttpNotFound();
           
            return View(entreprise);
        }

        // POST: Fournisseurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Entreprises.Remove(db.Entreprises.Find(id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
           
            base.Dispose(disposing);
        }
        */
    }
}
