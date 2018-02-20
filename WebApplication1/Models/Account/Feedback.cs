using System;

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
        public string Name { get; set; }

        // Email de l'utilisateur ayant envoyé le feedback.
        public string Email { get; set; }

        // Commentaire de l'utilisateur quant à son retour client.
        public string Comment { get; set; }

        // État du feedback ("en cours", ou "résolu") permettant de savoir s'il faut toujours réglé un problème.
        public string Etat { get; set; }

        // Identifiant de l'utilisateur ayant envoyé le feedback.
        public string UtilisateurID { get; set; }
    }
}