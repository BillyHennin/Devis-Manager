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
        private readonly OracleConnection dataBaseConnection = ConnectionOracle.OracleDatabase(Settings.Default.DatabaseConnectionString);
        private readonly Estimate estimate = new Estimate(ListMerchandise);
        private int qte;
        private double totalCost;

        private static bool IsInt(string str)
        {
            int value;
            return (str.Trim() != string.Empty) && int.TryParse(str, out value);
        }

        private void ErrorCost()
        {
            LabelPrice.Content = "Erreur";
            Ajouter.IsEnabled = false;
            LabelPrice.Foreground = TextBoxEstimateQte.CaretBrush =
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
                    if (ComboBoxProduct.Items.Count != 0)
                    {
                        qte = newQte;
                        LabelPrice.Foreground = new SolidColorBrush(Color.FromRgb(0xC1, 0xC1, 0xC1));
                        LabelPrice.Content = string.Format("{0}€", (((ComboboxItemMerchandise)ComboBoxProduct.SelectedItem).Value.prix * qte));
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
            dataBaseConnection.Open();
            try
            {
                var oCommand = ConnectionOracle.OracleCommand(dataBaseConnection, query);
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
            finally
            {
                dataBaseConnection.Close();
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
                        LabelPrice.Content = ((ComboboxItemMerchandise)ComboBoxProduct.SelectedItem).Value.prix * Convert.ToInt32(TextBoxEstimateQte.Text)
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
                dataBaseConnection.Open();
                var oCommand = ConnectionOracle.OracleCommand(dataBaseConnection, query);
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
                dataBaseConnection.Close();
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
                if (estimate[i].nom == ComboBoxProduct.Text)
                {
                    return;
                }
            }
            addMerchandise(((ComboboxItemMerchandise)ComboBoxProduct.SelectedItem).Value.id, ComboBoxProduct.Text, qte, merchandiseCost);
            AjouterEstimate.IsEnabled = true;
        }

        private void BTNAddEstimate_click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataBaseConnection.Open();
                const string querySelect = "SELECT max(ID_DEVIS), max(NUMERODEVIS) FROM DEVIS";
                var oCommand = ConnectionOracle.OracleCommand(dataBaseConnection, querySelect);
                var result = oCommand.ExecuteReader();
                var sizeList = ListMerchandise.Count;
                var date = DateTime.Now.ToString("dd/MM/yy");
                while(result.Read())
                {
                    var idEstimate = result[0].ToString() == "" ? 1 : Convert.ToInt32(result[0]);
                    var numberEstimate = result[1].ToString() == "" ? 1 : Convert.ToInt32(result[1]);
                    for(var i = 0; i < sizeList; i++)
                    {
                        CommandOracle.Insert(estimate.Client.id, estimate[i], idEstimate, numberEstimate, date);
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

            PanelEstimate.Children.Clear();
            ListMerchandise.Clear();
            totalCost = 0;
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
                addMerchandise(ListMerchandiseN2[i].id, ListMerchandiseN2[i].nom, ListMerchandiseN2[i].quantite, ListMerchandiseN2[i].prix);
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