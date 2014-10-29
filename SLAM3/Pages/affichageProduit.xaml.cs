using System.Collections.Generic;
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BorderDevis.BorderBrush =
                new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor));

            for (var i = 1; i < 11; i++)
            {
                var text = "Produit " + i;
                var qte = (100*i);
                var prixMarchandise = (10*i);
                var nouvelleMarchadise = new Marchandise(text, qte, prixMarchandise);
                var panelMarchandise = new StackPanel();
                var thick = new Thickness(5, 2, 0, 0);

                //nouvelle bordure
                var bordure = new Border
                {
                    BorderBrush =
                        new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor)),
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

                nouvelleMarchadise.Bordure = bordure;
                ListMarchandise.Add(nouvelleMarchadise);
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
            }// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                //Bro, do you even try ?
            }
        }
    }
}