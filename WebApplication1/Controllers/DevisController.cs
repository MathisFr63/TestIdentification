using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Compte;
using WebApplication1.Models.Entite;
using WebApplication1.Models.Papiers;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class DevisController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Devis
        public ActionResult Index()
        {
            Utilisateur user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            List<Devis> ListDevis = db.Devis.Where(devis => devis.UtilisateurID == user.ID).ToList();
            foreach (Devis devis in ListDevis)
            {
                devis.Articles = new Dictionary<Article, int>();
                foreach (DevisArticle DA in db.DevisArticle.ToList())
                {
                    if (DA.DevisID == devis.ID)
                        devis.Articles.Add(db.Articles.Find(DA.ArticleID), DA.Quantite);
                }
            }
            return View(ListDevis);
        }

        // GET: Devis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Devis devis = db.Devis.Find(id);
            if (devis == null)
            {
                return HttpNotFound();
            }
            return View(devis);
        }

        // GET: Devis/Create
        public ActionResult Create()
        {
            DevisViewModel DVM = new DevisViewModel
            {
                Articles = new List<SelectListItem>(),
                Entreprises = new List<SelectListItem>()
            };

            foreach (Entreprise entreprise in db.Clients.ToList())
            {
                DVM.Entreprises.Add(new SelectListItem
                {
                    Text = entreprise.Nom,
                    Value = entreprise.ID.ToString()
                });
            }

            foreach (Article article in db.Articles.ToList())
            {
                DVM.Articles.Add(new SelectListItem
                {
                    Text = article.Nom,
                    Value = article.ID.ToString()
                });
            }

            return View(DVM);
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
                    db.DevisArticle.Add(new DevisArticle { DevisID = dvm.Devis.ID, ArticleID = item.ID, Quantite = 1 });
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Devis devis = db.Devis.Find(id);
            if (devis == null)
            {
                return HttpNotFound();
            }
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Devis devis = db.Devis.Find(id);
            if (devis == null)
            {
                return HttpNotFound();
            }
            return View(devis);
        }

        // POST: Devis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Devis.Remove(db.Devis.Find(id));
            db.SaveChanges();

            foreach (DevisArticle DA in db.DevisArticle.ToList())
            {
                if (DA.DevisID == id)
                    db.DevisArticle.Remove(DA);
            }
            db.SaveChanges();

            //Client client = db.Clients.Find(devis.EntrepriseID);
            //client.Devis.Remove(devis);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Devis/Facturer
        public ActionResult Facturer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Devis devis = db.Devis.Find(id);
            if (devis == null)
            {
                return HttpNotFound();
            }
            return View(new Facture(devis));
        }

        // POST: Devis/Facturer
        [HttpPost, ActionName("Facturer")]
        [ValidateAntiForgeryToken]
        public ActionResult Facturer(int id, TypeReglement reglement)
        {
            db.Factures.Add(new Facture(db.Devis.Find(id), reglement));
            db.SaveChanges();

            return DeleteConfirmed(id);
        }

        // GET: Devis/Quantite
        public ActionResult Quantite(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Devis devis = db.Devis.Find(id);
            if (devis == null)
            {
                return HttpNotFound();
            }
            DevisViewModel DVM = new DevisViewModel
            {
                Articles = new List<SelectListItem>()
            };
            
            devis.Articles = new Dictionary<Article, int>();
            foreach (DevisArticle DA in db.DevisArticle.ToList())
            {
                if (DA.DevisID == devis.ID)
                    devis.Articles.Add(db.Articles.Find(DA.ArticleID), DA.Quantite);
            }

            foreach (Article article in devis.Articles.Keys)
            {
                DVM.Articles.Add(new SelectListItem
                {
                    Text = article.Nom,
                    Value = article.ID.ToString()
                });
            }
            return View(DVM);
        }

        // POST: Devis/Quantite
        [HttpPost, ActionName("Quantite")]
        [ValidateAntiForgeryToken]
        public ActionResult Quantite(int id)
        {
            string[] keys = Request.Form.AllKeys;
            string[] quantite = new string[keys.Length-1];
            for (int cpt = 1; cpt < keys.Length; cpt++)
            {
                quantite[cpt-1] = Request.Form[keys[cpt]];
            }

            Devis devis = db.Devis.Find(id);
            devis.Articles = new Dictionary<Article, int>();
            int i = 0;
            foreach (DevisArticle DA in db.DevisArticle.ToList())
            {
                if (DA.DevisID == devis.ID)
                {
                    DA.Quantite = Int32.Parse(quantite[i]);
                    i++;
                }
            }
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
