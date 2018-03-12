﻿using PagedList;
using Rotativa;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Account;
using WebApplication1.Models.Entite;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Controller permettant la gestion des utilisateurs de l'application (affichage, ajout, modification, suppression).
    /// </summary>
    [Authorize]
    public class UtilisateursController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Utilisateurs
        [Authorize]
        // Méthode permettant d'afficher la liste des utilisateurs de l'application si l'utilisateur courant est un administrateur.
        public ActionResult Index(string sortOrder, String searchstring, string currentFilter, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            if (user.Type == TypeUtilisateur.Administrateur || user.Type == TypeUtilisateur.SA)
            {
                var param = db.Parametres.Find(user.ParametreID);
                var users = db.Utilisateurs.ToList();

                if (searchstring != null) page = 1;
                else searchstring = currentFilter;

                ViewBag.CurrentFilter = searchstring;
                ViewBag.CurrentSort = sortOrder;

                var usersTrie = users.OrderBy(s => s.ID);

                switch (sortOrder)
                {
                    case "mailAZ":
                        usersTrie = users.OrderBy(s => s.ID);
                        break;
                    case "mailZA":
                        usersTrie = users.OrderByDescending(s => s.ID);
                        break;
                    case "nomAZ":
                        usersTrie = users.OrderBy(s => s.Nom);
                        break;
                    case "nomZA":
                        usersTrie = users.OrderByDescending(s => s.Nom);
                        break;
                    case "prénomAZ":
                        usersTrie = users.OrderBy(s => s.Prénom);
                        break;
                    case "prénomZA":
                        usersTrie = users.OrderByDescending(s => s.Prénom);
                        break;
                    case "typeAdmin":
                        usersTrie = users.OrderBy(s => s.Type);
                        break;
                    case "typeBasique":
                        usersTrie = users.OrderByDescending(s => s.Type);
                        break;
                    default:
                        usersTrie = users.OrderBy(s => s.ID);
                        break;
                }

                if (!String.IsNullOrEmpty(searchstring))
                    return View(usersTrie.Where(s => s.ID.ToUpper().Contains(searchstring.ToUpper())).ToPagedList((page ?? 1), param.NbElementPage));
                return View(usersTrie.ToPagedList((page ?? 1), param.NbElementPage));
            }
            return RedirectToAction("Details", new { id = user.ID.Replace(".", "~") });
        }

        // Méthode permettant à l'utilisateur d'accèder à la page de connexion.
        public ActionResult Connection()
        {
            return View();
        }

        // Méthode permettant à l'utilisateur de se connecter après avoir rentré son identifiant et son mot de passe.
        public ActionResult SeConnecter([Bind(Include = "Mail, MotDePasse")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                Utilisateur UtilisateurCourant = db.Utilisateurs.Contains(utilisateur) ? utilisateur : null;
            }
            return RedirectToAction("Index");
        }

        // GET: Utilisateurs/Details/5
        // Méthode permettant grâce à l'accès par l'url d'afficher les données de l'utilisateur sélectionné et dont l'id est passé dans l'url si l'utilisateur courant est un administrateur.
        public ActionResult Details(string id)
        {
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            var userType = db.ObtenirUtilisateur(id.Replace("~", ".")).Type;
            if ((userType == TypeUtilisateur.SA && id != HttpContext.User.Identity.Name) || type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA && HttpContext.User.Identity.Name != id.Replace("~", "."))
                return RedirectToAction("BadUserTypeError", "Home");

            Utilisateur utilisateur = db.Utilisateurs.Find(id.Replace('~', '.'));

            if (utilisateur == null)
                return HttpNotFound();

            var ListDevis = db.Devis.Where(devis => devis.UtilisateurID == utilisateur.ID).ToList();
            ViewBag.NbDevis = ListDevis.Count();
            var factures = db.Factures.Where(facture => facture.UtilisateurID == utilisateur.ID).ToList();
            ViewBag.NbFactures = factures.Count();
            var listProduit = db.Produits.ToList();
            ViewBag.NbProduits = listProduit.Count();

            return View(utilisateur);
        }

        // GET: Utilisateurs/Create
        // Méthode permettant grâce à l'accès par l'url d'accéder à la page de création d'un utilisateur si l'utilisateur courant est un administrateur.
        public ActionResult Create()
        {
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            if (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)
                return RedirectToAction("BadUserTypeError", "Home");
            return View();
        }

        // POST: Utilisateurs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Méthode permettant à l'administrateur de créer un utilisateur après avoir instancié les données sur la page de création.
        public ActionResult Create(UtilisateurViewModelConnection vm)
        {
            //if (db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type != TypeUtilisateur.Administrateur)
            //return RedirectToAction("BadUserTypeError", "Home");
            if (ModelState.IsValid)
            {
                if (db.Utilisateurs.Count(u => u.ID == vm.Utilisateur.ID) == 0)
                {
                    db.AjouterUtilisateur(vm.Utilisateur.ID, vm.motDePasse, vm.Utilisateur.Nom, vm.Utilisateur.Prénom, TypeUtilisateur.EnAttente, null, new Lieu(), vm.Utilisateur.Civilite, vm.Utilisateur.otherInfo);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("Utilisateur.ID", "Cette adresse e-mail est déjà utilisée");
            }
            return View(vm);
        }

        // GET: Utilisateurs/Edit/5
        // Méthode permettant à un administrateur d'accéder à la page de modification des données de l'utilisateur sélectionné dont l'id est passé dans l'url.
        public ActionResult Edit(string id)
        {
            var typeUserCourant = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            Utilisateur utilisateur = db.Utilisateurs.Find(id.Replace('~', '.'));

            if (utilisateur.Type == TypeUtilisateur.SA && HttpContext.User.Identity.Name != id)

            if (typeUserCourant != TypeUtilisateur.Administrateur && typeUserCourant != TypeUtilisateur.SA && id != HttpContext.User.Identity.Name || (utilisateur.Type == TypeUtilisateur.SA && HttpContext.User.Identity.Name != id))
                return RedirectToAction("BadUserTypeError", "Home");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            if (utilisateur == null)
                return HttpNotFound();

            utilisateur.Telephones = db.Telephones.Where(t => t.UtilisateurID == utilisateur.ID).ToList();

            return View(new UtilisateurViewModelConnection { Utilisateur = utilisateur, Lieu = db.Lieux.Find(utilisateur.LieuID) });
        }

        // POST: Utilisateurs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        // Méthode permettant à un administrateur de modifier les données de l'utilisateur sélectionné et dont l'id est passé dans l'url après avoir modifié les valeurs sur la page de modification.
        public ActionResult EditPost(UtilisateurViewModelConnection userVM)
        {
            if (ModelState.IsValid || userVM.motDePasse == null)
            {
                var utilisateur = db.Utilisateurs.Find(userVM.Utilisateur.ID.Replace('~', '.'));

                db.Telephones.Where(t => t.UtilisateurID == utilisateur.ID).ToList().ForEach(t => db.Telephones.Remove(t));

                var form = Request.Form;
                var keys = form.AllKeys;

                for (int i = 10; keys[i] != "motDePasse"; i++)
                {
                    var name = keys[i];

                    db.Telephones.Add(new Telephone()
                    {
                        Numéro = form.GetValues(name)[0],
                        UtilisateurID = utilisateur.ID
                    });
                }


                utilisateur.Nom = userVM.Utilisateur.Nom;
                utilisateur.Prénom = userVM.Utilisateur.Prénom;
                utilisateur.Type = userVM.Utilisateur.Type;

                var lieu = db.Lieux.Find(utilisateur.LieuID);
                lieu.Adresse = userVM.Lieu.Adresse;
                lieu.CodePostal = userVM.Lieu.CodePostal;
                lieu.Complement = userVM.Lieu.Complement;
                lieu.Pays = userVM.Lieu.Pays;
                lieu.Ville = userVM.Lieu.Ville;

                if (userVM.motDePasse != null && userVM.confirmation != null && userVM.motDePasse == userVM.confirmation)
                {
                    utilisateur.MotDePasse = userVM.motDePasse.GetHashCode();
                    db.SaveChanges();
                    return RedirectToAction("Deconnexion", "Login");
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userVM);
        }

        // GET: Utilisateurs/Delete/5
        // Méthode permettant d'afficher les détails de l'utilisateur sélectionné et dont l'id est passé dans l'url afin de vérifier qu'il veut le supprimer.
        public ActionResult Delete(string id)
        {
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            var type2 = db.ObtenirUtilisateur(id.Replace("~", ".")).Type;
            if (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA || type2 == TypeUtilisateur.Administrateur || type2 == TypeUtilisateur.SA)
                return RedirectToAction("BadUserTypeError", "Home");
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Utilisateur utilisateur = db.Utilisateurs.Find(id.Replace('~', '.'));

            if (utilisateur == null)
                return HttpNotFound();

            return View(utilisateur);
        }

        // POST: Utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        // Méthode appelée lorsque l'utilisateur souhaite réelement supprimer l'utilisateur sélectionné (lui même s'il n'est pas administrateur).
        public ActionResult DeleteConfirmed(string id)
        {
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            var type2 = db.ObtenirUtilisateur(id.Replace("~", ".")).Type;
            if (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)
                return RedirectToAction("BadUserTypeError", "Home");

            if (type2 == TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA || type2 == TypeUtilisateur.SA)
            {
                ViewBag.erreurMessage = "Vous ne pouvez pas supprimer d'administrateur !";
                return RedirectToAction("Index");
            }

            if (id == HttpContext.User.Identity.Name)
            {
                ViewBag.erreurMessage = "Vous ne pouvez pas vous supprimer !";
                return RedirectToAction("Index");
            }

            var user = db.Utilisateurs.Find(id.Replace('~', '.'));

            db.Telephones.RemoveRange(db.Telephones.Where(t => t.UtilisateurID == user.ID));
            db.Utilisateurs.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Print(string id)
        {
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            var userType = db.ObtenirUtilisateur(id.Replace("~", ".")).Type;
            if ((userType == TypeUtilisateur.SA && id != HttpContext.User.Identity.Name) || type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA && HttpContext.User.Identity.Name != id.Replace("~", "."))
                return RedirectToAction("BadUserTypeError", "Home");

            Utilisateur utilisateur = db.Utilisateurs.Find(id.Replace('~', '.'));

            if (utilisateur == null)
                return HttpNotFound();

            var ListDevis = db.Devis.Where(devis => devis.UtilisateurID == utilisateur.ID).ToList();
            ViewBag.NbDevis = ListDevis.Count();
            var factures = db.Factures.Where(facture => facture.UtilisateurID == utilisateur.ID).ToList();
            ViewBag.NbFactures = factures.Count();
            var listProduit = db.Produits.ToList();
            ViewBag.NbProduits = listProduit.Count();

            return new ViewAsPdf("UtilisateurToPdf", utilisateur);
        }

        public ActionResult UtilisateurToPdf(string id)
        {
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            if (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA && HttpContext.User.Identity.Name != id.Replace("~", "."))
                return RedirectToAction("BadUserTypeError", "Home");

            Utilisateur utilisateur = db.Utilisateurs.Find(id.Replace('~', '.'));

            if (utilisateur == null)
                return HttpNotFound();

            var ListDevis = db.Devis.Where(devis => devis.UtilisateurID == utilisateur.ID).ToList();
            ViewBag.NbDevis = ListDevis.Count();
            var factures = db.Factures.Where(facture => facture.UtilisateurID == utilisateur.ID).ToList();
            ViewBag.NbFactures = factures.Count();
            var listProduit = db.Produits.ToList();
            ViewBag.NbProduits = listProduit.Count();

            return View(utilisateur);
        }

        public ActionResult PrintList(string sortOrder, String searchstring, string currentFilter, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            if (user.Type == TypeUtilisateur.Administrateur || user.Type == TypeUtilisateur.SA)
            {
                var param = db.Parametres.Find(user.ParametreID);
                var users = db.Utilisateurs.ToList();

                if (searchstring != null) page = 1;
                else searchstring = currentFilter;

                ViewBag.CurrentFilter = searchstring;
                ViewBag.CurrentSort = sortOrder;

                var usersTrie = users.OrderBy(s => s.ID);

                switch (sortOrder)
                {
                    case "mailAZ":
                        usersTrie = users.OrderBy(s => s.ID);
                        break;
                    case "mailZA":
                        usersTrie = users.OrderByDescending(s => s.ID);
                        break;
                    case "nomAZ":
                        usersTrie = users.OrderBy(s => s.Nom);
                        break;
                    case "nomZA":
                        usersTrie = users.OrderByDescending(s => s.Nom);
                        break;
                    case "prénomAZ":
                        usersTrie = users.OrderBy(s => s.Prénom);
                        break;
                    case "prénomZA":
                        usersTrie = users.OrderByDescending(s => s.Prénom);
                        break;
                    case "typeAdmin":
                        usersTrie = users.OrderBy(s => s.Type);
                        break;
                    case "typeBasique":
                        usersTrie = users.OrderByDescending(s => s.Type);
                        break;
                    default:
                        usersTrie = users.OrderBy(s => s.ID);
                        break;
                }

                if (!String.IsNullOrEmpty(searchstring))
                    return View(usersTrie.Where(s => s.ID.ToUpper().Contains(searchstring.ToUpper())).ToPagedList((page ?? 1), param.NbElementPage));
                return new ViewAsPdf("ListToPdf", usersTrie.ToPagedList((page ?? 1), param.NbElementPage));
            }
            return RedirectToAction("BadUserTypeError", "Home");
        }

        public ActionResult ListToPdf(string sortOrder, String searchstring, string currentFilter, int? page)
        {

            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            if (user.Type == TypeUtilisateur.Administrateur || user.Type == TypeUtilisateur.SA)
            {
                var param = db.Parametres.Find(user.ParametreID);
                var users = db.Utilisateurs.ToList();

                if (searchstring != null) page = 1;
                else searchstring = currentFilter;

                ViewBag.CurrentFilter = searchstring;
                ViewBag.CurrentSort = sortOrder;

                var usersTrie = users.OrderBy(s => s.ID);

                switch (sortOrder)
                {
                    case "mailAZ":
                        usersTrie = users.OrderBy(s => s.ID);
                        break;
                    case "mailZA":
                        usersTrie = users.OrderByDescending(s => s.ID);
                        break;
                    case "nomAZ":
                        usersTrie = users.OrderBy(s => s.Nom);
                        break;
                    case "nomZA":
                        usersTrie = users.OrderByDescending(s => s.Nom);
                        break;
                    case "prénomAZ":
                        usersTrie = users.OrderBy(s => s.Prénom);
                        break;
                    case "prénomZA":
                        usersTrie = users.OrderByDescending(s => s.Prénom);
                        break;
                    case "typeAdmin":
                        usersTrie = users.OrderBy(s => s.Type);
                        break;
                    case "typeBasique":
                        usersTrie = users.OrderByDescending(s => s.Type);
                        break;
                    default:
                        usersTrie = users.OrderBy(s => s.ID);
                        break;
                }

                if (!String.IsNullOrEmpty(searchstring))
                    return View(usersTrie.Where(s => s.ID.ToUpper().Contains(searchstring.ToUpper())).ToPagedList((page ?? 1), param.NbElementPage));
                return View(usersTrie.ToPagedList((page ?? 1), param.NbElementPage));
            }
            return RedirectToAction("BadUserTypeError", "Home");
        }
    }
}
