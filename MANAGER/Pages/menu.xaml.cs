// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;
using System.Collections.Generic;
//using System.Data.SqlServerCe;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using MANAGER.Classes;
using MANAGER.Connection;
using MANAGER.Properties;

namespace MANAGER.Pages
{
    /// <summary>
    ///   Logique d'interaction pour devis.xaml
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public partial class devis
    {
        private static readonly List<Marchandise> ListMarchandise = new List<Marchandise>();
        private readonly Devis _leDevis = new Devis(ListMarchandise);
        private double _prixTotal;
        private int _qte;

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
                TextBoxDevisQte.CaretBrush = TextBoxDevisQte.SelectionBrush = TextBoxDevisQte.BorderBrush = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
        }

        private void QteChanged()
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
                    if (ComboBoxProduit.Items.Count != 0)
                    {
                        try
                        {
                            _qte = nouvQte;
                            LabelPrix.Foreground = new SolidColorBrush(Color.FromRgb(0xC1, 0xC1, 0xC1));
                            LabelPrix.Content = string.Format("{0}€", ((ComboBoxProduit.SelectedItem as ComboboxItemMarchandise).Value.GetPrix * _qte));
                            TextBoxDevisQte.BorderBrush =
                                TextBoxDevisQte.CaretBrush =
                                    TextBoxDevisQte.SelectionBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Settings.Default.AccentColor));
                            Ajouter.IsEnabled = true;
                        }
                        // ReSharper disable once EmptyGeneralCatchClause
                        catch{}
                    }
                    else
                    {
                        try
                        {
                            ErreurPrix();
                        }
                        // ReSharper disable once EmptyGeneralCatchClause
                        catch{}
                    }
                }
            }
            else
            {
                ErreurPrix();
            }
        }

        private void BTNAddFeed_click(object sender, RoutedEventArgs e)
        {
            var prixMarchandise = Convert.ToInt32(LabelPrix.Content.ToString().Substring(0, LabelPrix.Content.ToString().Length - 1));
            var panelMarchandise = new StackPanel();
            var nouvelleMarchandise = new Marchandise((ComboBoxProduit.SelectedItem as ComboboxItemMarchandise).Value.GetId, ComboBoxProduit.Text, _qte, prixMarchandise);

            var nbMarchandise = _leDevis.GetList.Count;
            for(var i = 0; i < nbMarchandise; i++)
            {
                if(_leDevis[i].GetNom == nouvelleMarchandise.GetNom)
                {
                    return;
                }
            }

            var bordure = new Border
            {
                BorderBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor)),
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

            nouvelleMarchandise.Bordure = bordure;
            PanelDevis.Children.Add(bordure);
            _leDevis.GetList.Add(nouvelleMarchandise);
            _prixTotal += prixMarchandise;
            LabelTotalPrix.Content = _prixTotal + "€";
            AjouterDevis.IsEnabled = true;
        }

        private void BTNAddDevis_click(object sender, RoutedEventArgs e)
        {
            var db = ConnectionOracle.OracleDatabase(Settings.Default.DatabaseConnectionString);
            db.Open();
            var tailleList = ListMarchandise.Count;
            try
            {
                const string query = "SELECT max(ID_DEVIS), max(NUMERODEVIS) FROM DEVIS";
                var oCommand = ConnectionOracle.OracleCommand(db, query);
                var resultat = oCommand.ExecuteReader();
                while(resultat.Read())
                {
                    var idDevis = resultat[0].ToString() == "" ? 1 : Convert.ToInt32(resultat[0]);
                    var numeroDevis = resultat[1].ToString() == "" ? 1 : Convert.ToInt32(resultat[1]);
                    for(var i = 0; i < tailleList; i++)
                    {
                        var query2 = "INSERT INTO DEVIS(ID_CLIENT, ID_MARCHANDISE, ID_DEVIS, QUANTITE, JOUR, PRIXMARCHANDISE, NUMERODEVIS)"
                            + "VALUES (" + _leDevis.Client.GetId + "," + _leDevis[i].GetId + "," 
                            + ((idDevis) + i + 1) + ","+ _leDevis[i].GetQte 
                            + ",'" + DateTime.Now.ToString("d/M/yyyy") + "'," + _leDevis[i].GetPrix 
                            + ",'" + ((numeroDevis) + 1) + "');";
                        MessageBox.Show(query2);
                        var oCommand2 = ConnectionOracle.OracleCommand(db, query2);
                        oCommand2.ExecuteNonQuery();
                    }
                }
                resultat.Close();
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch{}
            finally
            {
                db.Close();
            }

            //Garder ça
            PanelDevis.Children.Clear();
            ListMarchandise.Clear();
            _prixTotal = 0;
            LabelTotalPrix.Content = "";
            AjouterDevis.IsEnabled = false;
        }

        private void TextBoxDevisQte_TextChanged(object sender, TextChangedEventArgs e)
        {
            QteChanged();
        }

        private void ComboBoxClient_OnInitialized(object sender, EventArgs e)
        {
            /*
             *
             * TODO : Connexion BDD Oracle
             * 
             */
            //var db = new SqlCeConnection(Settings.Default.DatabaseConnectionString);
            var db = ConnectionOracle.OracleDatabase(Settings.Default.DatabaseConnectionString);
            const string query = "SELECT * FROM CLIENT";
            db.Open();
            try
            {
                //var oCommand = new SqlCeCommand {Connection = db, CommandText = query};
                var oCommand = ConnectionOracle.OracleCommand(db,query);
                var resultat = oCommand.ExecuteReader();
                while(resultat.Read())
                {
                    ComboBoxClient.Items.Add(new ComboboxItemClient
                    {
                        Text = resultat[2].ToString(),
                        Value = new Client(Convert.ToInt32(resultat[0]), resultat[2].ToString(), resultat[1].ToString(), resultat[3].ToString())
                    });
                }
                resultat.Close();
            }
            catch(Exception caught)
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
            // var db = new SqlCeConnection(Settings.Default.DatabaseConnectionString);
           
            var db = ConnectionOracle.OracleDatabase(Settings.Default.DatabaseConnectionString);
            const string query = "SELECT * FROM MARCHANDISE WHERE ENVENTE = 1";
            db.Open();
            try
            {
               // var oCommand = new SqlCeCommand {Connection = db, CommandText = query};
                var oCommand = ConnectionOracle.OracleCommand(db, query);
                var resultat = oCommand.ExecuteReader();
                while(resultat.Read())
                {
                    ComboBoxProduit.Items.Add(new ComboboxItemMarchandise
                    {
                        Text = resultat[1].ToString(),
                        Value = new Marchandise(Convert.ToInt32(resultat[0]), resultat[1].ToString(), Convert.ToInt32(resultat[3]), Convert.ToInt32(resultat[2]))
                    });
                }
                resultat.Close();
            }
            catch(Exception caught)
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
                switch(_qte)
                {
                    case 0:
                        ErreurPrix();
                        break;
                    default:
                        LabelPrix.Content = (ComboBoxProduit.SelectedItem as ComboboxItemMarchandise).Value.GetPrix * Convert.ToInt32(TextBoxDevisQte.Text) + "€";
                        break;
                }
            }
                // ReSharper disable once EmptyGeneralCatchClause
            catch {}
        }

        private void Menu_Loaded(object sender, RoutedEventArgs e)
        {
            var nbMarchandise = _leDevis.GetList.Count;
            for(var i = 0; i < nbMarchandise; i++)
            {
                _leDevis[i].Bordure.BorderBrush = Ajouter.BorderBrush;
            }
            QteChanged();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderDevis.Width = Menu.ActualWidth - 340;
            BorderDevis.Height = Menu.ActualHeight - 50;

            var nbMarchandise = _leDevis.GetList.Count;
            for(var i = 0; i < nbMarchandise; i++)
            {
                _leDevis[i].Bordure.Width = BorderDevis.Width - 6;
            }
        }
    }
}