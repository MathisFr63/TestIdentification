using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Papiers;
using PagedList;

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

            ListDevis.ForEach(devis => { devis.Articles = new Dictionary<Article, int>();
                                         db.DonneeArticle.Where(da => da.DonneeID == devis.ID).ToList()
                                            .ForEach(da => devis.Articles.Add(db.Articles.Find(da.ArticleID), da.Quantite));
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
            return View(new DevisViewModel(db.Entreprises.ToList(), db.Articles.ToList()));
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

                for (int i = 0; i < dvm.ArticlesID.Length; i++)
                {
                    Article item = db.Articles.ToList()[dvm.ArticlesID[i] - 1];
                    //A modifier plus tard pour pouvoir instancier la quantité en fonction du choix de l'utilisateur
                    db.DonneeArticle.Add(new DonneeArticle { DonneeID = dvm.Devis.ID, ArticleID = item.ID, Quantite = 1 });
                }

                //Client client = db.Clients.Find(devis.EntrepriseID);
                //if (client != null)
                //{
                //    client.Devis.Add(devis);
                //    db.SaveChanges();
                //}
                db.SaveChanges();
                
                return RedirectToAction("Quantite", new { id = dvm.Devis.ID });
            }

            return View(dvm);
        }

        // GET: Devis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Devis devis = db.Devis.Find(id);

            if (devis == null) return HttpNotFound();

            return View(devis);
        }

        // POST: Devis/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "Objet, Commentaire, Monnaie, Articles, EntrepriseID")] Devis devis)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(devis).State = EntityState.Modified;
                Devis u = db.Devis.Find(id);
                u.Objet = devis.Objet;
                u.Date = DateTime.Now;
                u.Valide = true;
                u.Commentaire = devis.Commentaire;
                u.Monnaie = devis.Monnaie;
                u.Articles = devis.Articles;
                u.EntrepriseID = devis.EntrepriseID;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(devis);
        }

        // GET: Devis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Devis devis = db.Devis.Find(id);

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

            db.DonneeArticle.Where(DA => DA.DonneeID == id).ToList().ForEach(DA => db.DonneeArticle.Remove(DA));
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

            
            devis.Articles = new Dictionary<Article, int>();
            db.DonneeArticle.Where(DA => DA.DonneeID == devis.ID).ToList().ForEach(DA => devis.Articles.Add(db.Articles.Find(DA.ArticleID), DA.Quantite));

            return View(new DevisViewModel(devis.Articles.Keys.ToList()));
        }

        // POST: Devis/Quantite
        [HttpPost, ActionName("Quantite")]
        [ValidateAntiForgeryToken]
        public ActionResult Quantite(int id, DevisViewModel dvm)
        {
            Devis devis = db.Devis.Find(id);
            devis.Articles = new Dictionary<Article, int>();

            int i = 0;
            foreach (DonneeArticle DA in db.DonneeArticle.ToList())
                if (DA.DonneeID == devis.ID)
                    DA.Quantite = dvm.Quantite[i++];

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
