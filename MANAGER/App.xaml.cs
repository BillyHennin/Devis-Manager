// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

#region

using System;
using System.Windows;
using System.Windows.Media;

using FirstFloor.ModernUI.Presentation;

using MANAGER.Classes;
using MANAGER.Properties;

#endregion

namespace MANAGER
{
    /// <summary>
    ///   Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppearanceManager.Current.AccentColor = (Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor);
            AppearanceManager.Current.FontSize = Settings.Default.FontSize == "Large" ? FontSize.Large : FontSize.Small;
            AppearanceManager.Current.ThemeSource = new Uri(Settings.Default.Theme, UriKind.Relative);
            xml.getLang();
            //Transharp.SetCurrentLanguage(xml.getLang());
            //var x = MANAGER.Properties.Resources.SettingsData;
        }
    }
}