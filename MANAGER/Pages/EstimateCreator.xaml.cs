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
using System.Windows.Media;

using MANAGER.Classes;
using MANAGER.ComboBox;
using MANAGER.Connection;
using MANAGER.Properties;

using Oracle.ManagedDataAccess.Client;

namespace MANAGER.Pages
{
    /// <summary>
    ///   This page is uses to create cost estimate. you can choose a customer and add some ( or every ) product, then insert
    ///   it in the database.
    /// </summary>
    public partial class EstimatePage
    {
        // An empty list of Merchandise, for future use.
        private static readonly List<Merchandise> ListMerchandise = new List<Merchandise>();
        // A empty second list of Merchandise, for future use.
        private static readonly List<Merchandise> ListMerchandiseN2 = new List<Merchandise>();
        // database connection to oracle. See /connection/ConnectionOracle for more info.
        private readonly OracleConnection dataBaseConnection = ConnectionOracle.OracleDatabase(Settings.Default.DatabaseConnectionString);
        // A Estimate that use the previous list.
        private readonly Estimate estimate = new Estimate(ListMerchandise);
        // The total cost of the devis.
        private int qte;
        // The quantity of the product you want, for future use.
        private double totalCost;

        /// <summary>
        ///   This method checks if "str" is a integer or not and return a boolean.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool IsInt(string str)
        {
            int value;
            return (str.Trim() != string.Empty) && int.TryParse(str, out value);
        }

        /// <summary>
        ///   If there's an error with the cost, this methode is called.
        /// </summary>
        private void ErrorCost()
        {
            LabelPrice.Content = "Erreur";
            Ajouter.IsEnabled = false;
            LabelPrice.Foreground =
                TextBoxEstimateQte.CaretBrush =
                    TextBoxEstimateQte.SelectionBrush = TextBoxEstimateQte.BorderBrush = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
        }

