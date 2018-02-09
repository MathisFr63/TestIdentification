using System;

namespace WebApplication1.Models.Account
{
    public class Feedback
    {
        public int ID { get; private set; }

        public String Name { get; set; }

        public String Email { get; set; }

        public String Comment { get; set; }

        public String Etat { get; set; }

        public int UtilisateurID { get; set; }
    }
}