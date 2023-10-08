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
using System.IO;
using MySql.Data.MySqlClient;

namespace Projet_INFO_DD_MG_2019
{
    /// <summary>
    /// Logique d'interaction pour Verficationmodifiermdp.xaml
    /// </summary>
    public partial class Verficationmodifiermdp : Window
    {
        public Verficationmodifiermdp()
        {
            InitializeComponent();
        }
        //int k = 0;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string id = "";
            string mdp = recupmdp.Password;
            string nouveaumdp = mdpnew.Password;
            string refnouveaumdp = repnewmdp.Password;
                                                                // Refaire code Modifier mot de passe
            try
            {
                StreamReader sr = new StreamReader("ProfilClient.txt");

                while (sr.EndOfStream == false)
                {
                    string[] tab = sr.ReadLine().Split(';');
                    id = tab[0];
                }
                sr.Close();

            }
            catch (Exception m)
            {
                MessageBox.Show(m.Message + "Verificationmodifiermdp.xaml.cs l.51");
            }

            string idmysql = "SERVER=localhost;PORT=3306;DATABASE=travelpark;UID=root;PASSWORD=Scorpio-Leon123!;";
            MySqlConnection mysqlok = new MySqlConnection(idmysql);
            mysqlok.Open();
            MySqlCommand cmd = new MySqlCommand("select mdpC from client where idC='" + id + "';", mysqlok);


            MySqlDataReader reader = cmd.ExecuteReader();

            string valeurmdp = "";
            bool trouver = false;

            while (reader.Read())
            {

                for (int i = 0; i < reader.FieldCount; i++)
                {

                    valeurmdp = reader.GetValue(i).ToString();
                    if (valeurmdp == mdp)
                    {
                        trouver = true;
                    }

                }
            }
            mysqlok.Close();


            if (trouver == true && nouveaumdp == refnouveaumdp)
            {

                MySqlCommand changermdp = new MySqlCommand("update travelpark.client set mdpC ='" + nouveaumdp + "' where idC='" + id + "' ;", mysqlok);

                mysqlok.Open();
                changermdp.ExecuteNonQuery();
                mysqlok.Close();

                try
                {
                    StreamWriter sw = new StreamWriter("ProfilClient.txt");
                    sw.Write(id + ";" + nouveaumdp);
                    sw.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "verificationmodifiermdp.xaml.cs l.97");
                }

                MessageBox.Show("Votre mot de passe à bien été changé !");
                Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
    }
}
