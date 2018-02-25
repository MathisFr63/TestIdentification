using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models.Entite;

namespace WebApplication1.Models.Account
{
    /// <summary>
    /// Classe permettant de regrouper toutes les données d'un utilisateur.
    /// </summary>
    public class Utilisateur
    {
        // Identifiant de l'utilisateur (son adresse mail) afin de l'identifier dans la base de données.
        [DisplayName("Adresse e-mail")]
        public string ID { get; set; }
        // Mot de passe de l'uitlisateur lui permettant de se connecter.
        [DisplayName("Mot de passe")]
        public int MotDePasse { get; set; }

        // Nom de l'utilisateur.
        public string Nom { get; set; }
        // Prénom de l'utilisateur.
        public string Prénom { get; set; }

        // Type de l'utilisateur (Administrateur ou enregistré).
        public TypeUtilisateur Type { get; set; }

        // Question qui sera posée à l'utilisateur lors de la récupération de son mot de passe.
        public string Question { get; set; }
        // Réponse correspondant à la question qui lui sera posée lors de la récupération de son mot de passe.
        public string Réponse { get; set; }

        //public virtual ICollection<Entreprise> Entreprises { get; set; }

        /// <summary>
        /// Constructeur par défaut d'un utilisateur
        /// </summary>
        public Utilisateur()
        {
        }

        ///// <param name="prenom">Prénom de l'utilisateur</param>
        ///// <param name="nom">Nom de l'utilisateur</param>
        ///// <param name="type">Type d'utilisateur</param>
        ///// <param name="question">Question pour retrouver le mot de passe de l'utilisateur</param>
        ///// <param name="reponse">Réponse à la question pour retrouver le mot de passe</param>

        /// <summary>
        /// Constructeur d'un Utilisateur prenant son adresse e-mail, mot de passe, nom, prénom, type, sa question et sa réponse.
        /// </summary>
        /// <param name="identifiant">Identifiant de l'utilisateur (son adresse mail)</param>
        /// <param name="motDePasse">Mot de passe de l'utilisateur</param>
        /// <param name="prenom">Prénom de l'utilisateur</param>
        /// <param name="nom">Nom de l'utilisateur</param>
        /// <param name="type">Type de l'utilisateur</param>
        /// <param name="question">Question à laquelle il souhait répondre pour récupérer son mot de passe</param>
        /// <param name="reponse">Réponse à la question qui lui sera posée</param>
        public Utilisateur(string identifiant, string motDePasse, string prenom, string nom, TypeUtilisateur type, string question, string reponse)
        {
            this.ID = identifiant;
            this.MotDePasse = motDePasse.GetHashCode();
            this.Prénom = prenom;
            this.Nom = nom;
            this.Type = type;
            this.Question = question;
            this.Réponse = reponse;
        }
    }
}