using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetInfo_MG_DD
{
    class Trajet
    {
        //Attributs
        private string lieuDeDepart;
        private string lieuDArriver;
        private int distance;

        //Propriété
        public string LieuDeDepart      // Retourne le lieu de départ de la location
        {
            get { return lieuDeDepart; } // Accesseur : retourne la valeur en lecture
        }
        public string LieuDArriver      // Retourne le lieu d'arriver de la location
        {
            get { return lieuDArriver; }
        }

        //Constructeur
        public Trajet(string lieuDeDepart, string lieuDArriver, int distance)   // Constructeur 1
        {
            this.lieuDeDepart = lieuDeDepart;
            this.lieuDArriver = lieuDArriver;
            this.distance = distance;
        }
        public Trajet()                 // constructeur 2
        {
            Console.WriteLine("Lieu de départ de la location :");
            lieuDeDepart = Console.ReadLine();
            Console.WriteLine("Lieu d'arriver de la location :");
            lieuDArriver = Console.ReadLine();
            RechercheGoogleMaps();
        }

        //Méthodes
        public void RechercheGoogleMaps()   // Effectue le recherche sur Google Maps et recupère la distance
        {
            // Afficher GoogleMaps
            // recupérer la valeure de la distance
        }
        public string ListAttributsAjoutFichier()   // // Retourne la chaine de caractères à enregistrer dans les fichiers
        {
            string str = lieuDeDepart + ";" + lieuDArriver + ";" + distance;
            return str;
        }
        public override string ToString()   // Retourne les informations du client
        {
            string str = "";
            str = "Départ de : " + lieuDeDepart + "\nArriver à " + lieuDArriver;
            return str;
        }
    }
}
