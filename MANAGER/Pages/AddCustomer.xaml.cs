// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using FirstFloor.ModernUI.Windows.Controls;

using MANAGER.Classes;

using Estimate = MANAGER.Table.Estimate;

namespace MANAGER.Pages
{
    public partial class AddCustomer
    {
        private static readonly List<Customer> ListCustomer = new List<Customer>();

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderCustomer.Width = CustomerCreator.ActualWidth - 340;
            BorderCustomer.Height = CustomerCreator.ActualHeight - 70;

            var nbCustomer = ListCustomer.Count;
            for(var i = 0; i < nbCustomer; i++)
            {
                ListCustomer[i].Border.Width = BorderCustomer.Width - 6;
            }
        }

        private void CustomerCreator_Loaded(object sender, RoutedEventArgs e)
        {
            //Trad
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
            var command = Connection.Connection.GetAll(Table.Customer.TableName);
            var resultat = command.ExecuteReader();
            while(resultat.Read())
            {
                showCustomer(Convert.ToInt32(resultat[Table.Customer.ID]), resultat[Table.Customer.Name].ToString(), resultat[Table.Customer.Phone].ToString(),
                    resultat[Table.Customer.Email].ToString());
            }
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

        private static Boolean isInt(string str)
        {
            int value;
            return (str.Trim() != string.Empty) && int.TryParse(str, out value);
        }

        private static Boolean validMail(string mailString)
        {
            try
            {
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
            BtnAdd.IsEnabled = TextBoxMail.Text != String.Empty && validMail(TextBoxMail.Text) && TextBoxName.Text != String.Empty && isInt(TextBoxPhone.Text);
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var queryVerify = String.Format("SELECT COUNT(*) FROM {0} WHERE {1}='{2}' OR {3}='{4}'", Table.Customer.TableName, Table.Customer.Phone,
                    TextBoxPhone.Text, Table.Customer.Email, TextBoxMail.Text);
                if(Convert.ToInt32(Connection.Connection.GetUniqueCell(queryVerify)) == 0)
                {
                    var querySelect = String.Format("SELECT max(ID_{0}) FROM {0}", Table.Customer.TableName);
                    var Command = Connection.Connection.GetUniqueCell(querySelect);
                    var idCustomer = Command.ToString() == String.Empty ? 1 : Convert.ToInt32(Command) + 1;
                    Connection.Connection.Insert(Table.Customer.TableName, idCustomer, TextBoxMail.Text, TextBoxName.Text, TextBoxPhone.Text);
                    ModernDialog.ShowMessage(Transharp.GetTranslation("Box_SuccessAddCustomer", TextBoxName.Text), Transharp.GetTranslation("Box_AC_Success"),
                        MessageBoxButton.OK);
                    showCustomer(idCustomer, TextBoxMail.Text, TextBoxName.Text, TextBoxPhone.Text);
                    TextBoxMail.Text = TextBoxPhone.Text = TextBoxName.Text = String.Empty;
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

        private void showCustomer(int ID, string Mail, string Name, string Phone)
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
            panelCustomer.Children.Add(new TextBlock {Margin = thick, Text = Name, Height = 16});

            // Mail
            panelCustomer.Children.Add(new TextBlock {Text = Mail, Margin = thick, Height = 16});

            // Phone
            panelCustomer.Children.Add(new TextBlock {Text = Phone, Margin = thick, Height = 16});

            // Button
            var BTN_Delete = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = Transharp.GetTranslation("DC_DeleteCustomer"),
                Margin = new Thickness(9, -30, 67, 50),
                BorderBrush = Brushes.Red,
                Tag = ID
            };

            panelCustomer.Children.Add(BTN_Delete);
            BTN_Delete.Click += BTN_Delete_Click;
            var newCustomer = new Customer(ID, Name, Phone, Mail);
            PanelCustomer.Children.Add(border);
            newCustomer.Border = border;
            ListCustomer.Add(newCustomer);
        }

        private void BTN_Delete_Click(object sender, EventArgs e)
        {
            var ID = ((Button) sender).Tag.ToString();
            var query = String.Format("SELECT {0} FROM {1} WHERE ID_{1} = {2}", Table.Customer.Name, Table.Customer.TableName, ID);
            var name = Connection.Connection.GetUniqueCell(query);
            if(ModernDialog.ShowMessage(Transharp.GetTranslation("Box_DeleteCustomer", name), Transharp.GetTranslation("Box_AskDelete"), MessageBoxButton.YesNo)
               != MessageBoxResult.Yes)
            {
                return;
            }
            Connection.Connection.Delete(Estimate.TableName, ID, Table.Customer.TableName);
            Connection.Connection.Delete(Table.Customer.TableName, ID);
            DisplayAll();
        }
    }
}