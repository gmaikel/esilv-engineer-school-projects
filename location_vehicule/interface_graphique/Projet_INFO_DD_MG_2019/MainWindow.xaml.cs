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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.IO;


namespace Projet_INFO_DD_MG_2019
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool connect = false;
        private string recherchePark = "";
        public void Connect(bool conn)
        {
            connect = conn;
        }
        public MainWindow()
        {
            try
            {
                StreamReader sr = new StreamReader("profilClient.txt");

                bool verifProfilClient = true;
                if (sr.EndOfStream == true)
                {
                    verifProfilClient = false;
                }
                sr.Close();
                if (!verifProfilClient)
                {
                    StreamWriter sw = new StreamWriter("profilClient.txt");
                    sw.Write(";");
                    sw.Close();
                }
            }
            catch
            {
                try
                {
                    StreamWriter sw = new StreamWriter("profilClient.txt");
                    sw.Write(";");
                    sw.Close();
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message + "\n MainWindow.xaml.cs l.48");
                }
            }
            InitializeComponent();

            

            string requetsql = "SERVER=localhost;PORT=3306;DATABASE=TravelPark;UID=root;PASSWORD=Scorpio-Leon123!;";
            MySqlConnection connec = new MySqlConnection(requetsql);
            MySqlCommand comma = new MySqlCommand("select nomSocieteP,adresseP from parking;", connec);
            connec.Open();

            MySqlDataReader reader = comma.ExecuteReader();
            string[] tab = new string[20];
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string valeurid = reader.GetValue(i).ToString();
                    if (valeurid != null)
                    {
                        if (i % 2 == 0)
                        {
                            tab[i] = valeurid;
                        }
                        else
                        {
                            tab[i] = tab[i - 1] + "\n" + valeurid;
                            listPark.Items.Add(tab[i] + "\n");
                        }
                    }
                }
            }
            connec.Close();

            string adresse = TextBoxInfo.Text;
            try
            {
                StringBuilder queryAddress = new StringBuilder();
                queryAddress.Append("http://maps.google.com/maps?q=");
                if (adresse != string.Empty)
                {
                    queryAddress.Append(adresse);
                }
                webBrowser.Navigate(queryAddress.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        private void GooglMapSearch(object sender, RoutedEventArgs e)
        {

            string adresse = "";
            if (recherchePark == "")
            {
                adresse = TextBoxInfo.Text;
            }
            else
            {
                adresse = recherchePark;
            }
            try
            {
                StringBuilder queryAddress = new StringBuilder();
                queryAddress.Append("http://maps.google.com/maps?q=");
                if (adresse != string.Empty)
                {
                    queryAddress.Append(adresse);
                }
                webBrowser.Navigate(queryAddress.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "MainWindow.xaml.cs l.114");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (connect)
            {
                profil changer = new profil();
                this.Visibility = Visibility.Hidden;
                changer.Show();
            }
            else
            {
                Connexion pageConnect = new Connexion();
                this.Visibility = Visibility.Hidden;
                pageConnect.Show();
            }
        }

        private void Lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string ok = listPark.SelectedItem.ToString();
            string[] tab = ok.Split('\n');
            recherchePark = tab[1];
            GooglMapSearch(sender, e);
        }

        private void ProfilParking(object sender, RoutedEventArgs e)
        {
            if (connect)
            {
                try
                {
                    StreamWriter sw = new StreamWriter("profilClient.txt", true);
                    sw.Write(";" + recherchePark);
                    sw.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Reservation reser = new Reservation();
                this.Visibility = Visibility.Hidden;
                reser.Show();
            }
            else
            {
                Connexion connect = new Connexion();
                this.Visibility = Visibility.Hidden;
                connect.Show();
            }
        }
    }
}
