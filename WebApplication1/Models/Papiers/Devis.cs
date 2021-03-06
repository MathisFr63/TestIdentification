﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Papiers
{
    /// <summary>
    /// Classe permettant de représenter les données d'un devis
    /// </summary>
    public class Devis : Document
    {
        // Booléen permettant de désigner si le devis est encore valide selon la durée écoulée
        [Display(Name = "État")]
        public EtatDevis Etat{ get; set; }

        //public int Total { get; set; } Prix calculé


        /// <summary>
        /// Constructeur par défaut d'un devis
        /// </summary>
        public Devis()
        {}

        /// <summary>
        /// Constructeur d'un devis prenant toutes les variables en paramètres.
        /// </summary>
        /// <param name="commentaire">Commentaire permettant d'ajouter une description au devis</param>
        /// <param name="monnaie">Monnaie utilisée lors du règlement du devis lors de sa facturation</param>
        /// <param name="produits">Produits contenus dans le devis</param>
        /// <param name="utilisateurID">Identifiant de l'utilisateur ayant créé le devis</param>
        public Devis(int nbMois, string commentaire, /*TypeMonnaie monnaie,*/ ICollection<DonneeProduit> produits, string utilisateurID, string clientID) : base("D", nbMois, commentaire, /*monnaie,*/ produits, utilisateurID, clientID)
        {}
    }
}