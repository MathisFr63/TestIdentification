using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.Compte;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        [HttpPost]
        public ActionResult Contact(Feedback feedback)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("afiacrocus@gmail.com", "Feedback Easybill");
            mail.To.Add(feedback.email);
            //mail.IsBodyHtml = true;
            mail.Subject = "Feedback de "+ feedback.name;
            mail.Body = feedback.comment;
            mail.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new System.Net.NetworkCredential("afiacrocus@gmail.com", "projetTut1718");
            smtp.EnableSsl = true;
            smtp.Send(mail);

            return RedirectToAction("FeedbackSent");

        }

        public ActionResult FeedbackSent()
        {
            ViewBag.Message = "Votre message a bien été envoyé. Pour revenir à l'acceuil, cliquez sur le bouton ci-dessous.";
            return View();
        }
    }
}