        /// <summary>
        ///   When the quantity changes, this method is called.
        ///   It checks if "TextBoxEstimateQte.Text" is a integer or not and call ErrorCost if not.
        ///   If it is, then it checks if it's more than zero and call ErrorCost if not.
        /// </summary>
        private void QteChanged()
        {
            qte = 0;

            if(IsInt(TextBoxEstimateQte.Text))
            {
                var nouvQte = Convert.ToInt32(TextBoxEstimateQte.Text);

                if(nouvQte <= 0)
                {
                    ErrorCost();
                }
                else
                {
                    if(ComboBoxProduit.Items.Count != 0)
                    {
                        qte = nouvQte;
                        LabelPrice.Foreground = new SolidColorBrush(Color.FromRgb(0xC1, 0xC1, 0xC1));
                        LabelPrice.Content = string.Format("{0}€", (((ComboboxItemMerchandise) ComboBoxProduit.SelectedItem).Value.GetPrix * qte));
                        TextBoxEstimateQte.BorderBrush =
                            TextBoxEstimateQte.CaretBrush =
                                TextBoxEstimateQte.SelectionBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor));
                        Ajouter.IsEnabled = true;
                    }
                    else
                    {
                        ErrorCost();
                    }
                }
            }
            else
            {
                ErrorCost();
            }
        }

        /// <summary>
        ///   Every time this page is loaded, this method is called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EstimateCreator_Loaded(object sender, RoutedEventArgs e)
        {
            var nbMerchandise = estimate.GetList.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                estimate[i].Border.BorderBrush = Ajouter.BorderBrush;
            }
            QteChanged();
        }

        /// <summary>
        ///   The first time the page is open, this method is called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxProduit_OnInitialized(object sender, EventArgs e)
        {
            // Select every merchandise which is on store
            const string query = "SELECT * FROM MARCHANDISE WHERE ENVENTE = 1";
            dataBaseConnection.Open();
            try
            {
                var oCommand = ConnectionOracle.OracleCommand(dataBaseConnection, query);
                var resultat = oCommand.ExecuteReader();
                //For each result :
                while(resultat.Read())
                {
                    //Fill the combobox with every result
                    ComboBoxProduit.Items.Add(new ComboboxItemMerchandise
                    {
                        //Show the text + possibly change his length.
                        Text = resultat[1].ToString(),
                        //For each items, add a merchandise as a value.
                        Value =
                            new Merchandise(Convert.ToInt32(resultat[0]), resultat[1].ToString(), Convert.ToInt32(resultat[3]), Convert.ToInt32(resultat[2]))
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
                dataBaseConnection.Close();
            }
            ComboBoxProduit.SelectedIndex = 0;
        }

        /// <summary>
        ///   Change the qte and the price when the user change his selection in the combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="selectionChangedEventArgs"></param>
        private void ComboBoxProduit_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            try
            {
                //If the merchandise price is zero, it's an error.
                switch(qte)
                {
                    case 0:
                        ErrorCost();
                        break;
                    default:
                        LabelPrice.Content = ((ComboboxItemMerchandise) ComboBoxProduit.SelectedItem).Value.GetPrix * Convert.ToInt32(TextBoxEstimateQte.Text)
                                             + "€";
                        break;
                }
            }
            catch(Exception caught)
            {
                Console.WriteLine(caught.Message);
                Console.Read();
            }
        }

        /// <summary>
        ///   The first time the page is open, this method is called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxClient_OnInitialized(object sender, EventArgs e)
        {
            //Obtain every client on the database 
            const string query = "SELECT ID_CLIENT, EMAIL, DENOMINATION, TELEPHONE FROM CLIENT";
            try
            {
                dataBaseConnection.Open();
                //Execute the query
                var oCommand = ConnectionOracle.OracleCommand(dataBaseConnection, query);
                var resultat = oCommand.ExecuteReader();
                //For each result :
                while(resultat.Read())
                {
                    //Fill the combobox with every result
                    ComboBoxClient.Items.Add(new ComboboxItemClient
                    {
                        //Show the text + possibly change his length.
                        Text = resultat[2].ToString(),
                        //For each items, add a client as a value.
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
                dataBaseConnection.Close();
            }
            ComboBoxClient.SelectedIndex = 0;
        }

        /// <summary>
        ///   Change the client on the devis when the user change his selection in the combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            estimate.Client = ((ComboboxItemClient) ComboBoxClient.SelectedItem).Value;
        }

        /// <summary>
        ///   If the quantity changes, verify if it's an integer > 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxEstimateQte_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                QteChanged();
            }
            catch(Exception caught)
            {
                Console.WriteLine(caught.Message);
                Console.Read();
            }
        }

        /// <summary>
        ///   When the user click on "BTNAddFeed" bouton, the app adding it to the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTNAddFeed_click(object sender, RoutedEventArgs e)
        {
            // get the cost of the merchandise (without the "€")
            var merchandiseCost = Convert.ToInt32(LabelPrice.Content.ToString().Substring(0, LabelPrice.Content.ToString().Length - 1));

            // Check if the merchandise isn't already in the list
            var nbMerchandise = estimate.GetList.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                if(estimate[i].GetNom == ComboBoxProduit.Text)
                {
                    return;
                }
            }
            addMerchandise(((ComboboxItemMerchandise) ComboBoxProduit.SelectedItem).Value.GetId, ComboBoxProduit.Text, qte, merchandiseCost);
            AjouterEstimate.IsEnabled = true;
        }

        /// <summary>
        ///   When the user wants to creat a devis, he clicks on this button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTNAddEstimate_click(object sender, RoutedEventArgs e)
        {
            //Open the connection
            try
            {
                dataBaseConnection.Open();
                // AFAIK, Oracle don't have auto-inc, so with this, you get the last id of what you want.
                const string querySelect = "SELECT max(ID_DEVIS), max(NUMERODEVIS) FROM DEVIS";
                var oCommand = ConnectionOracle.OracleCommand(dataBaseConnection, querySelect);
                var result = oCommand.ExecuteReader();
                //Get the size of the current list.
                var sizeList = ListMerchandise.Count;
                //for each row in result :
                while(result.Read())
                {
                    var idEstimate = result[0].ToString() == "" ? 1 : Convert.ToInt32(result[0]);
                    var numeroEstimate = result[1].ToString() == "" ? 1 : Convert.ToInt32(result[1]);
                    //Get the current date on the Oracle format.
                    //var date = "TO_DATE('" + DateTime.Now.ToString("dd/MM/yy") + "', 'DD/MM/RR')";
                    var date = DateTime.Now.ToString("dd/MM/yy");
                    for(var i = 0; i < sizeList; i++)
                    {
                        var Insert = ConnectionOracle.OracleCommandStored(dataBaseConnection, "INSERTDEVIS");
                        //Change every parameters with the proper value.
                        var paramIdClient = new OracleParameter(":1", OracleDbType.Int32) {Value = estimate.Client.GetId};
                        var paramIdMerchandise = new OracleParameter(":2", OracleDbType.Int32) {Value = estimate[i].GetId};
                        var paramIdEstimate = new OracleParameter(":3", OracleDbType.Int32) {Value = ((idEstimate) + i + 1)};
                        var paramQTE = new OracleParameter(":4", OracleDbType.Int32) {Value = estimate[i].GetQte};
                        var paramDate = new OracleParameter(":5", OracleDbType.Varchar2) {Value = date};
                        var paramPrice = new OracleParameter(":6", OracleDbType.Varchar2) {Value = estimate[i].GetPrix};
                        var paramNumEstimate = new OracleParameter(":7", OracleDbType.Varchar2) {Value = ((numeroEstimate) + 1)};

                        //add every parameters that have changed
                        Insert.Parameters.Add(paramIdClient);
                        Insert.Parameters.Add(paramIdMerchandise);
                        Insert.Parameters.Add(paramIdEstimate);
                        Insert.Parameters.Add(paramQTE);
                        Insert.Parameters.Add(paramDate);
                        Insert.Parameters.Add(paramPrice);
                        Insert.Parameters.Add(paramNumEstimate);

                        //Execute the stored command
                        Insert.ExecuteNonQuery();
                    }
                }
                result.Close();
            }
            catch
            {
                MessageBox.Show("Connexion à la base de donnée impossible.");
                return;
            }
            finally
            {
                dataBaseConnection.Close();
            }

            //Final stuff, reset everything.
            PanelEstimate.Children.Clear();
            ListMerchandise.Clear();
            totalCost = 0;
            LabelTotalPrix.Content = "";
            AjouterEstimate.IsEnabled = false;
        }

        /// <summary>
        ///   When the user changes UserControl's size, this method is called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Change the border size to always match with UserControl.
            BorderEstimate.Width = EstimateCreator.ActualWidth - 340;
            BorderEstimate.Height = EstimateCreator.ActualHeight - 50;

            //Do the same with the the sub-borders.
            var nbMerchandise = estimate.GetList.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                estimate[i].Border.Width = BorderEstimate.Width - 6;
            }
        }

        private void bouton_Click(object sender, EventArgs e)
        {
            totalCost = 0;
            LabelTotalPrix.Content = "";

            var id = ((Button) sender).Tag.ToString();
            var nbMerchandise = estimate.GetList.Count;

            for(var i = 0; i < nbMerchandise; i++)
            {
                if(ListMerchandise[i].ToString() != id)
                {
                    ListMerchandiseN2.Add(ListMerchandise[i]);
                }
            }

            nbMerchandise -= 1;

            PanelEstimate.Children.Clear();
            estimate.GetList.Clear();

            for(var i = 0; i < nbMerchandise; i++)
            {
                addMerchandise(ListMerchandiseN2[i].GetId, ListMerchandiseN2[i].GetNom, ListMerchandiseN2[i].GetQte, ListMerchandiseN2[i].GetPrix);
            }
            ListMerchandiseN2.Clear();
            if(estimate.GetList.Count == 0)
            {
                AjouterEstimate.IsEnabled = false;
            }
        }

        private void addMerchandise(int id, string name, int qte, double price)
        {
            var panelMerchandise = new StackPanel();

            var newMerchandise = new Merchandise(id, name, qte, price);

            var border = new Border
            {
                BorderBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor)),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(2, 2, 1, 0),
                BorderThickness = new Thickness(1),
                Width = BorderEstimate.Width - 6,
                Child = panelMerchandise,
                Height = 70
            };

            //Name
            panelMerchandise.Children.Add(new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 2, 0, 0),
                Text = name,
                Height = 16
            });

            // Price
            panelMerchandise.Children.Add(new TextBlock
            {
                Text = price + "€",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 2, 0, 0),
                Height = 16
            });

            // Quantity
            panelMerchandise.Children.Add(new TextBlock
            {
                Text = qte.ToString(CultureInfo.InvariantCulture),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5, 2, 0, 0),
                Height = 16
            });

            var BTN_Delete = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = "Enlever le produit",
                Margin = new Thickness(9, -30, 67, 50),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00)),
                Tag = newMerchandise
            };

            panelMerchandise.Children.Add(BTN_Delete);
            BTN_Delete.Click += bouton_Click;

            newMerchandise.Border = border;
            PanelEstimate.Children.Add(border);
            estimate.GetList.Add(newMerchandise);
            totalCost += price;
            LabelTotalPrix.Content = totalCost + "€";
        }
    }
}