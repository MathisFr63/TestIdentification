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
            return View(db.Factures.ToList());            
        }
        // GET: Factures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facture facture = db.Factures.Find(id);
            if (facture == null)
            {
                return HttpNotFound();
            }
            return View(facture);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        /* 
            var message = new MailMessage(new MailAddress("afiacrocus@gmail.com"), new MailAddress("afiacrocus@gmail.com"))
            {
                Subject = "Mon sujet",
                Body = "Message de mon email",
                IsBodyHtml = true
            };

            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("afiacrocus@gmail.com", "projetTut1718"),
                EnableSsl = false
            };
            smtp.Send(message);


            web.config
            <system.net>
                <mailSettings>
                    <smtp deliveryMethod="SpecifiedPickupDirectory">
                        <specifiedPickupDirectory pickupDirectoryLocation="C:\Mails\"/>
                    </smtp>
                </mailSettings>
            </system.net>
        */
    }
}
