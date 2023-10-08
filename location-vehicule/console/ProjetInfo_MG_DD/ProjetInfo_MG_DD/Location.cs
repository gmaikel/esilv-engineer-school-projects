using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetInfo_MG_DD
{
    class Location
    {
        // Attributs

        private uint id;
        private Client cli;
        private Vehicule vehi;
        private Trajet tra;
        private string dateDeDepart;
        private string dateDArriver;
        private double prixLoc;

        // Propriétés
        public Vehicule Vehi    // Retourne l'objet Vehicule de la location
        {
            get { return vehi; }
        }
        public uint Id  // Retourne l'identifiant de la location qui va permettre l'identification de la location dans les listes
        {
            get { return id; }
        }
        public Client Cli   // Retourne le Client de la location
        {
            get { return cli; }
        }
        // Constructeur
        public Location(Vehicule vehi, Client cli, Trajet tra, uint id, string dateDeDepart, string dateDArriver, double prixLoc)   // Constructeur 1
        {
            this.id = id;
            this.cli = cli;
            this.vehi = vehi;
            this.tra = tra;
            this.dateDeDepart = dateDeDepart;
            this.dateDArriver = dateDArriver;
            this.prixLoc = prixLoc;
        }
        public Location(Vehicule vehi, Client cli, Trajet tra) // Constructeur 2
        {
            this.cli = cli;
            this.vehi = vehi;
            this.tra = tra;
            Console.WriteLine("Date de départ :\t\t(jj/mm/aaaa)");
            dateDeDepart = Console.ReadLine();
            Console.WriteLine("Date d'arriver :\t\t(jj/mm/aaaa)");
            dateDArriver = Console.ReadLine();
            CalculCoutLocation();
            EnvoiMailValidationLoc();
        }

        // Méthodes
        public void CalculCoutLocation()        // Calcul le temps de location avec les dates et estime le prix de location total
        {
            string[] d = dateDeDepart.Split('/');
            string[] a = dateDArriver.Split('/');
            double nbJour = (Convert.ToInt32(a[2]) - Convert.ToInt32(d[2])) * 365 + (Convert.ToInt32(a[1]) - Convert.ToInt32(d[1])) * 30 + Convert.ToInt32(a[1]) % 2 + Convert.ToInt32(a[0]) - Convert.ToInt32(d[0]);
            prixLoc = vehi.PrixLocAuJour * nbJour;
            Console.WriteLine("Vous voulez louer pour : " + nbJour + "jour(s), cela vous coutera " + prixLoc + "€");
            Console.ReadKey();
        }
        public void EnvoiMailValidationLoc()
        {
            Console.WriteLine("\n\n\t\t*** Mail MGDD ***\n\n\tNous vous envoyons ce mail pour vous confirmer votre location chez nous\n\nInformations Location :\n\n" + ToString());
            Console.ReadKey();
        }

        public string ListAttributsAjoutFichier()   // Retourne la chaine de caractères à enregistrer dans les fichiers
        {
            Parc parc = new Parc();
            id = Convert.ToUInt32(parc.ListLoca.Count);
            string str = vehi.ListAttributsAjoutFichier() + ";" + cli.ListAttributsAjoutFichier() + ";" + tra.ListAttributsAjoutFichier() + ";" + id + ";" + dateDeDepart + ";" + dateDArriver + ";" + prixLoc;
            return str;
        }
        public override string ToString()   // Retourne les informations de la location
        {
            string str = vehi.ToString() + "\n\n"+ tra.ToString() + "\ndu " + dateDeDepart + "\nau " + dateDArriver + "\nà un prix de " + prixLoc + " euros";
            return str;
        }
    }
}
