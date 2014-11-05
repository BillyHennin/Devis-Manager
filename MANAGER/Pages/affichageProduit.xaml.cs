// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

using MANAGER.Classes;
using MANAGER.Properties;

namespace MANAGER.Pages
{
    /// <summary>
    ///   Logique d'interaction pour affichageProduit.xaml
    /// </summary>
    public partial class AffichageProduit
    {
        private static List<Marchandise> ListMarchandise = new List<Marchandise>();
        private static readonly List<Marchandise> ListMarchandiseN2 = new List<Marchandise>();

        public AffichageProduit()
        {
            InitializeComponent();
        }

        private void TextBoxDevisQte_TextChanged(object sender, TextChangedEventArgs e)
        {
            PanelProduit.Children.Clear();

            if(TextBoxDevisQte.Text == "")
            {
                var taille = ListMarchandise.Count;

                for(var i = 0; i < taille; i++)
                {
                    var text = ListMarchandise[i].GetNom;
                    var qte = ListMarchandise[i].GetQte;
                    var prixMarchandise = ListMarchandise[i].GetPrix;
                    var nouvelleMarchandise = new Marchandise(text, qte, prixMarchandise);
                    var panelMarchandise = new StackPanel();
                    var thick = new Thickness(5, 2, 0, 0);

                    //nouvelle bordure
                    var bordure = new Border
                    {
                        BorderBrush = BorderDevis.BorderBrush,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(2, 2, 1, 0),
                        BorderThickness = new Thickness(1),
                        Width = BorderDevis.Width - 5,
                        Child = panelMarchandise,
                        Height = 70
                    };

                    PanelProduit.Children.Add(bordure);

                    // Nom du produit
                    panelMarchandise.Children.Add(new TextBlock {Margin = thick, Text = text, Height = 16});

                    // Prix
                    panelMarchandise.Children.Add(new TextBlock {Text = qte.ToString(CultureInfo.InvariantCulture), Margin = thick, Height = 16});

                    // Quantité
                    panelMarchandise.Children.Add(new TextBlock
                    {
                        Text = prixMarchandise.ToString(CultureInfo.InvariantCulture),
                        Margin = new Thickness(5, 2, 0, 0),
                        Height = 16
                    });

                    nouvelleMarchandise.Bordure = bordure;
                    ListMarchandiseN2.Add(nouvelleMarchandise);
                }
                ListMarchandise = ListMarchandiseN2;
            }
            else
            {
                PanelProduit.Children.Clear();
                ListMarchandise.Clear();

                var db = new SqlCeConnection(Settings.Default.DatabaseConnectionString);
                var query = "SELECT * FROM MARCHANDISE WHERE Denomination LIKE '%" + TextBoxDevisQte.Text + "%'";
                db.Open();
                try
                {
                    var oCommand = new SqlCeCommand {Connection = db, CommandText = query};
                    var resultat = oCommand.ExecuteReader();
                    while(resultat.Read())
                    {
                        var text = resultat[0].ToString();
                        var qte = Convert.ToInt32(resultat[1]);
                        var prixMarchandise = Convert.ToInt32(resultat[2]);
                        var nouvelleMarchandise = new Marchandise(text, qte, prixMarchandise);
                        var panelMarchandise = new StackPanel();
                        var thick = new Thickness(5, 2, 0, 0);

                        //nouvelle bordure
                        var bordure = new Border
                        {
                            BorderBrush = BorderDevis.BorderBrush,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Margin = new Thickness(2, 2, 1, 0),
                            BorderThickness = new Thickness(1),
                            Width = BorderDevis.Width - 5,
                            Child = panelMarchandise,
                            Height = 70
                        };

                        PanelProduit.Children.Add(bordure);

                        // Nom du produit
                        panelMarchandise.Children.Add(new TextBlock {Margin = thick, Text = text, Height = 16});

                        // Prix
                        panelMarchandise.Children.Add(new TextBlock {Text = qte.ToString(CultureInfo.InvariantCulture), Margin = thick, Height = 16});

                        // Quantité
                        panelMarchandise.Children.Add(new TextBlock
                        {
                            Text = prixMarchandise.ToString(CultureInfo.InvariantCulture),
                            Margin = new Thickness(5, 2, 0, 0),
                            Height = 16
                        });

                        nouvelleMarchandise.Bordure = bordure;
                        ListMarchandise.Add(nouvelleMarchandise);
                    }
                    resultat.Close();
                }
                catch(Exception caught)
                {
                    Console.WriteLine(caught.Message);
                    Console.Read();
                }
                finally
                {
                    db.Close();
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PanelProduit.Children.Clear();
            ListMarchandise.Clear();

            var db = new SqlCeConnection(Settings.Default.DatabaseConnectionString);
            const string query = "SELECT * FROM MARCHANDISE";
            db.Open();
            try
            {
                var oCommand = new SqlCeCommand {Connection = db, CommandText = query};
                var resultat = oCommand.ExecuteReader();
                while(resultat.Read())
                {
                    var text = resultat[0].ToString();
                    var qte = Convert.ToInt32(resultat[1]);
                    var prixMarchandise = Convert.ToInt32(resultat[2]);
                    var nouvelleMarchandise = new Marchandise(text, qte, prixMarchandise);
                    var panelMarchandise = new StackPanel();
                    var thick = new Thickness(5, 2, 0, 0);

                    //nouvelle bordure
                    var bordure = new Border
                    {
                        BorderBrush = BorderDevis.BorderBrush,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(2, 2, 1, 0),
                        BorderThickness = new Thickness(1),
                        Width = BorderDevis.Width - 5,
                        Child = panelMarchandise,
                        Height = 70
                    };

                    PanelProduit.Children.Add(bordure);

                    // Nom du produit
                    panelMarchandise.Children.Add(new TextBlock {Margin = thick, Text = text, Height = 16});

                    // Prix
                    panelMarchandise.Children.Add(new TextBlock {Text = qte.ToString(CultureInfo.InvariantCulture), Margin = thick, Height = 16});

                    // Quantité
                    panelMarchandise.Children.Add(new TextBlock
                    {
                        Text = prixMarchandise.ToString(CultureInfo.InvariantCulture),
                        Margin = new Thickness(5, 2, 0, 0),
                        Height = 16
                    });

                    nouvelleMarchandise.Bordure = bordure;
                    ListMarchandise.Add(nouvelleMarchandise);
                }
                resultat.Close();
            }
            catch(Exception caught)
            {
                Console.WriteLine(caught.Message);
                Console.Read();
            }
            finally
            {
                db.Close();
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderDevis.Width = MenuProduit.ActualWidth - 40;
            BorderDevis.Height = MenuProduit.ActualHeight - 70;

            try
            {
                var nbMarchandise = ListMarchandise.Count;
                for(var i = 0; i < nbMarchandise; i++)
                {
                    ListMarchandise[i].Bordure.Width = BorderDevis.Width - 5;
                }
            } // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                /*Bro, do you even try ?*/
            }
        }
    }
}