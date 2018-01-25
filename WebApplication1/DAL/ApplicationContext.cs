using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using WebApplication1.Models.Compte;
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
        //public DbSet<Entreprise> Entreprises { get; set; }
        public DbSet<Devis> Devis { get; set; }
        public DbSet<DonneeProduit> DonneeProduit { get; set; }
        public DbSet<Facture> Factures { get; set; }
        public DbSet<Produit> Produits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public Utilisateur Authentifier(string identifiant, string motDePasse)
        {
            return UtilisateurCourant = Utilisateurs.FirstOrDefault(u => u.Identifiant == identifiant && u.MotDePasse == motDePasse);
        }

        public Utilisateur ObtenirUtilisateur(string identifiant)
        {
            return UtilisateurCourant = Utilisateurs.FirstOrDefault(u => u.ID.ToString() == identifiant);
        }

        public int AjouterUtilisateur(string identifiant, string motDePasse, string nom, string prenom, string mail, TypeUtilisateur type, string question, string reponse)
        {
            Utilisateur user = new Utilisateur(identifiant, motDePasse, nom, prenom, mail, type, question, reponse);
            Utilisateurs.Add(user);
            SaveChanges();
            return user.ID;
        }
    }
}