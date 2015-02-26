// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

#region

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

#endregion

namespace MANAGER.Pages
{
    public partial class EstimatePage
    {
        private static readonly List<Merchandise> ListMerchandise = new List<Merchandise>();
        private static readonly List<Merchandise> ListMerchandiseN2 = new List<Merchandise>();
        private readonly Estimate estimate = new Estimate(ListMerchandise);
        private double TotalCost;
        private int qte;

        private void EstimateCreator_Loaded(object sender, RoutedEventArgs e)
        {
            var nbMerchandise = estimate.GetList.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                estimate[i].Border.BorderBrush = Ajouter.BorderBrush;
            }
            QteChanged();
        }

        private void ComboBoxProduct_OnInitialized(object sender, EventArgs e)
        {
            const string query = "SELECT * FROM MARCHANDISE WHERE ENVENTE = 1";
            try
            {
                var oCommand = ConnectionOracle.OracleCommand(query);
                var resultat = oCommand.ExecuteReader();
                while(resultat.Read())
                {
                    ComboBoxProduct.Items.Add(new ComboboxItemMerchandise
                    {
                        Text = resultat[1].ToString(),
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
            ComboBoxProduct.SelectedIndex = 0;
        }

        private void ComboBoxProduct_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            try
            {
                switch(qte)
                {
                    case 0:
                        ErrorCost();
                        break;
                    default:
                        LabelPrice.Content = ((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.prix * Convert.ToInt32(TextBoxEstimateQte.Text)
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

        private void ComboBoxClient_OnInitialized(object sender, EventArgs e)
        {
            const string query = "SELECT ID_CLIENT, EMAIL, DENOMINATION, TELEPHONE FROM CLIENT";
            try
            {
                var Command = ConnectionOracle.OracleCommand(query);
                var resultat = Command.ExecuteReader();
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
            ComboBoxClient.SelectedIndex = 0;
        }

        private void comboBoxClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            estimate.Client = ((ComboboxItemClient) ComboBoxClient.SelectedItem).Value;
        }

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

        private void BTNAddFeed_click(object sender, RoutedEventArgs e)
        {
            var merchandiseCost = Convert.ToInt32(LabelPrice.Content.ToString().Substring(0, LabelPrice.Content.ToString().Length - 1));
            var nbMerchandise = estimate.GetList.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                if(estimate[i].nom == ComboBoxProduct.Text)
                {
                    return;
                }
            }
            AddMerchandise(((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.id, ComboBoxProduct.Text, qte, merchandiseCost);
            AjouterEstimate.IsEnabled = true;
        }

        private void BTNAddEstimate_click(object sender, RoutedEventArgs e)
        {
            try
            {
                const string querySelect = "SELECT max(ID_DEVIS), max(NUMERODEVIS) FROM DEVIS";
                var OracleCommand = ConnectionOracle.OracleCommand(querySelect);
                var result = OracleCommand.ExecuteReader();
                var sizeList = ListMerchandise.Count;
                var date = DateTime.Now.ToString("dd/MM/yy");
                while(result.Read())
                {
                    var idEstimate = result[0].ToString() == "" ? 1 : Convert.ToInt32(result[0]) + 1;
                    var numberEstimate = result[1].ToString() == "" ? 1 : Convert.ToInt32(result[1]) + 1;
                    for(var i = 0; i < sizeList; i++)
                    {
                        var Insert = ConnectionOracle.OracleCommandStored("INSERTDEVIS");

                        Insert.Parameters.Add(new OracleParameter(":1", OracleDbType.Int32) {Value = estimate.Client.id});
                        Insert.Parameters.Add(new OracleParameter(":2", OracleDbType.Int32) {Value = estimate[i].id});
                        Insert.Parameters.Add(new OracleParameter(":3", OracleDbType.Int32) {Value = ((idEstimate) + i)});
                        Insert.Parameters.Add(new OracleParameter(":4", OracleDbType.Int32) {Value = estimate[i].quantite});
                        Insert.Parameters.Add(new OracleParameter(":5", OracleDbType.Varchar2) {Value = date});
                        Insert.Parameters.Add(new OracleParameter(":6", OracleDbType.Varchar2) {Value = estimate[i].prix});
                        Insert.Parameters.Add(new OracleParameter(":7", OracleDbType.Varchar2) {Value = (numberEstimate)});

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

            PanelEstimate.Children.Clear();
            ListMerchandise.Clear();
            TotalCost = 0;
            LabelTotalPrix.Content = "";
            AjouterEstimate.IsEnabled = false;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderEstimate.Width = EstimateCreator.ActualWidth - 340;
            BorderEstimate.Height = EstimateCreator.ActualHeight - 50;

            var nbMerchandise = estimate.GetList.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                estimate[i].Border.Width = BorderEstimate.Width - 6;
            }
        }

        private void BTN_Delete_Click(object sender, EventArgs e)
        {
            TotalCost = 0;
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
                AddMerchandise(ListMerchandiseN2[i].id, ListMerchandiseN2[i].nom, ListMerchandiseN2[i].quantite, ListMerchandiseN2[i].prix);
            }
            ListMerchandiseN2.Clear();
            if(estimate.GetList.Count == 0)
            {
                AjouterEstimate.IsEnabled = false;
            }
        }

        private void AddMerchandise(int id, string name, int qte, double price)
        {
            var panelMerchandise = new StackPanel();
            var newMerchandise = new Merchandise(id, name, qte, price);
            var thick = new Thickness(5, 2, 0, 0);

            var border = new Border
            {
                BorderBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor)),
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(2, 2, 1, 0),
                BorderThickness = new Thickness(1),
                Width = BorderEstimate.Width - 6,
                Child = panelMerchandise,
                Height = 70
            };

            //Name
            panelMerchandise.Children.Add(new TextBlock {HorizontalAlignment = HorizontalAlignment.Left, Margin = thick, Text = name, Height = 16});

            // Price
            panelMerchandise.Children.Add(new TextBlock {Text = price + "€", HorizontalAlignment = HorizontalAlignment.Left, Margin = thick, Height = 16});

            // Quantity
            panelMerchandise.Children.Add(new TextBlock
            {
                Text = qte.ToString(CultureInfo.InvariantCulture),
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = thick,
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
            BTN_Delete.Click += BTN_Delete_Click;

            newMerchandise.Border = border;
            PanelEstimate.Children.Add(border);
            estimate.GetList.Add(newMerchandise);
            TotalCost += price;
            LabelTotalPrix.Content = TotalCost + "€";
        }

        private void ErrorCost()
        {
            LabelPrice.Content = "Erreur";
            Ajouter.IsEnabled = false;
            LabelPrice.Foreground =
                TextBoxEstimateQte.CaretBrush =
                    TextBoxEstimateQte.SelectionBrush = TextBoxEstimateQte.BorderBrush = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
        }

        private void QteChanged()
        {
            qte = 0;

            if(IsInt(TextBoxEstimateQte.Text))
            {
                var newQte = Convert.ToInt32(TextBoxEstimateQte.Text);

                if(newQte <= 0)
                {
                    ErrorCost();
                }
                else
                {
                    if(ComboBoxProduct.Items.Count != 0)
                    {
                        qte = newQte;
                        LabelPrice.Foreground = new SolidColorBrush(Color.FromRgb(0xC1, 0xC1, 0xC1));
                        LabelPrice.Content = string.Format("{0}€", (((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.prix * qte));
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

        private static bool IsInt(string str)
        {
            int value;
            return (str.Trim() != string.Empty) && int.TryParse(str, out value);
        }
    }
}