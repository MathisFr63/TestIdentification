using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        private string nom;
        public string Nom
        {
            get { return nom; }
            set { nom = value.ToUpper(); }
        }

        //public string Nom { get; set; }
        // Prénom de l'utilisateur.
        private string prénom;
        public string Prénom
        {
            get { return prénom; }
            set
            {
                TextInfo txtInfo = new CultureInfo("en-us", false).TextInfo;
                prénom = txtInfo.ToTitleCase(value).Replace(' ', '-');
            }
        }

        // Type de l'utilisateur (Administrateur ou enregistré).
        public TypeUtilisateur Type { get; set; }

        [Display(Name = "Code de récupération")]
        public string codeRecup { get; set; }


        public int ParametreID { get; set; }

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


        /// <summary>
        /// Constructeur d'un Utilisateur prenant son adresse e-mail, mot de passe, nom, prénom, type.
        /// </summary>
        /// <param name="identifiant">Identifiant de l'utilisateur (son adresse mail)</param>
        /// <param name="motDePasse">Mot de passe de l'utilisateur</param>
        /// <param name="prenom">Prénom de l'utilisateur</param>
        /// <param name="nom">Nom de l'utilisateur</param>
        /// <param name="type">Type de l'utilisateur</param>

        public Utilisateur(string identifiant, string motDePasse, string nom, string prenom, TypeUtilisateur type, Parametre parametre)
        {
            this.ID = identifiant;
            this.MotDePasse = motDePasse.GetHashCode();
            this.Prénom = prenom;
            this.Nom = nom;
            this.Type = type;

            this.ParametreID = parametre.ID;
            parametre.DefaultTextFeedback += $"<p>{prenom} {nom}</p>";
        }
    }
}