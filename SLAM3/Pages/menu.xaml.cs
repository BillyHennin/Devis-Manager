// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace SLAM3.Pages
{
    /// <summary>
    ///   Logique d'interaction pour devis.xaml
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public partial class devis
    {
        public devis()
        {
            InitializeComponent();
        }

                        

        private void BTNAddFeed_click(object sender, RoutedEventArgs e)
        {
            LabelPrix.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
            LabelPrix.Content = !nEstPasUnNombre(TextBoxDevisQte.Text) ? string.Format("{0}€", (2 * Convert.ToInt32(TextBoxDevisQte.Text))) : "Erreur quantité";
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderBlack.Width = Menu.ActualWidth > 341 ? Menu.ActualWidth - 340 : 1;

            BorderBlack.Height = Menu.ActualHeight - 60;
        }

        private void TextBoxDevisQte_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBoxDevisQte.BorderBrush = nEstPasUnNombre(TextBoxDevisQte.Text) ? 
                new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00)) : new SolidColorBrush();
        }

        private static bool nEstPasUnNombre(string QTE)
        {
            int value;
            return (QTE.Trim() == "") || !int.TryParse(QTE, out value);
        }
    }
}