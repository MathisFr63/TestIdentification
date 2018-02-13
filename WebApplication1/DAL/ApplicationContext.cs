using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using WebApplication1.Models.Account;
using WebApplication1.Models.Papiers;

namespace WebApplication1.DAL
{
    public class ApplicationContext : DbContext
    {
        public Utilisateur UtilisateurCourant { get; set; }

        public ApplicationContext() : base("ApplicationContext")
        {
            //l = Utilisateurs.ToList();
        }

        //public List<Utilisateur> l { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Compte> Comptes { get; set; }
        public DbSet<AdresseMail> AdressesMail { get; set; }
        public DbSet<Devis> Devis { get; set; }
        public DbSet<DonneeProduit> DonneeProduit { get; set; }
        public DbSet<Facture> Factures { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public Utilisateur Authentifier(string mail, string motDePasse)
        {
            //var user = l.FirstOrDefault(u => u.ID == mail);
            var user = Utilisateurs.Find(mail);
            return user.MotDePasse == motDePasse.GetHashCode() ? user : null;
        }

        public Utilisateur ObtenirUtilisateur(string identifiant)
        {
            return UtilisateurCourant = Utilisateurs.FirstOrDefault(u => u.ID == identifiant);
        }

        public string AjouterUtilisateur(string mail, string motDePasse, string nom, string prenom, TypeUtilisateur type, string question, string reponse)
        {
            var user = new Utilisateur(mail, motDePasse, nom, prenom, type, question, reponse);
            Utilisateurs.Add(user);

            SaveChanges();
            return user.ID;
        }
    }
}