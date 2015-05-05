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
        private readonly Estimate _estimate = new Estimate(ListMerchandise);
        private double _itemSelectedPrice;
        private int _itemSelectedQuantity;
        private double _totalCost;

        private void EstimateCreator_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateText();
            var nbMerchandise = _estimate.GetList.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                _estimate[i].Border.BorderBrush = BtnAdd.BorderBrush;
            }
            QuantityChanged();
        }

        private void ComboBoxCategory_Initialized(object sender, EventArgs e)
        {
            try
            {
                var command = Connection.Connection.GetAll(Category.TableName);
                var resultat = command.ExecuteReader();
                while(resultat.Read())
                {
                    ComboBoxCategory.Items.Add(new ComboboxItemCategory
                    {
                        Text = resultat[Category.Title].ToString(),
                        Value = new Classes.Category(Convert.ToInt32(resultat[Category.ID]), resultat[Category.Title].ToString())
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
                    Table.Merchandise.Quantity, Category.TableName, ((ComboboxItemCategory) ComboBoxCategory.SelectedItem).Value.Id);
                var command = Connection.Connection.GetAll(query);
                var resultat = command.ExecuteReader();
                while(resultat.Read())
                {
                    ComboBoxProduct.Items.Add(new ComboboxItemMerchandise
                    {
                        Text = resultat[Table.Merchandise.Name].ToString(),
                        Value =
                            new Merchandise(Convert.ToInt32(resultat[Table.Merchandise.ID]), resultat[Table.Merchandise.Name].ToString(),
                                Convert.ToInt32(resultat[Table.Merchandise.Price]), Convert.ToInt32(resultat[Table.Merchandise.Quantity]),
                                ((ComboboxItemCategory) ComboBoxCategory.SelectedItem).Value.Id)
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
                switch(_itemSelectedQuantity)
                {
                    case 0:
                        ErrorCost();
                        break;
                    default:
                        _itemSelectedPrice = ((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.Price * Convert.ToInt32(TextBoxEstimateQte.Text);
                        AllPrice.Text = String.Format("{0} {1}€", Transharp.GetTranslation("All_Price"), _itemSelectedPrice);
                        var nbMerchandise = _estimate.GetList.Count;
                        for(var i = 0; i < nbMerchandise; i++)
                        {
                            if(_estimate[i].Id == ((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.Id)
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
                var command = Connection.Connection.GetAll(Table.Customer.TableName);
                var resultat = command.ExecuteReader();
                while(resultat.Read())
                {
                    ComboBoxClient.Items.Add(new ComboboxItemCustomer
                    {
                        Text = resultat[Table.Customer.Name].ToString(),
                        Value =
                            new Customer(Convert.ToInt32(resultat[Table.Customer.ID]), resultat[Table.Customer.Name].ToString(),
                                resultat[Table.Customer.Phone].ToString(), resultat[Table.Customer.Email].ToString())
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
            _estimate.Customer = ((ComboboxItemCustomer) ComboBoxClient.SelectedItem).Value;
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
            var text = String.Format("{0} - {1}", ComboBoxCategory.Text, ComboBoxProduct.Text);
            var merchandiseCost = _itemSelectedPrice;
            var nbMerchandise = _estimate.GetList.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                if(_estimate[i].Name != text)
                {
                    continue;
                }
                UpdateEstimate(i, null);
                return;
            }
            AddMerchandise(((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.Id, text, _itemSelectedQuantity, merchandiseCost,
                ((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.CategoryId);
            AjouterEstimate.IsEnabled = true;

            BtnAdd.Content = Transharp.GetTranslation("BTN_Modify");
        }

        private void BTNAddEstimate_click(object sender, RoutedEventArgs e)
        {
            var numberEstimate = 0;
            try
            {
                var querySelect = String.Format("SELECT max(ID_{0}), max({1}) FROM {0}", Table.Estimate.TableName, Table.Estimate.NumberDevis);
                var oracleCommand = Connection.Connection.Command(querySelect);
                var result = oracleCommand.ExecuteReader();
                var sizeList = ListMerchandise.Count;
                while(result.Read())
                {
                    var idEstimate = result[0].ToString() == String.Empty ? 1 : Convert.ToInt32(result[0]) + 1;
                    numberEstimate = result[1].ToString() == String.Empty ? 1 : Convert.ToInt32(result[1]) + 1;
                    for(var i = 0; i < sizeList; i++)
                    {
                        Connection.Connection.Insert(Table.Estimate.TableName, _estimate.Customer.Id, _estimate[i].Id, ((idEstimate) + i), _estimate[i].Quantity,
                            DateTime.Now.ToString("dd/MM/yy"), _estimate[i].Price, (numberEstimate));
                    }
                }
                result.Close();
                BtnAdd.Content = Transharp.GetTranslation("BTN_Add");
                ModernDialog.ShowMessage(Transharp.GetTranslation("Box_SuccessAdd", numberEstimate, _totalCost), Transharp.GetTranslation("Box_CE_Success"),
                    MessageBoxButton.OK);
            }
            catch
            {
                ModernDialog.ShowMessage(Transharp.GetTranslation("Box_DBFail"), Transharp.GetTranslation("Box_Error"), MessageBoxButton.OK);
            }
            finally
            {
                PanelEstimate.Children.Clear();
                ListMerchandise.Clear();
                _totalCost = 0;
                LabelTotalPrix.Text = String.Empty;
                AjouterEstimate.IsEnabled = false;
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderEstimate.Width = EstimateCreator.ActualWidth - 340;
            BorderEstimate.Height = EstimateCreator.ActualHeight - 70;

            var nbMerchandise = _estimate.GetList.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                _estimate[i].Border.Width = BorderEstimate.Width - 6;
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

            var convertFromString = ColorConverter.ConvertFromString(Settings.Default.AccentColor);
            if(convertFromString != null) {
                var border = new Border
                {
                    BorderBrush = new SolidColorBrush((Color) convertFromString),
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
                var btnDelete = new Button
                {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Content = Transharp.GetTranslation("EC_DeleteMerchandise"),
                    Margin = new Thickness(9, -30, 67, 50),
                    BorderBrush = Brushes.Red,
                    Tag = newMerchandise
                };

                panelMerchandise.Children.Add(btnDelete);
                btnDelete.Click += BTN_Delete_Click;

                newMerchandise.Border = border;
                PanelEstimate.Children.Add(border);
            }
            _estimate.GetList.Add(newMerchandise);
            _totalCost += price;
            LabelTotalPrix.Text = Transharp.GetTranslation("All_Total", _totalCost);
        }

        private void ErrorCost()
        {
            AllPrice.Text = String.Format("{0} {1}", Transharp.GetTranslation("All_Price"), Transharp.GetTranslation("Box_Error"));
            BtnAdd.IsEnabled = false;
            AllPrice.Foreground = TextBoxEstimateQte.CaretBrush = TextBoxEstimateQte.SelectionBrush = TextBoxEstimateQte.BorderBrush = Brushes.Red;
        }

        private void QuantityChanged()
        {
            _itemSelectedQuantity = 0;

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
                        _itemSelectedQuantity = newQuantity;
                        _itemSelectedPrice = (((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.Price * newQuantity);
                        AllPrice.Foreground = LabelTotalPrix.Foreground;
                        AllPrice.Text = String.Format("{0} {1}€", Transharp.GetTranslation("All_Price"), _itemSelectedPrice);
                        var convertFromString = ColorConverter.ConvertFromString(Settings.Default.AccentColor);
                        if(convertFromString != null)
                        {
                            TextBoxEstimateQte.BorderBrush =
                                TextBoxEstimateQte.CaretBrush =
                                    TextBoxEstimateQte.SelectionBrush = new SolidColorBrush((Color) convertFromString);
                        }
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

        private static Boolean IsInt(string str)
        {
            int value;
            return (str.Trim() != string.Empty) && int.TryParse(str, out value);
        }

        private void UpdateEstimate(int? merchandise, int? id)
        {
            var listMerchandiseN2 = new List<Merchandise>();
            BtnAdd.Content = Transharp.GetTranslation("BTN_Add");
            _totalCost = 0;
            LabelTotalPrix.Text = String.Empty;

            var nbMerchandise = _estimate.GetList.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                if(merchandise.HasValue)
                {
                    if(i == merchandise.Value)
                    {
                        var text = String.Format("{0} - {1}", ComboBoxCategory.Text, ComboBoxProduct.Text);
                        var merchandiseCost = _itemSelectedPrice;
                        listMerchandiseN2.Add(new Merchandise(((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.Id, text, _itemSelectedQuantity,
                            merchandiseCost, ((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.CategoryId));
                    }
                    else
                    {
                        listMerchandiseN2.Add(ListMerchandise[i]);
                    }
                }
                else
                {
                    if(ListMerchandise[i].ToString() != id.ToString())
                    {
                        listMerchandiseN2.Add(ListMerchandise[i]);
                    }
                }
            }
            if(id.HasValue)
            {
                nbMerchandise -= 1;
            }

            for(var i = 0; i < nbMerchandise; i++)
            {
                if(listMerchandiseN2[i].Id == ((ComboboxItemMerchandise) ComboBoxProduct.SelectedItem).Value.Id)
                {
                    BtnAdd.Content = Transharp.GetTranslation("BTN_Modify");
                }
            }

            PanelEstimate.Children.Clear();
            _estimate.GetList.Clear();

            for(var i = 0; i < nbMerchandise; i++)
            {
                AddMerchandise(listMerchandiseN2[i].Id, listMerchandiseN2[i].Name, listMerchandiseN2[i].Quantity, listMerchandiseN2[i].Price,
                    listMerchandiseN2[i].CategoryId);
            }
            listMerchandiseN2.Clear();
            if(_estimate.GetList.Count == 0)
            {
                AjouterEstimate.IsEnabled = false;
            }
        }

        private void UpdateText()
        {
            EcTitle.Text = Transharp.GetTranslation("EC_Title");
            EcChooseCategory.Text = Transharp.GetTranslation("EC_ChooseCategory");
            EcAddMerchandise.Text = Transharp.GetTranslation("EC_AddMerchandise");
            AllQte.Text = Transharp.GetTranslation("All_QTE");
            AllPrice.Text = Transharp.GetTranslation("All_Price");
            EcCustomer.Text = Transharp.GetTranslation("EC_Customer");
            BtnAdd.Content = Transharp.GetTranslation("BTN_Add");
            AjouterEstimate.Content = Transharp.GetTranslation("BTN_Create");
        }
    }
}