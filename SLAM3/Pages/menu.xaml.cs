// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
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
            var nouvelleMarchadise = new Marchandise(ComboBoxProduit.Text, _qte, prixMarchandise);

            var nbMarchandise = _leDevis.GetList.Count;
            for (var i = 0; i < nbMarchandise; i++)
            {
                if (_leDevis[i].GetNom == nouvelleMarchadise.GetNom)
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
                Text = ComboBoxProduit.Text,
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
            PanelDevis.Children.Add(bordure);
            _leDevis.GetList.Add(nouvelleMarchadise);
            _prixTotal += prixMarchandise;
            LabelTotalPrix.Content = _prixTotal + "€";
            AjouterDevis.IsEnabled = true;
        }

        private void BTNAddDevis_click(object sender, RoutedEventArgs e)
        {
            // TODO : Demander a l'user si il veux vraiment add le devis
            var db = new SqlCeConnection(Settings.Default.DatabaseConnectionString);
            db.Open();
            var tailleList = ListMarchandise.Count;
            try{
                for (var i = 0; i < tailleList; i++)
                {
                    var query = "INSERT INTO DEVIS (Client, Marchandise, Quantite, Date)" +
                                "VALUES (" + _leDevis.Client.GetDenomination +
                                "," + _leDevis[i].GetNom + 
                                "," + _leDevis[i].GetPrix +
                                "," + DateTime.Now.ToString("M/d/yyyy h:mm") + ")";
                    var oCommand = new SqlCeCommand { Connection = db, CommandText = query };
                    oCommand.ExecuteNonQuery();
                }
            }catch (SqlCeException caught){
                Console.WriteLine(caught.Message);
                Console.Read();
            }finally{
                        db.Close();
            }



            //Garder ça
            PanelDevis.Children.Clear();
            ListMarchandise.Clear();
            _prixTotal = 0;
            LabelTotalPrix.Content = "";
            AjouterDevis.IsEnabled = false;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderDevis.Width = Menu.ActualWidth - 340;
            BorderDevis.Height = Menu.ActualHeight - 50;

            var nbMarchandise = _leDevis.GetList.Count;
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
                        ((ComboBoxProduit.SelectedItem as ComboboxItemProduit).Value.GetPrix*_qte));
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
                                    ComboBoxProduit.BorderBrush =
                                        ComboBoxClient.BorderBrush =
                                            new SolidColorBrush(
                                                (Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor));
            var nbMarchandise = _leDevis.GetList.Count;
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
            var db = new SqlCeConnection(Settings.Default.DatabaseConnectionString);
            const string query = "SELECT * FROM CLIENT";
            db.Open();
            try
            {
                var oCommand = new SqlCeCommand { Connection = db, CommandText = query };
                var resultat = oCommand.ExecuteReader();
                while (resultat.Read())
                {
                    ComboBoxClient.Items.Add(new ComboboxItemClient
                    {
                        Text = query[0].ToString(CultureInfo.InvariantCulture),
                        Value = new Client(query[0].ToString(CultureInfo.InvariantCulture),
                                                query[1].ToString(CultureInfo.InvariantCulture),
                                                    query[2].ToString(CultureInfo.InvariantCulture))
                    });
                    ComboBoxClient.SelectedIndex = 0;
                }
                resultat.Close();
            }
            catch (Exception caught)
            {
                Console.WriteLine(caught.Message);
                Console.Read();
            }
            finally
            {
                db.Close();
            }
            ComboBoxClient.SelectedIndex = 0;
        }

        private void comboBoxClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _leDevis.Client = (ComboBoxClient.SelectedItem as ComboboxItemClient).Value;
        }

        private void ComboBoxProduit_OnInitialized(object sender, EventArgs e)
        {
            /*
             *
             * TODO : Connexion BDD Oracle
             * 
             */
            var db = new SqlCeConnection(Settings.Default.DatabaseConnectionString);
            const string query = "SELECT * FROM MARCHANDISE";
            db.Open();
            try
            {
                var oCommand = new SqlCeCommand { Connection = db, CommandText = query };
                var resultat = oCommand.ExecuteReader();
                while (resultat.Read())
                {
                    ComboBoxProduit.Items.Add(new ComboboxItemProduit
                    {
                        Text = query[0].ToString(CultureInfo.InvariantCulture),
                        Value = new Produit(Convert.ToInt32(query[0]),
                                                query[1].ToString(CultureInfo.InvariantCulture))
                    });
                }
                resultat.Close();
            }
            catch (Exception caught)
            {
                Console.WriteLine(caught.Message);
                Console.Read();
            }
            finally
            {
                db.Close();
            }
            ComboBoxProduit.SelectedIndex = 0;
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
                        LabelPrix.Content = (ComboBoxProduit.SelectedItem as ComboboxItemProduit).Value.GetPrix*
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