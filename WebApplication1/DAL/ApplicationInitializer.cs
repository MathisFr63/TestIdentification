using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class ApplicationInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationContext>
    {
        protected override void Seed(ApplicationContext context)
        {
            var utilisateurs = new List<Utilisateur>
            {
                new Utilisateur{Identifiant="MathisFrizot", MotDePasse="root", Mail = "Mathis.FRIZOT@etu.uca.fr", Nom = "FRIZOT", Prénom = "Mathis", Type = TypeUtilisateur.Administrateur},
                new Utilisateur{Identifiant="MathieuRavel", MotDePasse="Mathieu", Mail = "Mathieu.RAVEL@etu.uca.fr", Nom = "RAVEL", Prénom = "Mathieu", Type = TypeUtilisateur.Enregistré},
                new Utilisateur{Identifiant="FlavioRanchon", MotDePasse="Flavio", Mail = "Flavio.RANCHON@etu.uca.fr", Nom = "RANCHON", Prénom = "Flavio", Type = TypeUtilisateur.Enregistré},
                new Utilisateur{Identifiant="BernardoPereiraAugusto", MotDePasse="Bernardo", Mail = "Bernardo.PEREIRA_AUGUSTO@etu.uca.fr", Nom = "PEREIRA AUGUSTO", Prénom = "Bernardo", Type = TypeUtilisateur.Enregistré},
                new Utilisateur{Identifiant="AurelienBerger", MotDePasse="Aurelien", Mail = "Aurelien.BERGER@etu.uca.fr", Nom = "BERGER", Prénom = "Aurélien", Type = TypeUtilisateur.Enregistré},
            };
            utilisateurs.ForEach(s => context.Utilisateurs.Add(s));
            context.SaveChanges();
        }
        }
}