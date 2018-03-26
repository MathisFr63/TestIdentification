using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Account
{
    /// <summary>
    /// Classe permettant de gérer les newsletter des utilisateurs.
    /// </summary>
    public class Newsletter
    {
        [Display(Name = "Identifiant")]
        public int ID { get; private set; }
        [Display(Name ="Objet")]
        public string Objet { get; set; }
        [Display(Name = "Contenu")]
        public string Contenu { get; set; }
        [Display(Name = "Date")]
        public string Date { get; set; }

        public Newsletter() {}

        public Newsletter(string Objet, string Contenu)
        {
            this.Objet = Objet;
            this.Contenu = Contenu;
        }
    }
}