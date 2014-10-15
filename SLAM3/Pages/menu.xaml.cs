// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using SLAM3.Properties;

namespace SLAM3.Pages
{
    /// <summary>
    ///   Logique d'interaction pour devis.xaml
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public partial class devis
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private static readonly List<Marchandise> listMarchandise = new List<Marchandise>();
        private readonly Devis nouvDevis = new Devis(listMarchandise);
        private int id;
        private readonly double prix = 1;

        private void BTNAddFeed_click(object sender, RoutedEventArgs e)
        {
            /* 
             * 
             * TODO : Verification des attributs (avec le bouton en isEnabled false/true ?).
             * 
             * TODO : Pouvoir recupere les marchandises au dessus et en dessous (avec la liste).
             * 
             */

            int intPrix = Convert.ToInt32(LabelPrix.Content.ToString().Substring(0, LabelPrix.Content.ToString().Length - 1));
            int qte = Convert.ToInt32(TextBoxDevisQte.Text);
            nouvDevis.getList().Add(new Marchandise(id, TextBoxProduit.Text, qte, intPrix));
            id++;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderBlack.Width = Menu.ActualWidth > 341 ? Menu.ActualWidth - 340 : 1;
            BorderBlack.Height = Menu.ActualHeight - 60;
        }

        private void TextBoxDevisQte_TextChanged(object sender, TextChangedEventArgs e)
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
            TextBoxDevisQte.SelectionBrush = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
            TextBoxDevisQte.CaretBrush = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
            Ajouter.IsEnabled = false;
        }

        private void prixArticle(string Text)
        {
            if(nEstPasUnNombre(Text))
            {
                erreur();
            }
            else
            {
                if(Convert.ToInt32(TextBoxDevisQte.Text) == 0 || prix == 0)
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
                    TextBoxDevisQte.SelectionBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor));
                    TextBoxDevisQte.CaretBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString("White"));
                    Ajouter.IsEnabled = true;
                }
            }
        }

        private void Menu_Loaded(object sender, RoutedEventArgs e)
        {
            id = 0;
            TextBoxProduit.Background = new SolidColorBrush((Color) ColorConverter.ConvertFromString(TextBoxDevisQte.Background.ToString()));
            TextBoxProduit.BorderBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(TextBoxDevisQte.BorderBrush.ToString()));
        }
    }
}