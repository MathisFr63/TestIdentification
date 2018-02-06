using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models.Entite;

namespace WebApplication1.Models.Compte
{
    public class Utilisateur
    {
        public int ID { get; set; }

        public string Nom { get; set; }
        public string Prénom { get; set; }

        public string Mail { get; set; }

        public TypeUtilisateur Type { get; set; }

        [Display(Name = "Mot de passe")]
        public int MotDePasse { get; set; }
        
        public string Question { get; set; } //Question permettant de retrouver son mot de passe { get; }
        public string Réponse { get; set; } //Réponse à la question permettant de retrouver son mot de passe { get; }

        //public virtual ICollection<Entreprise> Entreprises { get; set; }

        /// <summary>
        /// Constructeur par défaut d'un utilisateur
        /// </summary>
        public Utilisateur()
        {
        }

        /// <summary>
        /// Constructeur d'un Utilisateur prenant son adresse e-mail, mot de passe, nom, prénom, type, sa question et sa réponse.
        /// </summary>
        /// <param name="motDePasse">Mot de passe de l'utilisateur</param>
        /// <param name="prenom">Prénom de l'utilisateur</param>
        /// <param name="nom">Nom de l'utilisateur</param>
        /// <param name="mail">Adresse e-mail de l'utilisateur</param>
        /// <param name="type">Type d'utilisateur</param>
        /// <param name="question">Question pour retrouver le mot de passe de l'utilisateur</param>
        /// <param name="reponse">Réponse à la question pour retrouver le mot de passe</param>
        public Utilisateur(string mail, string motDePasse, string prenom, string nom, TypeUtilisateur type, string question, string reponse)
        {
            this.Mail = mail;
            this.MotDePasse = motDePasse.GetHashCode();
            this.Prénom = prenom;
            this.Nom = nom;
            this.Type = type;
            this.Question = question;
            this.Réponse = reponse;
        }

        //public override int GetHashCode()
        //{
        //    return ID.GetHashCode();
        //} 
    }
}