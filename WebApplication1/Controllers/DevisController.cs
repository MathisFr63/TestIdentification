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

            ListDevis.ForEach(devis => devis.Produits = db.DonneeProduit.Where(DP => DP.DevisID == devis.ID).ToList());

            if (searchstring != null)
                page = 1;
            else
                searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchstring))
                return View(ListDevis.Where(s => s.Objet.ToUpper().Contains(searchstring.ToUpper())).ToPagedList(pageNumber, pageSize));
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
            return View(new DevisProduitViewModel());
        }

        // POST: Devis/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DevisProduitViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var devis = new Devis
                {
                    Objet = Request.Form["Devis.Objet"],
                    Monnaie = (TypeMonnaie) Enum.Parse(typeof(TypeMonnaie), Request.Form["Devis.Monnaie"]),
                    Commentaire = Request.Form["Devis.Commentaire"],
                    UtilisateurID = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).ID,
                    Date = DateTime.Now,
                    Valide = true
                };
                db.Devis.Add(devis);
                db.SaveChanges();

                var keys = Request.Form.AllKeys;
                for (int i = 4; i < keys.Length; i++)
                {
                    var name = keys[i];
                    var produit = db.Produits.First(p => p.Nom == name);

                    db.DonneeProduit.Add(new DonneeProduit(produit, devis.ID, int.Parse(Request.Form.GetValues(keys[i])[0])));
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        // GET: Devis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var devis = db.Devis.Find(id);

            if (devis == null) return HttpNotFound();

            return View(new DevisProduitViewModel(db.DonneeProduit.Where(DP => DP.DevisID == id).ToList()));
        }

        // POST: Devis/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int id, DevisViewModel dvm)
        {
            //var r = Request.Form;
            var devis = db.Devis.Find(id);
            devis.Produits = new List<DonneeProduit>();

            int i = 0;
            foreach (DonneeProduit DP in db.DonneeProduit.ToList())
                if (DP.DevisID == devis.ID)
                    DP.Quantite = dvm.Quantite[i++];
            db.SaveChanges();

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
            db.DonneeProduit.Where(DP => DP.DevisID == id).ToList().ForEach(DP => db.DonneeProduit.Remove(DP));
            db.SaveChanges();

            db.Devis.Remove(db.Devis.Find(id));
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
            var facture = new Facture(db.Devis.Find(id), reglement);
            db.Factures.Add(facture);
            db.SaveChanges();
            
            foreach (DonneeProduit dp in db.DonneeProduit.Where(DP => DP.DevisID == id))
            {
                db.DonneeProduit.Add(new DonneeProduit (dp, facture.ID));
            }
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
