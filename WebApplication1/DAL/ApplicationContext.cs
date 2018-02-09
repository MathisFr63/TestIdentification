using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using WebApplication1.Models.Account;
using WebApplication1.Models.Entite;
using WebApplication1.Models.Papiers;

namespace WebApplication1.DAL
{
    public class ApplicationContext : DbContext
    {
        public Utilisateur UtilisateurCourant { get; set; }

        public ApplicationContext() : base("ApplicationContext")
        {
        }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Compte> Comptes { get; set; }
        //public DbSet<Entreprise> Entreprises { get; set; }
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
            int tmp = motDePasse.GetHashCode();
            Compte account = Comptes.FirstOrDefault(c => c.Mail == mail && c.MotDePasse == tmp);
            if (account == null)
                return null;
            return Utilisateurs.FirstOrDefault(u => u.compte.ID == account.ID);
        }

        public Utilisateur ObtenirUtilisateur(string identifiant)
        {
            int tmp;
            int.TryParse(identifiant, out tmp);
            return UtilisateurCourant = Utilisateurs.FirstOrDefault(u => u.compte.ID == tmp);
        }

        public int AjouterUtilisateur(string mail, string motDePasse, string nom, string prenom, TypeUtilisateur type, string question, string reponse)
        {
            Utilisateur user = new Utilisateur(mail, motDePasse, nom, prenom, type, question, reponse);
            Utilisateurs.Add(user);
            SaveChanges();
            return user.compte.ID;
        }
    }
}