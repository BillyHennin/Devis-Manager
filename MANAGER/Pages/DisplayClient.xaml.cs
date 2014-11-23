// This program is a private software, based on c# source code.
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

        private void ComboBoxClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxDevis.Items.Clear();
            PanelDevis.Children.Clear();
            var price = 0;
            var nbEstimate = 1;
            try
            {
                var query = "SELECT DISTINCT NUMERODEVIS FROM DEVIS WHERE ID_CLIENT = :1";
                var oCommand = ConnectionOracle.OracleCommand(database, query);
                var paramIdClient = new OracleParameter(":1", OracleDbType.Int32) { Value = ((ComboboxItemClient)ComboBoxClient.SelectedItem).Value.id };

                oCommand.Parameters.Add(paramIdClient);
                database.Open();

                var resultat = oCommand.ExecuteReader();
                while(resultat.Read())
                {
                    var query2 = "SELECT ID_MARCHANDISE, PRIXMARCHANDISE, QUANTITE, JOUR FROM DEVIS WHERE NUMERODEVIS = :1";
                    var oCommand2 = ConnectionOracle.OracleCommand(database, query2);
                    var paramNumeroDevis = new OracleParameter(":1", OracleDbType.Int32) { Value = resultat[0] };
                    oCommand2.Parameters.Add(paramNumeroDevis);
                    var resultat2 = oCommand2.ExecuteReader();
                    var listMarchandise = new List<Merchandise>();
                    while(resultat2.Read())
                    {
                        var idMerchandise = Convert.ToInt32(resultat2[0]);
                        var priceMerchandise = Convert.ToInt32(resultat2[1]);
                        var quantity = Convert.ToInt32(resultat2[2]);
                        var day = resultat2[3];
                        var merchandise = new Merchandise(idMerchandise, null, quantity, priceMerchandise);

                        var query3 = "SELECT ID_MARCHANDISE, NOM, QUANTITE, PRIX FROM MARCHANDISE WHERE ID_MARCHANDISE = :1";
                        var oCommand3 = ConnectionOracle.OracleCommand(database, query3);
                        var paramIdMarchandise = new OracleParameter(":1", OracleDbType.Int32) { Value = resultat2[0] };
                        oCommand3.Parameters.Add(paramIdMarchandise);
                        var resultat3 = oCommand3.ExecuteReader();
                        while (resultat3.Read())
                        {
                            listMarchandise.Add(new Merchandise(Convert.ToInt32(resultat3[0]), resultat3[1].ToString(), Convert.ToInt32(resultat3[2]),
                                Convert.ToInt32(resultat3[3])));
                            price += priceMerchandise * quantity;
                        }


                        

                        ComboBoxDevis.Items.Add(new ComboboxItemEstimate { Text = "Devis n°" + nbEstimate + " - " + price + "€", Value = estimateClient });
                        ((ComboboxItemClient)ComboBoxClient.SelectedItem).Value.listEstimate.Add(estimateClient);
                        
                    }

                    var estimateClient = new Estimate(listMarchandise) { date = Convert.ToDateTime(day), TotalPrix += priceMerchandise * quantity };

                    nbEstimate++;
                    price = 0;

                    resultat2.Close();
                }
                resultat.Close();
            }
            catch
            {
                MessageBox.Show("Connexion à la base de donnée impossible.");
                return;
            }
            finally
            {
                database.Close();
            }

            BTN_Supprimer.Visibility = Visibility.Visible;
            PanelDevis.Children.Clear();
        }

        private void ComboBoxDevis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PanelDevis.Children.Clear();
            if(ComboBoxDevis.Items.Count == 0)
            {
                return;
            }
            var price = 0;
            var listMarchandise = (ComboBoxDevis.SelectedItem as ComboboxItemEstimate).Value.GetList;
            var taille = listMarchandise.Count;
            for(var i = 0; i < taille; i++)
            {
                var id = listMarchandise[i].id;
                var text = listMarchandise[i].nom;
                var qte = Convert.ToInt32(listMarchandise[i].quantite);
                var prixMarchandise = Convert.ToInt32(listMarchandise[i].prix);
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
                price += prixMarchandise * qte;
            }
            TotalTextBlock.Text = "Total du devis : " + price + "€";
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
            var con = ConnectionOracle.OracleDatabase(Settings.Default.DatabaseConnectionString);
            var commandeModif = ConnectionOracle.OracleCommandStored(con, "DELETECLIENT");
            var ID = (ComboBoxClient.SelectedItem as ComboboxItemClient).Value.id;
            var param1 = new OracleParameter(":1", OracleDbType.Int32) {Value = ID};

            commandeModif.Parameters.Add(param1);

            try
            {
                con.Open();
                commandeModif.ExecuteNonQuery();
            }
            catch(Exception caught)
            {
                Console.WriteLine(caught.Message);
                Console.Read();
            }
            finally
            {
                con.Close();
            }
        }
    }
}