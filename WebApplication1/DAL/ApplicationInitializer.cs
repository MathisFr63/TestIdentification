﻿using System.Collections.Generic;
using System.Data.Entity;
using WebApplication1.Models.Account;
using WebApplication1.Models.Papiers;

namespace WebApplication1.DAL
{
    /// <summary>
    /// Classe permettant l'initialisation de la base de données avec des données en dures lorsque le model change afin de faire des tests sur l'application sans recréer les données à chaque fois.
    /// </summary>
    public class ApplicationInitializer : DropCreateDatabaseIfModelChanges<ApplicationContext>
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

            // Ajout d'utilisateurs.
            var utilisateurs = new List<Utilisateur>
            {
                new Utilisateur("Mathis.FRIZOT@etu.uca.fr", "root", "Mathis",   "FRIZOT",  TypeUtilisateur.Administrateur, "Nom du chat",    "Siboulette"),
                new Utilisateur("Flavio.RANCHON@etu.uca.fr", "Flavio", "Flavio",   "RANCHON", TypeUtilisateur.Enregistré,     "Sport",          "Football"),
                new Utilisateur("Mathieu.RAVEL@etu.uca.fr", "Mathieu", "Mathieu",  "RAVEL",   TypeUtilisateur.Enregistré,     "Nom de famille", "Ravel"),
                new Utilisateur("Aurelien.BERGER2@etu.uca.fr", "Aurélien", "Aurélien", "BERGER",  TypeUtilisateur.Enregistré,     "Nom de famille", "Berger"),
                new Utilisateur("Bernardo.PEREIRA_AUGUSTO@etu.uca.fr", "Bernardo", "Bernardo", "PEREIRA AUGUSTO", TypeUtilisateur.Enregistré, "Lieu d'étude l'an prochain", "IUT")
            };
            utilisateurs.ForEach(u => context.Utilisateurs.Add(u));

            // Ajout de produits.
            var produits = new List<Produit>
            {
                new Produit{ PrixHT = 10, Reduction =  0, TVA = 3, Type = TypeService.Bien, Nom = "Joint de culasse", Commentaire = "Petites pièces d'un moteur" },
                new Produit{ PrixHT = 20, Reduction = 15, TVA = 5, Type = TypeService.Bien, Nom = "Opium", Commentaire = "Latex qu'exsude le pavot somnifère"},
                new Produit{ PrixHT =  7, Reduction =  7, TVA = 7, Type = TypeService.Bien, Nom = "Boules de cristal", Commentaire = "Drago Ball Z"}
            };
            produits.ForEach(p => context.Produits.Add(p));

            // Ajout de feedbacks.
            var feedbacks = new List<Feedback>
            {
                new Feedback{Email = "Mathis.FRIZOT@etu.uca.fr", Comment = "Problème lors de l'affichage des feedbacks, je ne les vois pas apparaître !", UtilisateurID = "Mathis.FRIZOT@etu.uca.fr", Name = "Problèmes Feedbacks", Etat = "En cours"}
            };
            feedbacks.ForEach(f => context.Feedbacks.Add(f));

            // Sauvegarde des données dans la base.
            context.SaveChanges();
        }
    }
}