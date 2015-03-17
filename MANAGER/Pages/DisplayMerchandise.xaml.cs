// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

#region

using System;
using System.Collections.Generic;
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
                var newMerchandise = new Merchandise(id, text, ListMerchandiseN2[i].quantite, ListMerchandiseN2[i].price, ListMerchandiseN2[i].categoryID)
                {
                    onSale = ListMerchandiseN2[i].onSale
                };

                Display(text, newMerchandise);
            }
        }

        private void TextBoxDevisQte_TextChanged(object sender, TextChangedEventArgs e)
        {
            SelectMarchandiseLike(TextBoxDevisQte.Text == "" ? "" : TextBoxDevisQte.Text);
        }

        private void Display(string text, Merchandise newMerchandise)
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
            panelMerchandise.Children.Add(new TextBlock {Text = text, Margin = thick, Height = 16});

            // Prix
            panelMerchandise.Children.Add(new TextBlock {Text = string.Format("{0}€", newMerchandise.price), Margin = thick, Height = 16});

            // Quantité
            panelMerchandise.Children.Add(new TextBlock
            {
                Text = string.Format(Localisation.Localisation.DM_Stock, newMerchandise.price),
                Margin = new Thickness(5, 2, 0, 0),
                Height = 16
            });

            var BTN_Delete = new Button
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = newMerchandise.onSale ? Localisation.Localisation.DM_OnSale : Localisation.Localisation.DM_NotOnSale,
                Margin = new Thickness(9, -30, 67, 50),
                BorderBrush =
                    newMerchandise.onSale ? new SolidColorBrush(Color.FromRgb(0x7c, 0xfc, 0x00)) : new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00)),
                Tag = newMerchandise.id
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

            const string query = "SELECT * FROM MARCHANDISE";
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
                        Convert.ToInt32(resultat[5])) {onSale = Convert.ToBoolean(resultat[4])};
                    Display(text, newMerchandise);
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
            var id = ((Button) sender).Tag.ToString();
            try
            {
                var commandeModif = new OracleCommand("UPDATE MARCHANDISE SET ENVENTE=0 WHERE ID_MARCHANDISE=:ID_MARCHANDISE");
                commandeModif.Parameters.Add(new OracleParameter(":ID_MARCHANDISE", OracleDbType.Int32) {Value = Convert.ToInt32(id)});
                //commandeModif.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show(Localisation.Localisation.Box_DBFail, Localisation.Localisation.Box_Error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                var nbMerchandise = ListMerchandiseN2.Count;

                for(var i = 0; i < nbMerchandise; i++)
                {
                    if(ListMerchandiseN2[i].ToString() != id)
                    {
                        continue;
                    }
                    ListMerchandiseN2[i].onSale = !ListMerchandiseN2[i].onSale;
                    /*
                    ListMerchandiseN2.Remove(ListMerchandiseN2[i]);
                    nbMerchandise -= 1;*/
                }
                SelectMarchandiseLike("");
            }
        }
    }
}