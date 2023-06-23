using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetInfo_MG_DD
{
    class Camion : Vehicule, IAttributsFichier
    {
        //Attributs
        private int volume;
        private double charge_autorise;

        //Constructeur
        public Camion(uint id, string marque, string gamme, int kilometrage, string modele, int nbPlace, string permisUtil, string imma, double prixLocAuJour, int volume, double charge_autorise) : base(id, marque, gamme, kilometrage, modele, nbPlace, permisUtil, imma, prixLocAuJour)     // Constructeur fille (Camion)
        {
            this.volume = volume;
            this.charge_autorise = charge_autorise;
        }

        //Methodes
        public override string ListAttributsAjoutFichier()      // Retourne la chaine de caractères à enregistrer dans les fichiers
        {
            string str = id + ";" + marque + ";" + gamme + ";" + kilometrage + ";" + modele + ";" + nbPlace + ";" + permisUtil + ";" + imma + ";" + prixLocAuJour + ";" + volume + ";" + charge_autorise;
            return str;
        }
        public override string ToString()   // Retourne les informations du camion
        {
            return base.ToString() + "Volume : " + volume + "m3" + "\nCharge autorisé : " + charge_autorise + " Kg"; ;
        }
    }
}
