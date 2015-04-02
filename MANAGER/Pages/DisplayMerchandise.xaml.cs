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

using FirstFloor.ModernUI.Windows.Controls;

using MANAGER.Classes;

using Category = MANAGER.Table.Category;

#endregion

namespace MANAGER.Pages
{
    public partial class DisplayMerchandise
    {
        private static readonly List<Merchandise> ListMerchandise = new List<Merchandise>();
        private static readonly List<Merchandise> SecondListMerchandise = new List<Merchandise>();

        private void SelectMarchandiseLike(string merchandise)
        {
            PanelMerchandise.Children.Clear();
            ListMerchandise.Clear();

            var nbMerchandise = SecondListMerchandise.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                if(!SecondListMerchandise[i].name.ToLower().Contains(merchandise.ToLower())
                   && !SecondListMerchandise[i].price.ToString(CultureInfo.InvariantCulture).Contains(merchandise)
                   && !SecondListMerchandise[i].quantity.ToString(CultureInfo.InvariantCulture).Contains(merchandise))
                {
                    continue;
                }
                var id = SecondListMerchandise[i].id;
                var text = SecondListMerchandise[i].name;
                var newMerchandise = new Merchandise(id, text, SecondListMerchandise[i].quantity, SecondListMerchandise[i].price,
                    SecondListMerchandise[i].categoryID) {onSale = SecondListMerchandise[i].onSale};

                Display(text, newMerchandise);
            }
            if(ListMerchandise.Count != 0)
            {
                return;
            }
            var panelMerchandise = new StackPanel();
            // New border
            var border = new Border
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(2, 2, 1, 0),
                BorderThickness = new Thickness(1),
                Width = BorderEstimate.Width - 5,
                Child = panelMerchandise,
                Height = 100
            };

            PanelMerchandise.Children.Add(border);

            // Merchandise's name
            panelMerchandise.Children.Add(new TextBlock
            {
                Text = Transharp.GetTranslation("DM_SearchNull"),
                Margin = new Thickness(5, 2, 0, 0),
                Height = 40,
                TextAlignment = TextAlignment.Center
            });
        }

        private void TextBoxEstimateQte_TextChanged(object sender, TextChangedEventArgs e)
        {
            SelectMarchandiseLike(TextBoxEstimateQte.Text == String.Empty ? String.Empty : TextBoxEstimateQte.Text);
        }

        private void Display(string text, Merchandise newMerchandise)
        {
            var panelMerchandise = new StackPanel();
            var thick = new Thickness(5, 2, 0, 0);

            // New border
            var border = new Border
            {
                BorderBrush = BorderEstimate.BorderBrush,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(2, 2, 1, 0),
                BorderThickness = new Thickness(1),
                Width = BorderEstimate.Width - 5,
                Child = panelMerchandise,
                Height = 70
            };

            PanelMerchandise.Children.Add(border);

            // Merchandise's name
            panelMerchandise.Children.Add(new TextBlock {Text = text, Margin = thick, Height = 16});

            // Quantity
            panelMerchandise.Children.Add(new TextBlock
            {
                Text = Transharp.GetTranslation("DM_Stock", newMerchandise.quantity),
                Margin = new Thickness(5, 2, 0, 0),
                Height = 16
            });

            // Price
            panelMerchandise.Children.Add(new TextBlock {Text = String.Format("{0}€", newMerchandise.price), Margin = thick, Height = 16});

            var BTN_Sale = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = newMerchandise.onSale ? Transharp.GetTranslation("DM_OnSale") : Transharp.GetTranslation("DM_NotOnSale"),
                Margin = new Thickness(9, -30, 67, 50),
                BorderBrush = newMerchandise.onSale ? Brushes.Lime : Brushes.Red,
                Tag = newMerchandise
            };

            // Delete button
            panelMerchandise.Children.Add(BTN_Sale);

            BTN_Sale.Click += BTN_Sale_Click;

            newMerchandise.Border = border;
            ListMerchandise.Add(newMerchandise);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DM_Merchandise.Text = Transharp.GetTranslation("DM_Merchandise");

            PanelMerchandise.Children.Clear();
            ListMerchandise.Clear();
            SecondListMerchandise.Clear();
            try
            {
                var oCommand = Connection.Connection.GetAll(Table.Merchandise.TableName);
                var resultat = oCommand.ExecuteReader();
                while(resultat.Read())
                {
                    var category = string.Empty;
                    var query = String.Format("SELECT {0} FROM {1} WHERE ID_{1} = {2}", Category.Title, Category.TableName, resultat[Category.ID]);
                    var CommandCategory = Connection.Connection.Command(query);
                    var resultatCategory = CommandCategory.ExecuteReader();
                    while(resultatCategory.Read())
                    {
                        category = resultatCategory[Category.Title].ToString();
                    }
                    var text = String.Format("{0} - {1}", category, resultat[Table.Merchandise.Name]);
                    var newMerchandise = new Merchandise(Convert.ToInt32(resultat[Table.Merchandise.ID]), text,
                        Convert.ToInt32(resultat[Table.Merchandise.Quantity]), Convert.ToInt32(resultat[Table.Merchandise.Price]),
                        Convert.ToInt32(resultat[Category.ID])) { onSale = Convert.ToBoolean(resultat[Table.Merchandise.OnSale]) };
                    Display(text, newMerchandise);
                    SecondListMerchandise.Add(newMerchandise);
                }
                resultat.Close();
            }
            catch
            {
                ModernDialog.ShowMessage(Transharp.GetTranslation("Box_DBFail"), Transharp.GetTranslation("Box_Error"), MessageBoxButton.OK);
            }
        }

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

        private void BTN_Sale_Click(object sender, EventArgs e)
        {
            var id = ((Button) sender).Tag.ToString();
            var num = Convert.ToInt32(id) - 1;
            var onSale = !SecondListMerchandise[num].onSale ? 1 : 0;
            try
            {
                var query = String.Format("UPDATE {0} SET {1} = {2} WHERE ID_{0} = {3}", Table.Merchandise.TableName, Table.Merchandise.OnSale, onSale, id);
                var commandeModif = Connection.Connection.Command(query);
                commandeModif.ExecuteNonQuery();
            }
            catch
            {
                ModernDialog.ShowMessage(Transharp.GetTranslation("Box_DBFail"), Transharp.GetTranslation("Box_Error"), MessageBoxButton.OK);
            }
            finally
            {
                var nbMerchandise = SecondListMerchandise.Count;
                for(var i = 0; i < nbMerchandise; i++)
                {
                    if(SecondListMerchandise[i].ToString() != id)
                    {
                        continue;
                    }
                    SecondListMerchandise[i].onSale = !SecondListMerchandise[i].onSale;
                }
                SelectMarchandiseLike(String.Empty);
            }
        }
    }
}