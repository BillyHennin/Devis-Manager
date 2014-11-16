// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;
using System.Collections.Generic;
//using System.Data.SqlServerCe;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

using MANAGER.Classes;
using MANAGER.Connection;
using MANAGER.Properties;

namespace MANAGER.Pages
{
    /// <summary>
    ///   Logique d'interaction pour affichageClient.xaml
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

            //var db = new SqlCeConnection(Settings.Default.DatabaseConnectionString);
            var Co = new ConnectionOracle();
            var db = Co.OracleDatabase(Settings.Default.DatabaseConnectionString);
            const string query = "SELECT * FROM CLIENT";
            db.Open();
            try
            {
                //var oCommand = new SqlCeCommand {Connection = db, CommandText = query};
                var oCommand = Co.OracleCommand(db, query);
                var resultat = oCommand.ExecuteReader();
                while(resultat.Read())
                {
                    ComboBoxClient.Items.Add(new ComboboxItemClient
                    {
                        Text = resultat[0].ToString(),
                        Value = new Client(resultat[0].ToString(), resultat[1].ToString(), resultat[2].ToString())
                    });
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

        private void ComboBoxClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // garder ça
            ComboBoxDevis.Items.Clear();
            PanelDevis.Children.Clear();
            // oui
            //TODO : liaison ddb
            //var db = new SqlCeConnection(Settings.Default.DatabaseConnectionString);
            var Co = new ConnectionOracle();
            var db = Co.OracleDatabase(Settings.Default.DatabaseConnectionString);
            var query = "SELECT DISTINCT NumeroDevis FROM DEVIS WHERE Client ='" + (ComboBoxClient.SelectedItem as ComboboxItemClient).Value.GetDenomination
                        + "'";
            db.Open();
            try
            {
                //var oCommand = new SqlCeCommand {Connection = db, CommandText = query};
                var oCommand = Co.OracleCommand(db, query);
                var resultat = oCommand.ExecuteReader();
                while(resultat.Read())
                {
                    var query2 = "SELECT Marchandise, Quantite, PrixMarchandise FROM DEVIS WHERE NumeroDevis =" + resultat[0];
                    var oCommand2 = Co.OracleCommand(db, query2);
                    //var oCommand2 = new SqlCeCommand {Connection = db, CommandText = query2};
                    var resultat2 = oCommand2.ExecuteReader();
                    var listMarchandise2 = new List<Marchandise>();
                    while(resultat2.Read())
                    {
                        listMarchandise2.Add(new Marchandise(resultat2[0].ToString(), Convert.ToInt32(resultat2[1]), Convert.ToInt32(resultat2[2])));
                    }

                    ComboBoxDevis.Items.Add(new ComboboxItemDevis {Text = "Devis n°" + resultat[0], Value = new Devis(listMarchandise2)});
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
            PanelDevis.Children.Clear();
        }

        private void ComboBoxDevis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PanelDevis.Children.Clear();
            if(ComboBoxDevis.Items.Count == 0)
            {
                return;
            }

            var listMarchandise = (ComboBoxDevis.SelectedItem as ComboboxItemDevis).Value.GetList;
            var taille = listMarchandise.Count;
            for(var i = 0; i < taille; i++)
            {
                var text = listMarchandise[i].GetNom;
                var qte = Convert.ToInt32(listMarchandise[i].GetQte);
                var prixMarchandise = Convert.ToInt32(listMarchandise[i].GetPrix);
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

                // Nom du produit
                panelMarchandise.Children.Add(new TextBlock {Margin = thick, Text = text, Height = 16});

                // Prix
                panelMarchandise.Children.Add(new TextBlock {Text = qte.ToString(CultureInfo.InvariantCulture), Margin = thick, Height = 16});

                // Quantité
                panelMarchandise.Children.Add(new TextBlock {Text = prixMarchandise.ToString(CultureInfo.InvariantCulture), Margin = thick, Height = 16});

                item.Bordure = bordure;
                PanelDevis.Children.Add(bordure);
                _leDevis.GetList.Add(item);
            }
        }

        private void MenuClient_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderDevis.Width = MenuClient.ActualWidth - 40;
            BorderDevis.Height = MenuClient.ActualHeight - 70;
            try
            {
                var nbMarchandise = _leDevis.GetList.Count;
                for(var i = 0; i < nbMarchandise; i++)
                {
                    ListMarchandise[i].Bordure.Width = BorderDevis.Width - 5;
                }
            } // ReSharper disable once EmptyGeneralCatchClause
            catch {}
        }

        private void MenuClient_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var nbMarchandise = _leDevis.GetList.Count;
                for (var i = 0; i < nbMarchandise; i++)
                {
                    _leDevis[i].Bordure.BorderBrush = BorderDevis.BorderBrush;
                }
            } // ReSharper disable once EmptyGeneralCatchClause
            catch { }
        }
    }
}