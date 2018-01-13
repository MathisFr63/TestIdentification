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
            List<Client> listTrie = new List<Client>();

            if (searchstring != null)
            {
                page = 1;
            }
            else
            {
                searchstring = currentFilter;
            }
            ViewBag.CurrentFilter = searchstring;

            int pageSize = 15;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchstring))
            {
                foreach(Client c in db.UtilisateurCourant.Clients)
                {
                    if (c.Nom.ToUpper().Contains(searchstring.ToUpper()))
                    {
                        listTrie.Add(c);
                    }
                }
                return View(listTrie.ToPagedList(pageNumber, pageSize));
            }
            else
                return View(db.UtilisateurCourant.Clients.ToPagedList(pageNumber, pageSize));
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
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
        public ActionResult Create([Bind(Include = "ID, Nom, SiteWeb, Mail, Commentaire, CodeNAF, SIREN_SIRET")] Client client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (int.TryParse(HttpContext.User.Identity.Name, out int tmp))
                    {
                        client.UtilisateurID = tmp;
                        db.Clients.Add(client);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Create");
                }
            }
            catch (DataException dex )
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
}

            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "Nom, SiteWeb, Mail, Commentaire")] Client client)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(client).State = EntityState.Modified;
                Client u = db.Clients.Find(id);
                u.Nom = client.Nom;
                u.SiteWeb = client.SiteWeb;
                u.Mail = client.Mail;
                u.Commentaire = client.Commentaire;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
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
    }
}
