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
        #region attributes

        private static readonly List<Merchandise> ListMerchandise = new List<Merchandise>();
        private static readonly List<Merchandise> ListMerchandiseN2 = new List<Merchandise>();
        private readonly Estimate estimate = new Estimate(ListMerchandise);
        private double TotalCost;
        private int qte;

        #endregion

        private void EstimateCreator_Loaded(object sender, RoutedEventArgs e)
        {
            var nbMerchandise = estimate.GetList.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                estimate[i].Border.BorderBrush = BtnAdd.BorderBrush;
            }
            QteChanged();
        }

        private void ComboBoxCategory_Initialized(object sender, EventArgs e)
        {
            const string query = "SELECT * FROM CATEGORIE";
            try
            {
                var oCommand = ConnectionOracle.OracleCommand(query);
                var resultat = oCommand.ExecuteReader();
                while(resultat.Read())
                {
                    ComboBoxCategory.Items.Add(new ComboboxItemCategory
                    {
                        Text = resultat[1].ToString(),
                        Value = new Category(Convert.ToInt32(resultat[0]), resultat[1].ToString())
                    });
                }
                resultat.Close();
            }
            catch
            {
                MessageBox.Show(Localisation.Localisation.Box_DBFail, Localisation.Localisation.Box_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ComboBoxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBoxProduct.Items.Clear();
                var Command = ConnectionOracle.OracleCommand("SELECT * FROM MARCHANDISE WHERE ENVENTE = 1 AND QUANTITE > 0 AND ID_CATEGORIE=:CATEGORIE");
                Command.Parameters.Add(new OracleParameter(":ID_CLIENT", OracleDbType.Int32)
                {
                    Value = ((ComboboxItemCategory) ComboBoxCategory.SelectedItem).Value.ID
                });
                var resultat = Command.ExecuteReader();
                while(resultat.Read())
                {
                    ComboBoxProduct.Items.Add(new ComboboxItemMerchandise
                    {
                        Text = resultat[1].ToString(),
                        Value =
                            new Merchandise(Convert.ToInt32(resultat[0]), resultat[1].ToString(), Convert.ToInt32(resultat[3]), Convert.ToInt32(resultat[2]),
                                ((ComboboxItemCategory) ComboBoxCategory.SelectedItem).Value.ID)
                    });
                }
                resultat.Close();
                ComboBoxProduct.SelectedIndex = 0;
            }
            catch
            {
                MessageBox.Show(Localisation.Localisation.Box_DBFail, Localisation.Localisation.Box_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ComboBoxProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnAdd.Content = Localisation.Localisation.BTN_Add;
            
            try
            {
                switch(qte)
                {
                    case 0:
                        ErrorCost();
                        break;
                    default:
                        LabelPrice.Content = string.Format("{0}€",
                            ((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.price * Convert.ToInt32(TextBoxEstimateQte.Text));
                        var nbMerchandise = estimate.GetList.Count;
                        for (var i = 0; i < nbMerchandise; i++)
                        {
                            if (estimate[i].id == ((ComboboxItemMerchandise)ComboBoxProduct.SelectedItem).Value.id)
                            {
                                BtnAdd.Content = Localisation.Localisation.BTN_Modify;
                            }
                        }
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
                ComboBoxClient.SelectedIndex = 0;
            }
            catch
            {
                MessageBox.Show(Localisation.Localisation.Box_DBFail, Localisation.Localisation.Box_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            var Text = string.Format("{0} - {1}", ComboBoxCategory.Text, ComboBoxProduct.Text);
            var merchandiseCost = Convert.ToInt32(LabelPrice.Content.ToString().Substring(0, LabelPrice.Content.ToString().Length - 1));
            var nbMerchandise = estimate.GetList.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                if (estimate[i].nom == Text)
                {
                    UpdateEstimate(i, null);
                    return;
                }
            }
            AddMerchandise(((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.id, Text, qte, merchandiseCost,
                ((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.categoryID);
            AjouterEstimate.IsEnabled = true;

            BtnAdd.Content = Localisation.Localisation.BTN_Modify;
        }

        private void BTNAddEstimate_click(object sender, RoutedEventArgs e)
        {
            var numberEstimate = 0;
            try
            {
                const string querySelect = "SELECT max(ID_DEVIS), max(NUMERODEVIS) FROM DEVIS";
                var OracleCommand = ConnectionOracle.OracleCommand(querySelect);
                var result = OracleCommand.ExecuteReader();
                var sizeList = ListMerchandise.Count;
                while(result.Read())
                {
                    var idEstimate = result[0].ToString() == "" ? 1 : Convert.ToInt32(result[0]) + 1;
                    numberEstimate = result[1].ToString() == "" ? 1 : Convert.ToInt32(result[1]) + 1;
                    for(var i = 0; i < sizeList; i++)
                    {
                        var Insert = ConnectionOracle.OracleCommandStored("INSERTDEVIS");
                        Insert.Parameters.Add(new OracleParameter(":1", OracleDbType.Int32) {Value = estimate.Client.id});
                        Insert.Parameters.Add(new OracleParameter(":2", OracleDbType.Int32) {Value = estimate[i].id});
                        Insert.Parameters.Add(new OracleParameter(":3", OracleDbType.Int32) {Value = ((idEstimate) + i)});
                        Insert.Parameters.Add(new OracleParameter(":4", OracleDbType.Int32) {Value = estimate[i].quantite});
                        Insert.Parameters.Add(new OracleParameter(":5", OracleDbType.Varchar2) {Value = DateTime.Now.ToString("dd/MM/yy")});
                        Insert.Parameters.Add(new OracleParameter(":6", OracleDbType.Varchar2) {Value = estimate[i].price});
                        Insert.Parameters.Add(new OracleParameter(":7", OracleDbType.Varchar2) {Value = (numberEstimate)});
                        Insert.ExecuteNonQuery();
                    }
                }
                result.Close();
                var message = string.Format(Localisation.Localisation.Box_SuccessAdd, numberEstimate, TotalCost);
                MessageBox.Show(message, Localisation.Localisation.Box_Success, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show(Localisation.Localisation.Box_DBFail, Localisation.Localisation.Box_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            PanelEstimate.Children.Clear();
            ListMerchandise.Clear();
            TotalCost = 0;
            LabelTotalPrix.Text = "";
            AjouterEstimate.IsEnabled = false;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderEstimate.Width = EstimateCreator.ActualWidth - 340;
            BorderEstimate.Height = EstimateCreator.ActualHeight - 70;

            var nbMerchandise = estimate.GetList.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                estimate[i].Border.Width = BorderEstimate.Width - 6;
            }
        }

        private void BTN_Delete_Click(object sender, EventArgs e)
        {
            UpdateEstimate(null, Convert.ToInt32(((Button)sender).Tag.ToString()));
        }

        private void AddMerchandise(int id, string name, int quantity, double price, int category)
        {
            var panelMerchandise = new StackPanel();
            var newMerchandise = new Merchandise(id, name, quantity, price, category);
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

            // Name
            panelMerchandise.Children.Add(new TextBlock {HorizontalAlignment = HorizontalAlignment.Left, Margin = thick, Text = name, Height = 16});

            // Price
            panelMerchandise.Children.Add(new TextBlock
            {
                Text = string.Format("{0}€", price),
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = thick,
                Height = 16
            });

            // Quantity
            panelMerchandise.Children.Add(new TextBlock
            {
                Text = quantity.ToString(CultureInfo.InvariantCulture),
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = thick,
                Height = 16
            });

            var BTN_Delete = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = Localisation.Localisation.EC_DeleteMerchandise,
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
            LabelTotalPrix.Text = string.Format("Total : {0}€", TotalCost);
        }

        private void ErrorCost()
        {
            LabelPrice.Content = Localisation.Localisation.Box_Error;
            BtnAdd.IsEnabled = false;
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
                        LabelPrice.Content = string.Format("{0}€", (((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.price * qte));
                        TextBoxEstimateQte.BorderBrush =
                            TextBoxEstimateQte.CaretBrush =
                                TextBoxEstimateQte.SelectionBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor));
                        BtnAdd.IsEnabled = true;
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

        private void UpdateEstimate(int? merchandise, int? id)
        {
            BtnAdd.Content = Localisation.Localisation.BTN_Add;

            TotalCost = 0;
            LabelTotalPrix.Text = "";

            var nbMerchandise = estimate.GetList.Count;
            for (var i = 0; i < nbMerchandise; i++)
            {
                if (merchandise.HasValue)
                {
                    if(i == merchandise)
                    {
                        var Text = string.Format("{0} - {1}", ComboBoxCategory.Text, ComboBoxProduct.Text);
                        var merchandiseCost = Convert.ToInt32(LabelPrice.Content.ToString().Substring(0, LabelPrice.Content.ToString().Length - 1));
                        ListMerchandiseN2.Add(new Merchandise(((ComboboxItemMerchandise)ComboBoxProduct.SelectedItem).Value.id, Text, qte, merchandiseCost,
                            ((ComboboxItemMerchandise)ComboBoxProduct.SelectedItem).Value.categoryID));
                    }
                    else
                    {
                        ListMerchandiseN2.Add(ListMerchandise[i]);
                    }
                }
                else
                {
                    if(ListMerchandise[i].ToString() != id.ToString())
                    {
                        ListMerchandiseN2.Add(ListMerchandise[i]);
                    }
                    
                }
            }
            if(id.HasValue)
            {
                nbMerchandise -= 1;
            }

            for (var i = 0; i < nbMerchandise; i++)
            {
                if (ListMerchandiseN2[i].id == ((ComboboxItemMerchandise)ComboBoxProduct.SelectedItem).Value.id)
                {
                    BtnAdd.Content = Localisation.Localisation.BTN_Modify;
                }
            }

            PanelEstimate.Children.Clear();
            estimate.GetList.Clear();


            for (var i = 0; i < nbMerchandise; i++)
            {
                AddMerchandise(ListMerchandiseN2[i].id, ListMerchandiseN2[i].nom, ListMerchandiseN2[i].quantite, ListMerchandiseN2[i].price,
                    ListMerchandiseN2[i].categoryID);
            }
            ListMerchandiseN2.Clear();
            if (estimate.GetList.Count == 0)
            {
                AjouterEstimate.IsEnabled = false;
            }
        }
    }
}