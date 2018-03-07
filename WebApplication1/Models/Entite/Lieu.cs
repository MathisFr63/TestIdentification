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
        public string CodePostal { get; set; }

        // Ville correspondant au lieu
        public string Ville { get; set; }

        // Pays correspondant au lieu
        public string Pays { get; set; }


        /// <summary>
        /// Constructeur par défaut d'un lieu
        /// </summary>
        public Lieu()
        {
        }

        /// <summary>
        /// Constructeur d'un lieu prenant en paramètre une adresse, un complément, un code postal, une ville et un pays
        /// </summary>
        /// <param name="adresse">Adresse du lieu</param>
        /// <param name="complément">Complément du lieu</param>
        /// <param name="cp">Code postal du lieu</param>
        /// <param name="ville">Ville du lieu</param>
        /// <param name="pays">Pays du lieu</param>
        public Lieu(string adresse, string complément, string cp, string ville, string pays)
        {
            this.Adresse = adresse;
            this.Complement = complément;
            this.CodePostal = cp;
            this.Ville = ville;
            this.Pays = pays;
        }
    }
}