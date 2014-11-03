// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

using SLAM3.ViewModels;

namespace SLAM3.Pages
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