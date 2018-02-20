using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Account
{
    /// <summary>
    /// Classe représentant un lieu
    /// </summary>
    public class Lieu
    {
        // Identifiant du lieu, permettant de l'identifier dans la base de données.
        public int ID { get; private set; }
   
        // Adresse du lieu (rue)
        public string Adresse { get; set; }

        // Complément d'adresse du lieu (bis)
        public string Complement { get; set; }

        // Code postal du lieu
        public int CodePostal { get; set; }

        // Ville correspondant au lieu
        public string Ville { get; set; }

        // Pays correspondant au lieu
        public string Pays { get; set; }
    }
}