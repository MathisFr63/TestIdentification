using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using WebApplication1.DAL;

namespace WebApplication1.Controllers
{
    public class ParametresController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Parametres
        public ActionResult Index()
        {
            var user = db.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            return View(db.Parametres.Find(user.ParametreID));
        }

        // POST: Parametres/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Index(int id)
        {
            var parametre = db.Parametres.Find(id);
            if (TryUpdateModel(parametre, "", new string[] { "DureeValiditeDevis", "NbRelanceFacture", "Theme", "NbElementPage", "DefaultTextFeedback", "NbJourStat", "TailleHistorique", "Abonnee" }))
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
            return View(parametre);
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
