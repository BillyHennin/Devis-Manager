// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

using System;
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
                try
                {
                    comboBoxClient.Items.Add(new ComboboxItemClient
                    {
                        Text = "Item text1 " + i,
                        Value = new Client("Item text1 " + i, "02", "mes@couilles")
                    });
                    comboBoxClient.SelectedIndex = 0;
                    // ReSharper disable once EmptyGeneralCatchClause
                }catch{}
                
            }
            
        }

        //TODO : Creer un combobox contenant les devis des clients + afficher le devis lorsqu'on change la valeur de cette combobox

        private void comboBoxClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // garder ça
            comboBoxDevis.Items.Clear();
            // oui

            for (var i = 0; i < 10; i++)
            {
                    var listMarchandise = new List<Marchandise>();
                    var nouvelleMarchadise1 = new Marchandise("Test 1", 1+i, 40);
                    var nouvelleMarchadise2 = new Marchandise("Test 2", 3+i, 50);
                    var nouvelleMarchadise3 = new Marchandise("Test 3", 10+i, 60);
                    var leDevis = new Devis(listMarchandise);
                    leDevis.getList.Add(nouvelleMarchadise1);
                    leDevis.getList.Add(nouvelleMarchadise2);
                    leDevis.getList.Add(nouvelleMarchadise3);

                    comboBoxDevis.Items.Add(new ComboboxItemDevis
                    {
                        Text = "Devis " + i,
                        Value = leDevis
                    });

                    comboBoxClient.SelectedIndex = 0;
            }
            panelDevis.Children.Clear();
        }

        private void comboBoxDevis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var nbMarchandise = (comboBoxDevis.SelectedItem as ComboboxItemDevis).Value.getList.Count;
                panelDevis.Children.Clear();

                for(var i = 0; i < nbMarchandise; i++)
                {
                    var panelMarchandise = new StackPanel();

                    panelDevis.Children.Add(new Border
                    {
                        BorderBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor)),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(2, 2, 1, 0),
                        BorderThickness = new Thickness(1),
                        Width = BorderDevis.Width - 5,
                        Child = panelMarchandise,
                        Height = 70
                    });

                    // Nom du produit
                    panelMarchandise.Children.Add(new TextBlock
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(5, 2, 0, 0),
                        Text = (comboBoxDevis.SelectedItem as ComboboxItemDevis).Value[i].getNom,
                        Height = 16
                    });

                    // Prix
                    panelMarchandise.Children.Add(new TextBlock
                    {
                        Text = (comboBoxDevis.SelectedItem as ComboboxItemDevis).Value[i].getPrix.ToString(CultureInfo.InvariantCulture),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(5, 2, 0, 0),
                        Height = 16
                    });

                    // Quantité
                    panelMarchandise.Children.Add(new TextBlock
                    {
                        Text = (comboBoxDevis.SelectedItem as ComboboxItemDevis).Value[i].getQTE.ToString(CultureInfo.InvariantCulture),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(5, 2, 0, 0),
                        Height = 16
                    });

                }
            }// ReSharper disable once EmptyGeneralCatchClause
            catch {}
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            comboBoxClient.BorderBrush = 
                BorderDevis.BorderBrush = 
                    comboBoxDevis.BorderBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(Settings.Default.AccentColor));
        }

        private void MenuClient_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderDevis.Width = MenuClient.ActualWidth - 40;
            BorderDevis.Height = MenuClient.ActualHeight - 70;
            try
            {
                var nbMarchandise = (comboBoxDevis.SelectedItem as ComboboxItemDevis).Value.getList.Count;
                for(var i = 0; i < nbMarchandise; i++)
                {
                    (comboBoxDevis.SelectedItem as ComboboxItemDevis).Value[i].Bordure.Width = BorderDevis.Width - 5;
                }
                // ReSharper disable once EmptyGeneralCatchClause
            }catch{}
        }
    }
}