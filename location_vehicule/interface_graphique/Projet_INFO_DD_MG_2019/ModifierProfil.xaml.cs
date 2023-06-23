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
    /// Logique d'interaction pour ModifierProfil.xaml
    /// </summary>
    public partial class ModifierProfil : Window
    {
        string id;
        string numTel;
        string permis1;
        string permis2;
        string cPromo;
        public ModifierProfil(string id, string numTel, string permis1, string permis2, string cPromo)
        {
            InitializeComponent();
            this.id = id;
            this.numTel = numTel;
            this.permis1 = permis1;
            this.permis2 = permis2;
            this.cPromo = cPromo;

            numTelC.Text = numTel;
            permisC1.Text = permis1;
            permisC2.Text = permis2;
            cPromoC.Text = cPromo;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Verficationmodifiermdp modifMdp = new Verficationmodifiermdp();
            modifMdp.Show();
        }

        private void Actualiser(object sender, RoutedEventArgs e)
        {
            string[,] mat = new string[10, 7];
            if (numTelC.Text != numTel || permisC1.Text != permis1 || permisC2.Text != permis2 || cPromoC.Text != cPromo)
            {
                string requetsql = "SERVER=localhost;PORT=3306;DATABASE=TravelPark;UID=root;PASSWORD=Scorpio-Leon123!;";
                MySqlConnection connec = new MySqlConnection(requetsql);
                MySqlCommand comma = new MySqlCommand("select * from client where idC = '" + id + "';", connec);
                connec.Open();

                MySqlDataReader reader = comma.ExecuteReader(); while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string valeurid = reader.GetValue(i).ToString();
                        if (valeurid != null)
                        {
                            mat[0, i] = valeurid;
                        }
                    }
                }
                connec.Close();
                connec.Open();
                MySqlCommand comma2 = new MySqlCommand("select * from vehicule where idC = '" + id + "';", connec);

                MySqlDataReader reader2 = comma2.ExecuteReader();
                while (reader2.Read())
                {
                    for (int i = 0; i < reader2.FieldCount; i++)
                    {
                        string valeurid = reader2.GetValue(i).ToString();
                        if (valeurid != null)
                        {
                            mat[1, i] = valeurid;
                        }
                    }
                }

                connec.Close();
                connec.Open();
                MySqlCommand comma3 = new MySqlCommand("  select * from se_garer where idC = '" + id + "';", connec);
                MySqlDataReader reader3 = comma3.ExecuteReader();
                while (reader3.Read())
                {
                    for (int i = 0; i < reader3.FieldCount; i++)
                    {
                        string valeurid = reader3.GetValue(i).ToString();
                        if (valeurid != null)
                        {
                            mat[2, i] = valeurid;
                        }
                    }
                }
                connec.Close();
                try
                {
                    connec.Open();
                    MySqlCommand cmd2 = new MySqlCommand("delete from se_garer where idC = '" + mat[0,0] + "'; delete from vehicule where idC = '" + mat[0,0] + "'; delete from client where idC = '" + mat[0,0] + "';", connec);
                    cmd2.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "ModifierProfil.xaml.cs l.116");
                }
                string inter = "";
                string messageDErreur = "";

                if (numTelC.Text != numTel)
                {
                    if(numTelC.Text == "")
                    {
                        mat[0, 3] = "null";
                    }
                    else
                    {
                        if(numTelC.Text.Length <= 10)
                        {
                            inter = mat[0, 3];
                            mat[0, 3] = "'" + numTelC.Text + "'";
                        }
                        else
                        {
                            messageDErreur = "Numéro de téléphone incorrect,\n";
                            mat[0, 3] = "'" + mat[0, 3] + "'";
                        }
                    }
                }
                else
                {
                    mat[0, 3] = "'" + mat[0, 3] + "'";
                }

                if (permisC1.Text != permis1)
                {
                    if (permisC1.Text == "")
                    {
                        mat[0, 4] = "null";
                    }
                    else
                    {
                        if (permisC1.Text.Length <= 10)
                        {
                            mat[0, 4] = "'" + permisC1.Text + "'";
                        }
                        else
                        {
                            messageDErreur += "Valeur Permis 1 trop grande,\n";
                            mat[0, 4] = "'" + mat[0, 4] + "'";
                        }
                    }
                }
                else
                {
                    mat[0, 4] = "'" + mat[0, 4] + "'";
                }

                if (permisC2.Text != permis2)
                {
                    if (permisC2.Text == "")
                    {
                        mat[0, 5] = "null";
                    }
                    else
                    {
                        if (permisC2.Text.Length <= 10)
                        {
                            mat[0, 5] = "'" + permisC2.Text + "'";
                        }
                        else
                        {
                            messageDErreur += "Valeur Permis 2 trop grande,\n";
                            mat[0, 5] = "'" + mat[0,5] + "'";
                        }
                    }
                }
                else
                {
                    mat[0, 5] = "'" + mat[0, 5] + "'";
                }
                if (cPromoC.Text != cPromo)
                {
                    if (cPromoC.Text == "")
                    {
                        mat[0, 6] = "'aaaaa'";
                    }
                    else
                    {
                        if (cPromoC.Text.Length <= 5)
                        {
                            mat[0, 6] = "'" + cPromoC.Text + "'";
                        }
                        else
                        {
                            messageDErreur += "Valeur du code Promo trop grande,\n";
                            mat[0, 6] = "'" + mat[0, 6] + "'";
                        }
                    }
                }
                else
                {
                    mat[0, 6] = "'" + mat[0, 6] + "'";
                }
                connec.Close();

                if(messageDErreur != "")
                {
                    MessageBox.Show(messageDErreur);
                }
                connec.Open();
                MySqlCommand cmd3 = new MySqlCommand("insert into `TravelPark`.`client` (`idC`, `nomprenomC`, `mdpC`, `numtelC`, `permisC1`, `permisC2`, `idCodePromoC`) values ( " + "'" + mat[0,0] + "'" + "," + "'" + mat[0,1] + "'" + "," + "'" + mat[0,2] + "'" + "," + mat[0,3] + "," + mat[0,4] + "," + mat[0,5] + "," + mat[0,6] + ");", connec);
                cmd3.ExecuteNonQuery();
                connec.Close();

                connec.Open();
                MySqlCommand cmd4 = new MySqlCommand("insert into `TravelPark`.`vehicule` (`imatV`, `typeDeV`, `idC`) values ( '"+mat[1,0]+"' , '"+mat[1,1]+"' , '"+mat[1,2]+"');", connec);
                cmd4.ExecuteNonQuery();
                connec.Close();

                connec.Open();
                MySqlCommand cmd5 = new MySqlCommand("insert into `TravelPark`.`se_garer` (`idP`, `idC`, `prixStationnement`) values ( '" + mat[2, 0] + "','" + mat[2, 1] + "','" + mat[2, 2] + "');", connec);
                cmd5.ExecuteNonQuery();
                connec.Close();

                MessageBox.Show("Vous avez modifié votre profil avec succès");
                profil pro = new profil();
                Visibility = Visibility.Hidden;
                pro.Show();
            }
            else
            {
                MessageBox.Show("Vous n'avez pas modifié votre profil car aucun attribut n'a été modifié");
                profil pro = new profil();
                Visibility = Visibility.Hidden;
                pro.Show();
            }
        }
        

        private void ModifierMdp(object sender, RoutedEventArgs e)
        {
            Verficationmodifiermdp modifMdp = new Verficationmodifiermdp();
            modifMdp.Show();
        }
    }
}
