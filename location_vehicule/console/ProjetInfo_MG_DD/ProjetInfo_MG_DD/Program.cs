using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;


namespace ProjetInfo_MG_DD
{
    class Program
    {
        static void Main(string[] args)     // Main de la classe Program, première étape de la compilation du programme
        {
            bool boucleInfini = true;
            do
            {
                Parc parc = new Parc();
                bool nvClient = false;
                Client cli = MenuPrincipal(ref nvClient, parc);
                if (nvClient)
                {
                    parc.AjouterUnClient(cli);
                }
                if (cli.Admin)
                {
                    PageAdmin(cli, parc);
                }
                do
                {
                    PageClientConnecté(cli, parc);
                    PageAdmin(cli, parc);
                } while (boucleInfini);

            } while (boucleInfini);
            Console.ReadKey();
        }
        static int security(int nbPosibiliteChoix)  // Méthode de sécurité de saisie
        {
            string saisie = "";
            bool redemande = false;
            int rep = 0;
            do
            {
                if (redemande)
                {
                    Console.WriteLine("Saisie Incorrect, veuillez resaisir votre réponse..");
                }
                redemande = true;
                saisie = Console.ReadLine();
            } while (!int.TryParse(saisie, out rep) || rep <= 0 || rep > nbPosibiliteChoix);
            return rep;
        }
        static void PageAdmin(Client cli, Parc parc)   // Page du compte administrateur
        {
            bool boucleInfini = true;
            do
            {
                Console.Clear();
                Console.WriteLine("\t\tPage Administrateur :\n\n(1) Fonctions Page Client\n(2) Afficher Liste Clients\n(3) Afficher Liste Véhicules\n(4) Supprimer un véhicule\n(5) Ajouter un véhicule\n(6) Afficher Liste Locations\n(7) Ne plus être administrateur");
                switch (security(7))
                {
                    case 1: boucleInfini = false; break;
                    case 2: parc.AfficherlistClient(); break;
                    case 3: parc.AfficherlistVehicule(); break;
                    case 4: Console.Clear();
                        parc.AfficherlistVehicule();
                        Console.WriteLine("\nEntrez l'identifiant du véhicule à supprimer :");
                        if(parc.ListVeh.Count > 0)
                        {
                            int rep = 0;
                            string idVeh = Console.ReadLine();
                            while(!int.TryParse(idVeh, out rep) || rep <0 || rep > parc.ListVeh.Count )
                            {
                                Console.WriteLine("Saisie Incorrect..");
                            }
                            parc.SupprimerUnVehicule(rep);
                        }
                        else
                        {
                            Console.WriteLine("Il n'existe aucun véhicule..");
                            Console.ReadKey();
                        }
                        break;
                    case 5:
                        Console.Clear();
                        Console.WriteLine("\t\tAjout d'un véhicule :\n(1) Voiture\n(2) Moto\n(3) Camion");
                        parc.AjouterUnVehicule(security(3)); break;
                    case 6: parc.AfficherlistLocation(); break;
                    case 7:
                        cli.Admin = false;
                        cli.ModifierClientFichier();
                        boucleInfini = false;
                        Console.WriteLine("Vous n'êtes plus compte admin..");
                        Console.ReadKey();
                        break;
                }
            } while (boucleInfini);
        }
        static Client MenuPrincipal( ref bool nvClient, Parc parc)          // Page de menu principal
        {
            Console.Clear();
            Console.WriteLine("\t\t\t\t\tMGDD\n\t\tApplication de location de vehicule automatisé.\n\n\n(1) Se Connecter\n(2) S'inscrire");
            int rep = security(2);
            if(rep == 1)
            {
                return Connection(parc);
            }
            else
            {
                nvClient = true;
                return Inscription(parc, ref nvClient);
            }
        }
        static Client Connection(Parc parc) // Page de connection
        {
            byte varBoucleInfini = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("Page de Connexion :\n\nEntrez votre Prénom :");
                string prenom = Console.ReadLine();
                Console.WriteLine("Entrez votre nom :");
                string nom = Console.ReadLine();
                Console.WriteLine("\nConnection en tant que " + prenom + " " + nom + " :\nMot de passe :");
                string mdp = Console.ReadLine();
                Console.WriteLine("\nChargement...\n");
                for(int i=0; i<parc.ListCli.Count; i++)
                {
                    if(parc.ListCli[i].Prenom == prenom && parc.ListCli[i].Nom == nom && parc.ListCli[i].Mdp == mdp)
                    {
                        return parc.ListCli[i];
                    }
                }
                Console.WriteLine("Information Incorrect.. \n(1) Retour au menu principal\n(autre) Resaisir informations");
                if (Console.ReadLine() == "1")
                {
                    bool val = false;
                    return MenuPrincipal(ref val, parc);
                }
            } while (varBoucleInfini == 0);
            return null;
        }
        static Client Inscription(Parc parc, ref bool nvClient) // Page d'inscription
        {
            Console.Clear();
            Console.WriteLine("Page d'Inscription :\n\nEntrez votre Prénom :");
            string prenom = Console.ReadLine();
            Console.WriteLine("Entrez votre nom :");
            string nom = Console.ReadLine();
            Console.WriteLine("Saisissez un mot de passe :");
            string mdp = Console.ReadLine();
            Console.WriteLine("Confirmez votre mot de passe :");
            while (Console.ReadLine() != mdp)
            {
                Console.WriteLine("Les deux mots de passes sont différents, veuillez resaisir vos deux mots de passes :");
                mdp = Console.ReadLine();
                Console.WriteLine("Confirmez votre mot de passe :");
            }

            for (int i = 0; i < parc.ListCli.Count; i++)
            {
                if (parc.ListCli[i].Prenom == prenom && parc.ListCli[i].Nom == nom && parc.ListCli[i].Mdp == mdp)
                {
                    Console.WriteLine("\nVous avez déja un compte existant..\nVous êtes connecté..");
                    nvClient = false;
                    Console.ReadKey();
                    return parc.ListCli[i];
                }
            }
            Client cli = new Client(Convert.ToUInt32(parc.ListCli.Count), prenom, nom, mdp);
            return cli;                         // retourne client pour le main
        }
        static void PageClientConnecté(Client cli, Parc parc)          // Page du client qui est conneccté
        {
            bool boucleInfini = true;
            do
            {
                Console.Clear();
                Console.WriteLine("Vous êtes connecté..\n");
                Console.WriteLine(cli.ToString() + "\n\n");
                cli.ChargementLocationClient();
                cli.AfficherlistLocationClient();
                bool val = false;
                Console.WriteLine("\n(1) Modifier Mdp\n(2) Modifier Permis\n(3) Transformer compte en compte admin\n(4) Effectuer une recherche de véhicule\n(5) Supprimer Votre Compte \n(6) Changer de Compte\n(7) Supprimer Une Location");
                switch (security(7))
                {
                    case 1: cli.ModifierMdp(); break;
                    case 2: cli.ModifierPermis(); break;
                    case 3:
                        if (cli.Admin)
                        {
                            Console.WriteLine("Votre compte est déjà un compte admin..");
                            Console.ReadKey();
                        }
                        else
                        {
                            cli.TransformerCompteAdmin();
                        }
                        if (cli.Admin)
                        {
                            boucleInfini = false;
                        }
                        break;
                    case 4: RechercheVehicule(cli); break;
                    case 5: parc.SupprimerUnClient(cli);
                        cli = MenuPrincipal(ref val, parc);
                        if (val)
                        {
                            parc.AjouterUnClient(cli);
                        }
                        break;
                    case 6: cli = MenuPrincipal(ref val, parc);
                        if (val)
                        {
                            parc.AjouterUnClient(cli);
                        }
                        break;
                    case 7:
                        if(cli.ListLocaClient.Count != 0)
                        {
                            Console.WriteLine("Entrez l'identifiant de la location à supprimer :");
                            int id = 0;
                            string idVeh = Console.ReadLine();
                            while (!int.TryParse(idVeh, out id) || id < 0 || id > parc.ListVeh.Count)
                            {
                                Console.WriteLine("Saisie Incorrect..");
                            }
                            Console.WriteLine("\nChargement..");
                            cli.SupprimerUneLocation(Convert.ToUInt32(id), parc.SupprimerObjetFichier);
                            cli.ListLocaClient.RemoveAt(id);
                        }
                        else
                        {
                            Console.WriteLine("Vous n'avez aucune réservation en cours pour le moment..");
                            Console.ReadKey();
                        }
                        break;
                }
            } while (boucleInfini);
        }
        static void RechercheVehicule(Client cli)
        {
            Parc parc = new Parc();
            Console.WriteLine("Liste Vehicule disponible :\n\n");
            parc.AfficherlistVehicule();
            Console.WriteLine("Louer une voiture\n(1) Oui\n(2) Non");

            if(security(2) == 1)
            {
                Console.WriteLine("Sélectionner l'identifiant du vehicule que vous voulez louer :");
                CreerLocation(cli, parc.ListVeh[security(parc.ListVeh.Count)], parc);
            }
        }
        static void CreerLocation(Client cli, Vehicule veh, Parc parc)    // Méthode de création d'une location
        {
            Console.Clear();
            Console.WriteLine("\t\tPage de Location :\n\nLocation du vehicule :");
            veh.ToString();
            Trajet tra = new Trajet();
            Location loc = new Location(veh, cli, tra);
            parc.AjouterUneLocation(loc);
            cli.ChargementLocationClient();
        }
    }
}

