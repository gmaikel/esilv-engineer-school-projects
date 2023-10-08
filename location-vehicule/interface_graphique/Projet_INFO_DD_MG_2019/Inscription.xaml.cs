using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.IO;

namespace Projet_INFO_DD_MG_2019
{
    /// <summary>
    /// Logique d'interaction pour Inscription.xaml
    /// </summary>
    public partial class Inscription : Window
    {
        public Inscription()
        {
            InitializeComponent();
        }

        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            Connexion changer = new Connexion();
            this.Visibility = Visibility.Hidden;
            changer.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string mdps = mdp.Password;
            bool secuMdpptVirgule = false;
            for (int i = 0; i < mdps.Length; i++)
            {
                if (mdps[i] == ';')
                {
                    secuMdpptVirgule = true;
                }
            }
            if (!secuMdpptVirgule)
            {
                string iden = id.Text;
                string nomprenom = np.Text;
                string numero = num.Text;
                string permis1 = p1.Text;
                string permis2 = p2.Text;

                

                if (nomprenom == "")
                {
                    MessageBox.Show("Vous devez renseigner votre nom et prenom !");
                }
                else if (iden == "")
                {
                    MessageBox.Show("Vous devez renseigner un identifiant !");
                }
                else if (mdps == "")
                {
                    MessageBox.Show("Vous devez renseigner un mot de passe !");
                }

                else
                {
                    string idmysql = "SERVER=localhost;PORT=3306;DATABASE=travelpark;UID=root;PASSWORD=Scorpio-Leon123!;";
                    MySqlConnection mysqlok = new MySqlConnection(idmysql);

                    MySqlCommand cmd = new MySqlCommand("select mdpC from client where idC=" + "'" + iden + "'" + " ;", mysqlok);
                    mysqlok.Close();

                    mysqlok.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    string valeurid = "";
                    string trouver = "";
                    while (reader.Read())
                    {

                        for (int i = 0; i < reader.FieldCount; i++)
                        {

                            valeurid = reader.GetValue(i).ToString();
                            if (valeurid != null)
                            {
                                trouver = "ok";
                            }

                        }
                    }
                    mysqlok.Close();


                    if (trouver == "ok")
                    {
                        idexisteconnexion changer = new idexisteconnexion();
                        this.Visibility = Visibility.Hidden;
                        changer.Show();
                    }
                    else
                    {
                        mysqlok.Open();
                        MySqlCommand ajouter = new MySqlCommand("insert into `TravelPark`.`client` (`idC`, `nomprenomC`, `mdpC`, `numtelC`, `permisC1`, `permisC2`, `idCodePromoC`) values ( '" + iden + "','" + nomprenom + "','" + mdps + "','" + numero + "','" + permis1 + "','" + permis2 + "','aaaaa' );", mysqlok);
                        
                        ajouter.ExecuteNonQuery();
                        mysqlok.Close();
                        try
                        {
                            StreamWriter sw = new StreamWriter("ProfilClient.txt");
                            sw.Write(iden + ";" + mdps);
                            sw.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + "Inscription.xaml.cs l.120");
                        }
                        MessageBox.Show("Vous êtes désormais inscrit, vous allez être dirigé vers la page de connexion");
                        MainWindow changer = new MainWindow();
                        changer.Connect(true);
                        Visibility = Visibility.Hidden;
                        changer.Show();
                    }

                }
            }
            else
            {
                MessageBox.Show("Votre mot de passe ne doit pas contenir de ';' \nVeuillez en choisir un nouveau");
            }
        }
        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Connexion Connec = new Connexion();
            this.Visibility = Visibility.Hidden;
            Connec.Show();
        }
    }
}
