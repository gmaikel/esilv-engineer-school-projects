using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ProjetInfo_MG_DD
{
    class Parc
    {
        // Attributs
        static List<Vehicule> listVeh;
        static List<Client> listCli;
        static List<Location> listLoca;

        public List<Vehicule> ListVeh   // Retourne la liste des véhicule
        {
            get { return listVeh; }
        }
        public List<Client> ListCli     // Retourne la liste des clients
        {
            get { return listCli; }
        }
        public List<Location> ListLoca  // Retourne la liste des locations
        {
            get { return listLoca; }
            set { ListLoca = value; }
        }

        // Construteur
        public Parc()   // Constructeur, créé l'objet Parc et charge les fichiers dans les listes
        {
            listVeh = new List<Vehicule>();
            listCli = new List<Client>();
            listLoca = new List<Location>();
            CreationFichierSiNonExistant();
            ChargementDonnees();
        }

        // Méthodes
        public void CreationFichierSiNonExistant()
        {
            bool suppr = false;
            try
            {
                if (!File.Exists("FichierClient.txt"))
                {
                    File.Create("FichierClient.txt");
                    suppr = true;
                }
                if (!File.Exists("FichierLocation.txt"))
                {
                    File.Create("FichierLocation.txt");
                    suppr = true;
                }
                if (!File.Exists("FichierVehicule.txt"))
                {
                    File.Create("FichierVehicule.txt");
                    suppr = true;
                }
                if (suppr)
                {
                    Console.WriteLine("Un ou plusieurs fichiers ont été supprimés, veuillez fermer et réouvrir la console afin qu'ils soient recréés");
                    Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "CreationFichierSiNonExistant()");
            }
        }
        public bool RechercheObjet(uint id, string nomFichier, ref string[,] matFichier)    // Permet la recherche d'un élément dans une liste
        {
            bool res = false;
            int cpt = 0;
            int taille = 0;
            try
            {
                CreationFichierSiNonExistant();
                StreamReader fluxLecture = new StreamReader(nomFichier);
                while (fluxLecture.EndOfStream == false)
                {
                    string[] temp = fluxLecture.ReadLine().Split(';');
                    taille = temp.Length;
                    cpt++;
                }
                fluxLecture.Close();
                matFichier = new string[cpt, taille];
                cpt = 0;
                fluxLecture = new StreamReader(nomFichier);
                while (fluxLecture.EndOfStream == false)
                {
                    string[] tab = fluxLecture.ReadLine().Split(';');
                    if (Convert.ToUInt32(tab[0]) == id)
                    {
                        res = true;
                    }
                    else
                    {
                        for (int i = 0; i < matFichier.GetLength(1); i++)
                        {
                            matFichier[cpt, i] = tab[i];
                        }
                        cpt++;
                    }
                }
                fluxLecture.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "RechercheObjet()");
                Console.ReadKey();
            }
            return res;
        }
        public void SupprimerObjetFichier(uint id, string nomFichier)    // Méthode permettant la suppresion d'un élément dans un fichier, cette méthode est utile pour la délégation
        {
            string[,] matFichier = null;
            if (RechercheObjet(id, nomFichier, ref matFichier))
            {
                try
                {
                    StreamWriter fluxEcriture = new StreamWriter(nomFichier);
                    for (int i = 0; i < matFichier.GetLength(0)-1; i++)
                    {
                        for (int j = 0; j < matFichier.GetLength(1); j++)
                        {
                            fluxEcriture.Write(matFichier[i, j]);
                            if (j != matFichier.GetLength(1) - 1)
                            {
                                fluxEcriture.Write(";");
                            }
                        }

                        fluxEcriture.WriteLine("");
                    }
                    fluxEcriture.Close();
                    ChargementDonnees();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "SupprimerObjetFichier");
                }
            }
            else
            {
                Console.WriteLine("Objet inexistant dans le fichier, aucun élément n'a été supprimé..");
            }
        }
        public void AjouterUnClient(Client cli)     // Ajoute un client dans la liste et dans le fichier client
        {
            listCli.Add(cli);
            try
            {
                CreationFichierSiNonExistant();
                StreamWriter fluxEcriture = new StreamWriter("FichierClient.txt", true);
                fluxEcriture.WriteLine(cli.ListAttributsAjoutFichier());
                fluxEcriture.Close();
                Console.WriteLine("Client ajouté à la base de données.");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message +"parc.AjouterUnClient()");
                Console.ReadKey();
            }
        }
        public void SupprimerUnClient(Client cli)   // Supprime un client de la liste et du fichier client
        {
            for (int i = 0; i < listCli.Count; i++)
            {
                if (cli.Prenom == listCli[i].Prenom && cli.Nom == listCli[i].Nom)
                {
                    listCli.RemoveAt(i);
                    break;
                }
            }
            SupprimerObjetFichier(cli.Id, "FichierClient.txt");
            Console.WriteLine("Client supprimé de la base de données.");
            Console.ReadKey();
        }
        public void AjouterUnVehicule( int typeVeh)     // Ajoute un véhicule dans la liste et dans le fichier véhicule
        {
            Console.WriteLine("Marque :");
            string marque = Console.ReadLine();
            Console.WriteLine("Gamme :");
            string gamme = Console.ReadLine();
            Console.WriteLine("Modèle :");
            string modele = Console.ReadLine();
            Console.WriteLine("Nombre de places :");
            int nbPlace;
            while (!int.TryParse(Console.ReadLine(), out nbPlace))
            {
                Console.WriteLine("Valeure incorrect..\n Veuillez Resaisir la valeure :");
            }
            Console.WriteLine("Permis nécessaire pour l'utilisation du véhicule :");
            string permisUtil = Console.ReadLine();
            Console.WriteLine("Immatriculation :");
            string imma = Console.ReadLine();
            Console.WriteLine("Prix de Location à la journée :");
            double prixLocAuJour;
            while (!double.TryParse(Console.ReadLine(), out prixLocAuJour))
            {
                Console.WriteLine("Valeure incorrect..\n Veuillez Resaisir la valeure :");
            }
            Vehicule veh;
            if(typeVeh == 1)
            {
                Console.WriteLine("Nombre de Valise :");
                int nbValise;
                while (!int.TryParse(Console.ReadLine(), out nbValise))
                {
                    Console.WriteLine("Valeure incorrect..\n Veuillez Resaisir la valeure :");
                }

                Console.WriteLine("Nombre de Portes :");
                int nbPorte;
                while (!int.TryParse(Console.ReadLine(), out nbPorte))
                {
                    Console.WriteLine("Valeure incorrect..\n Veuillez Resaisir la valeure :");
                }
                Console.WriteLine("Type de boite :\n(1) Automatique\n(2) Manuelle");
                string boite = Console.ReadLine();
                while (boite != "1" && boite != "2")
                {
                    Console.WriteLine("Valeure incorrect..\n Veuillez Resaisir la valeure :");
                    boite = Console.ReadLine();
                }
                if (boite == "1")
                {
                    boite = "automatique";
                }
                else
                {
                    boite = "manuelle";
                }
                veh = new Voiture(Convert.ToUInt32(ListVeh.Count), marque, gamme, 0, modele, nbPlace, permisUtil, imma, prixLocAuJour, nbValise, nbPorte, boite);
            }
            else if(typeVeh == 2)
            {
                Console.WriteLine("Volume Moteur : (cm3)");
                int volumeMoteur;
                while (!int.TryParse(Console.ReadLine(), out volumeMoteur))
                {
                    Console.WriteLine("Valeure incorrect..\n Veuillez Resaisir la valeure :");
                }
                veh = new Moto(Convert.ToUInt32(ListVeh.Count), marque, gamme, 0, modele, nbPlace, permisUtil, imma, prixLocAuJour, volumeMoteur);
            }
            else
            {
                Console.WriteLine("Volume du camion : (m3)");
                int volume;
                while (!int.TryParse(Console.ReadLine(), out volume))
                {
                    Console.WriteLine("Valeure incorrect..\n Veuillez Resaisir la valeure :");
                }
                Console.WriteLine("Charge Autorisé : (Kg)");
                double chargeAutorise;
                while (!double.TryParse(Console.ReadLine(), out chargeAutorise))
                {
                    Console.WriteLine("Valeure incorrect..\n Veuillez Resaisir la valeure :");
                }
                veh = new Camion(Convert.ToUInt32(ListVeh.Count), marque, gamme, 0, modele, nbPlace, permisUtil, imma, prixLocAuJour, volume, chargeAutorise);
            }
            try
            {
                CreationFichierSiNonExistant();
                StreamWriter fluxEcriture = new StreamWriter("FichierVehicule.txt", true);
                fluxEcriture.WriteLine(veh.ListAttributsAjoutFichier());
                listVeh.Add(veh);
                Console.WriteLine("Vehicule ajouté à la base de données.");
                fluxEcriture.Close();
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "parc.AjouterUnVehicule()");
                Console.ReadKey();
            }
        }
        public void SupprimerUnVehicule(int idVeh)  // Supprime un véhicule de la liste et du fichier véhicule
        {ChargementDonnees();
            if (idVeh > 0)
            {
                SupprimerObjetFichier(Convert.ToUInt32(idVeh), "FichierVehicule.txt");
                listVeh.RemoveAt(idVeh);
                Console.WriteLine("Vehicule supprimé de la base de données.");
            }
            else
            {
                Console.WriteLine("Liste Véhicule Vide, rien n'a été supprimé");
                Console.ReadKey();
            }
        }
        public void AjouterUneLocation(Location loca)   // Ajoute une location dans la liste et dans le fichier location
        {
            try
            {
                CreationFichierSiNonExistant();
                StreamWriter fluxEcriture = new StreamWriter("FichierLocation.txt", true);
                fluxEcriture.WriteLine(loca.ListAttributsAjoutFichier());
                listLoca.Add(loca);
                Console.WriteLine("Location ajouté à la base de données.");
                Console.ReadKey();
                fluxEcriture.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "parc.AjouterUneLocation()");
                Console.ReadKey();
            }
            Console.WriteLine("Location ajoutée à la base de données.");
        }
        public void SupprimerUneLocation(Location loc)   // Supprime une location de la liste et du fichier location
        {
            for (int i = 0; i < listLoca.Count; i++)
            {
                if(listLoca[i].Vehi.Imma == loc.Vehi.Imma)
                {
                    listLoca.RemoveAt(i);
                    break;
                }
            }

            SupprimerObjetFichier(loc.Id, "FichierLocation.txt");
            Console.WriteLine("Location supprimé de la base de données.");
        }
        public void AfficherlistVehicule()  // Affiche la liste de véhicules
        {

            Console.WriteLine("Il existe actuellement : " + listVeh.Count + " vehicule(s)");
            if (listVeh.Count > 0)
            {
                Console.ReadKey();
                for (int i = 0; i < listVeh.Count; i++)
                {
                    Console.WriteLine("(" + i + ")");
                    Console.WriteLine(listVeh[i].ToString());
                    Console.WriteLine("\n\n");
                }
            }
            else
            {
                Console.WriteLine("Il n'existe aucun Vehicule..");
                Console.ReadKey();
            }
            Console.ReadKey();
        }
        public void AfficherlistClient()    // Affiche la liste des clients
        {
            Console.WriteLine("Il existe actuellement : " + listCli.Count + " client(s)");
            if (listCli.Count > 0)
            {
                Console.ReadKey();
                for (int i = 0; i < listCli.Count; i++)
                {
                    Console.WriteLine("(" + i + ")\n");
                    Console.WriteLine(listCli[i].ToString());
                    Console.WriteLine("\n\n");
                }
            }
            else
            {
                Console.WriteLine("Il n'existe aucun Client..");
                Console.ReadKey();
            }
            Console.ReadKey();
        }
        public void AfficherlistLocation()  // Affiche la liste de locations
        {
            ChargementDonnees();
            Console.WriteLine("Il existe actuellement : " + listLoca.Count + " location(s)");
            if (listLoca.Count > 0)
            {
                Console.ReadKey();
                for (int i = 0; i < listLoca.Count; i++)
                {
                    Console.WriteLine("(" + i + ")\n");
                    Console.WriteLine(listLoca[i].ToString());
                    Console.WriteLine("\n\n");
                }
            }
            else
            {
                Console.WriteLine("Il n'existe aucune Location..");
                Console.ReadKey();
            }
            Console.ReadKey();
        }
        public void ChargementDonnees()     // Chargement de tous les contenus des fichiers dans les listes
        {
            listCli = new List<Client>();
            listVeh = new List<Vehicule>();
            listLoca = new List<Location>();
            try
            {
                CreationFichierSiNonExistant();
                StreamReader fluxLec = new StreamReader("FichierClient.txt");   // Chargement du FichierClient dans la liste listCli
                while (fluxLec.EndOfStream == false)
                {
                    string[] tab = fluxLec.ReadLine().Split(';');
                    Client cli = new Client(Convert.ToUInt32(tab[0]), tab[1], tab[2], tab[3], Convert.ToUInt32(tab[4]), Convert.ToBoolean(tab[5]), Convert.ToBoolean(tab[6]), Convert.ToBoolean(tab[7]), Convert.ToBoolean(tab[8]));

                    listCli.Add(cli);
                }
                fluxLec.Close();

                fluxLec = new StreamReader("FichierVehicule.txt");        // Chargement du FichierVehicule dans la liste listVeh
                while (fluxLec.EndOfStream == false)
                {
                    string[] tab = fluxLec.ReadLine().Split(';');
                    Vehicule veh = null;
                    if (tab.Length == 10)
                    {
                        veh = new Moto(Convert.ToUInt32(tab[0]), tab[1], tab[2], Convert.ToInt32(tab[3]), tab[4], Convert.ToInt32(tab[5]), tab[6], tab[7], Convert.ToDouble(tab[8]), Convert.ToInt32(tab[9]));
                    }
                    else if (tab.Length == 11)
                    {
                        veh = new Camion(Convert.ToUInt32(tab[0]), tab[1], tab[2], Convert.ToInt32(tab[3]), tab[4], Convert.ToInt32(tab[5]), tab[6], tab[7], Convert.ToDouble(tab[8]), Convert.ToInt32(tab[9]), Convert.ToDouble(tab[10]));
                    }
                    else if (tab.Length == 12)
                    {
                        veh = new Voiture(Convert.ToUInt32(tab[0]), tab[1], tab[2], Convert.ToInt32(tab[3]), tab[4], Convert.ToInt32(tab[5]), tab[6], tab[7], Convert.ToDouble(tab[8]), Convert.ToInt32(tab[9]), Convert.ToInt32(tab[10]), tab[11]);
                    }
                    listVeh.Add(veh);
                }
                fluxLec.Close();

                fluxLec = new StreamReader("FichierLocation.txt");       // Chargement du FichierLocation dans la liste listLoca
                while (fluxLec.EndOfStream == false)
                {
                    string[] tab = fluxLec.ReadLine().Split(';');
                    Vehicule veh = null;
                    Client cli = null;
                    Trajet tra = null;
                    Location loca = null;
                    if (tab.Length == 26)
                    {
                        veh = new Moto(Convert.ToUInt32(tab[0]), tab[1], tab[2], Convert.ToInt32(tab[3]), tab[4], Convert.ToInt32(tab[5]), tab[6], tab[7], Convert.ToDouble(tab[8]), Convert.ToInt32(tab[9]));
                        cli = new Client(Convert.ToUInt32(tab[10]), tab[11], tab[12], tab[13], Convert.ToUInt32(tab[14]), Convert.ToBoolean(tab[15]), Convert.ToBoolean(tab[16]), Convert.ToBoolean(tab[17]), Convert.ToBoolean(tab[18]));
                        tra = new Trajet(tab[19], tab[20], Convert.ToInt32(tab[21]));
                        loca = new Location(veh, cli, tra, Convert.ToUInt32(tab[22]), tab[23], tab[24], Convert.ToDouble(tab[25]));
                        listLoca.Add(loca);
                    }
                    else if (tab.Length == 27)
                    {
                        veh = new Camion(Convert.ToUInt32(tab[0]), tab[1], tab[2], Convert.ToInt32(tab[3]), tab[4], Convert.ToInt32(tab[5]), tab[6], tab[7], Convert.ToDouble(tab[8]), Convert.ToInt32(tab[9]), Convert.ToDouble(tab[10]));
                        cli = new Client(Convert.ToUInt32(tab[11]), tab[12], tab[13], tab[14], Convert.ToUInt32(tab[15]), Convert.ToBoolean(tab[16]), Convert.ToBoolean(tab[17]), Convert.ToBoolean(tab[18]), Convert.ToBoolean(tab[19]));
                        tra = new Trajet(tab[20], tab[21], Convert.ToInt32(tab[22]));
                        loca = new Location(veh, cli, tra, Convert.ToUInt32(tab[23]), tab[24], tab[25], Convert.ToDouble(tab[26]));
                        listLoca.Add(loca);
                    }
                    else if (tab.Length == 28)
                    {
                        veh = new Voiture(Convert.ToUInt32(tab[0]), tab[1], tab[2], Convert.ToInt32(tab[3]), tab[4], Convert.ToInt32(tab[5]), tab[6], tab[7], Convert.ToDouble(tab[8]), Convert.ToInt32(tab[9]), Convert.ToInt32(tab[10]), tab[11]);
                        cli = new Client(Convert.ToUInt32(tab[12]), tab[13], tab[14], tab[15], Convert.ToUInt32(tab[16]), Convert.ToBoolean(tab[17]), Convert.ToBoolean(tab[18]), Convert.ToBoolean(tab[19]), Convert.ToBoolean(tab[20]));
                        tra = new Trajet(tab[21], tab[22], Convert.ToInt32(tab[23]));
                        loca = new Location(veh, cli, tra, Convert.ToUInt32(tab[24]), tab[25], tab[26], Convert.ToDouble(tab[27]));

                    }
                    listLoca.Add(loca);
                }
                fluxLec.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "parc.ChargementDonnees");
                Console.ReadKey();
            }
        }

    }
}
