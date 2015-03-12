// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using MANAGER.Classes;
using MANAGER.Connection;

using Oracle.ManagedDataAccess.Client;

#endregion

namespace MANAGER.Pages
{
    /// <summary>
    ///   Logique d'interaction pour DisplayMerchandise.xaml
    /// </summary>
    public partial class DisplayMerchandise
    {
        /// <summary>
        ///   A list of all merchandise that are buyable
        /// </summary>
        private static readonly List<Merchandise> ListMerchandise = new List<Merchandise>();

        /// <summary>
        ///   A second list, for future use.
        /// </summary>
        private static readonly List<Merchandise> ListMerchandiseN2 = new List<Merchandise>();

        /// <summary>
        ///   When a user select a specific marchandise clear everything, then recreate it with what the user wants to see.
        /// </summary>
        /// <param name="merchandise"></param>
        private void SelectMarchandiseLike(string merchandise)
        {
            PanelProduit.Children.Clear();
            ListMerchandise.Clear();

            var nbMerchandise = ListMerchandiseN2.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                if(!ListMerchandiseN2[i].nom.ToLower().Contains(merchandise.ToLower()))
                {
                    continue;
                }
                var id = ListMerchandiseN2[i].id;
                var text = ListMerchandiseN2[i].nom;
                var qte = string.Format(Localisation.Localisation.DM_Stock, ListMerchandiseN2[i].quantite);
                var prixMerchandise = string.Format("{0}€", ListMerchandiseN2[i].price);
                var newMerchandise = new Merchandise(id, text, ListMerchandiseN2[i].quantite, ListMerchandiseN2[i].price, ListMerchandiseN2[i].categoryID);

                Display(text, qte, prixMerchandise, newMerchandise);
            }
        }

        private void TextBoxDevisQte_TextChanged(object sender, TextChangedEventArgs e)
        {
            SelectMarchandiseLike(TextBoxDevisQte.Text == "" ? "" : TextBoxDevisQte.Text);
        }

        private void Display(string text, string qte, string prixMerchandise, Merchandise newMerchandise)
        {
            var panelMerchandise = new StackPanel();
            var thick = new Thickness(5, 2, 0, 0);

            //new border
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

            PanelProduit.Children.Add(border);

            // Nom du produit
            panelMerchandise.Children.Add(new TextBlock {Margin = thick, Text = text, Height = 16});

            // Prix
            panelMerchandise.Children.Add(new TextBlock {Text = qte.ToString(CultureInfo.InvariantCulture), Margin = thick, Height = 16});

            // Quantité
            panelMerchandise.Children.Add(new TextBlock
            {
                Text = prixMerchandise.ToString(CultureInfo.InvariantCulture),
                Margin = new Thickness(5, 2, 0, 0),
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

            // Button deleting
            panelMerchandise.Children.Add(BTN_Delete);

            BTN_Delete.Click += bouton_Click;

            newMerchandise.Border = border;
            ListMerchandise.Add(newMerchandise);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PanelProduit.Children.Clear();
            ListMerchandise.Clear();
            ListMerchandiseN2.Clear();

            const string query = "SELECT * FROM MARCHANDISE WHERE ENVENTE = 1";
            try
            {
                var oCommand = ConnectionOracle.OracleCommand(query);
                var resultat = oCommand.ExecuteReader();
                while(resultat.Read())
                {
                    var category = string.Empty;
                    var CommandCategory = ConnectionOracle.OracleCommand("SELECT LIBELLE FROM CATEGORIE WHERE ID_CATEGORIE=:ID_CATEGORIE");
                    CommandCategory.Parameters.Add(new OracleParameter(":ID_CATEGORIE", OracleDbType.Int32) {Value = Convert.ToInt32(resultat[5])});
                    var resultatCategory = CommandCategory.ExecuteReader();
                    while(resultatCategory.Read())
                    {
                        category = resultatCategory[0].ToString();
                    }
                    var text = string.Format("{0} - {1}", category, resultat[1].ToString());
                    var newMerchandise = new Merchandise(Convert.ToInt32(resultat[0]), text, Convert.ToInt32(resultat[3]), Convert.ToInt32(resultat[2]),
                        Convert.ToInt32(resultat[5]));

                    var price = string.Format("Quantity in stock : {0}", resultat[3]);
                    var qte = string.Format("{0}€", resultat[2]);
                    Display(text, qte, price, newMerchandise);
                    ListMerchandiseN2.Add(newMerchandise);
                }
                resultat.Close();
            }
            catch
            {
                MessageBox.Show(Localisation.Localisation.Box_DBFail, Localisation.Localisation.Box_Error, MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void bouton_Click(object sender, EventArgs e)
        {
            var commandeModif = new OracleCommand {CommandType = CommandType.StoredProcedure, CommandText = "DELETEPRODUIT"};
            var id = ((Button) sender).Tag.ToString();

            var paramID = new OracleParameter(":1", OracleDbType.Int32) {Value = id};
            commandeModif.Parameters.Add(paramID);

            try
            {
                commandeModif.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show(Localisation.Localisation.Box_DBFail, Localisation.Localisation.Box_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            var nbMerchandise = ListMerchandiseN2.Count;
            for(var i = 0; i < nbMerchandise; i++)
            {
                if(ListMerchandiseN2[i].ToString() == id)
                {
                    ListMerchandiseN2.Remove(ListMerchandiseN2[i]);
                }
            }
            SelectMarchandiseLike("");
        }
    }
}