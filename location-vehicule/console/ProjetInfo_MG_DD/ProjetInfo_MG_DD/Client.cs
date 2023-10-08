using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ProjetInfo_MG_DD
{
    class Client
    {
        // Attributs
        private uint id;
        private string nom;
        private string prenom;
        private bool permisVoiture = false; // true : a le permis, false : ne l'a pas
        private bool permisMoto = false; // true : a le permis, false : ne l'a pas
        private bool permisPoidsLourd = false; // true : a le permis, false : ne l'a pas
        private string mdp;
        private uint nbKmParcouru;
        private bool admin;
        public delegate void supprimeLocationFichier(uint id, string nomFichier);
        private List<Location> listLocaClient;

        // Propriété
        public string Nom   // Retourne la valeure nom
        {
            get { return nom; }
        }
        public string Prenom   // Retourne la valeure prenom
        {
            get { return prenom; }
        }
        public string Mdp   // Retourne le mdp
        {
            get { return mdp; }
        }
        public uint Id   // Retourne l'identifiant
        {
            get { return id; }
        }
        public bool Admin   // Retourne le booléen d'administrateur / recoit une valeure lors d'une modification éventuelle
        {
            get { return admin; }
            set { admin = value; }
        }
        public List<Location> ListLocaClient    // Retorune la liste des Locations du clients
        {
            get { return listLocaClient; }
        }
        // Constructeurs
        public Client(uint id, string prenom, string nom, string mdp)   // Constructeur 1, enregistrer client dans la base
        {
            Parc parc = new Parc();
            this.id = id;
            this.prenom = prenom;
            this.nom = nom;
            this.mdp = mdp;
            nbKmParcouru = 0;
            admin = false;
            listLocaClient = new List<Location>();
            ModifierPermis();
        }
        public Client(uint id, string prenom, string nom, string mdp, uint nbKmParcouru, bool permisVoiture, bool permisMoto, bool permisPoidsLourd, bool admin)    // Constructeur 2
        {
            this.id = id;
            this.prenom = prenom;
            this.nom = nom;
            this.mdp = mdp;
            this.nbKmParcouru = nbKmParcouru;
            this.permisVoiture = permisVoiture;
            this.permisMoto = permisMoto;
            this.permisPoidsLourd = permisPoidsLourd;
            this.admin = admin;
            listLocaClient = new List<Location>();
        }

        //Méthodes
        public void ModifierClientFichier()
        {
            try
            { 
                string[,] mat = null;
                bool verif = false;
                int cpt = 0;
                StreamReader fluxLec = new StreamReader("FichierClient.txt", true);                          // Rajouter mot de passe dans les fichiés
                while (fluxLec.EndOfStream == false)
                {
                    string a = fluxLec.ReadLine();
                    cpt++;
                }
                fluxLec.Close();

                mat = new string[cpt, 9];
                cpt = 0;
                fluxLec = new StreamReader("FichierClient.txt");                          // Rajouter mot de passe dans les fichiés
                while (fluxLec.EndOfStream == false)
                {
                    string[] tab = fluxLec.ReadLine().Split(';');
                    if (tab[1] == prenom)
                    {
                        tab = ListAttributsAjoutFichier().Split(';');
                        verif = true;
                    }
                    for (int i = 0; i < tab.Length; i++)
                    {
                        mat[cpt, i] = tab[i];
                    }
                    cpt++;
                }
                fluxLec.Close();
                if (verif)
                {
                    StreamWriter fluxEcr = new StreamWriter("FichierClient.txt");
                    for (int i = 0; i < mat.GetLength(0); i++)
                    {
                        for (int j = 0; j < mat.GetLength(1); j++)
                        {
                            fluxEcr.Write(mat[i, j]);
                            if (j != mat.GetLength(1) - 1)
                            {
                                fluxEcr.Write(";");
                            }
                        }
                        fluxEcr.WriteLine();
                    }
                    fluxEcr.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "ModifierClientFichier()");
                Console.ReadKey();
            }
        }
        public void ModifierMdp()   // Méthode permettant la modification du mot de passe
        {
            string mdp;
            Console.WriteLine("Modifier votre mot de passe ?\n(1) Oui\n(autre) Non");
            if (Console.ReadLine() == "1")
            {
                Console.WriteLine("Entrez votre mot de passe :\n");
                if (Console.ReadLine() == this.mdp)
                {
                    Console.WriteLine("Mot de passe correct..\n\nEntrez votre nouveau mot de passe :");
                    mdp = Console.ReadLine();
                    Console.WriteLine("Confirmez nouveau mot de passe :");
                    if (Console.ReadLine() == mdp)
                    {
                        this.mdp = mdp;
                        ModifierClientFichier();
                        Console.WriteLine("Mot de passe modifié avec succès..");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Les mots de passe entrés sont différents..\nAucune modification n'a été éfféctué\n\nRetour au menu principal.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Mot de passe incorrect..\n\nRetour au menu principal.");
                    Console.ReadKey();
                }
            }
        }
        public void TransformerCompteAdmin()    // Méthode perméttant la transformation d'un compte en compte admin
        {
            Console.WriteLine("Entrer le mdp :\t(Mdp : 123456)");
            if (Console.ReadLine() == "123456")
            {
                admin = true;
                ModifierClientFichier();
                Console.WriteLine("Votre est devenu un compte admin");
                Console.ReadKey();
            }
            else
            {
                admin = false;
                Console.WriteLine("Mdp incorrect");
            }
        }
        public void ModifierPermis()    // Permet les modificatons du permis du client
        {
            Console.WriteLine("Avez vous le permis voiture?\n(oui)\n(autre)");
            if (Console.ReadLine() == "oui")
            {
                permisVoiture = true;
            }
            else
            {
                permisVoiture = false;
            }
            Console.WriteLine("Avez vous le permis Moto?\n(oui)\n(autre)");
            if (Console.ReadLine() == "oui")
            {
                permisMoto = true;
            }
            else
            {
                permisMoto = false;
            }
            Console.WriteLine("Avez vous le permis Poids Lourd?\n(oui)\n(autre)");
            if (Console.ReadLine() == "oui")
            {
                permisPoidsLourd = true;
            }
            else
            {
                permisPoidsLourd = false;
            }
        }
        public void ChargementLocationClient()   // Permet d'ajouter une location à la liste du client
        {
            Parc parc = new Parc();
            try
            {
                int cpt = 0;
                StreamReader fluxLec = new StreamReader("FichierLocation.txt");
                while(fluxLec.EndOfStream == false)
                {
                    string[] temp = fluxLec.ReadLine().Split(';');
                    int inter = 0;
                    if(temp[6] == "Voiture")
                    {
                        inter = 3;
                    }
                    if (temp[6] == "Camion")
                    {
                        inter = 2;
                    }
                    if (temp[6] == "Moto")
                    {
                        inter = 1;
                    }
                    if (inter != 0)
                    {
                        if(temp[10+inter] == prenom && temp[11+inter] == nom && temp[12+inter] == mdp)
                        {
                            listLocaClient.Add(parc.ListLoca[cpt]);
                        }
                    }
                    cpt++;
                }
                fluxLec.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message + "cli.AjouterUneLocation()");
                Console.ReadKey();
            }
            
        }
        public void SupprimerUneLocation(uint idLoc, supprimeLocationFichier supprLoc)  // Permet de supprimer une location de la  liste du client
        {
            listLocaClient.RemoveAt(Convert.ToInt32(idLoc));
            supprLoc(idLoc, "FichierLocation.txt");
            Parc parc = new Parc();
        }
        public void AfficherlistLocationClient()    // Affiche la liste de la location du client
        {
            Console.WriteLine("Voici la liste de vos Locations actuelles :\n");
            for (int i = 0; i < listLocaClient.Count; i++)
            {
                Console.WriteLine("\tLocation(" + i + ") :\n");
                Console.WriteLine(listLocaClient[i].ToString());
                Console.WriteLine("\n\n");
            }
        }
        public string ListAttributsAjoutFichier() // Retourne la chaine de caractères à enregistrer dans les fichiers
        {
            string str = id + ";" + prenom + ";" + nom + ";" + mdp + ";" + nbKmParcouru + ";" + permisVoiture + ";" + permisMoto + ";" + permisPoidsLourd + ";" + admin;
            return str;
        }
        public override string ToString()   // Retourne les informations du client
        {
            string perMoto = "vous avez le permis moto";
            string perVoit = "vous avez le permis Voiture";
            string perPLou = "vous avez le permis Poids Lourd";
            if (!permisMoto)
            {
                perMoto = "vous n'avez pas le permis moto ";
            }
            if (!permisVoiture)
            {
                perVoit = "vous n'avez pas le permis Voiture";
            }
            if (!permisPoidsLourd)
            {
                perPLou = "vous n'avez pas le permis Poids Lourd";
            }
            string str = "Voici votre profil :\n\nPrénom : " + prenom + "\nNom : " + nom + "\nVous avez parcouru "+nbKmParcouru+ " Km\nVous avez " + listLocaClient.Count + " location(s) en ce moment\n" + "\n" + perVoit + "\n" + perMoto + "\n" + perPLou;
            return str;
        }
    }
}
