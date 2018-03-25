using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Papiers
{
    /// <summary>
    /// Classe permettant de représenter un document (devis ou facture).
    /// </summary>
    public abstract class Document
    {
        [Display(Name = "Numéro")]
        public string Identifiant { get; set; }

        // Identifiant permettant de retrouver le document dans la base de données.
        public int ID { get; private set; }

        // Objet du document
        // Date de création du document
        public DateTime Date { get; set; }

        public string DateString
        {
            get
            {
                return string.Format("{0:d/M/yyyy HH:mm}", Date);
            }
        }

        // Commentaire lié au document afin de mieux l'identifier.
        [Display(Name = "Autres informations")]
        public string Commentaire { get; set; }

        // Monnaie utilisée pour le paiement du document.
        //public TypeMonnaie Monnaie { get; set; }

        // Produits liés au document
        public ICollection<DonneeProduit> Produits { get; set; }

        // Identifiant de l'utilisateur ayant créée le document.
        public string UtilisateurID { get; set; }

        //Destinataire du document.
        public string ClientID { get; set; }

        /// <summary>
        /// Constructeur par défaut d'un document.
        /// </summary>
        public Document() { }


        /// <summary>
        /// Constructeur d'un document prenant toutes les variables en paramètres.
        /// </summary>
        /// <param name="objet">Objet du document</param>
        /// <param name="commentaire">Commentaire permettant d'ajouter une description au document</param>
        /// <param name="monnaie">Monnaie utilisée pour le règlement du document</param>
        /// <param name="produits">Produits contenus dans le document</param>
        /// <param name="utilisateurID">Identifiant de l'utilisateur ayant créé le document.</param>
        public Document(string type, int nbMois, string commentaire, /*TypeMonnaie monnaie,*/ ICollection<DonneeProduit> produits, string utilisateurID, string clientID)
        {
            Identifiant = $"{type}{string.Format("{0:yyyyMM}", DateTime.Now)}{string.Format("{0:0000}", nbMois + 1)}";

            Date = DateTime.Now;
            Commentaire = commentaire;
            //Monnaie = monnaie;
            Produits = produits;
            UtilisateurID = utilisateurID;
            ClientID = clientID;
        }
    }
}