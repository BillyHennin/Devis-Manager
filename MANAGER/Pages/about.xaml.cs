// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

#region

using System.Windows;
using System.Windows.Controls;

using FirstFloor.ModernUI.Windows.Controls;

#endregion

namespace MANAGER.Pages
{
    public partial class About
    {
        private void Text_Loaded(object sender, RoutedEventArgs e)
        {
            PanelMOTD.Children.Clear();
            var PreMOTD = "\r\nBienvenue dans l'application SIO2 - MANAGER pour la creation et la visualisation de devis."
                          + "\r\n\r\nA propos de l'application : "
                          + "\r\n\r\n\tCette application à été créée dans le cadre d'un projet de MANAGER. Le but était créer un application utilisant une base de données Oracle et de l'exploiter."
                          + "\r\n\tAvec cette application vous serez capable de creer des devis, de visualiser vos devis et de voir la liste de produit que vous disposez."
                          + "\r\n\tL'application que vous utilisez actuellement est open-source et est disponible [url='https://github.com/BillyHennin/Devis-Manager']ici (GitHub)[/url]."
                          + "\r\n\r\nMessage du jour : \r\n \r\n";

            var thick = new Thickness(5, 2, 0, 0);

            var panelMessage = new StackPanel();
            var border = new Border
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(2, 2, 1, 0),
                BorderThickness = new Thickness(1),
                Child = panelMessage
            };
            panelMessage.Children.Add(new BbCodeBlock {Margin = thick, BbCode = PreMOTD});
            PanelMOTD.Children.Add(border);
        }
    }
}