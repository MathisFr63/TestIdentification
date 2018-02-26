using System;
using System.Collections.Generic;

namespace WebApplication1.Models.Papiers
{
    /// <summary>
    /// Classe permettant de représenter les données d'un devis
    /// </summary>
    public class Devis : Document
    {
        // Booléen permettant de désigner si le devis est encore valide selon la durée écoulée
        public bool Valide { get; set; }

        //public int Total { get; set; } Prix calculé


        /// <summary>
        /// Constructeur par défaut d'un devis
        /// </summary>
        public Devis() { }

        /// <summary>
        /// Constructeur d'un devis prenant toutes les variables en paramètres.
        /// </summary>
        /// <param name="objet">Objet du devis</param>
        /// <param name="commentaire">Commentaire permettant d'ajouter une description au devis</param>
        /// <param name="monnaie">Monnaie utilisée lors du règlement du devis lors de sa facturation</param>
        /// <param name="produits">Produits contenus dans le devis</param>
        /// <param name="utilisateurID">Identifiant de l'utilisateur ayant créé le devis</param>
        public Devis(string objet, string commentaire, TypeMonnaie monnaie, ICollection<DonneeProduit> produits, string utilisateurID) : base(objet, commentaire, monnaie, produits, utilisateurID)
        {
            Valide = true;
        }
    }
}