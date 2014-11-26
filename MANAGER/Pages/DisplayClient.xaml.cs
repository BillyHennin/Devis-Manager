﻿// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

using MANAGER.Classes;
using MANAGER.ComboBox;
using MANAGER.Connection;
using MANAGER.Properties;

using Oracle.ManagedDataAccess.Client;

namespace MANAGER.Pages
{
    /// <summary>
    ///   Logique d'interaction pour DisplayClient.xaml
    /// </summary>
    public partial class DisplayClient
    {
        /// <summary>
        ///   A list of every merchandise contains in the cost estimate,
        ///   changes every time the user change a value from a combobox (ComboBoxClient or ComboBoxDevis).
        /// </summary>
        private static readonly List<Merchandise> ListMerchandise = new List<Merchandise>();

        /// <summary>
        /// Database connection, here for future use.
        /// </summary>
        private readonly OracleConnection database = ConnectionOracle.OracleDatabase(Settings.Default.DatabaseConnectionString);
        /// <summary>
        ///   An object "estimate".
        ///   changes every time the user change a value from a combobox (ComboBoxClient or ComboBoxDevis).
        /// </summary>
        private readonly Estimate estimate = new Estimate(ListMerchandise);

        /// <summary>
        ///   When the user loaded the page, this methode is called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxClient_OnInitialized(object sender, EventArgs e)
        {
            initComboClient();
        }

        private void ComboBoxClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxEstimate.Items.Clear();
            PanelDevis.Children.Clear();

            var totalPrice = 0;
            var nbEstimate = 1;
            var date = DateTime.Now;
            try
            {
                var OracleCommand = ConnectionOracle.OracleCommand(database, "SELECT DISTINCT NUMERODEVIS FROM DEVIS WHERE ID_CLIENT = :1");
                var paramIdClient = new OracleParameter(":1", OracleDbType.Int32) { Value = ((ComboboxItemClient)ComboBoxClient.SelectedItem).Value.id };

                OracleCommand.Parameters.Add(paramIdClient);
                database.Open();

                var resultatNumeroDevis = OracleCommand.ExecuteReader();
                while(resultatNumeroDevis.Read())
                {
                    var OracleCommand2 = ConnectionOracle.OracleCommand(database,"SELECT ID_MARCHANDISE, PRIXMARCHANDISE, QUANTITE, JOUR FROM DEVIS WHERE NUMERODEVIS = :1");
                    var paramNumeroDevis = new OracleParameter(":1", OracleDbType.Int32) { Value = resultatNumeroDevis[0] };
                    OracleCommand2.Parameters.Add(paramNumeroDevis);
                    var resultatMerchandise = OracleCommand2.ExecuteReader();
                    var ListMerchandise2 = new List<Merchandise>();
                    while (resultatMerchandise.Read())
                    {
                        totalPrice += Convert.ToInt32(resultatMerchandise[1]);
                        date = Convert.ToDateTime(resultatMerchandise[3]);

                        var merchandise = new Merchandise(Convert.ToInt32(resultatMerchandise[0]), null, Convert.ToInt32(resultatMerchandise[2]), Convert.ToInt32(resultatMerchandise[1]) / Convert.ToInt32(resultatMerchandise[2]));

                        var oCommand3 = ConnectionOracle.OracleCommand(database, "SELECT NOM, QUANTITE FROM MARCHANDISE WHERE ID_MARCHANDISE = :1");
                        var paramIdMarchandise = new OracleParameter(":1", OracleDbType.Int32) { Value = resultatMerchandise[0] };

                        oCommand3.Parameters.Add(paramIdMarchandise);
                        var resultat3 = oCommand3.ExecuteReader();
                        while (resultat3.Read())
                        {
                            merchandise.nom = resultat3[0].ToString();
                        }
                        resultat3.Close();
                        ListMerchandise2.Add(merchandise);
                    }
                    resultatMerchandise.Close();
                    var estimate2 = new Estimate(ListMerchandise2) { TotalPrix = totalPrice, date = date};
                    ComboBoxEstimate.Items.Add(new ComboboxItemEstimate { Text = "Devis n°" + nbEstimate + " - " + totalPrice + "€", Value = estimate2 });
                    nbEstimate++;
                    totalPrice = 0;
                }
                resultatNumeroDevis.Close();
            }
            catch (Exception caught)
            {
                Console.WriteLine(caught.Message);
                Console.Read();
            }
            finally
            {
                database.Close();
            }

