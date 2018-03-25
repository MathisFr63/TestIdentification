using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Account
{
    /// <summary>
    /// Classe permettant de gérer les feedbacks des utilisateurs.
    /// </summary>
    public class Feedback
    {
        // Identifiant du feedback afin de pouvoir le retrouver.
        public int ID { get; private set; }

        // Nom du feedback permettant d'avoir un aperçu rapide de ce dont il est question.
        [Display(Name = "Sujet")]
        public string Subject { get; set; }

        // Commentaire de l'utilisateur quant à son retour client.
        [Display(Name = "Message")]
        public string Comment { get; set; }

        // État du feedback ("en cours", ou "résolu") permettant de savoir s'il faut toujours réglé un problème.
        [Display(Name = "État")]
        public bool IsResolved { get; set; }
        // Identifiant de l'utilisateur ayant envoyé le feedback.
        [Display(Name = "Adresse e-mail")]
        public string UtilisateurID { get; set; }

        //public string userName { get; }
        [Display(Name = "Identité")]
        public string userName { get; set; }


        /// <summary>
        /// Constructeur par défaut d'un feedback
        /// </summary>
        public Feedback() { }

        /// <summary>
        /// Constructeur d'un feedback prenant en paramètre un string pour remplir automatiquement le mail de l'utilisateur
        /// </summary>
        /// <param name="userID">Identifiant de la personne voulant envoyé le feedback</param>
        public Feedback(string userID, string name) {
            this.UtilisateurID = userID;
            this.userName = name;
        }
    }
}