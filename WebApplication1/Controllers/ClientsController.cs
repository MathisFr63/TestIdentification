using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Entite;
using PagedList;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Clients
        public ActionResult Index(String searchstring, string currentFilter, int? page)
        {
            db.UtilisateurCourant = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            List<Entreprise> listTrie = new List<Entreprise>();

            if (searchstring != null)
                page = 1;
            else
                searchstring = currentFilter;
            
            ViewBag.CurrentFilter = searchstring;

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchstring))
            {
                foreach(Entreprise e in db.UtilisateurCourant.Entreprises.Where(e => e.Type == TypeEntreprise.CLient))
                    if (e.NomEntreprise.ToUpper().Contains(searchstring.ToUpper()))
                        listTrie.Add(e);

                return View(listTrie.ToPagedList(pageNumber, pageSize));
            }
            else
                return View(db.UtilisateurCourant.Entreprises.Where(e => e.Type == TypeEntreprise.CLient).ToPagedList(pageNumber, pageSize));
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Entreprise entreprise = db.Entreprises.Find(id);

            if (entreprise == null) return HttpNotFound();
            
            return View(entreprise);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID, NomEntreprise, NomContact, Mail")] Entreprise entreprise)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (int.TryParse(HttpContext.User.Identity.Name, out int tmp))
                    {
                        entreprise.UtilisateurID = tmp;
                        entreprise.Type = TypeEntreprise.CLient;
                        db.Entreprises.Add(entreprise);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Create");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(entreprise);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Entreprise entreprise = db.Entreprises.Find(id);

            if (entreprise == null)
                return HttpNotFound();

            return View(entreprise);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "NomEntreprise, NomContact, Mail")] Entreprise entreprise)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(client).State = EntityState.Modified;
                Entreprise u = db.Entreprises.Find(id);
                u.NomEntreprise = entreprise.NomEntreprise;
                u.Mail = entreprise.Mail;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entreprise);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Entreprise entreprise = db.Entreprises.Find(id);

            if (entreprise == null)
                return HttpNotFound();

            return View(entreprise);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Entreprises.Remove(db.Entreprises.Find(id));
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
