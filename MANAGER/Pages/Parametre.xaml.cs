// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using IniParser;

namespace MANAGER.Pages
{
    /// <summary>
    ///   Logique d'interaction pour Parametre.xaml
    /// </summary>
    public partial class Parametre
    {
        public Parametre()
        {
            InitializeComponent();
        }

        private void ComboBoxLang_Loaded(object sender, RoutedEventArgs e)
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory()+"\\Language", "*.lang");
            foreach(var file in files)
            {
                ComboBoxLang.Items.Add(file.Split('.')[0].Split('\\').Last());
            }
        }

        private void ComboBoxLang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TODO : Recuperer les valeurs .ini
            //TODO : Changer les strings
            var parser = new FileIniDataParser();
            var data = parser.ReadFile(Directory.GetCurrentDirectory() + "\\Language\\"+ComboBoxLang.SelectedItem+".lang");
            MessageBox.Show(data["Localisation"]["PM_SelectLanguage"]);
            MessageBox.Show(Localisation.Localisation.PM_SelectLanguage);

            MessageBox.Show("Not today, maybe later");
        }
    }
}