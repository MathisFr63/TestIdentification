using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
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

        public List<Client> Clients { get; set; } = new List<Client>();

        public override string ToString()
        {
            return Identifiant + " " + MotDePasse;
        }
    }
}