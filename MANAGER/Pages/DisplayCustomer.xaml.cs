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
using System.Windows.Media;

using MANAGER.Classes;
using MANAGER.ComboBox;
using MANAGER.Connection;

using Oracle.ManagedDataAccess.Client;

namespace MANAGER.Pages
{
    /// <summary>
    ///   Logique d'interaction pour DisplayCustomer.xaml
    /// </summary>
    public partial class DisplayCustomer
    {
        private static readonly List<Merchandise> ListMerchandise = new List<Merchandise>();
        private readonly Estimate estimate = new Estimate(ListMerchandise);

        private void ComboBoxCustomer_Loaded(object sender, EventArgs e)
        {
            //Traduction
            DC_ChooseCustomer.Text = Transharp.GetTranslation("DC_ChooseCustomer");
            LabelEstimate.Text = Transharp.GetTranslation("DC_CustomerCE");
            LabelCommand.Text = Transharp.GetTranslation("DC_CustomerCM");
            LabelPhone.Text = Transharp.GetTranslation("DC_LabelPhone");
            LabelMail.Text = Transharp.GetTranslation("DC_labelMail");
            BTN_Delete.Content = Transharp.GetTranslation("DC_DeleteCustomer");

            //Default Visibility 
            PanelClientEstimate.Visibility = Visibility.Hidden;
            BTN_Delete.Visibility = Visibility.Hidden;
            PanelDevis.Children.Clear();
            BorderDevis.Visibility = Visibility.Hidden;
            LabelPhone.Visibility = Visibility.Hidden;
            LabelMail.Visibility = Visibility.Hidden;
            TextPhone.Visibility = Visibility.Hidden;
            TextMail.Visibility = Visibility.Hidden;
            try
            {
                ComboBoxCustomer.Items.Clear();
                InitComboClient();
            }
            catch(Exception caught)
            {
                Console.WriteLine(caught.Message);
                //potatoe
            }
        }

        private void ComboBoxCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxEstimate.Items.Clear();
            PanelDevis.Children.Clear();

            var totalPrice = 0;
            var nbEstimate = 1;
            var date = DateTime.Now;
            try
            {
                var Command = ConnectionOracle.OracleCommand("SELECT DISTINCT NUMERODEVIS FROM DEVIS WHERE ID_CLIENT = :ID_CLIENT ORDER BY NUMERODEVIS");
                Command.Parameters.Add(new OracleParameter(":ID_CLIENT", OracleDbType.Int32)
                {
                    Value = ((ComboboxItemCustomer) ComboBoxCustomer.SelectedItem).Value.id
                });
                var resultCommand = Command.ExecuteReader();

                while(resultCommand.Read())
                {
                    var Command2 =
                        ConnectionOracle.OracleCommand(
                            "SELECT MARCHANDISE.ID_MARCHANDISE, MARCHANDISE.NOM, DEVIS.PRIXMARCHANDISE, DEVIS.QUANTITE, DEVIS.JOUR, MARCHANDISE.ID_CATEGORIE "
                            + "FROM MARCHANDISE, DEVIS WHERE DEVIS.ID_MARCHANDISE=MARCHANDISE.ID_MARCHANDISE AND DEVIS.NUMERODEVIS= :NUMERODEVIS");
                    Command2.Parameters.Add(new OracleParameter(":NUMERODEVIS", OracleDbType.Int32) {Value = resultCommand[0]});
                    var resultatMerchandise = Command2.ExecuteReader();
                    var ListMerchandise2 = new List<Merchandise>();
                    while(resultatMerchandise.Read())
                    {
                        totalPrice += Convert.ToInt32(resultatMerchandise[2]);
                        date = Convert.ToDateTime(resultatMerchandise[4]);
                        var merchandise = new Merchandise(Convert.ToInt32(resultatMerchandise[0]), resultatMerchandise[1].ToString(),
                            Convert.ToInt32(resultatMerchandise[3]), Convert.ToInt32(resultatMerchandise[2]) / Convert.ToInt32(resultatMerchandise[3]),
                            Convert.ToInt32(resultatMerchandise[5]));
                        ListMerchandise2.Add(merchandise);
                    }
                    resultatMerchandise.Close();
                    var estimate2 = new Estimate(ListMerchandise2) {TotalPrice = totalPrice, date = date};
                    ComboBoxEstimate.Items.Add(new ComboboxItemEstimate
                    {
                        Text = Transharp.GetTranslation("DC_ComboBoxCustomer", nbEstimate, date.ToShortDateString(), totalPrice),
                        Value = estimate2
                    });
                    nbEstimate++;
                    totalPrice = 0;
                }
                resultCommand.Close();
                TextMail.Text = ((ComboboxItemCustomer) ComboBoxCustomer.SelectedItem).Value.email;
                TextPhone.Text = ((ComboboxItemCustomer) ComboBoxCustomer.SelectedItem).Value.cellphone;
            }
            catch(Exception caught)
            {
                Console.WriteLine(caught.Message);
                //This show up when leaving the page, need to figure out why.

                //MessageBox.Show(Transharp.GetTranslation("Box_DBFail"), Transharp.GetTranslation("Box_Error"), MessageBoxButton.OK, MessageBoxImage.Error);
                //MessageBox.Show("YIH");
            }
            finally
            {
                PanelClientEstimate.Visibility = Visibility.Visible;

                LabelPhone.Visibility = Visibility.Visible;
                LabelMail.Visibility = Visibility.Visible;

                TextPhone.Visibility = Visibility.Visible;
                TextMail.Visibility = Visibility.Visible;
                BTN_Delete.Visibility = Visibility.Visible;
                PanelDevis.Children.Clear();
            }
        }

