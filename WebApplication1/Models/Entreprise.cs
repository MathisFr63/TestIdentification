﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Entreprise
    {
        public int ID { get; private set; }

        [ForeignKey("Utilisateur")]
        public int UtilisateurID { get; set; }

        public string Nom { get; set; }

        public string SiteWeb { get; set; }

        public string Mail { get; set; }

        public List<Telephone> Telephones { get; set; }

        public string Commentaire { get; set; }

        public Utilisateur Utilisateur { get; set; }

        //public Lieu Lieu { get; set; }

        //public List<Devis> Devis { get; set; }

        public override string ToString()
        {
            return Nom + (String.IsNullOrWhiteSpace(SiteWeb) ? ("(" + SiteWeb + ")") : "") + " : " + Mail + " | " + Telephones.ToString() + "\nCommentaire : " + Commentaire;
        }
    }
}