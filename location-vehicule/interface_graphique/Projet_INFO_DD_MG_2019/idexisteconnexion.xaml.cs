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

namespace Projet_INFO_DD_MG_2019
{
    /// <summary>
    /// Logique d'interaction pour idexisteconnexion.xaml
    /// </summary>
    public partial class idexisteconnexion : Window
    {
        public idexisteconnexion()
        {
            InitializeComponent();
        }

        private void Choisirautreid_Click(object sender, RoutedEventArgs e)
        {
            Inscription changer = new Inscription();
            this.Visibility = Visibility.Hidden;
            changer.Show();
        }

        private void Retourconnexion_Click(object sender, RoutedEventArgs e)
        {

            Connexion changer = new Connexion();
            this.Visibility = Visibility.Hidden;
            changer.Show();
        }
    }
}
