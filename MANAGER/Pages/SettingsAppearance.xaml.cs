﻿// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

#region

using MANAGER.ViewModels;

#endregion

namespace MANAGER.Pages
{
    /// <summary>
    ///   Logique d'interaction pour SettingsAppearance.xaml
    /// </summary>
    public partial class SettingsAppearance
    {
        public SettingsAppearance()
        {
            InitializeComponent();
            DataContext = new SettingsAppearanceViewModel();
        }
    }
}