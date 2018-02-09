using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.Account;

namespace WebApplication1.ViewModels
{
    public class UtilisateurViewModel
    {
        public Utilisateur Utilisateur { get; set; }
        public string motDePasse { get; set; }
        public bool Authentifie { get; set; }
    }
}