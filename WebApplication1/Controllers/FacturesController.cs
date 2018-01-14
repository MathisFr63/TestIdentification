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

namespace WebApplication1.Controllers
{
    public class FacturesController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Factures
        public ActionResult Index()
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            var factures = db.Factures.Where(facture => facture.UtilisateurID == user.ID).ToList();

            factures.ForEach(facture => {
                facture.Articles = new Dictionary<Article, int>();
                db.FactureArticle.Where(FA => FA.FactureID == facture.ID).ToList()
                   .ForEach(FA => facture.Articles.Add(db.Articles.Find(FA.ArticleID), FA.Quantite));
            });

            return View(factures);
        }
        // GET: Factures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Facture facture = db.Factures.Find(id);

            if (facture == null) return HttpNotFound();

            return View(facture);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
