﻿using PagedList;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Papiers;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Controller permettant la gestion des produits de l'utilisateur (afficahge, ajout, modification, suppression).
    /// </summary>
    public class ProduitsController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Produits
        // Méthode permettant grâce à l'accès par l'url d'afficher la liste des produits de l'utilisateur.
        public ActionResult Index(string sortOrder, string searchstring, string currentFilter, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var param = db.Parametres.Find(user.ParametreID);
            var listProduit = db.Produits.Where(p => p.UtilisateurID == user.ID).ToList();

            if (searchstring != null)
                page = 1;
            else
                searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;
            ViewBag.CurrentSort = sortOrder;


            var listeTrie = listProduit.OrderBy(s => s.Libelle);
            

            switch (sortOrder)
            {
                case "objetAZ":
                    listeTrie = listeTrie.OrderBy(s => s.Libelle);
                    break;
                case "objetZA":
                    listeTrie = listeTrie.OrderByDescending(s => s.Libelle);
                    break;
                case "prixFaibleFort":
                    listeTrie = listeTrie.OrderBy(s => s.PrixHT);
                    break;
                case "prixFortFaible":
                    listeTrie = listeTrie.OrderByDescending(s => s.PrixHT);
                    break;
                default:
                    listeTrie = listeTrie.OrderBy(s => s.Libelle);
                    break;
            }

            int pageSize = param.NbElementPage;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchstring))
            {
                return View(listeTrie.Where(s => s.Libelle.ToUpper().Contains(searchstring.ToUpper())).ToPagedList(pageNumber, pageSize));
            }
            return View(listeTrie.ToPagedList(pageNumber, pageSize));
        }

        // GET: Produits/Details/5
        // Méthode pemettant grâce à l'accès par l'url d'afficher les détails du produit dont l'id est passer dans l'url.
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit produit = db.Produits.Find(id);
            if (produit == null)
            {
                return HttpNotFound();
            }
            return View(produit);
        }

        // GET: Produits/Create
        // Méthode permettant grâce à l'accès par l'url d'accèder à la page d'ajout d'un produit.
        public ActionResult Create()
        {
            return View();
        }

        // POST: Produits/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Méthode permettant d'ajouter un produit à la liste des produits de l'utilisateur après avoir rempli les champs de la page de création.
        public ActionResult Create(Produit produit)
        {
            if (ModelState.IsValid)
            {
                produit.UtilisateurID = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).ID;
                db.Produits.Add(produit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(produit);
        }

        // GET: Produits/Edit/5
        // Méthode permettant grâce à l'accès par l'url d'afficher la page de modification du produit sélectionné et dont l'id et passé par l'url.
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit produit = db.Produits.Find(id);
            if (produit == null)
            {
                return HttpNotFound();
            }
            return View(produit);
        }

        // POST: Produits/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        // Méthode permettant de modifier le produit sélectionné dont l'id est passé en paramètre avec les valeurs modifiées sur la page de modification.
        public ActionResult EditPost(int id)
        {
            var produit = db.Produits.Find(id);
            if (TryUpdateModel(produit, "", new string[] { "Libelle", "Commentaire", "PrixHT", "Reduction", "TVA", "Type" }))
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
            return View(produit);
        }

        // GET: Produits/Delete/5
        // Méthode permettant grâce à l'accès par l'url d'accèder à l'affichage des détails du produit sélectionné afin de vérifier si l'utilisateur veut le supprimer.
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit produit = db.Produits.Find(id);
            if (produit == null)
            {
                return HttpNotFound();
            }
            return View(produit);
        }

        // POST: Produits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // Mèthode permettant de supprimer le produit sélectionné de la liste des produits de l'utilisateurs après qu'il ai vérifié qu'il souhaitait le supprimer.
        public ActionResult DeleteConfirmed(int id)
        {
            Produit produit = db.Produits.Find(id);
            db.Produits.Remove(produit);
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

        public ActionResult Print(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit produit = db.Produits.Find(id);
            if (produit == null)
            {
                return HttpNotFound();
            }
            return new ViewAsPdf("ProduitToPdf", produit);
        }

        public ActionResult ProduitToPdf(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit produit = db.Produits.Find(id);
            if (produit == null)
            {
                return HttpNotFound();
            }
            return View(produit);
        }

        //Methode permettant de créer un pdf à partir d'une vue html de la liste des devis
        public ActionResult PrintList(string sortOrder, String searchstring, string currentFilter, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var param = db.Parametres.Find(user.ParametreID);
            var listProduit = db.Produits.Where(p => p.UtilisateurID == user.ID).ToList();

            if (searchstring != null)
                page = 1;
            else
                searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;
            ViewBag.CurrentSort = sortOrder;


            var listeTrie = listProduit.OrderBy(s => s.Libelle);


            switch (sortOrder)
            {
                case "objetAZ":
                    listeTrie = listeTrie.OrderBy(s => s.Libelle);
                    break;
                case "objetZA":
                    listeTrie = listeTrie.OrderByDescending(s => s.Libelle);
                    break;
                case "prixFaibleFort":
                    listeTrie = listeTrie.OrderBy(s => s.PrixHT);
                    break;
                case "prixFortFaible":
                    listeTrie = listeTrie.OrderByDescending(s => s.PrixHT);
                    break;
                default:
                    listeTrie = listeTrie.OrderBy(s => s.Libelle);
                    break;
            }

            int pageSize = param.NbElementPage;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchstring))
            {
                return View(listeTrie.Where(s => s.Libelle.ToUpper().Contains(searchstring.ToUpper())).ToPagedList(pageNumber, pageSize));
            }
            return new ViewAsPdf("ListToPdf", listeTrie.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ListToPdf(string sortOrder, String searchstring, string currentFilter, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var param = db.Parametres.Find(user.ParametreID);
            var listProduit = db.Produits.Where(p => p.UtilisateurID == user.ID).ToList();

            if (searchstring != null)
                page = 1;
            else
                searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;
            ViewBag.CurrentSort = sortOrder;


            var listeTrie = listProduit.OrderBy(s => s.Libelle);


            switch (sortOrder)
            {
                case "objetAZ":
                    listeTrie = listeTrie.OrderBy(s => s.Libelle);
                    break;
                case "objetZA":
                    listeTrie = listeTrie.OrderByDescending(s => s.Libelle);
                    break;
                case "prixFaibleFort":
                    listeTrie = listeTrie.OrderBy(s => s.PrixHT);
                    break;
                case "prixFortFaible":
                    listeTrie = listeTrie.OrderByDescending(s => s.PrixHT);
                    break;
                default:
                    listeTrie = listeTrie.OrderBy(s => s.Libelle);
                    break;
            }

            int pageSize = param.NbElementPage;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchstring))
            {
                return View(listeTrie.Where(s => s.Libelle.ToUpper().Contains(searchstring.ToUpper())).ToPagedList(pageNumber, pageSize));
            }
            return View(listeTrie.ToPagedList(pageNumber, pageSize));
        }
    }
}
