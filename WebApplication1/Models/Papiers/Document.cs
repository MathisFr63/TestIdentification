using System;
using System.Collections.Generic;

namespace WebApplication1.Models.Papiers
{
    /// <summary>
    /// Classe permettant de représenter un document (devis ou facture).
    /// </summary>
    public abstract class Document
    {
        // Identifiant permettant de retrouver le document dans la base de données.
        public int ID { get; private set; }

        // Objet du document
        public string Objet { get; set; }

        // Date de création du document
        public DateTime Date { get; set; }

        // Commentaire lié au document afin de mieux l'identifier.
        public string Commentaire { get; set; }

        // Monnaie utilisée pour le paiement du document.
        public TypeMonnaie Monnaie { get; set; }

        // Produits liés au document
        public ICollection<DonneeProduit> Produits { get; set; }

        // Entreprise pour laquelle ou par qui a était fait le document.
        //public int EntrepriseID { get; set; }

        // Identifiant de l'utilisateur ayant créée le document.
        public string UtilisateurID { get; set; }
    }
}