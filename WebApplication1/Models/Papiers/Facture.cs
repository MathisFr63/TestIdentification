using System;

namespace WebApplication1.Models.Papiers
{
    /// <summary>
    /// Classe représentant les données d'une facture
    /// </summary>
    public class Facture : Document
    {
        // Nombre de relances de la facture (au bout de 3 relances, des poursuites peuvent être engagées) désigne le nombre de fois que la facture a été retransmise car l'entreprise n'avait pas payée.
        public int Relances { get; set; }

        // Type de réglement de la facture
        public TypeReglement Reglement { get; set; }

        /// <summary>
        /// Constructeur par défaut d'une facture
        /// </summary>
        public Facture() { }


        // Je suis pas sûr qu'on ai le droit de faire ça là vu que faut forcément qu'on indique le type de règlement sur une facture.
        /// <summary>
        /// Constructeur d'une facture prenant en paramètre un devis que l'on souhaite facturer
        /// </summary>
        /// <param name="devis">Devis que l'on souhaite facturer</param>
        public Facture(Devis devis)
        {
            Objet = devis.Objet;
            Date = DateTime.Now;
            Commentaire = devis.Commentaire;
            Monnaie = devis.Monnaie;
            Produits = devis.Produits;
            UtilisateurID = devis.UtilisateurID;

            Relances = 0;
        }

        /// <summary>
        /// Constructeur d'une facture prenant en paramètre un devis et un type de règlement
        /// </summary>
        /// <param name="devis">Devis que l'on souhaite facturer</param>
        /// <param name="type">Type de règlement de la facture</param>
        public Facture(Devis devis, TypeReglement type) : this(devis)
        {
            Reglement = type;
        }
    }
}