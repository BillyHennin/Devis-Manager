// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

#region

using System;
using System.Windows;

#endregion

namespace MANAGER
{
    /// <summary>
    ///     Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private void ModernWindow_Initialized(object sender, EventArgs e)
        {
            //Initialisation de la bdd
            Connection.Connection.Default.DatabaseConnectionString = "user id=SLAM3;password=pw;data source=localhost:1521/xe";

            if (Connection.Connection.Default.IsDatabaseSet) return;
            ContentSource = new Uri(@"/Pages/Parametre.xaml", UriKind.Relative);
            MessageBox.Show("La base de données utilisée pour l'application est introuvable, veuillez en choisir une", "Erreur");
        }
    }
}