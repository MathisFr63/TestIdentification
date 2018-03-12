using System.Collections.Generic;
using System.Data.Entity;
using WebApplication1.Models.Account;
using WebApplication1.Models.Entite;
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
                new Parametre(),
                new Parametre()
            };
            parametres.ForEach(p => context.Parametres.Add(p));

            // Construction de plusieurs lieux
            var lieux = new List<Lieu>
            {
                new Lieu(                  "admin", "admin", "admin",   "admin",  "admin"), /* admin */
                new Lieu("10 Rue de la Maugagnade",    null, "63370", "Lempdes", "France"), /* Mathis.FRIZOT@etu.uca.fr */
                new Lieu("14 route de la Sauvetat",    null, "63730", "Plauzat", "France"), /* Flavio.RANCHON@etu.uca.fr */
                new Lieu(      "10 Rue de la Paix",    null, "63800", "Cournon", "France"), /* Mathieu.RAVEL@etu.uca.fr */
                new Lieu( "7 Avenue Blaise Pascal",    null, "63170", "Aubière", "France"), /* Aurelien.BERGER2@etu.uca.fr */
                new Lieu( "59 Avenue du Radiateur",    null, "63170", "Aubière", "France")  /* Bernardo.PEREIRA_AUGUSTO@etu.uca.fr */
            };
            lieux.ForEach(l => context.Lieux.Add(l));
            context.SaveChanges();


            // Construction de plusieurs téléphones
            var telephones = new List<List<Telephone>>
            {
                new List<Telephone>{ new Telephone("0602393655", "+33", TypeTelephone.Portable) { UtilisateurID = "admin" } },
                new List<Telephone>{ new Telephone("0473836373", "+33", TypeTelephone.Fixe) { UtilisateurID = "Mathis.FRIZOT@etu.uca.fr" } },
                new List<Telephone>{ new Telephone("0602393652", "+33", TypeTelephone.Portable) { UtilisateurID = "Flavio.RANCHON@etu.uca.fr" } },
                new List<Telephone>{ new Telephone("0473836377", "+33", TypeTelephone.Fixe) { UtilisateurID = "Mathieu.RAVEL@etu.uca.fr" } },
                new List<Telephone>{ new Telephone("0771271298", "+33", TypeTelephone.Fixe) { UtilisateurID = "Aurelien.BERGER2@etu.uca.fr" } },
                new List<Telephone>{ new Telephone("0473836377", "+33", TypeTelephone.Fixe) { UtilisateurID = "Bernardo.PEREIRA_AUGUSTO@etu.uca.fr" } }
            };
            telephones.ForEach(t => context.Telephones.Add(t[0]));
            
            // Ajout d'utilisateurs.
            var utilisateurs = new List<Utilisateur>
            {
                new Utilisateur("admin",                                  "admin",           "Admin",    "Admin", telephones[0],             TypeUtilisateur.SA, lieux[0], Civilite.Homme, parametres[0], null, false),
                new Utilisateur("Mathis.FRIZOT@etu.uca.fr",                "root",          "FRIZOT",   "Mathis", telephones[1], TypeUtilisateur.Administrateur, lieux[1], Civilite.Femme, parametres[1], null, false),
                new Utilisateur("Flavio.RANCHON@etu.uca.fr",             "Flavio",         "RANCHON",   "Flavio", telephones[2],         TypeUtilisateur.Client, lieux[2], Civilite.Homme, parametres[2], null, false),
                new Utilisateur("Mathieu.RAVEL@etu.uca.fr",             "Mathieu",           "RAVEL",  "Mathieu", telephones[3],      TypeUtilisateur.EnAttente, lieux[3], Civilite.Homme, parametres[3], null, false),
                new Utilisateur("Aurelien.BERGER2@etu.uca.fr",         "Aurélien",          "BERGER", "Aurélien", telephones[4],      TypeUtilisateur.EnAttente, lieux[4], Civilite.Homme, parametres[4], null, true),
                new Utilisateur("Bernardo.PEREIRA_AUGUSTO@etu.uca.fr", "Bernardo", "PEREIRA AUGUSTO", "Bernardo", telephones[5],      TypeUtilisateur.EnAttente, lieux[5], Civilite.Homme, parametres[5], null, false)
            };
            utilisateurs.ForEach(u => context.Utilisateurs.Add(u));

            // Ajout de produits.
            var produit1 = new Produit { PrixHT = 90, Reduction = 0, TVA = 2, Type = TypeService.Bien, Libelle = "Radiateur", Commentaire = "Radiateur milieu de gamme", UtilisateurID = "Mathieu.RAVEL@etu.uca.fr" };
            var produits = new List<Produit>
            {
                produit1,
                new Produit { PrixHT =        10, Reduction =  0, TVA =  3, Type = TypeService.Bien, Libelle = "Joint de culasse", Commentaire = "Petites pièces d'un moteur",         UtilisateurID = "Mathis.FRIZOT@etu.uca.fr" },
                new Produit { PrixHT =        20, Reduction = 15, TVA =  5, Type = TypeService.Bien, Libelle = "Opium",            Commentaire = "Latex qu'exsude le pavot somnifère", UtilisateurID = "Mathis.FRIZOT@etu.uca.fr" },
                new Produit { PrixHT =         7, Reduction =  0, TVA =  1, Type = TypeService.Bien, Libelle = "Boule de cristal", Commentaire = "Shenron exaucera vos voeux",         UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT =  35000000, Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Ocre",       Commentaire = "Obtenez 1 PA",                       UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT = 325000000, Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Vulbis",     Commentaire = "Obtenez 1 PM",                       UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT =   7000000, Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Pourpre",    Commentaire = "Obtenez 25% de dommage",             UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT =    675000, Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Cawotte",    Commentaire = "Obtenez 50 sagesse",                 UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT =   2500000, Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Emeraude",   Commentaire = "Obtenez 100 PV",                     UtilisateurID = "Flavio.RANCHON@etu.uca.fr" },
                new Produit { PrixHT =   7000000, Reduction = 99, TVA = 15, Type = TypeService.Bien, Libelle = "Dofus Turquoise",  Commentaire = "Obtenez 20% de CC",                  UtilisateurID = "Flavio.RANCHON@etu.uca.fr" }
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