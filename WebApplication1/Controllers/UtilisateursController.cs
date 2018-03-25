using PagedList;
using Rotativa;
using System;
using System.Collections.Generic;
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

                var usersTrie = SortOrder(users, sortOrder);

                if (!String.IsNullOrEmpty(searchstring))
                    return View(usersTrie.Where(s => s.ID.ToUpper().Contains(searchstring.ToUpper())).ToPagedList((page ?? 1), param.NbElementPage));

                return View(usersTrie.ToPagedList((page ?? 1), param.NbElementPage));
            }
            return RedirectToAction("Details", new { @id = user.ID.Replace(".", "~") });
        }

        public ActionResult RechercheAvancee(string Nom, string Prénom, string Mail, string Type, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);

            if (user.Type == TypeUtilisateur.Administrateur || user.Type == TypeUtilisateur.SA)
            {
                var param = db.Parametres.Find(user.ParametreID);
                IEnumerable<Utilisateur> myListTrier = db.Utilisateurs.ToList();

                if (!string.IsNullOrWhiteSpace(Nom))
                    myListTrier = myListTrier.Where(u => u.Nom.ToUpper().Contains(Nom.ToUpper()));

                if (!string.IsNullOrWhiteSpace(Prénom))
                    myListTrier = myListTrier.Where(u => u.Prénom.ToUpper().Contains(Prénom.ToUpper()));

                if (!string.IsNullOrWhiteSpace(Mail))
                    myListTrier = myListTrier.Where(u => u.ID.ToUpper().Contains(Mail.ToUpper()));

                if (!string.IsNullOrWhiteSpace(Type))
                    myListTrier = myListTrier.Where(u => u.Type.ToString().ToUpper().Contains(Type.ToUpper()));

                return View("Index", myListTrier.ToPagedList((page ?? 1), param.NbElementPage));
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
            id = id.Replace("~", ".");
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            var userType = db.ObtenirUtilisateur(id).Type;
            if ((userType == TypeUtilisateur.SA && id != HttpContext.User.Identity.Name) || type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA && HttpContext.User.Identity.Name != id)
                return RedirectToAction("BadUserTypeError", "Home", new { method = "Index", controller = "Home" });

            var utilisateur = db.Utilisateurs.Find(id.Replace('~', '.'));

            ViewBag.lieu = db.Lieux.Find(utilisateur.LieuID);

            utilisateur.Telephones = db.Telephones.Where(t => t.UtilisateurID == utilisateur.ID).ToList();

            if (utilisateur == null)
                return HttpNotFound();

            ViewBag.NbDevis = db.Devis.Where(devis => devis.UtilisateurID == utilisateur.ID).Count();
            ViewBag.NbFactures = db.Factures.Where(facture => facture.UtilisateurID == utilisateur.ID).Count();
            ViewBag.NbProduits = db.Produits.Count();

            return View(utilisateur);
        }

        // GET: Utilisateurs/Create
        // Méthode permettant grâce à l'accès par l'url d'accéder à la page de création d'un utilisateur si l'utilisateur courant est un administrateur.
        public ActionResult Create()
        {
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            if (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)
                return RedirectToAction("BadUserTypeError", "Home", new { method = "Index", controller = "Home" });
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
            if (ModelState.IsValid)
            {
                if (db.Utilisateurs.Count(u => u.ID == vm.Utilisateur.ID) == 0)
                {
                    db.AjouterUtilisateur(vm.Utilisateur.ID, vm.motDePasse, vm.Utilisateur.Nom, vm.Utilisateur.Prénom, vm.Utilisateur.Type, vm.Utilisateur.Telephones, vm.Lieu, vm.Utilisateur.Civilite, vm.Utilisateur.OtherInfo);
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
            id = id.Replace('~', '.');
            var typeUserCourant = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            var utilisateur = db.Utilisateurs.Find(id);

            if (typeUserCourant != TypeUtilisateur.Administrateur && typeUserCourant != TypeUtilisateur.SA && id != HttpContext.User.Identity.Name || (utilisateur.Type == TypeUtilisateur.SA && HttpContext.User.Identity.Name != id) || (utilisateur.Type == TypeUtilisateur.Administrateur && typeUserCourant != TypeUtilisateur.SA && id != HttpContext.User.Identity.Name))
                return RedirectToAction("BadUserTypeError", "Home", new { @message = "", @method = "Index", @control = "Home" });

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
                int i = 1;
                var utilisateur = db.Utilisateurs.Find(userVM.Utilisateur.ID.Replace('~', '.'));

                db.Telephones.Where(t => t.UtilisateurID == utilisateur.ID).ToList().ForEach(t => db.Telephones.Remove(t));

                var form = Request.Form;
                var keys = form.AllKeys;

                while ( i<keys.Length)
                {
                    if (keys[i].Contains("prefixe"))
                    {
                        db.Telephones.Add(new Telephone()
                        {
                            Préfixe = form.GetValues(keys[i])[0],
                            Numéro = form.GetValues(keys[i + 1])[0],
                            UtilisateurID = utilisateur.ID
                        });
                        i++;
                    }
                    i++;
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
                return RedirectToAction("BadUserTypeError", "Home", new { method = "Index", controller = "Utilisateurs" });

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var utilisateur = db.Utilisateurs.Find(id.Replace('~', '.'));

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
                return RedirectToAction("BadUserTypeError", "Home", new { method = "Index", controller = "Utilisateurs" });

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
            id = id.Replace("~", ".");
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            var userType = db.ObtenirUtilisateur(id).Type;
            if ((userType == TypeUtilisateur.SA && id != HttpContext.User.Identity.Name) || type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA && HttpContext.User.Identity.Name != id)
                return RedirectToAction("BadUserTypeError", "Home", new { method = "Index", controller = "Home" });

            var utilisateur = db.Utilisateurs.Find(id.Replace('~', '.'));

            ViewBag.lieu = db.Lieux.Find(utilisateur.LieuID);

            utilisateur.Telephones = db.Telephones.Where(t => t.UtilisateurID == utilisateur.ID).ToList();

            if (utilisateur == null)
                return HttpNotFound();

            ViewBag.NbDevis = db.Devis.Where(devis => devis.UtilisateurID == utilisateur.ID).Count();
            ViewBag.NbFactures = db.Factures.Where(facture => facture.UtilisateurID == utilisateur.ID).Count();
            ViewBag.NbProduits = db.Produits.Count();


            return new ViewAsPdf("UtilisateurToPdf", utilisateur);
        }

        public ActionResult UtilisateurToPdf(string id)
        {
            id = id.Replace("~", ".");
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            var userType = db.ObtenirUtilisateur(id).Type;
            if ((userType == TypeUtilisateur.SA && id != HttpContext.User.Identity.Name) || type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA && HttpContext.User.Identity.Name != id)
                return RedirectToAction("BadUserTypeError", "Home", new { method = "Index", controller = "Home" });

            var utilisateur = db.Utilisateurs.Find(id.Replace('~', '.'));

            ViewBag.lieu = db.Lieux.Find(utilisateur.LieuID);

            utilisateur.Telephones = db.Telephones.Where(t => t.UtilisateurID == utilisateur.ID).ToList();

            if (utilisateur == null)
                return HttpNotFound();

            ViewBag.NbDevis = db.Devis.Where(devis => devis.UtilisateurID == utilisateur.ID).Count();
            ViewBag.NbFactures = db.Factures.Where(facture => facture.UtilisateurID == utilisateur.ID).Count();
            ViewBag.NbProduits = db.Produits.Count();


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

                var usersTrie = SortOrder(users, sortOrder);

                if (!String.IsNullOrEmpty(searchstring))
                    return View(usersTrie.Where(s => s.ID.ToUpper().Contains(searchstring.ToUpper())).ToPagedList((page ?? 1), param.NbElementPage));
                return new ViewAsPdf("ListToPdf", usersTrie.ToPagedList((page ?? 1), param.NbElementPage));
            }
            return RedirectToAction("BadUserTypeError", "Home", new { method = "Index", controller = "Home" });
        }

        public ActionResult ListToPdf(string sortOrder, String searchstring, string currentFilter, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            if (user.Type != TypeUtilisateur.Administrateur && user.Type != TypeUtilisateur.SA)
                return RedirectToAction("BadUserTypeError", "Home", new { method = "Index", controller = "Home" });

            var param = db.Parametres.Find(user.ParametreID);
            var users = db.Utilisateurs.ToList();

            if (searchstring != null) page = 1;
            else searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;
            ViewBag.CurrentSort = sortOrder;

            var usersTrie = SortOrder(users, sortOrder);

            if (!String.IsNullOrEmpty(searchstring))
                return View(usersTrie.Where(s => s.ID.ToUpper().Contains(searchstring.ToUpper())).ToPagedList((page ?? 1), param.NbElementPage));
            return View(usersTrie.ToPagedList((page ?? 1), param.NbElementPage));
        }

        private IOrderedEnumerable<Utilisateur> SortOrder(List<Utilisateur> users, string sortOrder)
        {
            switch (sortOrder)
            {
                case "mailZA":
                    return users.OrderByDescending(s => s.ID);
                case "nomAZ":
                    return users.OrderBy(s => s.Nom);
                case "nomZA":
                    return users.OrderByDescending(s => s.Nom);
                case "prénomAZ":
                    return users.OrderBy(s => s.Prénom);
                case "prénomZA":
                    return users.OrderByDescending(s => s.Prénom);
                case "typeAdmin":
                    return users.OrderBy(s => s.Type);
                case "typeBasique":
                    return users.OrderByDescending(s => s.Type);
            }

            return users.OrderBy(s => s.ID);
        }
    }
}
