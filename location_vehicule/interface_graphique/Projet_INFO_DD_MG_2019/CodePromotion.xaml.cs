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
    /// Logique d'interaction pour codepromo.xaml
    /// </summary>
    public partial class codepromo : Window
    {
        public codepromo()
        {
            string[] idMdp = null;
            try
            {
                StreamReader sr = new StreamReader("ProfilClient.txt");

                while (sr.EndOfStream == false)
                {
                    idMdp = sr.ReadLine().Split(';');
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "CodePromotion.xaml.cs l.37");
            }
            string idmysql = "SERVER=localhost;PORT=3306;DATABASE=travelpark;UID=root;PASSWORD=Scorpio-Leon123!;";
            MySqlConnection mysqlok = new MySqlConnection(idmysql);

            MySqlCommand cmdver = new MySqlCommand("select idCodePromoC, pourcentageCP from client where idC ='" + idMdp[0] + "'", mysqlok);
            MySqlDataReader ver = cmdver.ExecuteReader();
            string[] truc = new string[ver.FieldCount + 1];
            int i = 0;
            while (ver.Read())
            {
                truc[i] = ver.GetValue(i).ToString();
                i++;
            }
            mysqlok.Close();
            InitializeComponent();
            CodePromo1.Text = truc[0];
            CodePromo2.Text = truc[1];
        }

        private void Entrercode_Click(object sender, RoutedEventArgs e)
        {
            string idCodePromo = recupcode.Text;

            if (idCodePromo != null)
            {
                string id = "";
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
                catch (Exception em)
                {
                    MessageBox.Show(em.Message + "CodePromotion.xaml.cs l.79");
                }

                string idmysql = "SERVER=localhost;PORT=3306;DATABASE=travelpark;UID=root;PASSWORD=Scorpio-Leon123!;";
                MySqlConnection mysqlok = new MySqlConnection(idmysql);

                MySqlCommand cmdver = new MySqlCommand("select idCodePromo from Promotion where idCodePromo ='" + idCodePromo + "'", mysqlok);
                mysqlok.Close();

                mysqlok.Open();
                MySqlDataReader ver = cmdver.ExecuteReader();
                mysqlok.Close();
                if (ver.GetValue(0) != null)
                {
                    MessageBox.Show("Code promotion innexistant..");
                }
                else
                {
                    // refaire la requete de modification du code promo
                    MySqlCommand cmd = new MySqlCommand("update client set idCodePromoC=" + "'" + idCodePromo + "'" + "where idC=" + "'" + id + "'" + " ;", mysqlok);
                    cmd.ExecuteNonQuery();
                    mysqlok.Close();

                    MessageBox.Show("Code promo ajouté avec succès !");
                    this.Visibility = Visibility.Hidden;

                }
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
    }
}
