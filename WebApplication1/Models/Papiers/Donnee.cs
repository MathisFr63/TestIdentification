using System;
using System.Collections.Generic;

namespace WebApplication1.Models.Papiers
{
    public class Donnee
    {
        public int ID { get; private set; }

        public string Objet { get; set; }

        public DateTime Date { get; set; }

        public string Commentaire { get; set; }

        public TypeMonnaie Monnaie { get; set; }

        public ICollection<DonneeProduit> Produits { get; set; }

        //public int EntrepriseID { get; set; }

        public string UtilisateurID { get; set; }
    }
}