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
using FirstFloor.ModernUI.Windows.Controls;
using MANAGER.Classes;
using MANAGER.Classes.Table;
using MANAGER.ComboBox;
using MANAGER.Table;

namespace MANAGER.Pages.Customer
{
    /// <summary>
    ///   Logique d'interaction pour DisplayCustomer.xaml
    /// </summary>
    public partial class DisplayCustomer
    {
        private static readonly List<Classes.Merchandise> ListMerchandise = new List<Classes.Merchandise>();
        private readonly Estimate _estimate = new Estimate(ListMerchandise);

        private void ComboBoxCustomer_Loaded(object sender, EventArgs e)
        {
            //Traduction
            DC_ChooseCustomer.Text = Transharp.GetTranslation("DC_ChooseCustomer");
            LabelEstimate.Text = Transharp.GetTranslation("DC_CustomerCE");
            LabelCommand.Text = Transharp.GetTranslation("DC_CustomerCM");
            LabelPhone.Text = Transharp.GetTranslation("DC_LabelPhone");
            LabelMail.Text = Transharp.GetTranslation("DC_labelMail");
            BTN_Delete.Content = Transharp.GetTranslation("DC_DeleteCustomer");
            BTN_Update.Content = Transharp.GetTranslation("DC_UpdateCustomer");

            //Default Visibility 

            PanelDevis.Children.Clear();
            ChangeVisibility(false);
            try
            {
                ComboBoxCustomer.Items.Clear();
                InitComboClient();
            }
            catch(Exception caught)
            {
                ModernDialog.ShowMessage(caught.Message, Transharp.GetTranslation("Box_Error"), MessageBoxButton.OK);
            }
        }

        private void ComboBoxCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Besoin d'avoir cette ligne car l'event est fire au changement de page
            if ((ComboboxItemCustomer) ComboBoxCustomer.SelectedItem == null) { return; }

            var selectedCustomer = ((ComboboxItemCustomer) ComboBoxCustomer.SelectedItem).Value;
            ComboBoxEstimate.Items.Clear();
            PanelDevis.Children.Clear();

            var totalPrice = 0;
            var nbEstimate = 1;
            var date = DateTime.Now;
            try
            {
                // Récupération de tous les devis fais par le client séléctionné
                var param = new Dictionary<string, object> {{ "idClient", selectedCustomer.Id }};
                var command = Classes.Connection.Connection.CommandStored("GetAllEstimateOfCustomer", param);
                var resultCommand = command.ExecuteReader();

                while(resultCommand.Read())
                {
                    var query2 =
                        string.Format(
                            "SELECT {0}.ID_{0}, {0}.{1}, {2}.{3}, {2}.{4}, {2}.{5}, {0}.ID_{7} FROM {0}, {2} WHERE {2}.ID_{0} = {0}.ID_{0} AND {2}.{6} = {8}",
                            SQL_Merchandise.TableName, SQL_Merchandise.Name, SQL_Estimate.TableName, SQL_Estimate.PriceMerchandise,
                            SQL_Estimate.Quantity, SQL_Estimate.Day, SQL_Estimate.NumberDevis, SQL_Category.TableName, resultCommand[0]);
                    var command2 = Classes.Connection.Connection.Command(query2);
                    var resultatMerchandise = command2.ExecuteReader();
                    var listMerchandise2 = new List<Classes.Merchandise>();
                    while(resultatMerchandise.Read())
                    {
                        totalPrice += Convert.ToInt32(resultatMerchandise[2]);
                        date = Convert.ToDateTime(resultatMerchandise[4]);
                        var merchandise = new Classes.Merchandise(Convert.ToInt32(resultatMerchandise[0]), resultatMerchandise[1].ToString(),
                            Convert.ToInt32(resultatMerchandise[3]), Convert.ToInt32(resultatMerchandise[2]) / Convert.ToInt32(resultatMerchandise[3]), Convert.ToInt32(resultatMerchandise[5]));
                        listMerchandise2.Add(merchandise);
                    }
                    resultatMerchandise.Close();
                    var estimate2 = new Estimate(listMerchandise2) {TotalPrice = totalPrice, Date = date};
                    ComboBoxEstimate.Items.Add(new ComboboxItemEstimate
                    {
                        Text = Transharp.GetTranslation("DC_ComboBoxCustomer", nbEstimate, date.ToShortDateString(), totalPrice),
                        Value = estimate2
                    });
                    nbEstimate++;
                    totalPrice = 0;
                }
                resultCommand.Close();
                // Maj de l'interface
                TextMail.Text = selectedCustomer.Email;
                TextPhone.Text = selectedCustomer.Phone;
            }
            catch(Exception caught)
            {
                ModernDialog.ShowMessage(caught.Message, Transharp.GetTranslation("Box_Error"), MessageBoxButton.OK);
            }
            finally
            {
                ChangeVisibility(true);
                PanelDevis.Children.Clear();
                var panelMerchandise = new StackPanel();
                // New border
                var border = new Border
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(2, 2, 1, 0),
                    BorderThickness = new Thickness(1),
                    Width = BorderDevis.Width - 5,
                    Child = panelMerchandise,
                    Height = 100
                };

