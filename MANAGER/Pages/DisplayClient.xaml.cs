// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

#region using

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

using MANAGER.Classes;
using MANAGER.ComboBox;
using MANAGER.Connection;

using Oracle.ManagedDataAccess.Client;

#endregion

namespace MANAGER.Pages
{
    public partial class DisplayClient
    {
        private static readonly List<Merchandise> ListMerchandise = new List<Merchandise>();
        private readonly Estimate estimate = new Estimate(ListMerchandise);

        private void ComboBoxClient_OnInitialized(object sender, EventArgs e)
        {
            InitComboClient();
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
                var Command = ConnectionOracle.OracleCommand("SELECT DISTINCT NUMERODEVIS FROM DEVIS WHERE ID_CLIENT = :ID_CLIENT ORDER BY NUMERODEVIS");
                Command.Parameters.Add(new OracleParameter(":ID_CLIENT", OracleDbType.Int32)
                {
                    Value = ((ComboboxItemClient) ComboBoxClient.SelectedItem).Value.id
                });
                var resultCommand = Command.ExecuteReader();

                while(resultCommand.Read())
                {
                    var Command2 =
                        ConnectionOracle.OracleCommand("SELECT MARCHANDISE.ID_MARCHANDISE, MARCHANDISE.NOM, DEVIS.PRIXMARCHANDISE, DEVIS.QUANTITE, DEVIS.JOUR, MARCHANDISE.ID_CATEGORIE "
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
                    var estimate2 = new Estimate(ListMerchandise2) {TotalPrix = totalPrice, date = date};
                    ComboBoxEstimate.Items.Add(new ComboboxItemEstimate
                    {
                        Text = string.Format(Localisation.Localisation.DC_ComboBoxCustomer, nbEstimate, date.ToShortDateString(), totalPrice),
                        Value = estimate2
                    });
                    nbEstimate++;
                    totalPrice = 0;
                }
                resultCommand.Close();
            }
            catch
            {
                MessageBox.Show(Localisation.Localisation.Box_DBFail, Localisation.Localisation.Box_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            PanelClientEstimate.Visibility = Visibility.Visible;
            BTN_Supprimer.Visibility = Visibility.Visible;
            PanelDevis.Children.Clear();
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
                    ComboBoxClient.Items.Add(new ComboboxItemClient
                    {
                        Text = resultat[2].ToString(),
                        Value = new Client(Convert.ToInt32(resultat[0]), resultat[2].ToString(), resultat[1].ToString(), resultat[3].ToString())
                    });
                }
                resultat.Close();
            }
            catch
            {
                MessageBox.Show(Localisation.Localisation.Box_DBFail, Localisation.Localisation.Box_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ComboBoxEstimate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PanelDevis.Children.Clear();

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
                CommandCategory.Parameters.Add(new OracleParameter(":ID_CATEGORIE", OracleDbType.Int32)
                {
                    Value = Convert.ToInt32(listMarchandise[i].categoryID)
                });
                var resultatCategory = CommandCategory.ExecuteReader();
                while (resultatCategory.Read())
                {
                    categoryString = resultatCategory[0].ToString();
                }
                var id = listMarchandise[i].id;
                var text = String.Format("{0} - {1}", categoryString, listMarchandise[i].nom);
                var qte = listMarchandise[i].quantite;
                var prixMarchandise = listMarchandise[i].price;
                var category = listMarchandise[i].categoryID;
                var item = new Merchandise(id, text, qte, prixMarchandise, category);
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

                // Quantité
                panelMarchandise.Children.Add(new TextBlock
                {
                    Text = string.Format(Localisation.Localisation.DC_Command, qte.ToString(CultureInfo.InvariantCulture)),
                    Margin = thick,
                    Height = 16
                });

                // Prix
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

            TotalTextBlock.Text = string.Format("Total : {0}€", ((ComboboxItemEstimate) ComboBoxEstimate.SelectedItem).Value.TotalPrix);
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
                //On initialisation it don't works so here's a try/catch.
                //Need to figure out how to bypass it since it's not useful.
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
                var commandeModif = ConnectionOracle.OracleCommandStored("DELETECLIENT");
                var CustomerId = new OracleParameter(":1", OracleDbType.Int32) {Value = ((ComboboxItemClient) ComboBoxClient.SelectedItem).Value.id};
                commandeModif.Parameters.Add(CustomerId);
                commandeModif.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show(Localisation.Localisation.Box_DBFail, Localisation.Localisation.Box_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ComboBoxClient.Items.Clear();
            InitComboClient();
        }
    }
}