            BTN_Supprimer.Visibility = Visibility.Visible;
            PanelDevis.Children.Clear();
        }

        private void initComboClient()
        {
            const string query = "SELECT ID_CLIENT, EMAIL, DENOMINATION, TELEPHONE FROM CLIENT";
            try
            {
                database.Open();
                var oCommand = ConnectionOracle.OracleCommand(database, query);
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
                database.Close();
            }
        }

        private void ComboBoxEstimate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PanelDevis.Children.Clear();
            if (ComboBoxEstimate.Items.Count == 0)
            {
                return;
            }
            var listMarchandise = ((ComboboxItemEstimate)ComboBoxEstimate.SelectedItem).Value.GetList;
            var taille = listMarchandise.Count;
            for(var i = 0; i < taille; i++)
            {
                var id = listMarchandise[i].id;
                var text = listMarchandise[i].nom;
                var qte = listMarchandise[i].quantite;
                var prixMarchandise = listMarchandise[i].prix;
                var item = new Merchandise(id, text, qte, prixMarchandise);
                var panelMarchandise = new StackPanel();
                var thick = new Thickness(5, 2, 0, 0);

                var bordure = new Border
                {
                    BorderBrush = ComboBoxClient.BorderBrush,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(2, 2, 1, 0),
                    BorderThickness = new Thickness(1),
                    Width = BorderDevis.Width - 5,
                    Child = panelMarchandise,
                    Height = 70
                };

                // Nom du produit
                panelMarchandise.Children.Add(new TextBlock {Margin = thick, Text = text, Height = 16});

                // Prix
                panelMarchandise.Children.Add(new TextBlock
                {
                    Text = "Quantité commandée : " + qte.ToString(CultureInfo.InvariantCulture),
                    Margin = thick,
                    Height = 16
                });

                // Quantité
                panelMarchandise.Children.Add(new TextBlock {Text = prixMarchandise.ToString(CultureInfo.InvariantCulture) + "€", Margin = thick, Height = 16});

                item.Border = bordure;
                PanelDevis.Children.Add(bordure);
                estimate.GetList.Add(item);
            }
            TotalTextBlock.Text = "Total du devis : " + ((ComboboxItemEstimate)ComboBoxEstimate.SelectedItem).Value.TotalPrix + "€";
        }

        private void MenuClient_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderDevis.Width = MenuClient.ActualWidth - 40;
            BorderDevis.Height = MenuClient.ActualHeight - 100;
            try
            {
                var nbMarchandise = estimate.GetList.Count;
                for(var i = 0; i < nbMarchandise; i++)
                {
                    ListMerchandise[i].Border.Width = BorderDevis.Width - 5;
                }
            }
            catch(Exception caught)
            {
                Console.WriteLine(caught.Message);
                Console.Read();
            }
        }

        private void MenuClient_Loaded(object sender, RoutedEventArgs e)
        {
            var nbMarchandise = estimate.GetList.Count;

            if(nbMarchandise == 0)
            {
                return;
            }

            for(var i = 0; i < nbMarchandise; i++)
            {
                estimate[i].Border.BorderBrush = BorderDevis.BorderBrush;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var commandeModif = ConnectionOracle.OracleCommandStored(database, "DELETECLIENT");
                var ID = ((ComboboxItemClient)ComboBoxClient.SelectedItem).Value.id;
                var param1 = new OracleParameter(":1", OracleDbType.Int32) { Value = ID };
                commandeModif.Parameters.Add(param1);
                database.Open();
                commandeModif.ExecuteNonQuery();
            }
            catch(Exception caught)
            {
                Console.WriteLine(caught.Message);
                Console.Read();
            }
            finally
            {
                database.Close();
            }

            ComboBoxClient.Items.Clear();
            initComboClient();
        }
    }
}