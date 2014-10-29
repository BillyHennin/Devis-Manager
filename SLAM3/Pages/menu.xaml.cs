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
    ///     Logique d'interaction pour devis.xaml
    /// </summary>
// ReSharper disable once InconsistentNaming
    public partial class devis
    {
        private static readonly List<Marchandise> ListMarchandise = new List<Marchandise>();
        private readonly Devis _leDevis = new Devis(ListMarchandise);
        private int _qte;
        private double _prixTotal;

        private static bool EstUnNombre(string qte)
        {
            int value;
            return (qte.Trim() != string.Empty) && int.TryParse(qte, out value);
        }

        private void ErreurPrix()
        {
            LabelPrix.Content = "Erreur";
            Ajouter.IsEnabled = false;
            LabelPrix.Foreground =
                TextBoxDevisQte.CaretBrush =
                    TextBoxDevisQte.SelectionBrush =
                        TextBoxDevisQte.BorderBrush = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
        }

        private void BTNAddFeed_click(object sender, RoutedEventArgs e)
        {
            var prixMarchandise =
                Convert.ToInt32(LabelPrix.Content.ToString().Substring(0, LabelPrix.Content.ToString().Length - 1));
            var panelMarchandise = new StackPanel();
            var nouvelleMarchadise = new Marchandise(comboBoxProduit.Text, _qte, prixMarchandise);

            var nbMarchandise = _leDevis.getList.Count;
            for (var i = 0; i < nbMarchandise; i++)
            {
                if (_leDevis[i].getNom == nouvelleMarchadise.getNom)
                {
                    return;
                }
            }

            var bordure = new Border
            {
                BorderBrush =
                    new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor)),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(2, 2, 1, 0),
                BorderThickness = new Thickness(1),
                Width = BorderDevis.Width - 6,
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
                Text = _qte.ToString(CultureInfo.InvariantCulture),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 2, 0, 0),
                Height = 16
            });

            nouvelleMarchadise.Bordure = bordure;
            panelDevis.Children.Add(bordure);
            _leDevis.getList.Add(nouvelleMarchadise);
            _prixTotal += prixMarchandise;
            LabelTotalPrix.Content = _prixTotal + "€";
            AjouterDevis.IsEnabled = true;
        }

        private void BTNAddDevis_click(object sender, RoutedEventArgs e)
        {
            // TODO : Demander a l'user si il veux vraiment add le devis + Ajout bdd

            panelDevis.Children.Clear();
            ListMarchandise.Clear();
            _prixTotal = 0;
            LabelTotalPrix.Content = "";
            AjouterDevis.IsEnabled = false;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderDevis.Width = Menu.ActualWidth - 340;
            BorderDevis.Height = Menu.ActualHeight - 50;

            var nbMarchandise = _leDevis.getList.Count;
            for (var i = 0; i < nbMarchandise; i++)
            {
                _leDevis[i].Bordure.Width = BorderDevis.Width - 6;
            }
        }

        private void TextBoxDevisQte_TextChanged(object sender, TextChangedEventArgs e)
        {
            _qte = 0;

            if (EstUnNombre(TextBoxDevisQte.Text))
            {
                var nouvQte = Convert.ToInt32(TextBoxDevisQte.Text);

                if (nouvQte <= 0)
                {
                    try
                    {
                        ErreurPrix();
                    }
                        // ReSharper disable once EmptyGeneralCatchClause
                    catch
                    {
                        //This is just like you, you don't get it
                    }
                }
                else
                {
                    _qte = nouvQte;
                    LabelPrix.Foreground = new SolidColorBrush(Color.FromRgb(0xC1, 0xC1, 0xC1));
                    LabelPrix.Content = string.Format("{0}€",
                        ((comboBoxProduit.SelectedItem as ComboboxItemProduit).Value.GetPrix*_qte));
                    TextBoxDevisQte.BorderBrush =
                        TextBoxDevisQte.CaretBrush =
                            TextBoxDevisQte.SelectionBrush =
                                new SolidColorBrush(
                                    (Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor));
                    Ajouter.IsEnabled = true;
                }
            }
            else
            {
                ErreurPrix();
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
                                        comboBoxClient.BorderBrush =
                                            new SolidColorBrush(
                                                (Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor));
            var nbMarchandise = _leDevis.getList.Count;
            for (var i = 0; i < nbMarchandise; i++)
            {
                _leDevis[i].Bordure.BorderBrush = Ajouter.BorderBrush;
            }
        }

        private void ComboBoxClient_OnInitialized(object sender, EventArgs e)
        {
            /*
             *
             * TODO : Connexion BDD Oracle
             * 
             */
            for (var i = 0; i < 10; i++)
            {
                var text = "Client " + i;
                comboBoxClient.Items.Add(new ComboboxItemClient
                {
                    Text = text,
                    Value = new Client(text, "02", "mes@couilles")
                });
            }
            comboBoxClient.SelectedIndex = 0;
        }

        private void comboBoxClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _leDevis.client = (comboBoxClient.SelectedItem as ComboboxItemClient).Value;
        }

        private void ComboBoxProduit_OnInitialized(object sender, EventArgs e)
        {
            /*
             *
             * TODO : Connexion BDD Oracle
             * 
             */
            for (var i = 0; i < 10; i++)
            {
                var text = "Produit " + i;
                comboBoxProduit.Items.Add(new ComboboxItemProduit
                {
                    Text = text,
                    Value = new Produit(10 + i, text)
                });
            }
            comboBoxProduit.SelectedIndex = 0;
        }

        private void ComboBoxProduit_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            try
            {
                switch (_qte)
                {
                    case 0:
                        ErreurPrix();
                        break;
                    default:
                        LabelPrix.Content = (comboBoxProduit.SelectedItem as ComboboxItemProduit).Value.GetPrix*
                                            Convert.ToInt32(TextBoxDevisQte.Text) + "€";
                        break;
                }
            }
                // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }
    }
}