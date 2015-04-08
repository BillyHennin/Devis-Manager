// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

#region

//using System.Windows;
using System;
using System.Windows;

using MANAGER.Classes;

using Customer = MANAGER.Table.Customer;
using Estimate = MANAGER.Table.Estimate;
using Merchandise = MANAGER.Table.Merchandise;

#endregion

namespace MANAGER
{
    /// <summary>
    ///   Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private void ModernWindow_Initialized(object sender, EventArgs e)
        {
            //Initialisation de la bdd
            Properties.Connection.Default.DatabaseConnectionString = "user id=SLAM3;password=pw;data source=localhost:1521/xe";

            //if(Properties.Connection.Default.IsDatabaseSet) {}
            //ContentSource = new Uri(@"/Pages/Parametre.xaml", UriKind.Relative);
            //MessageBox.Show(Localisation.Localisation.Box_DBFail, Localisation.Localisation.Box_Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ModernWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Link_About.DisplayName = Transharp.GetTranslation("Link_About");
            Link_Apparence.DisplayName = Transharp.GetTranslation("Link_Apparence");
            Link_Param.DisplayName = Transharp.GetTranslation("Link_Param");
            Link_CE.DisplayName = Transharp.GetTranslation("Link_CE");
            Link_CreateCE.DisplayName = Transharp.GetTranslation("Link_CreateCE");
            Link_DisplayCE.DisplayName = Transharp.GetTranslation("Link_DisplayCE");
            Link_Customer.DisplayName = Transharp.GetTranslation("Link_Customer");
            Link_DisplayCustomer.DisplayName = Transharp.GetTranslation("Link_DisplayCustomer");
            Link_CreateCustomer.DisplayName = Transharp.GetTranslation("Link_CreateCustomer");
            Link_Merchandise.DisplayName = Transharp.GetTranslation("Link_Merchandise");
            Link_DisplayMerchandise.DisplayName = Transharp.GetTranslation("Link_DisplayMerchandise");
            Link_CreateMerchandise.DisplayName = Transharp.GetTranslation("Link_CreateMerchandise");

            //

            Merchandise.TableName = "MARCHANDISE";
            Merchandise.ID = String.Format("ID_{0}", Merchandise.TableName);
            Merchandise.Name = "NOM";
            Merchandise.OnSale = "ENVENTE";
            Merchandise.Price = "PRIX";
            Merchandise.Quantity = "QUANTITE";

            Customer.TableName = "CLIENT";
            Customer.ID = String.Format("ID_{0}", Customer.TableName);
            Customer.Email = "EMAIL";
            Customer.Name = "DENOMINATION";
            Customer.Phone = "TELEPHONE";
            Customer.Code = "CODE";

            Estimate.TableName = "DEVIS";
            Estimate.Day = "JOUR";
            Estimate.NumberDevis = "NUMERODEVIS";
            Estimate.PriceMerchandise = "PRIXMARCHANDISE";
            Estimate.Quantity = "QUANTITE";
        }
    }
}