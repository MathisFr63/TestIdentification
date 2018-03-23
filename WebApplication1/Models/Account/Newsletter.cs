using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Account
{
    /// <summary>
    /// Classe permettant de gérer les newsletter des utilisateurs.
    /// </summary>
    public class Newsletter
    {
        public string Objet { get; set; }
        public string Contenu { get; set; }
        public string date { get; set; }

        public Newsletter() {}

        public Newsletter(string Objet, string Contenu)
        {
            this.Objet = Objet;
            this.Contenu = Contenu;
        }
    }
}