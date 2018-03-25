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
            if (user.Type != TypeUtilisateur.SA)
                if (user.Type != TypeUtilisateur.Administrateur)
                    ListDevis = ListDevis.Where(devis => devis.ClientID.ToUpper() == user.ID.ToUpper()).ToList();
                else
                    ListDevis = ListDevis.Where(devis => devis.UtilisateurID.ToUpper() == user.ID.ToUpper()).ToList();


            ListDevis.ForEach(devis =>
            {
                devis.Produits = db.DonneeProduit.Where(DP => DP.DevisID == devis.ID).ToList();
                if (devis.Etat == EtatDevis.EnCours)
                    if (!(devis.Date.AddDays(param.DureeValiditeDevis) >= DateTime.Today))
                        devis.Etat = EtatDevis.Rejeté;
            });


            db.SaveChanges();

            if (searchstring != null) page = 1;
            else searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;
            ViewBag.CurrentSort = sortOrder;

            var listeTrie = SortOrder(ListDevis, sortOrder);

            if (!String.IsNullOrEmpty(searchstring))
                return View(listeTrie.Where(s => s.Identifiant.Contains(searchstring.ToUpper())).ToPagedList((page ?? 1), param.NbElementPage));
            return View(listeTrie.ToPagedList((page ?? 1), param.NbElementPage));
        }

        public ActionResult RechercheAvancee(string Numéro, string User, string Date, string État, string Produits, string TotalTTC, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var param = db.Parametres.Find(user.ParametreID);

            IEnumerable<Devis> myListTrier = db.Devis.ToList();

            if (user.Type != TypeUtilisateur.SA && user.Type != TypeUtilisateur.Administrateur)
                myListTrier = myListTrier.Where(devis => devis.UtilisateurID == user.ID).ToList();

            myListTrier.ToList().ForEach(devis =>
            {
                devis.Produits = db.DonneeProduit.Where(DP => DP.DevisID == devis.ID).ToList();
                //devis.Valide = devis.Date.AddDays(param.DureeValiditeDevis) >= DateTime.Today;
            });

            if (!string.IsNullOrWhiteSpace(Numéro))
                myListTrier = myListTrier.Where(s => s.Identifiant.ToUpper().Contains(Numéro.ToUpper()));

            if (!string.IsNullOrWhiteSpace(User))
            {
                if (user.Type == TypeUtilisateur.Administrateur || user.Type == TypeUtilisateur.SA)
                    myListTrier = myListTrier.Where(s => s.ClientID.ToUpper().Contains(User.ToUpper()));
                else
                    myListTrier = myListTrier.Where(s => s.UtilisateurID.ToUpper().Contains(User.ToUpper()));
            }

            if (!string.IsNullOrWhiteSpace(Date))
                myListTrier = myListTrier.Where(s => string.Format("{0:d/M/yyyy HH:mm}", s.Date).Contains(Date.ToUpper()));

            if (!string.IsNullOrWhiteSpace(État))
            {
                myListTrier = myListTrier.Where(s => s.Etat.ToString().ToUpper().Contains(État.ToUpper()));
            }

            //if (bool.TryParse(Valide, out var valide))
            //    myListTrier = myListTrier.Where(s => s.Valide == valide);

            if (!string.IsNullOrWhiteSpace(Produits))
                myListTrier = myListTrier.Where(d =>
                {
                    return d.Produits.ToList().Any(x => x.Nom.ToUpper().Contains(Produits.ToUpper()));
                });

            if (double.TryParse(TotalTTC, out var totalTTC))
                myListTrier = myListTrier.Where(d =>
                {
                    double total = 0;
                    d.Produits.ToList().ForEach(x => total += x.TotalTTC);
                    return total == totalTTC;
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

            return View(new DevisProduitViewModel(HttpContext.User.Identity.Name));
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

            var user = db.ObtenirUtilisateur(vm.Devis.ClientID);
            if (user == null)
            {
                ModelState.AddModelError("Devis.ClientID", "L'adresse mail du client doit être valide");
            }

            if (!ModelState.IsValid)
                return View(vm);

            vm.Devis.Date = DateTime.Now;
            vm.Devis.Etat = EtatDevis.EnCours;

            var nbMois = 0;

            var lastDevis = db.Devis.OrderByDescending(f => f.Identifiant).FirstOrDefault();
            if (lastDevis != null && lastDevis.Date.Year == DateTime.Now.Year && lastDevis.Date.Month == DateTime.Now.Month)
                nbMois = int.Parse(lastDevis.Identifiant.Substring(lastDevis.Identifiant.Length - 4));

            vm.Devis.Identifiant = $"D{string.Format("{0:yyyyMM}", DateTime.Now)}{string.Format("{0:0000}", nbMois + 1)}";

            db.Devis.Add(vm.Devis);

            var keys = Request.Form.AllKeys;
            for (int i = 4; i < keys.Length; i++)
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
            if (devis.Etat == EtatDevis.Facturé)
            {
                return RedirectToAction("Index");
            }

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
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var param = db.Parametres.Find(user.ParametreID);
            if (user.Type != TypeUtilisateur.Administrateur && user.Type != TypeUtilisateur.SA)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var devis = db.Devis.Find(id);
            devis.Produits = new List<DonneeProduit>();

            db.DonneeProduit.RemoveRange(db.DonneeProduit.Where(dp => dp.DevisID == id));

            var form = Request.Form;
            var keys = form.AllKeys;
            for (int i = 4; i < keys.Length; i++)
            {
                var name = keys[i];
                var produit = db.Produits.First(p => p.Libelle == name);

                db.DonneeProduit.Add(new DonneeProduit(produit)
                {
                    Quantite = int.Parse(form.GetValues(keys[i])[0]),
                    DevisID = devis.ID
                });
            }

            //devis.Monnaie = (TypeMonnaie)Enum.Parse(typeof(TypeMonnaie), form.GetValues(keys[3])[0]);
            devis.Commentaire = form.GetValues(keys[3])[0];
            devis.Date = DateTime.Now;
            devis.Etat = (devis.Date.AddDays(param.DureeValiditeDevis) >= DateTime.Today) ? EtatDevis.EnCours : EtatDevis.Rejeté;
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

            double totalHT = 0;
            double totalTTC = 0;

            db.DonneeProduit.Where(dp => dp.DevisID == id).ToList().ForEach(p => { totalHT += p.TotalHT; totalTTC += p.TotalTTC; });

            ViewBag.totalHT = totalHT;
            ViewBag.totalTTC = totalTTC;

            var date = DateTime.Today.AddMonths(-1);
            int nbMois = db.Factures.Where(f => f.Date > date).ToList().Count;
            return View(new Facture(devis, nbMois));
        }

        // POST: Devis/Facturer
        [HttpPost, ActionName("Facturer")]
        [ValidateAntiForgeryToken]
        // Méthode permettant de facturer un devis c'est à dire d'ajouter une facture de ce devis dans sa liste des factures après avoir spécifié le type de réglement.
        public ActionResult Facturer(int id, TypeReglement reglement)
        {
            int nbMois = db.Factures.Where(f => f.Date.Month == DateTime.Today.Month).ToList().Count;

            var devis = db.Devis.Find(id);
            var facture = new Facture(devis, nbMois, reglement);
            db.Factures.Add(facture);
            devis.Etat = EtatDevis.Facturé;
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
            var param = db.Parametres.Find(user.ParametreID);
            var client = db.Utilisateurs.Find(devis.ClientID);
            ViewBag.user = user;
            ViewBag.client = client;
            ViewBag.param = param;
            ViewBag.lieu = db.Lieux.Find(user.LieuID);
            ViewBag.lieuC = db.Lieux.Find(client.LieuID);


            if (devis == null) return HttpNotFound();

            string footer = "--footer-center \"" + param.FooterDevis + "\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";

            return new ViewAsPdf(new DevisProduitViewModel(db.DonneeProduit.Where(DP => DP.DevisID == id).ToList()) { Devis = devis })
            {
                CustomSwitches = footer
            };
        }

        public ActionResult ListToPdf(string sortOrder, string searchstring, string currentFilter, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var param = db.Parametres.Find(user.ParametreID);
            var ListDevis = db.Devis.ToList();
            if (user.Type != TypeUtilisateur.SA && user.Type != TypeUtilisateur.Administrateur)
                ListDevis = ListDevis.Where(devis => devis.UtilisateurID == user.ID).ToList();

            ListDevis.ForEach(devis =>
            {
                devis.Produits = db.DonneeProduit.Where(DP => DP.DevisID == devis.ID).ToList();
                if (devis.Etat == EtatDevis.EnCours)
                    if (!(devis.Date.AddDays(param.DureeValiditeDevis) >= DateTime.Today))
                        devis.Etat = EtatDevis.Rejeté;
            });


            db.SaveChanges();

            if (searchstring != null) page = 1;
            else searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;
            ViewBag.CurrentSort = sortOrder;

            var listeTrie = SortOrder(ListDevis, sortOrder);

            if (!string.IsNullOrEmpty(searchstring))
                return new ViewAsPdf(listeTrie.Where(s => s.Identifiant.ToUpper().Contains(searchstring.ToUpper())).ToPagedList((page ?? 1), param.NbElementPage));
            return new ViewAsPdf(listeTrie.ToPagedList((page ?? 1), param.NbElementPage));
        }

        private IOrderedEnumerable<Devis> SortOrder(List<Devis> ListDevis, string sortOrder)
        {
            switch (sortOrder)
            {
                case "numeroZA":
                    return ListDevis.OrderByDescending(s => s.Identifiant);
                case "dateOldNew":
                    return ListDevis.OrderBy(s => s.Date);
                case "dateNewOld":
                    return ListDevis.OrderByDescending(s => s.Date);
            }

            return ListDevis.OrderBy(s => s.Identifiant);
        }
    }
}
