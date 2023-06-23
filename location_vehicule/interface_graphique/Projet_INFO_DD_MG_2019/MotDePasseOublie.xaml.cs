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

namespace Projet_INFO_DD_MG_2019
{
    /// <summary>
    /// Logique d'interaction pour motdepasseoublie.xaml
    /// </summary>
    public partial class motdepasseoublie : Window
    {
        public motdepasseoublie()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string id = mdprecupere.Text;


            string idmysql = "SERVER=localhost;PORT=3306;DATABASE=travelpark;UID=root;PASSWORD=Scorpio-Leon123!;";
            MySqlConnection mysqlok = new MySqlConnection(idmysql);

            MySqlCommand cmd = new MySqlCommand("select mdpC from client where idC='" + id + "';", mysqlok);
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
                recuperemdp.Text = "votre mot de passe vous a été envoyé par mail !" + "(" + valeurid + ")";
            }
            else
            {
                recuperemdp.Text = "Votre identifiant n'existe pas !";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Connexion connec = new Connexion();
            this.Visibility = Visibility.Hidden;
            connec.Show();
        }
        
    }
}