                PanelDevis.Children.Add(border);

                // Merchandise's name
                panelMerchandise.Children.Add(new TextBlock
                {
                    Text = Transharp.GetTranslation("DM_SearchNull"),
                    Margin = new Thickness(5, 2, 0, 0),
                    Height = 40,
                    TextAlignment = TextAlignment.Center
                });
            }
        }

        private void InitComboClient()
        {
            try
            {
                var resultat = Classes.Connection.Connection.GetAll(SQL_Customer.TableName).ExecuteReader();
                while(resultat.Read())
                {
                    ComboBoxCustomer.Items.Add(new ComboboxItemCustomer
                    {
                        Text = resultat[2].ToString(),
                        Value = new Classes.Customer(Convert.ToInt32(resultat[0]), resultat[2].ToString(), resultat[3].ToString(), resultat[1].ToString())
                    });
                }
                resultat.Close();
            }
            catch
            {
                ModernDialog.ShowMessage(Transharp.GetTranslation("Box_DBFail"), Transharp.GetTranslation("Box_Error"), MessageBoxButton.OK);
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
                var query = String.Format("SELECT {1} FROM {0} WHERE ID_{0} = {2}", SQL_Category.TableName, SQL_Category.Title, listMarchandise[i].CategoryId);
                var commandCategory = Classes.Connection.Connection.Command(query);
                var resultatCategory = commandCategory.ExecuteReader();
                while(resultatCategory.Read())
                {
                    categoryString = resultatCategory[0].ToString();
                }
                var text = $"{categoryString} - {listMarchandise[i].Name}";
                var qte = listMarchandise[i].Quantity;
                var prixMarchandise = listMarchandise[i].Price;
                var category = listMarchandise[i].CategoryId;
                var item = new Classes.Merchandise(listMarchandise[i].Id, text, qte, prixMarchandise, category);
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
                    Text = $"{prixMarchandise.ToString(CultureInfo.InvariantCulture)}€",
                    Margin = thick,
                    Height = 16
                });

                item.Border = bordure;
                PanelDevis.Children.Add(bordure);
                _estimate.GetList.Add(item);
            }
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
                var nbMarchandise = _estimate.GetList.Count;
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
            var nbMerchandise = _estimate.GetList.Count;

            if(nbMerchandise == 0)
            {
                return;
            }

            for(var i = 0; i < nbMerchandise; i++)
            {
                _estimate[i].Border.BorderBrush = BorderDevis.BorderBrush;
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Classes.Connection.Connection.Delete(SQL_Estimate.TableName, ((ComboboxItemCustomer) ComboBoxCustomer.SelectedItem).Value.Id, SQL_Customer.TableName);
                Classes.Connection.Connection.Delete(SQL_Customer.TableName, ((ComboboxItemCustomer) ComboBoxCustomer.SelectedItem).Value.Id);
            }
            catch
            {
                ModernDialog.ShowMessage(Transharp.GetTranslation("Box_DBFail"), Transharp.GetTranslation("Box_Error"), MessageBoxButton.OK);
            }

            ComboBoxCustomer.Items.Clear();
            InitComboClient();
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            var customer = ((ComboboxItemCustomer) ComboBoxCustomer.SelectedItem).Value;
            try
            {
                Classes.Connection.Connection.Update(SQL_Customer.TableName, customer.Id, new[,] {{ SQL_Customer.Phone, TextPhone.Text }, { SQL_Customer.Email, TextMail.Text }});
            }
            catch
            {
                ModernDialog.ShowMessage(Transharp.GetTranslation("Box_DBFail"), Transharp.GetTranslation("Box_Error"), MessageBoxButton.OK);
            }
            finally
            {
                customer.Phone = TextPhone.Text;
                customer.Email = TextMail.Text;
                ModernDialog.ShowMessage(Transharp.GetTranslation("Box_SuccessUpdate", customer.Name), Transharp.GetTranslation("Box_Update_Success_Title"),
                    MessageBoxButton.OK);
            }
        }

        private void ComboBoxCommands_SelectionChanged(object sender, SelectionChangedEventArgs e) {}

        private void ChangeVisibility(bool visibility)
        {
            PanelClientEstimate.Visibility = BorderDevis.Visibility = LabelPhone.Visibility = LabelMail.Visibility =
                TextPhone.Visibility = TextMail.Visibility = BTN_Delete.Visibility = BTN_Update.Visibility = visibility ? Visibility.Visible : Visibility.Hidden;
        }
    }
}