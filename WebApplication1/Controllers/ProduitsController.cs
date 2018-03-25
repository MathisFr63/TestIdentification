using PagedList;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Account;
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
            var listProduit = db.Produits.ToList();

            if (searchstring != null)
                page = 1;
            else
                searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;
            ViewBag.CurrentSort = sortOrder;


            var listeTrie = listProduit.OrderBy(s => s.Libelle);

            listeTrie = SortOrder(listeTrie, sortOrder);

            int pageSize = param.NbElementPage;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchstring))
            {
                return View(listeTrie.Where(s => s.Libelle.ToUpper().Contains(searchstring.ToUpper())).ToPagedList(pageNumber, pageSize));
            }
            return View(listeTrie.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult RechercheAvancee(string Libelle, string Commentaire, string PrixTTC, string Type, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var param = db.Parametres.Find(user.ParametreID);

            IEnumerable<Produit> myListTrier = db.Produits.ToList().OrderBy(s => s.Libelle);

            if (Libelle != string.Empty)
                myListTrier = myListTrier.Where(s => s.Libelle.ToUpper().Contains(Libelle.ToUpper()));

            if (Commentaire != string.Empty)
                myListTrier = myListTrier.Where(s => s.Détails.ToUpper().Contains(Commentaire.ToUpper()));

            if (Enum.TryParse(Type, out TypeService searchType))
                myListTrier = myListTrier.Where(s => s.Type == searchType);

            if (double.TryParse(PrixTTC, out double searchPrixTTC))
                myListTrier = myListTrier.Where(s => s.TotalTTC == searchPrixTTC);

            return View("Index", myListTrier.ToPagedList((page ?? 1), param.NbElementPage));
        }

        // GET: Produits/Details/5
        // Méthode pemettant grâce à l'accès par l'url d'afficher les détails du produit dont l'id est passer dans l'url.
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var produit = db.Produits.Find(id);
            if (produit == null)
                return HttpNotFound();

            return View(produit);
        }

        // GET: Produits/Create
        // Méthode permettant grâce à l'accès par l'url d'accèder à la page d'ajout d'un produit.
        public ActionResult Create()
        {
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            if (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

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
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            if (user.Type != TypeUtilisateur.Administrateur && user.Type != TypeUtilisateur.SA)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (string.IsNullOrEmpty(produit.UrlImage))
                produit.UrlImage = db.Parametres.Find(user.ParametreID).DefaultUrl;

            if (!ModelState.IsValid)
                return View(produit);

            db.Produits.Add(produit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Produits/Edit/5
        // Méthode permettant grâce à l'accès par l'url d'afficher la page de modification du produit sélectionné et dont l'id et passé par l'url.
        public ActionResult Edit(int? id)
        {
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;

            if (id == null || (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var produit = db.Produits.Find(id);

            if (produit == null)
                return HttpNotFound();
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
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            if (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var produit = db.Produits.Find(id);
            if (TryUpdateModel(produit, "", new string[] { "Libelle", "UrlImage", "Commentaire", "PrixHT", "Reduction", "TVA", "Type" }))
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
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;

            if (id == null || (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var produit = db.Produits.Find(id);
            if (produit == null)
                return HttpNotFound();
            
            return View(produit);
        }

        // POST: Produits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // Mèthode permettant de supprimer le produit sélectionné de la liste des produits de l'utilisateurs après qu'il ai vérifié qu'il souhaitait le supprimer.
        public ActionResult DeleteConfirmed(int id)
        {
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            if (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var produit = db.Produits.Find(id);
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

        [AllowAnonymous]
        public ActionResult Print(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var produit = db.Produits.Find(id);
            if (produit == null)
                return HttpNotFound();
            
            return new ViewAsPdf("ProduitToPdf", produit);
        }

        public ActionResult ProduitToPdf(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var produit = db.Produits.Find(id);
            if (produit == null)
                return HttpNotFound();
            
            return View(produit);
        }

        //Methode permettant de créer un pdf à partir d'une vue html de la liste des devis
        public ActionResult PrintList(string sortOrder, String searchstring, string currentFilter, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var param = db.Parametres.Find(user.ParametreID);
            var listProduit = db.Produits.ToList();

            if (searchstring != null)
                page = 1;
            else
                searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;
            ViewBag.CurrentSort = sortOrder;


            var listeTrie = listProduit.OrderBy(s => s.Libelle);

            listeTrie = SortOrder(listeTrie, sortOrder);

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
            var listProduit = db.Produits.ToList();

            if (searchstring != null)
                page = 1;
            else
                searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;
            ViewBag.CurrentSort = sortOrder;


            var listeTrie = listProduit.OrderBy(s => s.Libelle);

            listeTrie = SortOrder(listeTrie, sortOrder);

            int pageSize = param.NbElementPage;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchstring))
            {
                return View(listeTrie.Where(s => s.Libelle.ToUpper().Contains(searchstring.ToUpper())).ToPagedList(pageNumber, pageSize));
            }
            return View(listeTrie.ToPagedList(pageNumber, pageSize));
        }

        private IOrderedEnumerable<Produit> SortOrder(IOrderedEnumerable<Produit> listeTrie, string sortOrder)
        {
            switch (sortOrder)
            {
                case "objetZA":
                    return listeTrie.OrderByDescending(s => s.Libelle);
                case "prixFaibleFort":
                    return listeTrie.OrderBy(s => s.PrixHT);
                case "prixFortFaible":
                    return listeTrie.OrderByDescending(s => s.PrixHT);
            }

            return listeTrie.OrderBy(s => s.Libelle);
        }
    }
}
