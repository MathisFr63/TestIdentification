using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebApplication1.Models.Account;

namespace WebApplication1.Models.Entite
{
    /// <summary>
    /// Classe représentant les données d'une entreprise.
    /// </summary>
    public class Entreprise
    {
        // Identifiant permettant d'identifier l'entreprise dans la base de données.
        public int ID { get; set; }

        // Identifiant de l'utilisateur possédant l'entreprise
        public int UtilisateurID { get; set; }
        // Uilisateur possédant l'entreprise
        public virtual Utilisateur Utilisateur { get; set; }

        // Type d'entreprise
        public TypeEntreprise Type { get; set; }

        // Nom de l'entreprise
        [Display(Name = "Nom de l'entreprise")]
        public string NomEntreprise { get; set; }

        // Nom du contact dans l'entreprise
        [Display(Name = "Nom du contact")]
        public string NomContact { get; set; }

        // Mail de l'entreprise afin de la contacter
        [Display(Name = "Adresse e-mail")]
        public string Mail { get; set; }

        // Liste des téléphones de l'entreprise
        public List<Telephone> Telephones { get; set; }

        // Lieu de l'entreprise (emplacement géographique)
        public Lieu Lieu { get; set; }

        /// <summary>
        /// Méthode permettant de retranscrire les données de l'entreprise sous forme de chaîne de caractères.
        /// </summary>
        /// <returns>string contenant les informations de l'entreprise</returns>
        public override string ToString()
        {
            return NomEntreprise + " | " + Mail + " | " + Telephones.ToString();
        }
    }
}