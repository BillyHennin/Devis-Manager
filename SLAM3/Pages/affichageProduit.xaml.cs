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
    ///     Logique d'interaction pour affichageProduit.xaml
    /// </summary>
    public partial class AffichageProduit
    {
        private static readonly List<Marchandise> ListMarchandise = new List<Marchandise>();

        public AffichageProduit()
        {
            InitializeComponent();
        }

        private void TextBoxDevisQte_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxDevisQte.Text == "")
            {
                Page_Loaded(sender, e);
            }
            else
            {
                PanelProduit.Children.Clear();
                ListMarchandise.Clear();

                TextBoxDevisQte.BorderBrush =
                    BorderDevis.BorderBrush =
                        new SolidColorBrush((Color)ColorConverter.ConvertFromString(Settings.Default.AccentColor));

                var db = new SqlCeConnection(Settings.Default.DatabaseConnectionString);
                var query = "SELECT * FROM MARCHANDISE WHERE Denomination LIKE '%" + TextBoxDevisQte.Text + "%'";
                db.Open();
                try
                {
                    var oCommand = new SqlCeCommand { Connection = db, CommandText = query };
                    var resultat = oCommand.ExecuteReader();
                    while (resultat.Read())
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
                            Margin = new Thickness(5, 2, 0, 0),
                            Height = 16
                        });

                        nouvelleMarchandise.Bordure = bordure;
                        ListMarchandise.Add(nouvelleMarchandise);
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
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PanelProduit.Children.Clear();
            ListMarchandise.Clear();

            TextBoxDevisQte.BorderBrush =
                BorderDevis.BorderBrush =
                    new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor));

            var db = new SqlCeConnection(Settings.Default.DatabaseConnectionString);
            const string query = "SELECT * FROM MARCHANDISE";
            db.Open();
            try
            {
                var oCommand = new SqlCeCommand {Connection = db, CommandText = query};
                var resultat = oCommand.ExecuteReader();
                while (resultat.Read())
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
                        Margin = new Thickness(5, 2, 0, 0),
                        Height = 16
                    });

                    nouvelleMarchandise.Bordure = bordure;
                    ListMarchandise.Add(nouvelleMarchandise);
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
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderDevis.Width = MenuProduit.ActualWidth - 40;
            BorderDevis.Height = MenuProduit.ActualHeight - 70;

            try
            {
                var nbMarchandise = ListMarchandise.Count;
                for (var i = 0; i < nbMarchandise; i++)
                {
                    ListMarchandise[i].Bordure.Width = BorderDevis.Width - 5;
                }
            } // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                //Bro, do you even try ?
            }
        }
    }
}