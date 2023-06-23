using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetInfo_MG_DD
{
    class Moto : Vehicule, IAttributsFichier
    {
        //Attributs
        private int volumeMoteur;

        //Constructeur
        public Moto(uint id, string marque, string gamme, int kilometrage, string modele, int nbPlace, string permisUtil, string imma, double prixLocAuJour, int volumeMoteur) : base(id, marque, gamme, kilometrage, modele, nbPlace, permisUtil, imma, prixLocAuJour)     // Constructeur fille (Moto)
        {
            this.volumeMoteur = volumeMoteur;
        }

        //Methodes
        public override string ListAttributsAjoutFichier()   // Retourne la chaine de caractères à enregistrer dans les fichiers
        {
            string str = id + ";" + marque + ";" + gamme + ";" + kilometrage + ";" + modele + ";" + nbPlace + ";" + permisUtil + ";" + imma + ";" + prixLocAuJour + ";" + volumeMoteur;
            return str;
        }
        public override string ToString()   // Retourne les informations de la moto
        {
            return base.ToString() + "Volume de moteur : " + volumeMoteur + "cm3";
        }
    }
}
