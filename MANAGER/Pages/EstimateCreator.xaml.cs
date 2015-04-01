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

using FirstFloor.ModernUI.Windows.Controls;

using MANAGER.Classes;
using MANAGER.ComboBox;
using MANAGER.Properties;

using Category = MANAGER.Table.Category;

#endregion

namespace MANAGER.Pages
{
    public partial class EstimatePage
    {
        private static readonly List<Merchandise> ListMerchandise = new List<Merchandise>();
        private readonly Estimate estimate = new Estimate(ListMerchandise);
        private double ItemSelectedPrice;
        private int ItemSelectedQuantity;
        private double TotalCost;

        private void EstimateCreator_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateText();
            var nbMerchandise = estimate.GetList.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                estimate[i].Border.BorderBrush = BtnAdd.BorderBrush;
            }
            QuantityChanged();
        }

        private void ComboBoxCategory_Initialized(object sender, EventArgs e)
        {
            try
            {
                var Command = Connection.Connection.GetAll(Category.TableName);
                var resultat = Command.ExecuteReader();
                while(resultat.Read())
                {
                    ComboBoxCategory.Items.Add(new ComboboxItemCategory
                    {
                        Text = resultat[1].ToString(),
                        Value = new Classes.Category(Convert.ToInt32(resultat[0]), resultat[1].ToString())
                    });
                }
                resultat.Close();
            }
            catch
            {
                ModernDialog.ShowMessage(Transharp.GetTranslation("Box_DBFail"), Transharp.GetTranslation("Box_Error"), MessageBoxButton.OK);
            }
        }

        private void ComboBoxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBoxProduct.Items.Clear();
                var query = String.Format("{0} WHERE {1} = 1 AND {2} > 0 AND ID_{3}={4}", Table.Merchandise.TableName, Table.Merchandise.OnSale,
                    Table.Merchandise.Quantity, Category.TableName, ((ComboboxItemCategory) ComboBoxCategory.SelectedItem).Value.ID);
                var Command = Connection.Connection.GetAll(query);
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
                ModernDialog.ShowMessage(Transharp.GetTranslation("Box_DBFail"), Transharp.GetTranslation("Box_Error"), MessageBoxButton.OK);
            }
        }

        private void ComboBoxProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnAdd.Content = Transharp.GetTranslation("BTN_Add");
            try
            {
                switch(ItemSelectedQuantity)
                {
                    case 0:
                        ErrorCost();
                        break;
                    default:
                        ItemSelectedPrice = ((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.price * Convert.ToInt32(TextBoxEstimateQte.Text);
                        All_Price.Text = String.Format("{0} {1}€", Transharp.GetTranslation("All_Price"), ItemSelectedPrice);
                        var nbMerchandise = estimate.GetList.Count;
                        for(var i = 0; i < nbMerchandise; i++)
                        {
                            if(estimate[i].id == ((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.id)
                            {
                                BtnAdd.Content = Transharp.GetTranslation("BTN_Modify");
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
            try
            {
                var Command = Connection.Connection.GetAll(Table.Customer.TableName);
                var resultat = Command.ExecuteReader();
                while(resultat.Read())
                {
                    ComboBoxClient.Items.Add(new ComboboxItemCustomer
                    {
                        Text = resultat[2].ToString(),
                        Value = new Customer(Convert.ToInt32(resultat[0]), resultat[2].ToString(), resultat[1].ToString(), resultat[3].ToString())
                    });
                }
                resultat.Close();
                ComboBoxClient.SelectedIndex = 0;
            }
            catch
            {
                ModernDialog.ShowMessage(Transharp.GetTranslation("Box_DBFail"), Transharp.GetTranslation("Box_Error"), MessageBoxButton.OK);
            }
        }

        private void comboBoxClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            estimate.Customer = ((ComboboxItemCustomer) ComboBoxClient.SelectedItem).Value;
        }

        private void TextBoxEstimateQte_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                QuantityChanged();
            }
            catch(Exception caught)
            {
                Console.WriteLine(caught.Message);
                Console.Read();
            }
        }

        private void BTNAddFeed_click(object sender, RoutedEventArgs e)
        {
            var Text = String.Format("{0} - {1}", ComboBoxCategory.Text, ComboBoxProduct.Text);
            var merchandiseCost = ItemSelectedPrice;
            var nbMerchandise = estimate.GetList.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                if(estimate[i].name != Text)
                {
                    continue;
                }
                UpdateEstimate(i, null);
                return;
            }
            AddMerchandise(((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.id, Text, ItemSelectedQuantity, merchandiseCost,
                ((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.categoryID);
            AjouterEstimate.IsEnabled = true;

            BtnAdd.Content = Transharp.GetTranslation("BTN_Modify");
        }

        private void BTNAddEstimate_click(object sender, RoutedEventArgs e)
        {
            var numberEstimate = 0;
            try
            {
                var querySelect = String.Format("SELECT max(ID_{0}), max({1}) FROM {0}", Table.Estimate.TableName, Table.Estimate.NumberDevis);
                var OracleCommand = Connection.Connection.Command(querySelect);
                var result = OracleCommand.ExecuteReader();
                var sizeList = ListMerchandise.Count;
                while(result.Read())
                {
                    var idEstimate = result[0].ToString() == String.Empty ? 1 : Convert.ToInt32(result[0]) + 1;
                    numberEstimate = result[1].ToString() == String.Empty ? 1 : Convert.ToInt32(result[1]) + 1;
                    for(var i = 0; i < sizeList; i++)
                    {
                        Connection.Connection.Insert(Table.Estimate.TableName, estimate.Customer.id, estimate[i].id, ((idEstimate) + i), estimate[i].quantity,
                            DateTime.Now.ToString("dd/MM/yy"), estimate[i].price, (numberEstimate));
                    }
                }
                result.Close();
                BtnAdd.Content = Transharp.GetTranslation("BTN_Add");
                ModernDialog.ShowMessage(Transharp.GetTranslation("Box_SuccessAdd", numberEstimate, TotalCost), Transharp.GetTranslation("Box_CE_Success"),MessageBoxButton.OK);
            }
            catch
            {
                ModernDialog.ShowMessage(Transharp.GetTranslation("Box_DBFail"), Transharp.GetTranslation("Box_Error"), MessageBoxButton.OK);
            }
            finally
            {
                PanelEstimate.Children.Clear();
                ListMerchandise.Clear();
                TotalCost = 0;
                LabelTotalPrix.Text = String.Empty;
                AjouterEstimate.IsEnabled = false;
            }
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
            UpdateEstimate(null, Convert.ToInt32(((Button) sender).Tag.ToString()));
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
                Text = String.Format("{0}€", price),
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = thick,
                Height = 16
            });

            // Quantity
            panelMerchandise.Children.Add(new TextBlock
            {
                Text = String.Format("{0} : {1}", Transharp.GetTranslation("EC_Quantity"), quantity.ToString(CultureInfo.InvariantCulture)),
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = thick,
                Height = 16
            });

            // Button
            var BTN_Delete = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = Transharp.GetTranslation("EC_DeleteMerchandise"),
                Margin = new Thickness(9, -30, 67, 50),
                BorderBrush = Brushes.Red,
                Tag = newMerchandise
            };

            panelMerchandise.Children.Add(BTN_Delete);
            BTN_Delete.Click += BTN_Delete_Click;

            newMerchandise.Border = border;
            PanelEstimate.Children.Add(border);
            estimate.GetList.Add(newMerchandise);
            TotalCost += price;
            LabelTotalPrix.Text = Transharp.GetTranslation("All_Total", TotalCost);
        }

        private void ErrorCost()
        {
            All_Price.Text = String.Format("{0} {1}", Transharp.GetTranslation("All_Price"), Transharp.GetTranslation("Box_Error"));
            BtnAdd.IsEnabled = false;
            All_Price.Foreground = TextBoxEstimateQte.CaretBrush = TextBoxEstimateQte.SelectionBrush = TextBoxEstimateQte.BorderBrush = Brushes.Red;
        }

        private void QuantityChanged()
        {
            ItemSelectedQuantity = 0;

            if(IsInt(TextBoxEstimateQte.Text))
            {
                var newQuantity = Convert.ToInt32(TextBoxEstimateQte.Text);

                if(newQuantity <= 0)
                {
                    ErrorCost();
                }
                else
                {
                    if(ComboBoxProduct.Items.Count != 0)
                    {
                        ItemSelectedQuantity = newQuantity;
                        ItemSelectedPrice = (((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.price * newQuantity);
                        All_Price.Foreground = LabelTotalPrix.Foreground;
                        All_Price.Text = String.Format("{0} {1}€", Transharp.GetTranslation("All_Price"), ItemSelectedPrice);
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
            var ListMerchandiseN2 = new List<Merchandise>();
            BtnAdd.Content = Transharp.GetTranslation("BTN_Add");
            TotalCost = 0;
            LabelTotalPrix.Text = String.Empty;

            var nbMerchandise = estimate.GetList.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                if(merchandise.HasValue)
                {
                    if(i == merchandise.Value)
                    {
                        var Text = String.Format("{0} - {1}", ComboBoxCategory.Text, ComboBoxProduct.Text);
                        var merchandiseCost = ItemSelectedPrice;
                        ListMerchandiseN2.Add(new Merchandise(((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.id, Text, ItemSelectedQuantity,
                            merchandiseCost, ((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.categoryID));
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

            for(var i = 0; i < nbMerchandise; i++)
            {
                if(ListMerchandiseN2[i].id == ((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.id)
                {
                    BtnAdd.Content = Transharp.GetTranslation("BTN_Modify");
                }
            }

            PanelEstimate.Children.Clear();
            estimate.GetList.Clear();

            for(var i = 0; i < nbMerchandise; i++)
            {
                AddMerchandise(ListMerchandiseN2[i].id, ListMerchandiseN2[i].name, ListMerchandiseN2[i].quantity, ListMerchandiseN2[i].price,
                    ListMerchandiseN2[i].categoryID);
            }
            ListMerchandiseN2.Clear();
            if(estimate.GetList.Count == 0)
            {
                AjouterEstimate.IsEnabled = false;
            }
        }

        private void UpdateText()
        {
            EC_Title.Text = Transharp.GetTranslation("EC_Title");
            EC_ChooseCategory.Text = Transharp.GetTranslation("EC_ChooseCategory");
            EC_AddMerchandise.Text = Transharp.GetTranslation("EC_AddMerchandise");
            All_QTE.Text = Transharp.GetTranslation("All_QTE");
            All_Price.Text = Transharp.GetTranslation("All_Price");
            EC_Customer.Text = Transharp.GetTranslation("EC_Customer");
            BtnAdd.Content = Transharp.GetTranslation("BTN_Add");
            AjouterEstimate.Content = Transharp.GetTranslation("BTN_Create");
        }
    }
}