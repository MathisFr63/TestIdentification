using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Account
{
    /// <summary>
    /// Classe permettant de stocker l'adresse mail des utilisateurs en base de données (par exemple pour l'abonnement d'utilisateurs aux newsletter).
    /// </summary>
    public class AdresseMail
    {
        // Identifiant de la classe (une adresse mail)
        public string ID { get; set; }

        /// <summary>
        /// Constructeur par défaut d'une AdresseMail
        /// </summary>
        /// <param name="mail">Mail correspondant à l'adresse mail</param>
        public AdresseMail(string mail)
        {
            ID = mail;
        }
    }
}