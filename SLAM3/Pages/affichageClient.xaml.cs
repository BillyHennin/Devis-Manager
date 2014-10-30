// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SLAM3.Classes;
using SLAM3.Properties;

namespace SLAM3.Pages
{
    /// <summary>
    ///     Logique d'interaction pour affichageClient.xaml
    /// </summary>
    public partial class AffichageClient
    {
        private static readonly List<Marchandise> ListMarchandise = new List<Marchandise>();
        private readonly Devis _leDevis = new Devis(ListMarchandise);

        public AffichageClient()
        {
            InitializeComponent();
        }

        private void ComboBoxClient_OnInitialized(object sender, EventArgs e)
        {
            /*
             *
             * TODO : Connexion BDD Oracle
             * 
             */

            var db = new SqlCeConnection(Settings.Default.DatabaseConnectionString);
            const string query = "SELECT * FROM CLIENT";
            db.Open();
            try{
                var oCommand = new SqlCeCommand { Connection = db, CommandText = query };
                var resultat = oCommand.ExecuteReader();
            while (resultat.Read())
            {
                ComboBoxClient.Items.Add(new ComboboxItemClient
                {
                    Text = query[0].ToString(CultureInfo.InvariantCulture),
                    Value = new Client(query[0].ToString(CultureInfo.InvariantCulture),
                                            query[1].ToString(CultureInfo.InvariantCulture),
                                                query[2].ToString(CultureInfo.InvariantCulture))
                });
                ComboBoxClient.SelectedIndex = 0;
            }
            resultat.Close();
            }catch (Exception caught){
                        Console.WriteLine(caught.Message);
                        Console.Read();
            }finally{
                        db.Close();
            }
        }

        private void ComboBoxClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // garder ça
            ComboBoxDevis.Items.Clear();
            // oui
            //TODO : liaison ddb
            var db = new SqlCeConnection(Settings.Default.DatabaseConnectionString);
            var query = "SELECT Id FROM DEVIS WHERE Client =" + (ComboBoxClient.SelectedItem as ComboboxItemClient).Value.GetDenomination;
            db.Open();
            try
            {
                var oCommand = new SqlCeCommand { Connection = db, CommandText = query };
                var resultat = oCommand.ExecuteReader();
                while (resultat.Read())
                {
                    ComboBoxDevis.Items.Add(new ComboboxItemDevis
                    {
                        Text = "Devis n°" + query[0].ToString(CultureInfo.InvariantCulture),
                        Value = query[0]
                    });
                }
                resultat.Close();
            }
            catch (Exception caught)
            {
                Console.WriteLine(caught.Message);
                Console.Read();
            }
            finally
            {
                db.Close();
            }
            ComboBoxClient.SelectedIndex = 0;
            PanelDevis.Children.Clear();
        }

        private void ComboBoxDevis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // garder ça
            PanelDevis.Children.Clear();
            ListMarchandise.Clear();
            // oui
            var db = new SqlCeConnection(Settings.Default.DatabaseConnectionString);
            var query = "SELECT Marchandise FROM DEVIS WHERE Client ="
                + (ComboBoxClient.SelectedItem as ComboboxItemClient).Value.GetDenomination 
                + " AND Id = "
                + (ComboBoxDevis.SelectedItem as ComboboxItemDevis).Value;
            db.Open();
            try
            {
                var oCommand = new SqlCeCommand { Connection = db, CommandText = query };
                var resultat = oCommand.ExecuteReader();
                while (resultat.Read())
                {

                    var query2 = "SELECT MARCHANDISE.* FROM DEVIS, MARCHANDISE WHERE Id ="
                        + (ComboBoxDevis.SelectedItem as ComboboxItemDevis).Value
                        + "AND Marchandise = "
                        + query[0].ToString(CultureInfo.InvariantCulture);
                    var oCommand2 = new SqlCeCommand { Connection = db, CommandText = query2 };
                    var resultat2 = oCommand2.ExecuteReader();
                    while (resultat2.Read())
                    {
                        var text = query2[0].ToString(CultureInfo.InvariantCulture);
                        var qte = Convert.ToInt32(query2[1]);
                        var prixMarchandise = Convert.ToInt32(query2[2]);
                        var item = new Marchandise(text, qte, prixMarchandise);


                        var panelMarchandise = new StackPanel();
                        var thick = new Thickness(5, 2, 0, 0);
                        var bordure = new Border
                        {
                            BorderBrush = ComboBoxClient.BorderBrush,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            Margin = new Thickness(2, 2, 1, 0),
                            BorderThickness = new Thickness(1),
                            Width = BorderDevis.Width - 5,
                            Child = panelMarchandise,
                            Height = 70
                        };

                        item.Bordure = bordure;
                        PanelDevis.Children.Add(bordure);

                        // Nom du produit
                        panelMarchandise.Children.Add(new TextBlock
                        {
                            Margin = thick,
                            Text = text,
                            Height = 16
                        });

                        // Prix
                        panelMarchandise.Children.Add(new TextBlock
                        {
                            Text = qte.ToString(CultureInfo.InvariantCulture),
                            Margin = thick,
                            Height = 16
                        });

                        // Quantité
                        panelMarchandise.Children.Add(new TextBlock
                        {
                            Text = prixMarchandise.ToString(CultureInfo.InvariantCulture),
                            Margin = thick,
                            Height = 16
                        });
                    }                    
                }
                resultat.Close();
            }
            catch (Exception caught)
            {
                Console.WriteLine(caught.Message);
                Console.Read();
            }
            finally
            {
                db.Close();
            }
            ComboBoxClient.SelectedIndex = 0;
           
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBoxClient.BorderBrush =
                BorderDevis.BorderBrush =
                    ComboBoxDevis.BorderBrush =
                        new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor));
        }

        private void MenuClient_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderDevis.Width = MenuClient.ActualWidth - 40;
            BorderDevis.Height = MenuClient.ActualHeight - 70;
            try
            {
                var nbMarchandise = _leDevis.GetList.Count;
                for (var i = 0; i < nbMarchandise; i++)
                {
                    ListMarchandise[i].Bordure.Width = BorderDevis.Width - 5;
                }
            }// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }
    }
}