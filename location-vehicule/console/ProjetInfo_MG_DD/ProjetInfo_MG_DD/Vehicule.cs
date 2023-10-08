using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ProjetInfo_MG_DD
{
    abstract class Vehicule
    {
        // Attributs
        protected uint id;
        protected string marque;
        protected string gamme;
        protected int kilometrage;
        protected string modele;
        protected int nbPlace;
        protected string permisUtil;
        protected string imma;
        protected double prixLocAuJour;

        // Propriété
        public string Imma      // Retourne l'immatriculation du véhicule
        {
            get { return imma; }
        }
        public double PrixLocAuJour     // Retourne le prix de location du véhicule à la journée
        {
            get { return prixLocAuJour; }
        }
        public uint Id      // Retourne l'identifiant du véhicule permettant la recherche dans la liste
        {
            get { return id; }
        }

        // Constructeur 
        public Vehicule(uint id, string marque, string gamme, int kilometrage, string modele, int nbPlace, string permisUtil, string imma, double prixLocAuJour)    // Constructeur mère
        {
            this.id = id;
            this.marque = marque;
            this.gamme = gamme;
            this.kilometrage = kilometrage;
            this.modele = modele;
            this.nbPlace = nbPlace;
            this.permisUtil = permisUtil;
            this.imma = imma;
            this.prixLocAuJour = prixLocAuJour;
        }

        //Methodes
        public abstract string ListAttributsAjoutFichier();     // Méthode abstract permettant le retour de la chaine de caractères à enregistrer dans les listes
        public override string ToString() // retourne les informations du véhicule
        {
            string str = "Identifiant Véhicule : " + id + "\nGamme : " + gamme + "\nMarque : " + marque + "\nModele : " + modele + "\nKilomètrage : " + kilometrage + "\nNombre de places disponible : " + nbPlace + "\nType de permis Necessaire pour la location : " + permisUtil + "\n";
            return str;
        }
    }
}
