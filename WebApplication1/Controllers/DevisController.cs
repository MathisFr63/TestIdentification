using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;
using WebApplication1.DAL;
using WebApplication1.Models.Papiers;
using WebApplication1.ViewModels;
using Rotativa;
using WebApplication1.Models.Account;

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
        public ActionResult Index(string sortOrder, String searchstring, string currentFilter, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var param = db.Parametres.Find(user.ParametreID);
            var ListDevis = db.Devis.ToList();
            if (user.Type != TypeUtilisateur.SA && user.Type != TypeUtilisateur.Administrateur)
                ListDevis = ListDevis.Where(devis => devis.UtilisateurID == user.ID).ToList();
            
            ListDevis.ForEach(devis =>
            {
                devis.Produits = db.DonneeProduit.Where(DP => DP.DevisID == devis.ID).ToList();
                devis.Valide = devis.Date.AddDays(param.DureeValiditeDevis) >= DateTime.Today;
            });

            db.SaveChanges();

            if (searchstring != null) page = 1;
            else searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;
            ViewBag.CurrentSort = sortOrder;

            var listeTrie = SortOrder(ListDevis, sortOrder);

            if (!String.IsNullOrEmpty(searchstring))
                return View(listeTrie.Where(s => s.Objet.ToUpper().Contains(searchstring.ToUpper())).ToPagedList((page ?? 1), param.NbElementPage));
            return View(listeTrie.ToPagedList((page ?? 1), param.NbElementPage));
        }

        public ActionResult RechercheAvancee(string Objet, string Date, string Valide, string Produit, string TotalHT, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var param = db.Parametres.Find(user.ParametreID);

            IEnumerable<Devis> myListTrier = db.Devis.ToList();

            if (user.Type != TypeUtilisateur.SA && user.Type != TypeUtilisateur.Administrateur)
                myListTrier = myListTrier.Where(devis => devis.UtilisateurID == user.ID).ToList();

            myListTrier.ToList().ForEach(devis =>
            {
                devis.Produits = db.DonneeProduit.Where(DP => DP.DevisID == devis.ID).ToList();
                devis.Valide = devis.Date.AddDays(param.DureeValiditeDevis) >= DateTime.Today;
            });

            if (Objet != string.Empty)
                myListTrier = myListTrier.Where(s => s.Objet.ToUpper().Contains(Objet.ToUpper()));

            if (DateTime.TryParse(Date, out var date))
                myListTrier = myListTrier.Where(s => s.Date.ToString("MMMM dd yyyy") == date.ToString("MMMM dd yyyy"));

            if (bool.TryParse(Valide, out var valide))
                myListTrier = myListTrier.Where(s => s.Valide == valide);

            if (Produit != null)
                myListTrier = myListTrier.Where(d =>
                {
                    return d.Produits.ToList().Any(x => x.Nom.ToUpper().Contains(Produit.ToUpper()));
                });

            if (int.TryParse(TotalHT, out var totalHT))
                myListTrier = myListTrier.Where(d =>
                {
                    double total = 0;
                    d.Produits.ToList().ForEach(x => total += x.PrixHT);
                    return total == totalHT;
                });

            return View("Index", myListTrier.ToPagedList((page ?? 1), param.NbElementPage));
        }

        // GET: Devis/Details/5
        // Méthode permettant grâce à l'accès par l'url d'afficher les détails d'un devis de l'utilisateur grâce à son id.
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var devis = db.Devis.Find(id);

            if (devis == null) return HttpNotFound();
            
            return View(new DevisProduitViewModel(db.DonneeProduit.Where(DP => DP.DevisID == id).ToList()) { Devis = devis });
        }

        // GET: Devis/Create
        // Méthode permettant à l'utilisateur d'ajouter un nouveau devis parmis sa liste grâce à l'accès par l'url.
        public ActionResult Create()
        {
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            if (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

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
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            if (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (!ModelState.IsValid)
                return View(vm);

            vm.Devis.Date = DateTime.Now;
            vm.Devis.Valide = true;

            db.Devis.Add(vm.Devis);

            var keys = Request.Form.AllKeys;
            for (int i = 5; i < keys.Length; i++)
            {
                var name = keys[i];
                var produit = db.Produits.First(p => p.Libelle == name);

                db.DonneeProduit.Add(new DonneeProduit(produit, int.Parse(Request.Form.GetValues(keys[i])[0]))
                {
                    DevisID = vm.Devis.ID
                });
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Devis/Edit/5
        // Méthode permettant grâce à l'accès par l'url de modifier le devis sélectionné en passant son id dans l'url.
        public ActionResult Edit(int? id)
        {
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            if (id == null || (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

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
        public ActionResult EditPost(int id)
        {
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            if (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var devis = db.Devis.Find(id);
            devis.Produits = new List<DonneeProduit>();

            db.DonneeProduit.RemoveRange(db.DonneeProduit.Where(dp => dp.DevisID == id));

            var form = Request.Form;
            var keys = form.AllKeys;
            for (int i = 5; i < keys.Length; i++)
            {
                var name = keys[i];
                var produit = db.Produits.First(p => p.Libelle == name);

                db.DonneeProduit.Add(new DonneeProduit(produit)
                {
                    Quantite = int.Parse(form.GetValues(keys[i])[0]),
                    DevisID = devis.ID
                });
            }

            devis.Objet = form.GetValues(keys[2])[0];
            devis.Monnaie = (TypeMonnaie)Enum.Parse(typeof(TypeMonnaie), form.GetValues(keys[3])[0]);
            devis.Commentaire = form.GetValues(keys[4])[0];
            devis.Date = DateTime.Now;
            devis.Valide = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Devis/Delete/5
        // Méthode permettant grâce à l'accès par l'url en passant l'id du devis d'afficher les détails de celui-ci afin de vérifier si l'utilisateur souhaite réellement le supprimer.
        public ActionResult Delete(int? id)
        {
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            if (id == null || (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

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
            var type = db.ObtenirUtilisateur(HttpContext.User.Identity.Name).Type;
            if (type != TypeUtilisateur.Administrateur && type != TypeUtilisateur.SA)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            db.DonneeProduit.RemoveRange(db.DonneeProduit.Where(DP => DP.DevisID == id));
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
                db.DonneeProduit.Add(new DonneeProduit(dp) { FactureID = facture.ID });
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

        public ActionResult DevisToPdf(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var devis = db.Devis.Find(id);
            var user = db.Utilisateurs.Find(devis.UtilisateurID);

            if (devis == null) return HttpNotFound();

            return new ViewAsPdf(new DevisProduitViewModel(db.DonneeProduit.Where(DP => DP.DevisID == id).ToList()) { Devis = devis });
        }

        public ActionResult ListToPdf(string sortOrder, string searchstring, string currentFilter, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var param = db.Parametres.Find(user.ParametreID);

            var ListDevis = db.Devis.ToList();
            if (user.Type != TypeUtilisateur.SA && user.Type != TypeUtilisateur.Administrateur)
                ListDevis = ListDevis.Where(devis => devis.UtilisateurID == user.ID).ToList();

            ListDevis.ForEach(devis => devis.Produits = db.DonneeProduit.Where(DP => DP.DevisID == devis.ID).ToList());

            if (searchstring != null) page = 1;
            else searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;
            ViewBag.CurrentSort = sortOrder;

            var listeTrie = SortOrder(ListDevis, sortOrder);

            if (!string.IsNullOrEmpty(searchstring))
                return new ViewAsPdf(listeTrie.Where(s => s.Objet.ToUpper().Contains(searchstring.ToUpper())).ToPagedList((page ?? 1), param.NbElementPage));
            return new ViewAsPdf(listeTrie.ToPagedList((page ?? 1), param.NbElementPage));
        }

        private IOrderedEnumerable<Devis> SortOrder(List<Devis> ListDevis, string sortOrder)
        {
            switch (sortOrder)
            {
                case "objetZA":
                    return ListDevis.OrderByDescending(s => s.Objet);
                case "dateOldNew":
                    return ListDevis.OrderBy(s => s.Date);
                case "dateNewOld":
                    return ListDevis.OrderByDescending(s => s.Date);
            }

            return ListDevis.OrderBy(s => s.Objet);
        }
    }
}
