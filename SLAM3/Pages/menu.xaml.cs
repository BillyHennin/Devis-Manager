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
        // TODO : Ajout d'un devis à un client via btn

        private static readonly List<Marchandise> listMarchandise = new List<Marchandise>();
        private readonly Devis leDevis = new Devis(listMarchandise);
        private double prixTotal;
        private int QTE;

        private static bool estUnNombre(string QTE)
        {
            int value;
            return (QTE.Trim() != string.Empty) && int.TryParse(QTE, out value);
        }

        private void erreurPrix()
        {
            LabelPrix.Content = "Erreur";
            Ajouter.IsEnabled = false;
            LabelPrix.Foreground =
                TextBoxDevisQte.CaretBrush = TextBoxDevisQte.SelectionBrush = TextBoxDevisQte.BorderBrush = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
        }

        private void BTNAddFeed_click(object sender, RoutedEventArgs e)
        {
            var prixMarchandise = Convert.ToInt32(LabelPrix.Content.ToString().Substring(0, LabelPrix.Content.ToString().Length - 1));
            var panelMarchandise = new StackPanel();
            var nouvelleMarchadise = new Marchandise(comboBoxProduit.Text, QTE, prixMarchandise);

            var nbMarchandise = leDevis.getList.Count;
            for(var i = 0; i < nbMarchandise; i++)
            {
                if(leDevis[i].getNom == nouvelleMarchadise.getNom)
                {
                    return;
                }
            }

            var bordure = new Border
            {
                BorderBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor)),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(1, 1, 0, 0),
                BorderThickness = new Thickness(1),
                Width = BorderDevis.Width - 4,
                Child = panelMarchandise,
                Height = 70
            };

            // Nom du produit
            panelMarchandise.Children.Add(new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 2, 0, 0),
                Text = comboBoxProduit.Text,
                Height = 16
            });

            // Prix
            panelMarchandise.Children.Add(new TextBlock
            {
                Text = prixMarchandise.ToString(CultureInfo.InvariantCulture) + "€",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 2, 0, 0),
                Height = 16
            });

            // Quantité
            panelMarchandise.Children.Add(new TextBlock
            {
                Text = QTE.ToString(CultureInfo.InvariantCulture),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 2, 0, 0),
                Height = 16
            });

            nouvelleMarchadise.Bordure = bordure;
            panelDevis.Children.Add(bordure);
            leDevis.getList.Add(nouvelleMarchadise);
            prixTotal += prixMarchandise;
            LabelTotalPrix.Content = prixTotal + "€";
            AjouterDevis.IsEnabled = true;
        }

        private void BTNAddDevis_click(object sender, RoutedEventArgs e)
        {
            // TODO : Demander a l'user si il veux vraiment add le devis + Ajout bdd
            leDevis.client.listDevis.Add(leDevis);
            panelDevis.Children.Clear();
            listMarchandise.Clear();
            prixTotal = 0;
            LabelTotalPrix.Content = "";
            AjouterDevis.IsEnabled = false;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderDevis.Width = Menu.ActualWidth - 340;
            BorderDevis.Height = Menu.ActualHeight - 50;

            var nbMarchandise = leDevis.getList.Count;
            for(var i = 0; i < nbMarchandise; i++)
            {
                leDevis[i].Bordure.Width = BorderDevis.Width - 4;
            }
        }

        private void TextBoxDevisQte_TextChanged(object sender, TextChangedEventArgs e)
        {
            QTE = 0;

            if(estUnNombre(TextBoxDevisQte.Text))
            {
                var nouvQTE = Convert.ToInt32(TextBoxDevisQte.Text);

                if(nouvQTE <= 0)
                {
                    try
                    {
                        erreurPrix();
                    }
                        // ReSharper disable once EmptyGeneralCatchClause
                    catch
                    {
                        //This is just like you, you don't get it
                    }
                }
                else
                {
                    QTE = nouvQTE;
                    LabelPrix.Foreground = new SolidColorBrush(Color.FromRgb(0xC1, 0xC1, 0xC1));
                    LabelPrix.Content = string.Format("{0}€", ((comboBoxProduit.SelectedItem as ComboboxItemProduit).Value.getPrix * QTE));
                    TextBoxDevisQte.BorderBrush =
                        TextBoxDevisQte.CaretBrush =
                            TextBoxDevisQte.SelectionBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor));
                    Ajouter.IsEnabled = true;
                }
            }
            else
            {
                erreurPrix();
            }
        }

        private void Menu_Loaded(object sender, RoutedEventArgs e)
        {
            Ajouter.BorderBrush =
                AjouterDevis.BorderBrush =
                    BorderDevis.BorderBrush =
                        TextBoxDevisQte.BorderBrush =
                            TextBoxDevisQte.CaretBrush =
                                TextBoxDevisQte.SelectionBrush =
                                    comboBoxProduit.BorderBrush =
                                        comboBoxClient.BorderBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor));
            var nbMarchandise = leDevis.getList.Count;
            for(var i = 0; i < nbMarchandise; i++)
            {
                leDevis[i].Bordure.BorderBrush = Ajouter.BorderBrush;
            }
        }

        private void ComboBoxClient_OnInitialized(object sender, EventArgs e)
        {
            /*
             *
             * TODO : Connexion BDD Oracle
             * 
             */
            var listDevis = new List<Devis>();
            for(var i = 0; i < 10; i++)
            {
                comboBoxClient.Items.Add(new ComboboxItemClient { Text = "Item text1 " + i, Value = new Client("Item text1 " + i, "02", "mes@couiuklles", listDevis) });
            }
            comboBoxClient.SelectedIndex = 0;
        }

        private void comboBoxClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            leDevis.client = (comboBoxClient.SelectedItem as ComboboxItemClient).Value;
        }

        private void ComboBoxProduit_OnInitialized(object sender, EventArgs e)
        {
            /*
             *
             * TODO : Connexion BDD Oracle
             * 
             */
            for(var i = 0; i < 10; i++)
            {
                comboBoxProduit.Items.Add(new ComboboxItemProduit {Text = "Item text1 " + i, Value = new Produit(10 + i, "yolo")});
            }
            comboBoxProduit.SelectedIndex = 0;
        }

        private void ComboBoxProduit_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            try
            {
                switch(QTE)
                {
                    case 0:
                        erreurPrix();
                        break;
                    default:
                        LabelPrix.Content = (comboBoxProduit.SelectedItem as ComboboxItemProduit).Value.getPrix * Convert.ToInt32(TextBoxDevisQte.Text) + "€";
                        break;
                }
            }
                // ReSharper disable once EmptyGeneralCatchClause
            catch {}
        }
    }
}