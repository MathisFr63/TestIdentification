using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Papiers;
using PagedList;
using Rotativa;
using WebApplication1.ViewModels;
using WebApplication1.Models.Account;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Controller permettant la gestion des factures de l'utilisateur (affichage des factures (après recherche ou non)sur plusieurs pages, détails d'une facture).
    /// </summary>
    public class FacturesController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Factures
        // Méthode permettant grâce à l'accès par l'url d'afficher la liste des factures de l'utilisateur (après recherche ou non).
        public ActionResult Index(string sortOrder, String searchstring, string currentFilter, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var param = db.Parametres.Find(user.ParametreID);

            var factures = db.Factures.ToList();
            if (user.Type != TypeUtilisateur.SA && user.Type != TypeUtilisateur.Administrateur)
                factures = factures.Where(facture => facture.UtilisateurID == user.ID).ToList();

            factures.ForEach(facture => facture.Produits = db.DonneeProduit.Where(DP => DP.FactureID == facture.ID).ToList());

            if (searchstring != null)
                page = 1;
            else
                searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;
            ViewBag.CurrentSort = sortOrder;

            var listeTrie = SortOrder(factures, sortOrder);

            int pageSize = param.NbElementPage;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchstring))
                return View(listeTrie.Where(s => s.Objet.ToUpper().Contains(searchstring.ToUpper())).ToPagedList(pageNumber, pageSize));
            return View(listeTrie.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult RechercheAvancee(string Objet, string Date, string Produit, /*string Relance,*/ string Reglement, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var param = db.Parametres.Find(user.ParametreID);

            IEnumerable<Facture> myListTrier = db.Factures.Where(facture => facture.UtilisateurID == user.ID).ToList();
            myListTrier.ToList().ForEach(facture => facture.Produits = db.DonneeProduit.Where(DP => DP.FactureID == facture.ID).ToList());

            if (Objet != string.Empty)
                myListTrier = myListTrier.Where(s => s.Objet.ToUpper().Contains(Objet.ToUpper()));

            if (DateTime.TryParse(Date, out var date))
                myListTrier = myListTrier.Where(s => s.Date.ToString("MMMM dd yyyy") == date.ToString("MMMM dd yyyy"));

            if (Produit != null)
                myListTrier = myListTrier.Where(d => {
                    return d.Produits.ToList().Any(x => x.Nom.ToUpper().Contains(Produit.ToUpper()));
                });

            //if (int.TryParse(Relance, out var relance))
                //myListTrier = myListTrier.Where(d => d.Relances == relance);

            if (Enum.TryParse<TypeReglement>(Reglement, out var reglement))
                myListTrier = myListTrier.Where(d => d.Reglement == reglement);

            return View("Index", myListTrier.ToPagedList((page ?? 1), param.NbElementPage));
        }

        // GET: Factures/Details/5
        // Méthode permettant grâce à l'accès par l'url d'afficher les détails de la facture sélectionnée
        public ActionResult Details(int? id/*, bool? erreurRelance*/)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var facture = db.Factures.Find(id);

            if (facture == null) return HttpNotFound();

            //if (erreurRelance != null && erreurRelance == true)
            //    ViewBag.ErreurRelance = true;
            

            return View(new FactureProduitViewModel(db.DonneeProduit.Where(DP => DP.FactureID == id).ToList()) { Facture = facture });
        }

        // GET: Factures/Details/5
        // Méthode permettant d'incrémenter le nombre de relances de la facture sélectionnée si le client n'a pas encore réglé la facture.
        //public ActionResult Relancer(int? id)
        //{
        //    if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        //    var facture = db.Factures.Find(id);

        //    if (facture == null) return HttpNotFound();
        //    var param = db.Parametres.Find(db.ObtenirUtilisateur(HttpContext.User.Identity.Name).ParametreID);
        //    if (facture.Relances < param.NbRelanceFacture)
        //    {
        //        facture.Relances++;
        //        db.SaveChanges();
        //       return RedirectToAction("Index");
        //    }

        //    return RedirectToAction("Details", new { id, @erreurRelance = true });
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }

        public ActionResult FactureToPdf(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Facture facture = db.Factures.Find(id);
            var user = db.Utilisateurs.Find(facture.UtilisateurID);
            var param = db.Parametres.Find(user.ParametreID);
            ViewBag.user = user;
            ViewBag.param = param;
            ViewBag.lieu = db.Lieux.Find(ViewBag.user.LieuID);

            if (facture == null) return HttpNotFound();

            string footer = "--footer-center \"" + param.FooterFacture + "\"" + " --footer-line --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";

            return new ViewAsPdf(new FactureProduitViewModel(db.DonneeProduit.Where(DP => DP.FactureID == id).ToList()) { Facture = facture })
            {
                CustomSwitches =  footer
            };
        }

        public ActionResult ListToPdf(string sortOrder, String searchstring, string currentFilter, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var param = db.Parametres.Find(user.ParametreID);

            var factures = db.Factures.ToList();
            if (user.Type != TypeUtilisateur.SA && user.Type != TypeUtilisateur.Administrateur)
                factures = factures.Where(facture => facture.UtilisateurID == user.ID).ToList();
            
            factures.ForEach(facture => facture.Produits = db.DonneeProduit.Where(DP => DP.FactureID == facture.ID).ToList());

            if (searchstring != null)
                page = 1;
            else
                searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;
            ViewBag.CurrentSort = sortOrder;

            var listeTrie = SortOrder(factures, sortOrder);            

            int pageSize = param.NbElementPage;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchstring))
                return new ViewAsPdf(listeTrie.Where(s => s.Objet.ToUpper().Contains(searchstring.ToUpper())).ToPagedList(pageNumber, pageSize));
            return new ViewAsPdf(listeTrie.ToPagedList(pageNumber, pageSize));
        }

        private IOrderedEnumerable<Facture> SortOrder(List<Facture> factures, string sortOrder)
        {
            switch (sortOrder)
            {
                case "objetZA":
                    return factures.OrderByDescending(s => s.Objet);
                case "dateOldNew":
                    return factures.OrderBy(s => s.Date);
                case "dateNewOld":
                    return factures.OrderByDescending(s => s.Date);
            }
            return factures.OrderBy(s => s.Objet);
        }
    }
}
