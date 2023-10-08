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
    /// Logique d'interaction pour verificationsupprimer.xaml
    /// </summary>
    public partial class verificationsupprimer : Window
    {
        public verificationsupprimer()
        {
            InitializeComponent();
        }

        private void Oui_Click(object sender, RoutedEventArgs e)
        {
            string[] tab = null;
            try
            {
                StreamReader sr = new StreamReader("ProfilClient.txt");

                while (sr.EndOfStream == false)
                {
                    tab = sr.ReadLine().Split(';');
                }
                sr.Close();
                string idmysql = "SERVER=localhost;PORT=3306;DATABASE=travelpark;UID=root;PASSWORD=Scorpio-Leon123!;";
                MySqlConnection mysqlok = new MySqlConnection(idmysql);

                MySqlCommand cmd = new MySqlCommand("delete from se_garer where idC = '"+tab[0]+"'; delete from vehicule where idC = '"+tab[0]+"'; delete from client where idC = '"+tab[0]+"';", mysqlok);
                mysqlok.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                MessageBox.Show("Votre compte a bien été supprimé !");
                StreamWriter sw = new StreamWriter("ProfilClient.txt");
                sw.Write(";");
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "VerificationSupprimer.xaml.cs l.57");
            }
        }
        private void Non_Click_1(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
    }
}
