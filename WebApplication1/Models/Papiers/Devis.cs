using System;
using System.Collections.Generic;

namespace WebApplication1.Models.Papiers
{
    public class Devis
    {
        public int ID { get; private set; }

        public string Objet { get; set; }

        public DateTime Date { get; set; }

        public bool Valide { get; set; }

        public string Commentaire { get; set; }

        public TypeMonnaie Monnaie { get; set; }

        public Dictionary<Article, int> Articles { get; set; }
        
        public int EntrepriseID { get; set; }
        
        public int UtilisateurID { get; set; }

        // public int Total { get; set; } Prix calculé
    }
}