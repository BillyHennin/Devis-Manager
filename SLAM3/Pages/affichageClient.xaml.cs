// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using SLAM3.Classes;
using SLAM3.Properties;

namespace SLAM3.Pages
{
    /// <summary>
    ///   Logique d'interaction pour affichageClient.xaml
    /// </summary>
    public partial class affichageClient
    {
        public affichageClient()
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
            for(var i = 0; i < 10; i++)
            {
                comboBoxClient.Items.Add(new ComboboxItemClient
                {
                    Text = "Item text1 " + i,
                    Value = new Client("Item text1 " + i, "02", "mes@couilles")
                });
            }
            comboBoxClient.SelectedIndex = 0;
        }

        //TODO : Creer un combobox contetant les devis des clients + afficher le devis lorsqu'on change la valeur de cette combobox

        private void comboBoxClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var panelMarchandise = new StackPanel();
                for(var i = 0; i < 10; i++)
                {
                    var bordure = new Border
                    {
                        BorderBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor)),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(1, 1, 0, 0),
                        BorderThickness = new Thickness(1),
                        Width = BorderDevis.Width - 4,
                        Child = panelMarchandise,
                        Height = 70
                    };

                    // Nom du produit
                    panelMarchandise.Children.Add(new TextBlock
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(5, 2, 0, 0),
                        Text = "Sweg",
                        Height = 16
                    });

                    // Prix
                    panelMarchandise.Children.Add(new TextBlock
                    {
                        Text = "2000€",
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(5, 2, 0, 0),
                        Height = 16
                    });

                    // Quantité
                    panelMarchandise.Children.Add(new TextBlock
                    {
                        Text = "30",
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(5, 2, 0, 0),
                        Height = 16
                    });
                    panelDevis.Children.Add(bordure);
                }
                // ReSharper disable once EmptyGeneralCatchClause
            }catch{}
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            comboBoxClient.BorderBrush = BorderDevis.BorderBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor));
        }

        private void MenuClient_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderDevis.Width = MenuClient.ActualWidth - 40;
            BorderDevis.Height = MenuClient.ActualHeight - 70;
        }
    }
}