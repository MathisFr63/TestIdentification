using System;
using System.Collections.Generic;

namespace WebApplication1.Models.Papiers
{
    public class Donnee
    {
        public string Objet { get; set; }

        public DateTime Date { get; set; }

        public string Commentaire { get; set; }

        public TypeMonnaie Monnaie { get; set; }

        public Dictionary<Produit, int> Produits { get; set; }

        public int EntrepriseID { get; set; }

        public int UtilisateurID { get; set; }
    }
}