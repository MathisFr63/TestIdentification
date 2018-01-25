using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebApplication1.Models.Compte;

namespace WebApplication1.Models.Entite
{
    public class Entreprise
    {
        public int ID { get; set; }

        public int UtilisateurID { get; set; }
        public virtual Utilisateur Utilisateur { get; set; }

        public TypeEntreprise Type { get; set; }

        public string NomEntreprise { get; set; }

        public string NomContact { get; set; }

        public string Mail { get; set; }

        public List<Telephone> Telephones { get; set; }

        public Lieu Lieu { get; set; }

        public override string ToString()
        {
            return NomEntreprise + " | " + Mail + " | " + Telephones.ToString();
        }
    }
}