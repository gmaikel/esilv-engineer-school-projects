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
    /// Logique d'interaction pour profil.xaml
    /// </summary>
    public partial class profil : Window
    {
        string id ="";
        string nomPrenom = "";
        string numTel = "";
        string permis1 = "";
        string permis2 = "";
        string cPromo = "";
        public profil()
        {
            InitializeComponent();
            string[] tab = null;
            try
            {
                StreamReader sr = new StreamReader("profilClient.txt");
                tab = sr.ReadLine().Split(';');
                sr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "profil.xaml.cs l.40");
            }
            string requetsql = "SERVER=localhost;PORT=3306;DATABASE=TravelPark;UID=root;PASSWORD=Scorpio-Leon123!;";
            MySqlConnection connec = new MySqlConnection(requetsql);
            MySqlCommand comma = new MySqlCommand("select * from client where idC ='" + tab[0] + "';", connec);
            connec.Open();
            MySqlDataReader reader = comma.ExecuteReader();
            tab = new string[7];
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string valeurid = reader.GetValue(i).ToString();
                    if (valeurid != null)
                    {
                        tab[i] = valeurid;
                    }
                }
            }

            idC.Content = tab[0];
            nomPrenomC.Content = tab[1];
            numTelC.Content = tab[3];
            permisC1.Content = tab[4];
            permisC2.Content = tab[5];
            cPromoC.Content = tab[6];

            id = tab[0];
            nomPrenom = tab[1];
            numTel = tab[3];
            permis1 = tab[4];
            permis2 = tab[5];
            cPromo= tab[6];

            connec.Close();
           
        }

        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow changer = new MainWindow();
            changer.Connect(true);
            this.Visibility = Visibility.Hidden;
            changer.Show();
        }

        private void Smc_Click(object sender, RoutedEventArgs e)
        {
            verificationsupprimer changer = new verificationsupprimer();
            changer.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow PageMain = new MainWindow();
            PageMain.Connect(true);
            this.Visibility = Visibility.Hidden;
            PageMain.Show();
        }

        

        private void Entrercode_Click(object sender, RoutedEventArgs e)
        {
            ModifierProfil modif = new ModifierProfil(id, numTel, permis1, permis2, cPromo);
            this.Visibility = Visibility.Hidden;
            modif.Show();
        }
    }
}
