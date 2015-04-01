// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

#region

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;

using FirstFloor.ModernUI.Windows.Controls;

using MANAGER.Classes;

using Newtonsoft.Json.Linq;

#endregion

namespace MANAGER.Pages
{
    public partial class About
    {
        private void Text_Loaded(object sender, RoutedEventArgs e)
        {
            PanelMOTD.Children.Clear();
            var JsonMOTD = String.Empty;
            try
            {
                //JsonMOTD = new WebClient().DownloadString(String.Format("http://billyhennin.github.io/Devis-Manager/MOTD{0}.json", Transharp.getCurrentLanguage()));

                var json = JObject.Parse(new WebClient().DownloadString("http://billyhennin.github.io/Devis-Manager/MOTD.json"));
                JsonMOTD = String.Format("{0} - {1}\r\n\r\n\t\t{2}", json["date"], json["title"], json["tags"]);
                //JsonMOTD = String.Format("{0} - {1}\r\n\r\n\t\t{2}\r\n\r\n[img]{3}[/img]", json["date"], json["title"], json["tags"], json["image"]);
            }
            catch
            {
                JsonMOTD = Transharp.GetTranslation("Curl_Fail_MOTD");
            }

            var MOTD = String.Format("\r\n{0}\r\n\r\n{1}\r\n\r\n\t{2}\r\n\t{3}\r\n\t{4}\r\n\r\n{5}\r\n\r\n\t{6}", Transharp.GetTranslation("AB_MOTD1"),
                Transharp.GetTranslation("AB_MOTD2"), Transharp.GetTranslation("AB_MOTD3"), Transharp.GetTranslation("AB_MOTD4"),
                Transharp.GetTranslation("AB_MOTD5", "[url='https://github.com/BillyHennin/Devis-Manager']GitHub[/url]"), Transharp.GetTranslation("AB_MOTD6"),
                JsonMOTD);

            var panelMessage = new StackPanel();
            var border = new Border
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(2, 2, 1, 0),
                BorderThickness = new Thickness(1),
                Child = panelMessage
            };
            panelMessage.Children.Add(new BbCodeBlock {Margin = new Thickness(5, 2, 0, 0), BbCode = MOTD});
            PanelMOTD.Children.Add(border);
        }
    }
}