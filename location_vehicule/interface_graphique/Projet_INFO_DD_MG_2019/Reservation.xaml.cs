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
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class Reservation : Window
    {
       

        double pourc = 0;
        public Reservation()
        {
            InitializeComponent();
            string[] tab = new string[3];
            try
            {
                StreamReader sr = new StreamReader("ProfilClient.txt");
                while (sr.EndOfStream == false)
                {
                    tab = sr.ReadLine().Split(';');
                }
                sr.Close();
            }
            catch (Exception me)
            {
                MessageBox.Show(me.Message);
            }

            string requetsql = "SERVER=localhost;PORT=3306;DATABASE=TravelPark;UID=root;PASSWORD=Scorpio-Leon123!;";
            MySqlConnection connec = new MySqlConnection(requetsql);
            MySqlCommand comma = new MySqlCommand("select nomprenomC from client where idC= '" + tab[0] + "';", connec);
            connec.Open();
            MySqlDataReader read = comma.ExecuteReader();

            while (read.Read())
            {
                nomp.Content = read.GetString(0);

            }
            connec.Close();
            MySqlCommand immav = new MySqlCommand("select imatV from vehicule where idC= '" + tab[0] + "';", connec);
            connec.Open();

            MySqlDataReader imaread = immav.ExecuteReader();

            while (imaread.Read())
            {
                immatri.Content = imaread.GetString(0);
            }
            connec.Close();

            MySqlCommand adressep = new MySqlCommand("select adresseP from parking where adressep= '" + tab[2] + "';", connec);
            connec.Open();

            MySqlDataReader adresse = adressep.ExecuteReader();

            while (adresse.Read())
            {
                adressse.Content = adresse.GetString(0);
            }
            connec.Close();

            MySqlCommand prix = new MySqlCommand("select prixHeure from parking where adressep= '" + tab[2] + "';", connec);
            connec.Open();

            MySqlDataReader prixachat = prix.ExecuteReader();

            while (prixachat.Read())
            {
                prix_achat.Content = prixachat.GetString(0);
            }
            connec.Close();


            MySqlCommand noms = new MySqlCommand("select nomSocieteP from parking where adressep = '" + tab[2] + "';", connec);
            connec.Open();

            MySqlDataReader nomsoci = noms.ExecuteReader();

            while (nomsoci.Read())
            {
                nomS.Content = nomsoci.GetString(0);
            }
            connec.Close();


            MySqlCommand codepromoC = new MySqlCommand("select idCodePromo, pourcentageCP from client C NATURAL JOIN promotion P where C.idCodePromoC = P.idCodePromo and C.idC = '" + tab[0] + "';", connec);
            connec.Open();

            MySqlDataReader codepr = codepromoC.ExecuteReader();
            
            

            while (codepr.Read())
            {
                pourc = Convert.ToDouble(codepr.GetString(1));
                double pourcet = pourc * 100;

                codePromo.Content = codepr.GetString(0) +" "+Convert.ToString(pourcet)+" %";

            }
            connec.Close();

        }


        private void ReserverParking(object sender, RoutedEventArgs e)
        {
            if (nombreheure.Text != "")
            {
                double m = Convert.ToDouble(nombreheure.Text.ToString());
                double n = Convert.ToDouble(prix_achat.Content);

                double prixtotal = n * m * (1 - pourc);

                total.Content = Convert.ToString(prixtotal);

                string[] tab = null;
                try
                {
                    StreamReader sr = new StreamReader("ProfilClient.txt");
                    tab = sr.ReadLine().Split(';');
                    sr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "Reservation.xaml.cs l.138");
                }
                string requetsql = "SERVER=localhost;PORT=3306;DATABASE=TravelPark;UID=root;PASSWORD=Scorpio-Leon123!;";
                MySqlConnection connec = new MySqlConnection(requetsql);
                MySqlCommand comma = new MySqlCommand("select idP from parking where adresseP = '" + tab[2] + "'", connec);
                connec.Open();
                MySqlDataReader read = comma.ExecuteReader();

                string idP = "";
                string[] tab1 = tab[0].Split('c');
                string idC = tab[0];
                connec.Close();
                string prixStat = Convert.ToString(total.Content);

                //connec.Open();
                //MySqlCommand comm = new MySqlCommand("insert into TravelPark.`se_Garer` (`idP`, `idC`, `prixStationnement`) values ('00" + idP + "','" + idC + "','" + Convert.ToDouble(prixStat) + "');", connec);
                //comm.ExecuteNonQuery();
                //MessageBox.Show("La réservation du parking enregistrée dans la base de donnée");
                //connec.Close();
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow coco = new MainWindow();
            this.Visibility = Visibility.Hidden;
            coco.Show();
        }
    }
}