        private void InitComboClient()
        {
            const string query = "SELECT ID_CLIENT, EMAIL, DENOMINATION, TELEPHONE FROM CLIENT";
            try
            {
                var oCommand = ConnectionOracle.OracleCommand(query);
                var resultat = oCommand.ExecuteReader();
                while(resultat.Read())
                {
                    ComboBoxCustomer.Items.Add(new ComboboxItemCustomer
                    {
                        Text = resultat[2].ToString(),
                        Value = new Customer(Convert.ToInt32(resultat[0]), resultat[2].ToString(), resultat[3].ToString(), resultat[1].ToString())
                    });
                }
                resultat.Close();
            }
            catch
            {
                MessageBox.Show(Transharp.GetTranslation("Box_DBFail"), Transharp.GetTranslation("Box_Error"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ComboBoxEstimate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PanelDevis.Children.Clear();
            BorderDevis.Visibility = Visibility.Visible;
            if(ComboBoxEstimate.Items.Count == 0)
            {
                return;
            }

            var listMarchandise = ((ComboboxItemEstimate) ComboBoxEstimate.SelectedItem).Value.GetList;
            var taille = listMarchandise.Count;
            for(var i = 0; i < taille; i++)
            {
                var categoryString = string.Empty;
                var CommandCategory = ConnectionOracle.OracleCommand("SELECT LIBELLE FROM CATEGORIE WHERE ID_CATEGORIE=:ID_CATEGORIE");
                CommandCategory.Parameters.Add(new OracleParameter(":ID_CATEGORIE", OracleDbType.Int32) {Value = Convert.ToInt32(listMarchandise[i].categoryID)});
                var resultatCategory = CommandCategory.ExecuteReader();
                while(resultatCategory.Read())
                {
                    categoryString = resultatCategory[0].ToString();
                }
                var id = listMarchandise[i].id;
                var text = String.Format("{0} - {1}", categoryString, listMarchandise[i].name);
                var qte = listMarchandise[i].quantity;
                var prixMarchandise = listMarchandise[i].price;
                var category = listMarchandise[i].categoryID;
                var item = new Merchandise(id, text, qte, prixMarchandise, category);
                var panelMarchandise = new StackPanel();
                var thick = new Thickness(5, 2, 0, 0);

                // New border
                var bordure = new Border
                {
                    BorderBrush = ComboBoxCustomer.BorderBrush,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(2, 2, 1, 0),
                    BorderThickness = new Thickness(1),
                    Width = BorderDevis.Width - 5,
                    Child = panelMarchandise,
                    Height = 70
                };

                // Merchandise's name
                panelMarchandise.Children.Add(new TextBlock {Margin = thick, Text = text, Height = 16});

                // Quantity
                panelMarchandise.Children.Add(new TextBlock
                {
                    Text = Transharp.GetTranslation("DC_Command", qte.ToString(CultureInfo.InvariantCulture)),
                    Margin = thick,
                    Height = 16
                });

                // Price
                panelMarchandise.Children.Add(new TextBlock
                {
                    Text = string.Format("{0}€", prixMarchandise.ToString(CultureInfo.InvariantCulture)),
                    Margin = thick,
                    Height = 16
                });

                item.Border = bordure;
                PanelDevis.Children.Add(bordure);
                estimate.GetList.Add(item);
            }

            //colorChange(Brushes.LimeGreen);

            //TotalTextBlock.Text = string.Format("{0} : {1}€", Transharp.GetTranslation("All_Total"),
            //((ComboboxItemEstimate) ComboBoxEstimate.SelectedItem).Value.TotalPrice);
        }

/*
        private void colorChange(Brush newColour)
        {
            BorderDevis.BorderBrush = newColour;
            var nbMarchandise = estimate.GetList.Count;
            for(var i = 0; i < nbMarchandise; i++)
            {
                ListMerchandise[i].Border.BorderBrush = newColour;
            }
        }
*/

        private void MenuCustomer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderDevis.Width = MenuCustomer.ActualWidth - 400;
            BorderDevis.Height = MenuCustomer.ActualHeight - 100;
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
                //On initialisation it don't works so here's a try/catch.
                //Need to figure out how to bypass it since it's not useful.
                Console.WriteLine(caught.Message);
                Console.Read();
            }
        }

        private void MenuCustomer_Loaded(object sender, RoutedEventArgs e)
        {
            var nbMerchandise = estimate.GetList.Count;

            if(nbMerchandise == 0)
            {
                return;
            }

            for(var i = 0; i < nbMerchandise; i++)
            {
                estimate[i].Border.BorderBrush = BorderDevis.BorderBrush;
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var commandeModif = ConnectionOracle.OracleCommandStored("DELETECLIENT");
                var CustomerId = new OracleParameter(":1", OracleDbType.Int32) {Value = ((ComboboxItemCustomer) ComboBoxCustomer.SelectedItem).Value.id};
                commandeModif.Parameters.Add(CustomerId);
                commandeModif.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show(Transharp.GetTranslation("Box_DBFail"), Transharp.GetTranslation("Box_Error"), MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ComboBoxCustomer.Items.Clear();
            InitComboClient();
        }

        private void ComboBoxCommands_SelectionChanged(object sender, SelectionChangedEventArgs e) {}
    }
}