// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

using System;
using System.IO;
using System.Windows;
using SLAM3.Properties;


namespace SLAM3
{
    /// <summary>
    ///     Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ModernWindow_Initialized(object sender, EventArgs e)
        {

            var file = Environment.CurrentDirectory + "//Database.sdf";
            if (File.Exists(file))
            {
                Settings.Default.DatabaseConnectionString = "Data Source=" + file;
            }
            else
            {
                MessageBox.Show("Le fichier de base de donnée (Database.sdf) est manquant dans la dossier de l'executable","Erreur");
                Application.Current.Shutdown();
            }
        }
    }
}