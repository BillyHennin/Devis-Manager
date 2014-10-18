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
        private readonly Devis devisA = new Devis(listMarchandise);
        private readonly double prix = 1;
        private int id;
        private double prixTotal;

        private void BTNAddFeed_click(object sender, RoutedEventArgs e)
        {
            var intPrix = Convert.ToInt32(LabelPrix.Content.ToString().Substring(0, LabelPrix.Content.ToString().Length - 1));
            var qte = Convert.ToInt32(TextBoxDevisQte.Text);
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
                Text = TextBoxProduit.Text,
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

            var qteProduit = new TextBlock
            {
                Text = qte.ToString(CultureInfo.InvariantCulture),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 1, 0, 0),
                Height = 16
            };

            panelMarchandise.Children.Add(libelle);
            panelMarchandise.Children.Add(prixProduit);
            panelMarchandise.Children.Add(qteProduit);
            panelDevis.Children.Add(bordure);

            devisA.list.Add(new Marchandise(id, TextBoxProduit.Text, qte, intPrix, bordure));

            updatePrix();
            id++;
            LabelTotalPrix.Content = prixTotal + "€";
        }

        private void updatePrix()
        {
            int nbMarchandise = devisA.list.Count;
            for(int i = 0; i < nbMarchandise; i++)
            {
                prixTotal += devisA[i].getPrix;
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderDevis.Width = Menu.ActualWidth - 340;
            BorderDevis.Height = Menu.ActualHeight - 50;

            int nbMarchandise = devisA.list.Count;
            for(int i = 0; i < nbMarchandise; i++)
            {
                devisA[i].getBordure.Width = BorderDevis.Width - 4;
            }
        }

        private void TextBoxDevisQte_TextChanged(object sender, TextChangedEventArgs e)
        {
            prixArticle(TextBoxDevisQte.Text);
        }

        private static bool nEstPasUnNombre(string QTE)
        {
            int value;
            return (QTE.Trim() == string.Empty) || !int.TryParse(QTE, out value);
        }

        private void erreur()
        {
            LabelPrix.Content = "Erreur";
            LabelPrix.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
            Ajouter.IsEnabled = false;
        }

        private void TextBoxDevisQteErreur()
        {
            TextBoxDevisQte.CaretBrush = TextBoxDevisQte.SelectionBrush = TextBoxDevisQte.BorderBrush = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
        }

        private void TextBoxProduitErreur()
        {
            TextBoxProduit.CaretBrush = TextBoxProduit.SelectionBrush = TextBoxProduit.BorderBrush = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
        }

        private void prixArticle(string Text)
        {
            if(nEstPasUnNombre(Text))
            {
                erreur();
                TextBoxDevisQteErreur();
            }
            else
            {
                if(Convert.ToInt32(TextBoxDevisQte.Text) <= 0 || prix == 0)
                {
                    try
                    {
                        erreur();
                        TextBoxDevisQteErreur();
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
                    TextBoxDevisQte.BorderBrush =
                        TextBoxDevisQte.CaretBrush =
                            TextBoxDevisQte.SelectionBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor));
                    Ajouter.IsEnabled = true;
                }
            }
        }

        private void TextBoxProduit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(TextBoxProduit.Text == "Produit")
            {
                try
                {
                    erreur();
                    TextBoxProduitErreur();
                }
                    // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                    //Catching is for the weaks
                }
            }
            else
            {
                TextBoxProduit.BorderBrush =
                    TextBoxProduit.CaretBrush =
                        TextBoxProduit.SelectionBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor));
            }
        }

        private void Menu_Loaded(object sender, RoutedEventArgs e)
        {
            Ajouter.BorderBrush =
                BorderDevis.BorderBrush =
                    TextBoxDevisQte.BorderBrush =
                        TextBoxDevisQte.CaretBrush =
                            TextBoxDevisQte.SelectionBrush =
                                TextBoxProduit.BorderBrush =
                                    TextBoxProduit.CaretBrush =
                                        TextBoxProduit.SelectionBrush =
                                            new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor));
            int nbMarchandise = devisA.list.Count;
            for(int i = 0; i < nbMarchandise; i++)
            {
                devisA[i].getBordure.BorderBrush = Ajouter.BorderBrush;
            }
        }

        // TODO : this.
        private void ComboBoxClient_OnInitialized(object sender, EventArgs e)
        {
            /*
             *
             * TODO : Connexion BDD Oracle
             * 
             */
            //Maybe later
            var item = new ComboboxItem {Text = "Item text1", Value = new Client()};
            comboBoxClient.Items.Add(item);
            comboBoxClient.SelectedIndex = 0;
        }

        private void comboBoxClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*comboBoxClient.SelectedIndex
            devisA.setClient(comboBoxClient.SelectedValue);*/
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public Client Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}