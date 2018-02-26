using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModels
{
    public class UtilisateurViewModelConnection : UtilisateurViewModel
    {
        [DisplayName("Confirmation")]
        [Required(ErrorMessage = "Vous devez confirmer votre mot de passe")]
        [Compare("motDePasse", ErrorMessage = "La confirmation doit être identique au mot de passe")]
        public string confirmation { get; set; }
    }
}