using System;

namespace WebApplication1.Models.Account
{
    public class Feedback
    {
        public int ID { get; private set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Comment { get; set; }

        public string Etat { get; set; }

        public string UtilisateurID { get; set; }
    }
}