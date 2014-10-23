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
    /// Logique d'interaction pour affichageProduit.xaml
    /// </summary>
    public partial class affichageProduit
    {

        private static readonly List<Marchandise> listMarchandise = new List<Marchandise>();

        public affichageProduit()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BorderDevis.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Settings.Default.AccentColor));
            try
            {
                
                for (var i = 0; i < 12; i++)
                {
                    var text = "Produit " + i;
                    var QTE = (100 * i);
                    var prixMarchandise = (10*i);
                    var nouvelleMarchadise = new Marchandise(text, QTE, prixMarchandise);
                    var panelMarchandise = new StackPanel();

                    //nouvelle bordure
                    var bordure = new Border
                    {
                        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Settings.Default.AccentColor)),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(2, 2, 1, 0),
                        BorderThickness = new Thickness(1),
                        Width = BorderDevis.Width - 5,
                        Child = panelMarchandise,
                        Height = 70
                    };

                    panelDevis.Children.Add(bordure);

                    // Nom du produit
                    panelMarchandise.Children.Add(new TextBlock
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(5, 2, 0, 0),
                        Text = text,
                        Height = 16
                    });

                    // Prix
                    panelMarchandise.Children.Add(new TextBlock
                    {
                        Text = QTE.ToString(CultureInfo.InvariantCulture),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(5, 2, 0, 0),
                        Height = 16
                    });

                    // Quantité
                    panelMarchandise.Children.Add(new TextBlock
                    {
                        Text = prixMarchandise.ToString(CultureInfo.InvariantCulture),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(5, 2, 0, 0),
                        Height = 16
                    });

                    nouvelleMarchadise.Bordure = bordure;
                    listMarchandise.Add(nouvelleMarchadise);
                }
            }// ReSharper disable once EmptyGeneralCatchClause
            catch {}
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderDevis.Width = MenuProduit.ActualWidth - 40;
            BorderDevis.Height = MenuProduit.ActualHeight - 70;

            try
            {
                var nbMarchandise = listMarchandise.Count;
                for (var i = 0; i < nbMarchandise; i++)
                {
                    listMarchandise[i].Bordure.Width = BorderDevis.Width - 5;
                }
                // ReSharper disable once EmptyGeneralCatchClause
            }catch { }
        }
    }
}
