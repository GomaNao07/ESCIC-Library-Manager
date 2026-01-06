using System;
using System.Collections.Generic;
using ESCIC_Library_Manager.Enums;

namespace ESCIC_Library_Manager.Models;

public class Utilisateur
{
    public string Id { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Email { get; set; }
    public string Tel { get; set; }
    public DateTime DateInscription { get; set; }
    public TypeUtilisateur Type { get; set; }
    public int NbEmprunts { get; set; }
    public decimal Amendes { get; set; }
    public bool Bloque { get; set; }
    public List<string> HistoriqueEmprunts { get; set; }

    public Utilisateur(string id, string nom, string prenom, string email, string tel, TypeUtilisateur type)
    {
        Id = id;
        Nom = nom;
        Prenom = prenom;
        Email = email;
        Tel = tel;
        DateInscription = DateTime.Now;
        Type = type;
        NbEmprunts = 0;
        Amendes = 0;
        Bloque = false;
        HistoriqueEmprunts = new List<string>();
    }

    public void AfficherProfil()
    {
        Console.WriteLine($"ID: {Id}");
        Console.WriteLine($"Nom complet: {Prenom} {Nom}");
        Console.WriteLine($"Type: {Type}");
        Console.WriteLine($"Emprunts: {NbEmprunts}");
        Console.WriteLine($"Amendes: {Amendes} FCFA");
        Console.WriteLine($"Statut: {(Bloque ? "Bloqué" : "Actif")}");
    }

    public int GetLimiteEmprunts()
    {
        return Type switch
        {
            TypeUtilisateur.Etudiant => 2,
            TypeUtilisateur.Formateur => 5,
            TypeUtilisateur.Administrateur => 10,
            _ => 0
        };
    }

    public bool PeutEmprunter()
    {
        if (Bloque) return false;
        return NbEmprunts < GetLimiteEmprunts();
    }

    public void AjouterAmende(decimal montant)
    {
        Amendes += montant;
        if (Amendes > 2000)
        {
            Bloque = true;
        }
    }

    public void PayerAmende(decimal montant)
    {
        Amendes -= montant;
        if (Amendes <= 0)
        {
            Amendes = 0;
            Bloque = false;
        }
    }
}