using System;
using System.Collections.Generic;
using System.Data.Entity;
using WebApplication1.Models.Account;
using WebApplication1.Models.Papiers;

namespace WebApplication1.DAL
{
    /// <summary>
    /// Classe permettant l'initialisation de la base de données avec des données en dures lorsque le model change afin de faire des tests sur l'application sans recréer les données à chaque fois.
    /// </summary>
    //public class ApplicationInitializer : DropCreateDatabaseAlways<ApplicationContext>
    public class ApplicationInitializer : DropCreateDatabaseAlways<ApplicationContext>
    {
        /// <summary>
        /// Méthode permettant d'initialiser la base de données.
        /// </summary>
        /// <param name="context">Contexte de l'application dans lequel on veut sauvegarder les données</param>
        protected override void Seed(ApplicationContext context)
        {
            // Ajout d'adresses mails abonnées aux newsletters.
            var adressesMail = new List<AdresseMail>
            {
                new AdresseMail("Mathis.FRIZOT@etu.uca.fr"),
                new AdresseMail("Flavio.RANCHON@etu.uca.fr"),
                new AdresseMail("Mathieu.RAVEL@etu.uca.fr"),
                new AdresseMail("Aurelien.BERGER2@etu.uca.fr"),
                new AdresseMail("Bernardo.PEREIRA_AUGUSTO@etu.uca.fr"),
            };
            adressesMail.ForEach(am => context.AdressesMail.Add(am));

            // Ajout des paramètres des utilisateurs.
            var parametres = new List<Parametre>
            {
                new Parametre(),
                new Parametre(),
                new Parametre(),
                new Parametre(),
                new Parametre()
            };
            parametres.ForEach(p => context.Parametres.Add(p));
            context.SaveChanges();

            // Ajout d'utilisateurs.
            var utilisateurs = new List<Utilisateur>
            {
                new Utilisateur("Mathis.FRIZOT@etu.uca.fr", "root", "FRIZOT", "Mathis",TypeUtilisateur.Administrateur, parametres[0]),
                new Utilisateur("Flavio.RANCHON@etu.uca.fr", "Flavio", "RANCHON", "Flavio",TypeUtilisateur.Enregistré, parametres[1]),
                new Utilisateur("Mathieu.RAVEL@etu.uca.fr", "Mathieu", "RAVEL", "Mathieu", TypeUtilisateur.Enregistré, parametres[2]),
                new Utilisateur("Aurelien.BERGER2@etu.uca.fr", "Aurélien", "BERGER", "Aurélien",TypeUtilisateur.Enregistré, parametres[3]),
                new Utilisateur("Bernardo.PEREIRA_AUGUSTO@etu.uca.fr", "Bernardo", "PEREIRA AUGUSTO", "Bernardo",TypeUtilisateur.Enregistré, parametres[4])
            };
            utilisateurs.ForEach(u => context.Utilisateurs.Add(u));

            // Ajout de produits.
            var produit1 = new Produit { PrixHT = 90, Reduction = 0, TVA = 2, Type = TypeService.Bien, Libelle = "Radiateur", Commentaire = "Radiateur milieu de gamme", UtilisateurID = "Mathieu.RAVEL@etu.uca.fr" };
            var produits = new List<Produit>
            {
                produit1,
                new Produit { PrixHT = 10, Reduction = 0, TVA = 3, Type = TypeService.Bien, Libelle = "Joint de culasse", Commentaire = "Petites pièces d'un moteur", UtilisateurID = "Mathis.FRIZOT@etu.uca.fr" },
                new Produit { PrixHT = 20, Reduction = 15, TVA = 5, Type = TypeService.Bien, Libelle = "Opium", Commentaire = "Latex qu'exsude le pavot somnifère", UtilisateurID = "Mathis.FRIZOT@etu.uca.fr" },
                new Produit { PrixHT = 7, Reduction = 0, TVA = 1, Type = TypeService.Bien, Libelle = "Boule de cristal", Commentaire = "Shenron exaucera vos voeux", UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT = 35000000,  Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Ocre",      Commentaire = "Obtenez 1 PA",           UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT = 325000000, Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Vulbis",    Commentaire = "Obtenez 1 PM",           UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT = 7000000,   Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Pourpre",   Commentaire = "Obtenez 25% de dommage", UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT = 675000,    Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Cawotte",   Commentaire = "Obtenez 50 sagesse",     UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT = 2500000,   Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Emeraude",  Commentaire = "Obtenez 100 PV",         UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT = 7000000,   Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Turquoise", Commentaire = "Obtenez 20% de CC",      UtilisateurID = "Flavio.RANCHON@etu.uca.fr" }
            };
            produits.ForEach(p => context.Produits.Add(p));

            //Ajout de devis.
            var devis = new List<Devis>
            {
                new Devis("Installation radiateur", "Installation d'un radiateur neuf à la place d'un radiateur défectueux", TypeMonnaie.Euro, new List<DonneeProduit>{ new DonneeProduit(produit1, 1)}, "Mathis.FRIZOT@etu.uca.fr")
            };
            devis.ForEach(d => context.Devis.Add(d));

            // Ajout de feedbacks.
            var feedbacks = new List<Feedback>
            {
                new Feedback{Comment = "<strong>Problème</strong> lors de l'affichage des <b>feedbacks</b>, je ne les vois pas <i>apparaître</i> !", UtilisateurID = "Mathis.FRIZOT@etu.uca.fr", userName = "FRIZOT Mathis", Subject = "Problèmes Feedbacks", IsResolved = false}
            };
            feedbacks.ForEach(f => context.Feedbacks.Add(f));

            // Sauvegarde des données dans la base.
            context.SaveChanges();
        }
    }
}