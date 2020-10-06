// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

#region Using

using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FirstFloor.ModernUI.Windows.Controls;
using MANAGER.Classes;
using MANAGER.Classes.Table;
using MANAGER.Table;

#endregion

namespace MANAGER.Pages.Merchandise
{
    public partial class AddMerchandise
    {
        private static readonly List<Classes.Merchandise> ListMerchandise = new List<Classes.Merchandise>();

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderCustomer.Width = CustomerCreator.ActualWidth - 340;
            BorderCustomer.Height = CustomerCreator.ActualHeight - 70;

            var nbCustomer = ListMerchandise.Count;
            for(var i = 0; i < nbCustomer; i++)
            {
                ListMerchandise[i].Border.Width = BorderCustomer.Width - 6;
            }
        }

        private void CustomerCreator_Loaded(object sender, RoutedEventArgs e)
        {
            //Traduction
            AC_Title.Text = Transharp.GetTranslation("AC_Title");
            AC_AddCustomer.Text = Transharp.GetTranslation("AC_AddCustomer");
            AC_AddMail.Text = Transharp.GetTranslation("AC_AddMail");
            AC_AddName.Text = Transharp.GetTranslation("AC_AddName");
            AC_AddPhone.Text = Transharp.GetTranslation("AC_AddPhone");
            BtnAdd.Content = Transharp.GetTranslation("BTN_Add");

            //
            DisplayAll();
        }

        private void DisplayAll()
        {
            PanelCustomer.Children.Clear();
            var command = Classes.Connection.Connection.GetAll(SQL_Customer.TableName);
            var resultat = command.ExecuteReader();
            while(resultat.Read())
            {
                ShowCustomer(Convert.ToInt32(resultat[SQL_Customer.ID]), resultat[SQL_Customer.Name].ToString(), resultat[SQL_Customer.Phone].ToString(), resultat[SQL_Customer.Email].ToString());
            }
            resultat.Close();
        }

        private void TextBoxMail_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged();
        }

        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged();
        }

        private void TextBoxPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged();
        }

        private static bool IsInt(string str) => str.Trim() != string.Empty && int.TryParse(str, out _);

        private static bool ValidMail(string mailString)
        {
            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new MailAddress(mailString);
                return true;
            }
            catch(FormatException)
            {
                return false;
            }
        }

        private void TextChanged()
        {
            BtnAdd.IsEnabled = TextBoxMail.Text != string.Empty && ValidMail(TextBoxMail.Text) && TextBoxName.Text != String.Empty && IsInt(TextBoxPhone.Text);
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var queryVerify = $"{SQL_Customer.TableName} WHERE {SQL_Customer.Phone}='{TextBoxPhone.Text}' OR {SQL_Customer.Email}='{TextBoxMail.Text}'";
                if(Classes.Connection.Connection.SizeOf(Classes.Connection.Connection.GetAll(queryVerify)) == 0)
                {
                    var querySelect = string.Format("SELECT max(ID_{0}) FROM {0}", SQL_Customer.TableName);
                    var command = Classes.Connection.Connection.GetUniqueCell(querySelect);
                    var idCustomer = command.ToString() == string.Empty ? 1 : Convert.ToInt32(command) + 1;
                    Classes.Connection.Connection.Insert(SQL_Customer.TableName, idCustomer, TextBoxMail.Text, TextBoxName.Text, TextBoxPhone.Text);
                    ModernDialog.ShowMessage(Transharp.GetTranslation("Box_SuccessAddCustomer", TextBoxName.Text), Transharp.GetTranslation("Box_AC_Success"),
                        MessageBoxButton.OK);
                    ShowCustomer(idCustomer, TextBoxMail.Text, TextBoxName.Text, TextBoxPhone.Text);
                    TextBoxMail.Text = TextBoxPhone.Text = TextBoxName.Text = string.Empty;
                }
                else
                {
                    ModernDialog.ShowMessage(Transharp.GetTranslation("Box_CustomerAlreadyExist"), Transharp.GetTranslation("Box_Error"), MessageBoxButton.OK);
                }
            }
            catch
            {
                ModernDialog.ShowMessage(Transharp.GetTranslation("Box_DBFail"), Transharp.GetTranslation("Box_Error"), MessageBoxButton.OK);
            }
        }

        private void ShowCustomer(int id, string mail, string name, string phone)
        {
            var panelCustomer = new StackPanel();
            var thick = new Thickness(5, 2, 0, 0);

            // New border
            var border = new Border
            {
                BorderBrush = BtnAdd.BorderBrush,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(2, 2, 1, 0),
                BorderThickness = new Thickness(1),
                Width = BorderCustomer.Width - 5,
                Child = panelCustomer,
                Height = 70
            };

            // Customer's name
            panelCustomer.Children.Add(new TextBlock {Margin = thick, Text = name, Height = 16});

            // Mail
            panelCustomer.Children.Add(new TextBlock {Text = mail, Margin = thick, Height = 16});

            // Phone
            panelCustomer.Children.Add(new TextBlock {Text = phone, Margin = thick, Height = 16});

            // Button
            var btnDelete = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = Transharp.GetTranslation("DC_DeleteCustomer"),
                Margin = new Thickness(9, -30, 67, 50),
                BorderBrush = Brushes.Red,
                Tag = id
            };

            panelCustomer.Children.Add(btnDelete);
            btnDelete.Click += BTN_Delete_Click;
            var newCustomer = new Classes.Customer(id, name, phone, mail);
            PanelCustomer.Children.Add(border);
            newCustomer.Border = border;
            ListMerchandise.Add(newCustomer);
        }

        private void BTN_Delete_Click(object sender, EventArgs e)
        {
            var id = ((Button) sender).Tag.ToString();
            var query = string.Format("SELECT {0} FROM {1} WHERE ID_{1} = {2}", SQL_Customer.Name, SQL_Customer.TableName, id);
            var name = Classes.Connection.Connection.GetUniqueCell(query);
            if(ModernDialog.ShowMessage(Transharp.GetTranslation("Box_DeleteCustomer", name), Transharp.GetTranslation("Box_AskDelete"), MessageBoxButton.YesNo)
               != MessageBoxResult.Yes)
            {
                return;
            }
            Classes.Connection.Connection.Delete(SQL_Estimate.TableName, id, SQL_Customer.TableName);
            Classes.Connection.Connection.Delete(SQL_Customer.TableName, id);
            DisplayAll();
        }
    }
}