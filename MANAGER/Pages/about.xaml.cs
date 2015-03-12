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

            var PreMOTD = string.Format("\r\n{0}\r\n\r\n{1}\r\n\r\n\t{2}\r\n\t{3}\r\n\t{4}\r\n\r\n{5}", Localisation.Localisation.AB_MOTD1,
                Localisation.Localisation.AB_MOTD2, Localisation.Localisation.AB_MOTD3, Localisation.Localisation.AB_MOTD4, 
                string.Format(Localisation.Localisation.AB_MOTD5,"[url='https://github.com/BillyHennin/Devis-Manager']GitHub[/url]")
                , Localisation.Localisation.AB_MOTD6);

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