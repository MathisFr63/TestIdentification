using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using WebApplication1.Models.Account;
using WebApplication1.Models.Entite;
using WebApplication1.Models.Papiers;

namespace WebApplication1.DAL
{
    /// <summary>
    /// Contexte de l'application permettant la gestion des données de la base par l'application.
    /// </summary>
    public class ApplicationContext : DbContext
    {
        // Utilisateur connecté à l'application durant son utilisation
        public Utilisateur UtilisateurCourant { get; set; }

        /// <summary>   
        /// Constructeur par défaut du contexte de l'application.
        /// </summary>
        public ApplicationContext() : base("ApplicationContext")
        {
        }

        // Liste des utilisateurs de l'application.
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        // Liste de tous les Paramètres des utilisateurs.
        public DbSet<Parametre> Parametres { get; set; }
        // Liste des adresses mails s'étant abonnées aux newsletters.
        public DbSet<AdresseMail> AdressesMail { get; set; }
        // Liste des devis de tous les utilisateurs.
        public DbSet<Devis> Devis { get; set; }
        // Liste des données de chaque produit en fonction de chaque devis.
        public DbSet<DonneeProduit> DonneeProduit { get; set; }
        // Liste des factures de tous les utilisateurs.
        public DbSet<Facture> Factures { get; set; }
        // Liste des produits de tous les utilisateurs.
        public DbSet<Produit> Produits { get; set; }
        // Liste de tous les feebacks reçus.
        public DbSet<Feedback> Feedbacks { get; set; }
        // Liste des lieux de tous les utilisateurs.
        public DbSet<Lieu> Lieux { get; set; }
        // Liste des telephones de tous les utilisateurs.
        public DbSet<Telephone> Telephones { get; set; }

        // Méthode appelée lors de la création du modèle afin de supprimer les tables ayant le même nom.
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        /// <summary>
        /// Méthode permettant à un utilisateur de se connecter.
        /// </summary>
        /// <param name="mail">mail de l'utilisateur (identifiant permettant de se connecter)</param>
        /// <param name="motDePasse">mot de passe de l'utilisateur afin de se connecter</param>
        /// <returns>Utilisateur correspondant aux identifiants et mot de passes passés en paramètres ou null si aucun utilisateur ne correspond</returns>
        public Utilisateur Authentifier(string mail, string motDePasse)
        {
            var user = Utilisateurs.Find(mail);
            return user != null && user.MotDePasse == motDePasse.GetHashCode() ? UtilisateurCourant = user : null;
        }

        /// <summary>
        /// Méthode permettant de récupérer l'utilisateur connecté sur l'application.
        /// </summary>
        /// <param name="identifiant">identifiant de l'utilisateur</param>
        /// <returns>Utilisateur courant si il est trouvé, null sinon</returns>
        public Utilisateur ObtenirUtilisateur(string identifiant)
        {
            return UtilisateurCourant = Utilisateurs.Find(identifiant);
        }

        /// <summary>
        /// Méthode permettant d'ajouter un utilisateur à la liste des utilisateurs de l'application (lors de l'inscription d'un utilisateur).
        /// </summary>
        /// <param name="mail">Mail de l'utilisateur</param>
        /// <param name="motDePasse">Mot de passe de l'utilisateur</param>
        /// <param name="nom">Nom de l'utilisateur</param>
        /// <param name="prenom">Prénom de l'utilisateur</param>
        /// <param name="type">Type de l'utilisateur</param>
        /// <returns>string: Identifiant de l'utilisateur créé</returns>
        public string AjouterUtilisateur(string mail, string motDePasse, string nom, string prenom, TypeUtilisateur type, ICollection<Telephone> telephones, Lieu lieu, Civilite civilite, string otherInfo, System.Boolean subscribe)
        {
            var param = new Parametre();
            Parametres.Add(param);

            Lieux.Add(lieu);
            SaveChanges();

            var user = new Utilisateur(mail, motDePasse, nom, prenom, telephones, type, lieu, civilite, param, otherInfo, subscribe);
            Utilisateurs.Add(user);
            SaveChanges();

            Telephones.AddRange(telephones);
            SaveChanges();

            return user.ID;
        }

        /*public List<String> GetAllMail()
        {
            List<String> m; 
            foreach( String mail in Utilisateurs.mail){
                m.add(mail);
            }
            return m;
        }*/

        public List<Utilisateur> GetAllUsers()
        {
            List<Utilisateur> users= new List<Utilisateur>(); 
            foreach( Utilisateur u in Utilisateurs){
                users.Add(u);
            }
            return users;
        }
    }
}