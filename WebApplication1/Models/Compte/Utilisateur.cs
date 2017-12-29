using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models.Entreprise;

namespace WebApplication1.Models.Compte
{
    public class Utilisateur
    {
        //[Required]
        public int ID { get; private set; }
        //[Required]
        public string Identifiant { get; set; }
        //[Required]
        [Display(Name = "Mot de passe")]
        public string MotDePasse { get; set; }
        //[Required]
        public string Nom { get; set; }
        //[Required]
        public string Prénom { get; set; }
        public string Mail { get; set; }
        public TypeUtilisateur Type { get; set; }
        public Lieu Lieu { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Fournisseur> Fournisseurs { get; set; }

        public override string ToString()
        {
            return Identifiant + " " + MotDePasse;
        }
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        } 
    }
}