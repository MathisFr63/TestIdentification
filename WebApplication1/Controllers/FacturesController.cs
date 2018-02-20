using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Papiers;
using PagedList;

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
        public ActionResult Index(String searchstring, string currentFilter, int? page)
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var factures = db.Factures.Where(facture => facture.UtilisateurID == user.ID).ToList();

            factures.ForEach(facture => facture.Produits = db.DonneeProduit.Where(DP => DP.FactureID == facture.ID).ToList());

            if (searchstring != null)
                page = 1;
            else
                searchstring = currentFilter;

            ViewBag.CurrentFilter = searchstring;

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchstring))
            {
                return View(factures.Where(s => s.Objet.ToUpper().Contains(searchstring.ToUpper())).ToPagedList(pageNumber, pageSize));
            }
            else
                return View(factures.ToPagedList(pageNumber, pageSize));
        }

        // GET: Factures/Details/5
        // Méthode permettant grâce à l'accès par l'url d'afficher les détails de la facture sélectionnée
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Facture facture = db.Factures.Find(id);

            if (facture == null) return HttpNotFound();

            return View(facture);
        }

        // GET: Factures/Details/5
        // Méthode permettant d'incrémenter le nombre de relances de la facture sélectionnée si le client n'a pas encore réglé la facture.
        public ActionResult Relancer(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Facture facture = db.Factures.Find(id);

            if (facture == null) return HttpNotFound();

            facture.Relances++;
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
