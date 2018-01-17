using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models.Compte;
using WebApplication1.Models.Entite;

namespace WebApplication1.Models.Papiers
{
    public class Donnee
    {
        public string Objet { get; set; }

        public DateTime Date { get; set; }

        public string Commentaire { get; set; }

        public TypeMonnaie Monnaie { get; set; }

        public Dictionary<Article, int> Articles { get; set; }

        public int EntrepriseID { get; set; }

        public int UtilisateurID { get; set; }
    }
}