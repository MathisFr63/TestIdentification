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
                Articles = new List<SelectListItem>()
            };

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
                db.Devis.Add(dvm.Devis);
                db.SaveChanges();

                for (int i =0; i<dvm.ArticlesID.Length; i++)
                {
                    Article item = db.Articles.ToList()[dvm.ArticlesID[i]-1];
                    db.DevisArticle.Add(new DevisArticle { DevisID = dvm.Devis.ID, ArticleID = item.ID });
                }

                //Client client = db.Clients.Find(devis.EntrepriseID);
                //if (client != null)
                //{
                //    client.Devis.Add(devis);
                //    db.SaveChanges();
                //}
                db.SaveChanges();

                return RedirectToAction("Index");
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
        public ActionResult Edit(int id, [Bind(Include = "ID, Objet, Date, Valide, Commentaire, Monnaie, Articles, EntrepriseID")] Devis devis)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(devis).State = EntityState.Modified;
                Devis u = db.Devis.Find(id);
                u.Objet = devis.Objet;
                u.Date = devis.Date;
                u.Valide = devis.Valide;
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
            Devis devis = db.Devis.Find(id);
            db.Devis.Remove(devis);
            db.SaveChanges();

            foreach (DevisArticle DA in db.DevisArticle.ToList())
            {
                if (DA.DevisID == devis.ID)
                    db.DevisArticle.Remove(DA);
            }
            db.SaveChanges();
            
            //Client client = db.Clients.Find(devis.EntrepriseID);
            //client.Devis.Remove(devis);
            //db.SaveChanges();
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
