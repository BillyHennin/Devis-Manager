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
            /*var file = Environment.CurrentDirectory + "//Database.sdf";
            //var file = Environment.CurrentDirectory + "//Config.xmls";
            
            if(!File.Exists(file))
            {
                File.Create(file);
                //ini.IniWriteValue("Database", "Connection", Environment.CurrentDirectory + "\\Database.sdf");

                ContentSource = new Uri(@"/Pages/Parametre.xaml", UriKind.Relative);
            }*/

            var reader = new System.Xml.XmlTextReader(Environment.CurrentDirectory + "\\Config.xml");
            var contents = "";
            while (reader.Read())
            {
                reader.MoveToContent();
                if (reader.NodeType == System.Xml.XmlNodeType.Element)
                    contents += "<" + reader.Name + ">\n";
                if (reader.NodeType == System.Xml.XmlNodeType.Text)
                    contents += reader.Value + "\n";
            }
            MessageBox.Show(contents);
            //MessageBox.Show(ini.IniReadValue("Database", "Connection"));
            //Settings.Default.DatabaseConnectionString = "Data Source=" + ini.IniReadValue("Database","Connection");
            Settings.Default.DatabaseConnectionString = "Data Source=" + Environment.CurrentDirectory + "\\Database.sdf";

            //MessageBox.Show("Le fichier de base de donnée (Database.sdf) est manquant dans la dossier de l'executable", "Erreur");
            //Environment.Exit(0);



        }
    }
}