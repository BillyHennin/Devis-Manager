// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

using System;
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
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private double prix = 0;

        public devis()
        {
            InitializeComponent();
        }
        
        private void BTNAddFeed_click(object sender, RoutedEventArgs e)
        {
            
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderBlack.Width = Menu.ActualWidth > 341 ? Menu.ActualWidth - 340 : 1;
            
            BorderBlack.Height = Menu.ActualHeight - 60;
        }

        private void TextBoxDevisQte_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            prixArticle(TextBoxDevisQte.Text);
        }

        private static bool nEstPasUnNombre(string QTE)
        {
            int value;
            return (QTE.Trim() == "") || !int.TryParse(QTE, out value);
        }

        private void erreur()
        {
            LabelPrix.Content = "Erreur";
            LabelPrix.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
            TextBoxDevisQte.BorderBrush = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
        }

        private void prixArticle(string Text)
        {
            if(nEstPasUnNombre(Text))
            {
                erreur();
            } 
            else
            {
                if(Convert.ToInt32(TextBoxDevisQte.Text)==0)
                {
                    try
                    {
                        erreur();
                    }
                    // ReSharper disable once EmptyGeneralCatchClause
                    catch
                    {
                        //This is just like you, you don't get it
                    }
                }
                else
                {
                    LabelPrix.Foreground = new SolidColorBrush(Color.FromRgb(0xC1, 0xC1, 0xC1));
                    LabelPrix.Content = string.Format("{0}€", (prix * Convert.ToInt32(TextBoxDevisQte.Text)));
                    TextBoxDevisQte.BorderBrush = new SolidColorBrush();
                }
            }
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}