using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetInfo_MG_DD
{
    class Voiture : Vehicule, IAttributsFichier
    {
        //Attributs 
        private int nb_valise;
        private int nb_porte;
        private string boite;

        //Constructeur
        public Voiture(uint id, string marque, string gamme, int kilometrage, string modele, int nbPlace, string permisUtil, string imma, double prixLocAuJour, int nb_valise, int nb_porte, string boite) : base(id, marque, gamme, kilometrage, modele, nbPlace, permisUtil, imma, prixLocAuJour)     // Constructeur fille (Voiture)
        {
            this.nb_valise = nb_valise;
            this.nb_porte = nb_porte;
            this.boite = boite;
        }

        //Methodes
        public override string ListAttributsAjoutFichier()  // Retourne la chaine de caractères à enregistrer dans les fichiers
        {
            string str = id + ";" + marque + ";" + gamme + ";" + kilometrage + ";" + modele + ";" + nbPlace + ";" + permisUtil + ";" + imma + ";" + prixLocAuJour + ";" + nb_valise + ";" + nb_porte + ";" + boite;
            return str;
        }
        public override string ToString()   // Retourne les informations de la voiture
        {
            return base.ToString() + "Volume du coffre : " + nb_valise + "\nNombre de portes : " + nb_porte + "\nBoite : " + boite;
        }
    }
}
