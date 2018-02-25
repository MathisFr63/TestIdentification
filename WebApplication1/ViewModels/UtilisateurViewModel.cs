using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models.Account;

namespace WebApplication1.ViewModels
{
    /// <summary>
    /// ViewModel permettant de créer un utilisateur
    /// </summary>
    public class UtilisateurViewModel
    {
        // Utilisateur à créer
        public Utilisateur Utilisateur { get; set; }
        // Mot de passe de l'utilisateur a créer
        [DisplayName("Mot de passe")]
        [Required(ErrorMessage = "Le mot de passe est requis")]
        public string motDePasse { get; set; }
        // Booléen permettant de savoir si l'utilisateur est authentifié ou non
        public bool Authentifie { get; set; }
    }
}