﻿using System;
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
    /// <summary>
    /// Controller permettant la gestion des devis de l'utilisateur (affichage des devis (après recherche ou non)sur plusieurs pages, ajout d'un devis, détails d'un devis, modification ou suppression, et pour finir facturation si l'utilisateur le souhaite).
    /// </summary>
    [Authorize]
    public class DevisController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Devis
        // Méthode permettant grâce à l'accès par l'url d'afficher la liste des devis de l'utilisateur.
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
        // Méthode permettant grâce à l'accès par l'url d'afficher les détails d'un devis de l'utilisateur grâce à son id.
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var devis = db.Devis.Find(id);

            if (devis == null) return HttpNotFound();

            return View(devis);
        }

        // GET: Devis/Create
        // Méthode permettant à l'utilisateur d'ajouter un nouveau devis parmis sa liste grâce à l'accès par l'url.
        public ActionResult Create()
        {
            return View(new DevisProduitViewModel());
        }

        // POST: Devis/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Méthode permettant à l'utilisateur d'ajouter le devis qu'il vient de créer sur la page create (get) si le model est valide.
        public ActionResult Create(DevisProduitViewModel vm)
        {
            if (ModelState.IsValid)
            {
                vm.Devis.UtilisateurID = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).ID;
                vm.Devis.Date = DateTime.Now;
                vm.Devis.Valide = true;

                db.Devis.Add(vm.Devis);

                var keys = Request.Form.AllKeys;
                for (int i = 4; i < keys.Length; i++)
                {
                    var name = keys[i];
                    var produit = db.Produits.First(p => p.Nom == name);

                    db.DonneeProduit.Add(new DonneeProduit(produit, vm.Devis.ID, int.Parse(Request.Form.GetValues(keys[i])[0])));
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        // GET: Devis/Edit/5
        // Méthode permettant grâce à l'accès par l'url de modifier le devis sélectionné en passant son id dans l'url.
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var devis = db.Devis.Find(id);

            if (devis == null) return HttpNotFound();

            return View(new DevisProduitViewModel(db.DonneeProduit.Where(DP => DP.DevisID == id).ToList()) { Devis = devis });
        }

        // POST: Devis/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        // Méthode permettant la modification du devis sélectionné après avoir modifier les valeurs souhaitées sur la page edit (get)
        public ActionResult EditPost(int id, DevisViewModel dvm)
        {
            var devis = db.Devis.Find(id);
            devis.Produits = new List<DonneeProduit>();

            var donneeProduit = db.DonneeProduit;
            foreach (var item in donneeProduit.Where(dp => dp.DevisID == id))
            {
                donneeProduit.Remove(item);
            }

            var keys = Request.Form.AllKeys;
            for (int i = 5; i < keys.Length; i++)
            {
                var name = keys[i];
                var produit = db.Produits.First(p => p.Nom == name);

                db.DonneeProduit.Add(new DonneeProduit(produit, devis.ID, int.Parse(Request.Form.GetValues(keys[i])[0])));
                db.SaveChanges();
            }

            if (TryUpdateModel(devis, "", new string[] { "Objet", "Commentaire", "Monnaie" }))
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
        // Méthode permettant grâce à l'accès par l'url en passant l'id du devis d'afficher les détails de celui-ci afin de vérifier si l'utilisateur souhaite réellement le supprimer.
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var devis = db.Devis.Find(id);

            if (devis == null) return HttpNotFound();

            return View(devis);
        }

        // POST: Devis/Delete/5
        // Méthode permettant à l'utilisateur de supprimer le devis sélectionné après qu'il ai vérifié qu'il le souhaitait vraiment.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.DonneeProduit.Where(DP => DP.DevisID == id).ToList().ForEach(DP => db.DonneeProduit.Remove(DP));
            db.SaveChanges();

            db.Devis.Remove(db.Devis.Find(id));
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Devis/Facturer
        // Méthode permettant à l'utilisateur de facturer le devis sélecitonner en ayant passer son id par l'url.
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
        // Méthode permettant de facturer un devis c'est à dire d'ajouter une facture de ce devis dans sa liste des factures après avoir spécifié le type de réglement.
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
