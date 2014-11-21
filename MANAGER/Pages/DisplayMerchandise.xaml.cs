// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using MANAGER.Classes;
using MANAGER.Connection;
using MANAGER.Properties;

using Oracle.ManagedDataAccess.Client;

namespace MANAGER.Pages
{
    /// <summary>
    ///   Logique d'interaction pour DisplayMerchandise.xaml
    /// </summary>
    public partial class DisplayMerchandise
    {
      

        /// <summary>
        ///   A list of all merchandise that are buyable
        /// </summary>
        private static List<Merchandise> ListMerchandise = new List<Merchandise>();

        /// <summary>
        ///   A second list, for future use.
        /// </summary>
        private static readonly List<Merchandise> ListMerchandiseN2 = new List<Merchandise>();

        /// <summary>
        ///   If the use wants to search a specific merchandise.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxDevisQte_TextChanged(object sender, TextChangedEventArgs e)
        {
            PanelProduit.Children.Clear();

            if(TextBoxDevisQte.Text == "")
            {
                var taille = ListMerchandise.Count;

                for(var i = 0; i < taille; i++)
                {
                    var id = ListMerchandise[i].GetId;
                    var text = ListMerchandise[i].GetNom;
                    var qte = "Quantitée en stock : " + ListMerchandise[i].GetQte;
                    var prixMerchandise = ListMerchandise[i].GetPrix + "€";
                    var newMerchandise = new Merchandise(id, text, ListMerchandise[i].GetQte, ListMerchandise[i].GetPrix);
                    var panelMerchandise = new StackPanel();
                    var thick = new Thickness(5, 2, 0, 0);

                    //nouvelle bordure
                    var bordure = new Border
                    {
                        BorderBrush = BorderEstimate.BorderBrush,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(2, 2, 1, 0),
                        BorderThickness = new Thickness(1),
                        Width = BorderEstimate.Width - 5,
                        Child = panelMerchandise,
                        Height = 70
                    };

                    PanelProduit.Children.Add(bordure);

                    // Nom du produit
                    panelMerchandise.Children.Add(new TextBlock {Margin = thick, Text = text, Height = 16});

                    // Prix
                    panelMerchandise.Children.Add(new TextBlock {Text = qte.ToString(CultureInfo.InvariantCulture), Margin = thick, Height = 16});

                    // Quantité
                    panelMerchandise.Children.Add(new TextBlock
                    {
                        Text = prixMerchandise.ToString(CultureInfo.InvariantCulture),
                        Margin = new Thickness(5, 2, 0, 0),
                        Height = 16
                    });

                    newMerchandise.Border = bordure;
                    ListMerchandiseN2.Add(newMerchandise);
                }
                ListMerchandise = ListMerchandiseN2;
            }
            else
            {
                PanelProduit.Children.Clear();
                ListMerchandise.Clear();

                // var db = new SqlCeConnection(Settings.Default.DatabaseConnectionString);
                var db = ConnectionOracle.OracleDatabase(Settings.Default.DatabaseConnectionString);
                var query = "SELECT * FROM MARCHANDISE WHERE NOM LIKE '%" + TextBoxDevisQte.Text + "%'";
                try
                {
                    db.Open();
                    //var oCommand = new SqlCeCommand {Connection = db, CommandText = query};
                    var oCommand = ConnectionOracle.OracleCommand(db, query);
                    var resultat = oCommand.ExecuteReader();
                    while(resultat.Read())
                    {
                        var id = Convert.ToInt32(resultat[0]);
                        var text = resultat[1].ToString();
                        var qte = "Quantitée en stock : " + Convert.ToInt32(resultat[3]);
                        var prixMerchandise = Convert.ToInt32(resultat[2]) + "€";
                        var newMerchandise = new Merchandise(id, text, Convert.ToInt32(resultat[3]), Convert.ToInt32(resultat[2]));
                        var panelMerchandise = new StackPanel();
                        var thick = new Thickness(5, 2, 0, 0);

                        //nouvelle bordure
                        var bordure = new Border
                        {
                            BorderBrush = BorderEstimate.BorderBrush,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Margin = new Thickness(2, 2, 1, 0),
                            BorderThickness = new Thickness(1),
                            Width = BorderEstimate.Width - 5,
                            Child = panelMerchandise,
                            Height = 70
                        };

                        PanelProduit.Children.Add(bordure);

                        // Nom du produit
                        panelMerchandise.Children.Add(new TextBlock {Margin = thick, Text = text, Height = 16});

                        // Prix
                        panelMerchandise.Children.Add(new TextBlock {Text = qte.ToString(CultureInfo.InvariantCulture), Margin = thick, Height = 16});

                        // Quantité
                        panelMerchandise.Children.Add(new TextBlock
                        {
                            Text = prixMerchandise.ToString(CultureInfo.InvariantCulture),
                            Margin = new Thickness(5, 2, 0, 0),
                            Height = 16
                        });

                        // Suppression
                        panelMerchandise.Children.Add(new Button
                        {
                            HorizontalAlignment = HorizontalAlignment.Right,
                            Name = "BTN_Supprimer",
                            Content = "Supprimer le produit",
                            Margin = new Thickness(9, -30, 67, 50),
                            BorderBrush = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00))
                        });

                        newMerchandise.Border = bordure;
                        ListMerchandise.Add(newMerchandise);
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
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PanelProduit.Children.Clear();
            ListMerchandise.Clear();

            // var db = new SqlCeConnection(Settings.Default.DatabaseConnectionString);

            var db = ConnectionOracle.OracleDatabase(Settings.Default.DatabaseConnectionString);
            const string query = "SELECT * FROM MARCHANDISE WHERE ENVENTE = 1";
            try
            {
                db.Open();
                //   var oCommand = new SqlCeCommand {Connection = db, CommandText = query};
                var oCommand = ConnectionOracle.OracleCommand(db, query);
                var resultat = oCommand.ExecuteReader();
                while(resultat.Read())
                {
                    var id = Convert.ToInt32(resultat[0]);
                    var text = resultat[1].ToString();
                    var qte = "Quantitée en stock : " + Convert.ToInt32(resultat[3]);
                    var prixMerchandise = Convert.ToInt32(resultat[2]) + "€";
                    var newMerchandise = new Merchandise(id, text, Convert.ToInt32(resultat[3]), Convert.ToInt32(resultat[2]));
                    var panelMerchandise = new StackPanel();
                    var thick = new Thickness(5, 2, 0, 0);

                    //nouvelle bordure
                    var bordure = new Border
                    {
                        BorderBrush = BorderEstimate.BorderBrush,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(2, 2, 1, 0),
                        BorderThickness = new Thickness(1),
                        Width = BorderEstimate.Width - 5,
                        Child = panelMerchandise,
                        Height = 70
                    };

                    PanelProduit.Children.Add(bordure);

                    // Nom du produit
                    panelMerchandise.Children.Add(new TextBlock {Margin = thick, Text = text, Height = 16});

                    // Prix
                    panelMerchandise.Children.Add(new TextBlock {Text = qte.ToString(CultureInfo.InvariantCulture), Margin = thick, Height = 16});

                    // Quantité
                    panelMerchandise.Children.Add(new TextBlock
                    {
                        Text = prixMerchandise.ToString(CultureInfo.InvariantCulture),
                        Margin = new Thickness(5, 2, 0, 0),
                        Height = 16
                    });

                    var BTN_Supprimer = new Button
                    {
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Name = "BTN_Supprimer",
                        Content = "Supprimer le client",
                        Margin = new Thickness(9, -30, 67, 50),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00)),
                        Tag = id
                    };

                    // Suppression
                    panelMerchandise.Children.Add(BTN_Supprimer);

                    BTN_Supprimer.Click += bouton_Click;

                    newMerchandise.Border = bordure;
                    ListMerchandise.Add(newMerchandise);
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
        }

        /*
        var newMerchandise = new Merchandise(0, "Aucune marchandise n'est en vente actuellement.", 0, 0);
                var panelMerchandise = new StackPanel();
                var bordure = new Border
                {
                    BorderBrush = BorderEstimate.BorderBrush,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(2, 2, 1, 0),
                    BorderThickness = new Thickness(1),
                    Width = BorderEstimate.Width - 5,
                    Child = panelMerchandise,
                    Height = 70
                };

                PanelProduit.Children.Add(bordure);

                // Nom du produit
                panelMerchandise.Children.Add(new TextBlock { Margin = new Thickness(5, 2, 0, 0), Text = "Aucune ùarchandise n'est en vente actuellement.", Height = 16 });
                newMerchandise.Border = bordure;
                ListMerchandise.Add(newMerchandise);
        */

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderEstimate.Width = MenuMerchandise.ActualWidth - 40;
            BorderEstimate.Height = MenuMerchandise.ActualHeight - 70;

            var nbMerchandise = ListMerchandise.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                ListMerchandise[i].Border.Width = BorderEstimate.Width - 5;
            }
        }

        private void bouton_Click(object sender, EventArgs e)
        {
            var con = ConnectionOracle.OracleDatabase(Settings.Default.DatabaseConnectionString);
            var commandeModif = new OracleCommand {CommandType = CommandType.StoredProcedure, Connection = con, CommandText = "DELETEPRODUIT"};

            var param1 = new OracleParameter(":1", OracleDbType.Int32) {Value = ((Button) sender).Tag.ToString()};
            commandeModif.Parameters.Add(param1);

            try
            {
                con.Open();
                commandeModif.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {                
                con.Close();
            }
            Environment.Exit(255);
        }
    }
}