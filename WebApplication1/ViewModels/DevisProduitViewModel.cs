using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models.Account;
using WebApplication1.Models.Papiers;

namespace WebApplication1.ViewModels
{
    /// <summary>
    /// ViewModel permettant de créer un devis
    /// </summary>
    public class DevisProduitViewModel
    {
        // Contexte de l'application permettant de récupérer les données en base.
        private ApplicationContext db = new ApplicationContext();

        // Devis à créer
        public Devis Devis { get; set; }
        public DonneeProduit DonneeProduit { get; set; }
        //public List<Utilisateur> Users { get; set; }
        // Liste des produits sélectionnés afin de les ajouter au devis
        public List<Produit> Produits { get; set; }
        // Liste des données produits que l'on souhaite ajouter au devis
        public List<DonneeProduit> ProduitsSelected { get; set; }

        /// <summary>
        /// Constructeur par défaut d'un DevisProduitViewModel afin de charger les produits et de pouvoir les sélectionner sur la page de création d'un devis
        /// </summary>
        public DevisProduitViewModel()
        {
            /*Users = db.Utilisateurs.ToList();
            List<Utilisateur> tmp = Users;
            foreach(Utilisateur u in tmp)
            {
                if(u.Type == TypeUtilisateur.SA || u.Type == TypeUtilisateur.Administrateur || u.Type == TypeUtilisateur.EnAttente)
                {
                    Users.Remove(u);
                }
            }*/
            Produits = db.Produits.ToList();//.ForEach(a => Produits.Add(new SelectListItem { Text = a.Libelle, Value = a.ID.ToString() }));
        }

        public DevisProduitViewModel(string userID)
        {
            this.Devis = new Devis { UtilisateurID = userID };
            Produits = db.Produits.ToList();
        }

        /// <summary>
        /// Constructeur d'une DevisProduitViewModel permettant de construire un devisproduit après avoir sélectionner les produits souhaités
        /// </summary>
        /// <param name="produits"></param>
        public DevisProduitViewModel(List<DonneeProduit> produits) : this()
        {
            ProduitsSelected = produits;
        }
    }
}
