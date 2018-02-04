using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity.Infrastructure;
using WebApplication1.DAL;
using WebApplication1.Models.Papiers;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class DevisController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Devis
        public ActionResult Index(String searchstring, string currentFilter, int? page)
        {
            List<Devis> listTrie = new List<Devis>();

            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var ListDevis = db.Devis.Where(devis => devis.UtilisateurID == user.ID).ToList();

            ListDevis.ForEach(devis => { devis.Produits = new Dictionary<Produit, int>();
                                         db.DonneeProduit.Where(DP => DP.DonneeID == devis.ID).ToList()
                                            .ForEach(DP => devis.Produits.Add(db.Produits.Find(DP.ProduitID), DP.Quantite));
                                       });

            if (searchstring != null)
                page = 1;
            else
                searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchstring))
            {
                return View(ListDevis.Where(s => s.Objet.ToUpper().Contains(searchstring.ToUpper())).ToPagedList(pageNumber, pageSize));
            }
            else
                return View(ListDevis.ToPagedList(pageNumber, pageSize));
        }

        // GET: Devis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var devis = db.Devis.Find(id);

            if (devis == null) return HttpNotFound();

            return View(devis);
        }

        // GET: Devis/Create
        public ActionResult Create()
        {
            return View(new DevisViewModel(db.Produits.ToList()));
            //return View(new DevisViewModel(db.Entreprises.ToList(), db.Produits.ToList()));
        }

        // POST: Devis/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DevisViewModel dvm)
        {
            if (ModelState.IsValid)
            {
                dvm.Devis.UtilisateurID = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).ID;
                dvm.Devis.Date = DateTime.Now;
                dvm.Devis.Valide = true;
                db.Devis.Add(dvm.Devis);
                db.SaveChanges();

                for (int i = 0; i < dvm.ProduitsID.Length; i++)
                {
                    Produit item = db.Produits.ToList()[dvm.ProduitsID[i] - 1];
                    //A modifier plus tard pour pouvoir instancier la quantité en fonction du choix de l'utilisateur
                    db.DonneeProduit.Add(new DonneeProduit { DonneeID = dvm.Devis.ID, ProduitID = item.ID, Quantite = 1 });
                }

                db.SaveChanges();
                return RedirectToAction("Quantite", new { id = dvm.Devis.ID });
            }

            return View(dvm);
        }

        // GET: Devis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var devis = db.Devis.Find(id);

            if (devis == null) return HttpNotFound();

            return View(devis);
        }

        // POST: Devis/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int id)
        {
            var devis = db.Devis.Find(id);
            if (TryUpdateModel(devis, "", new string[] { "Objet", "Commentaire", "Monnaie", "Produits", "EntrepriseID" }))
            {
                try
                {
                    devis.Date = DateTime.Now;
                    devis.Valide = true;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Impossible d'enregistrer les modifications. Réessayez et si le problème persiste, consultez votre administrateur système.");
                }
            }
            return View(devis);
        }

        // GET: Devis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var devis = db.Devis.Find(id);

            if (devis == null) return HttpNotFound();

            return View(devis);
        }

        // POST: Devis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Devis.Remove(db.Devis.Find(id));
            db.SaveChanges();

            db.DonneeProduit.Where(DP => DP.DonneeID == id).ToList().ForEach(DP => db.DonneeProduit.Remove(DP));
            db.SaveChanges();

            //Client client = db.Clients.Find(devis.EntrepriseID);
            //client.Devis.Remove(devis);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Devis/Facturer
        public ActionResult Facturer(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Devis devis = db.Devis.Find(id);

            if (devis == null) return HttpNotFound();

            return View(new Facture(devis));
        }

        // POST: Devis/Facturer
        [HttpPost, ActionName("Facturer")]
        [ValidateAntiForgeryToken]
        public ActionResult Facturer(int id, TypeReglement reglement)
        {
            db.Factures.Add(new Facture(db.Devis.Find(id), reglement));
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Devis/Quantite
        public ActionResult Quantite(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Devis devis = db.Devis.Find(id);

            if (devis == null) return HttpNotFound();

            
            devis.Produits = new Dictionary<Produit, int>();
            db.DonneeProduit.Where(DP => DP.DonneeID == devis.ID).ToList().ForEach(DP => devis.Produits.Add(db.Produits.Find(DP.ProduitID), DP.Quantite));

            return View(new DevisViewModel(devis.Produits.Keys.ToList()));
        }

        // POST: Devis/Quantite
        [HttpPost, ActionName("Quantite")]
        [ValidateAntiForgeryToken]
        public ActionResult Quantite(int id, DevisViewModel dvm)
        {
            Devis devis = db.Devis.Find(id);
            devis.Produits = new Dictionary<Produit, int>();

            int i = 0;
            foreach (DonneeProduit DP in db.DonneeProduit.ToList())
                if (DP.DonneeID == devis.ID)
                    DP.Quantite = dvm.Quantite[i++];

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
