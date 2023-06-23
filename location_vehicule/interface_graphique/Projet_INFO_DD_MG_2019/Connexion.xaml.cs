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
    /// Logique d'interaction pour Connexion.xaml
    /// </summary>
    public partial class Connexion : Window
    {
        public Connexion()
        {
            InitializeComponent();
            string[] tab = null;
            tab = RecuperationIdMdp(tab);
            if(tab != null)
            {
                id_textbox.Text = tab[0];
                mdp_textbox.Password = tab[1];
            }
        }
        private void Cxn_button_Click(object sender, RoutedEventArgs e)
        {
            string id = id_textbox.Text;
            string mdp = mdp_textbox.Password;
            if (id == "")
            {
                MessageBox.Show("Veuillez remplir l'identifiant svp !");
            }
            else if (mdp == "")
            {
                MessageBox.Show("Veuillez remplir le mot de passe svp !");
            }
            else
            {
                string idmysql = "SERVER=localhost;PORT=3306;DATABASE=TravelPark;UID=root;PASSWORD=Scorpio-Leon123!;";
                MySqlConnection mysqlok = new MySqlConnection(idmysql);
                MySqlCommand cmd = new MySqlCommand("select idC, mdpC from client where idC = '" + id + "' and mdpC = '" + mdp + "';", mysqlok);
                mysqlok.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                bool trouve = false;
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string valeurid = reader.GetValue(i).ToString();
                        if (valeurid != null)
                        {
                            trouve = true;
                        }
                    }
                }
                mysqlok.Close();

                if (trouve)
                {
                    try
                    {
                        StreamWriter sw = new StreamWriter("ProfilClient.txt");
                        sw.Write(id + ";" + mdp);
                        sw.Close();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message + "Connesion.xaml.cs l.77");
                    }
                    MainWindow changer = new MainWindow();
                    changer.Connect(true);
                    this.Visibility = Visibility.Hidden;
                    changer.Show();
                }

                else
                {
                    MessageBox.Show("Identifiant inexistant ou mot de passe incorrect, veuillez recommencer !");
                }
            }
        }

        private void Ins_button_Click(object sender, RoutedEventArgs e)
        {
            Inscription changer = new Inscription();
            this.Visibility = Visibility.Hidden;
            changer.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            motdepasseoublie motoubli = new motdepasseoublie();
            this.Visibility = Visibility.Hidden;
            motoubli.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        
        public string[] RecuperationIdMdp(string[] tab)
        {
            try
            {
                StreamReader sr = new StreamReader("ProfilClient.txt");
                while (sr.EndOfStream == false)
                {
                    tab = sr.ReadLine().Split(';');
                }
                sr.Close();
        }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "Connexion.xaml.cs l.133");
            }
            return tab;
        }

        
    }
}
