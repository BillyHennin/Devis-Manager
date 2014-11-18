// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;
using System.Xml;

using MANAGER.Properties;

namespace MANAGER
{
    /// <summary>
    ///   Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ModernWindow_Initialized(object sender, EventArgs e)
        {
            var file = Environment.CurrentDirectory + "//Config.xmls";

            /*var file = Environment.CurrentDirectory + "//Database.sdf";
            
            
            if(!File.Exists(file))
            {

            }*/

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            var node = xmlDoc.SelectSingleNode("/params");
            node.Attributes["DatabaseType"].Value = "test";
            xmlDoc.Save(file);

            if(Convert.ToInt32(node.Attributes["IsDatabaseCreated"].Value) == 0)
            {
                //ContentSource = new Uri(@"/Pages/Parametre.xaml", UriKind.Relative);
            }

            //Settings.Default.DatabaseConnectionString = "Data Source=" + Environment.CurrentDirectory + "\\Database.sdf";

            Settings.Default.DatabaseConnectionString = "user id=SLAM3;password=pw;data source=localhost:1521/xe";

            //MessageBox.Show("Le fichier de base de donnée (Database.sdf) est manquant dans la dossier de l'executable", "Erreur");
            //Environment.Exit(0);
        }
    }
}