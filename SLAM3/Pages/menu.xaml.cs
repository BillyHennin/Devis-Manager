// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using SLAM3.Classes;
using SLAM3.Properties;

namespace SLAM3.Pages
{
    /// <summary>
    ///   Logique d'interaction pour devis.xaml
    /// </summary>

    public partial class devis
    {
        private static readonly List<Marchandise> listMarchandise = new List<Marchandise>();
        private readonly Devis leDevis = new Devis(listMarchandise);
        private readonly double prix = 1;
        private int id;
        private double prixTotal;
        private int QTE;

        private void BTNAddFeed_click(object sender, RoutedEventArgs e)
        {
            var intPrix = Convert.ToInt32(LabelPrix.Content.ToString().Substring(0, LabelPrix.Content.ToString().Length - 1));
            var panelMarchandise = new StackPanel();

            var bordure = new Border
            {
                Height = 70,
                Width = BorderDevis.Width - 4,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(1, 1, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                BorderBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor)),
                Child = panelMarchandise
            };

            var libelle = new TextBlock
            {
                Text = comboBoxProduit.Text,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 1, 0, 0),
                Height = 16
            };

            var prixProduit = new TextBlock
            {
                Text = intPrix.ToString(CultureInfo.InvariantCulture) + "€",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 1, 0, 0),
                Height = 16
            };

            var QTEProduit = new TextBlock
            {
                Text = QTE.ToString(CultureInfo.InvariantCulture),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 1, 0, 0),
                Height = 16
            };

            panelMarchandise.Children.Add(libelle);
            panelMarchandise.Children.Add(prixProduit);
            panelMarchandise.Children.Add(QTEProduit);
            panelDevis.Children.Add(bordure);

            leDevis.list.Add(new Marchandise(id, comboBoxProduit.Text, QTE, intPrix, bordure));

            updatePrixTotal();
            id++;
            LabelTotalPrix.Content = prixTotal + "€";
        }

        private void updatePrixTotal()
        {
            int nbMarchandise = leDevis.list.Count;
            for(int i = 0; i < nbMarchandise; i++)
            {
                prixTotal += leDevis[i].getPrix;
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderDevis.Width = Menu.ActualWidth - 340;
            BorderDevis.Height = Menu.ActualHeight - 50;

            int nbMarchandise = leDevis.list.Count;
            for(int i = 0; i < nbMarchandise; i++)
            {
                leDevis[i].getBordure.Width = BorderDevis.Width - 4;
            }
        }

        private void TextBoxDevisQte_TextChanged(object sender, TextChangedEventArgs e)
        {
           VerifePrixArticle(TextBoxDevisQte.Text);
        }

        private static bool estUnNombre(string QTE)
        {
            int value;
            return (QTE.Trim() != string.Empty) && int.TryParse(QTE, out value);
        }

        private void erreurPrix()
        {
            LabelPrix.Content = "Erreur";
            LabelPrix.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
            Ajouter.IsEnabled = false;
        }

        private void TextBoxDevisQteerreurPrix()
        {
            TextBoxDevisQte.CaretBrush = 
                TextBoxDevisQte.SelectionBrush = 
                    TextBoxDevisQte.BorderBrush = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
        }

        private void VerifePrixArticle(string Text)
        {
            if(estUnNombre(Text))
            {
                if(Convert.ToInt32(Text) <= 0 || prix == 0)
                {
                    try
                    {
                        erreurPrix();
                        TextBoxDevisQteerreurPrix();
                    }
                        // ReSharper disable once EmptyGeneralCatchClause
                    catch
                    {
                        //This is just like you, you don't get it
                    }
                }
                else
                {
                    QTE = Convert.ToInt32(Text);
                    LabelPrix.Foreground = new SolidColorBrush(Color.FromRgb(0xC1, 0xC1, 0xC1));
                    LabelPrix.Content = string.Format("{0}€", (prix * QTE));
                    TextBoxDevisQte.BorderBrush =
                        TextBoxDevisQte.CaretBrush =
                            TextBoxDevisQte.SelectionBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor));
                    Ajouter.IsEnabled = true;
                }
            }
            else
            {
                erreurPrix();
                TextBoxDevisQteerreurPrix();
            }
        }

        private void Menu_Loaded(object sender, RoutedEventArgs e)
        {

            Ajouter.BorderBrush =
                BorderDevis.BorderBrush =
                    TextBoxDevisQte.BorderBrush =
                        TextBoxDevisQte.CaretBrush =
                            TextBoxDevisQte.SelectionBrush =
                                comboBoxProduit.BorderBrush =
                                    comboBoxClient.BorderBrush =
                                            new SolidColorBrush((Color)ColorConverter.ConvertFromString(Settings.Default.AccentColor));
            int nbMarchandise = leDevis.list.Count;
            for (int i = 0; i < nbMarchandise; i++)
            {
                leDevis[i].getBordure.BorderBrush = Ajouter.BorderBrush;
            }
        }

        private void ComboBoxClient_OnInitialized(object sender, EventArgs e)
        {
            /*
             *
             * TODO : Connexion BDD Oracle
             * 
             */
            for(int i = 0; i < 10; i++)
            {
                comboBoxClient.Items.Add(new ComboboxItemClient {Text = "Item text1 "+ i, Value = new Client()});
            }
            comboBoxClient.SelectedIndex = 0;
        }

        private void comboBoxClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            leDevis.setClient = (comboBoxClient.SelectedItem as ComboboxItemClient).Value;
        }

        private void ComboBoxProduit_OnInitialized(object sender, EventArgs e)
        {
            /*
             *
             * TODO : Connexion BDD Oracle
             * 
             */
            for (int i = 0; i < 10; i++)
            {
                comboBoxProduit.Items.Add(new ComboboxItemProduit { Text = "Item text1 " + i, Value = new Produit(10 + i, "yolo") });
            }
            comboBoxProduit.SelectedIndex = 0;
        }
        
        private void ComboBoxProduit_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            try
            {
                LabelPrix.Content = (comboBoxProduit.SelectedItem as ComboboxItemProduit).Value.getPrix * Convert.ToInt32(TextBoxDevisQte.Text) + "€";
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch { }
        } 
    }
}