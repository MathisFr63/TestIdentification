using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Account
{
    public class Compte : AdresseMail
    //public class Compte
    {
        //public int ID { get; set; }

        //public string Mail { get; set; }

        [Display(Name = "Mot de passe")]
        public int MotDePasse { get; set; }

        public Compte(string mail, string motDePasse)
        {
            this.Mail = mail;
            this.MotDePasse = motDePasse.GetHashCode();
        }

        public Compte() { }
    }
}