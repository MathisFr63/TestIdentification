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
            mail.From = new MailAddress(feedback.email, "Feedback Easybill");
            mail.To.Add("afiacrocus@gmail.com");
            mail.IsBodyHtml = true;
            mail.Subject = "Feedback de "+ feedback.name;
            mail.Body = "Commentaire de : "+feedback.name+ "<br/>Email : " + feedback.email+ "<br/>Message : " + feedback.comment;
            mail.Priority = MailPriority.High;

            MailMessage mail2 = new MailMessage();
            mail2.From = new MailAddress("afiacrocus@gmail.com", "Résumé Feedback Easybill");
            mail2.To.Add(feedback.email);
            mail2.IsBodyHtml = true;
            mail2.Subject = "Votre Feedback";
            mail2.Body = "Le commentaire suivant a été transmis à notre équipe et sera traiter dès que possible : <br/><br/>" + feedback.comment+ "<br/><br/>Nous vous remercions pour votre commentaire. <br/>Cordialement, l'équipe d'EasyBill.";
            mail2.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new System.Net.NetworkCredential("afiacrocus@gmail.com", "projetTut1718");
            smtp.EnableSsl = true;
            smtp.Send(mail);
            smtp.Send(mail2);

            return RedirectToAction("FeedbackSent");

        }

        public ActionResult FeedbackSent()
        {
            ViewBag.Message = "Votre message a bien été envoyé. Pour revenir à l'acceuil, cliquez sur le bouton ci-dessous.";
            return View();
        }
    }
}
