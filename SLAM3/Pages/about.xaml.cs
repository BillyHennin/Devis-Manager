// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

using System;
using System.IO;
using System.Net;

namespace SLAM3.Pages
{
    /// <summary>
    ///   Logique d'interaction pour about.xaml
    /// </summary>
    public partial class About
    {
        public About()
        {
            InitializeComponent();
        }

        private static string Motd()
        {
            var webRequest = WebRequest.Create(@"http://niitr0x.free.fr/VirtualNews/LATEST_VERSION.txt");
            string msg;
            using(var response = webRequest.GetResponse())
            {
                using(var content = response.GetResponseStream())
                {
                    if(content != null)
                    {
                        using(var reader = new StreamReader(content))
                        {
                            msg = reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        msg = "Erreur dans l'optention du message du jour.";
                    }
                }
            }
            return msg;
        }

        private void Label_Initialized(object sender, EventArgs e)
        {
            Text.BbCode = "\r\nBienvenue dans l'application SIO2 - SLAM3 pour la creation et la visualisation de devis."
                          + "\r\n\r\nA propos de l'application : "
                          + "\r\n\r\n\tCette application à été créée dans le cadre d'un projet de SLAM3. Le but était créer un application utilisant une base de données Oracle et de l'exploiter."
                          + "\r\n\tAvec cette application vous serez capable de creer des devis, de visualiser vos devis et de voir la liste de produit que vous disposez."
                          + "\r\nL'application que vous utilisez actuellement est open-source et est disponible [url='https://github.com/BillyHennin/APPSLAM3']ici (GitHub)[/url]."
                          + "\r\n\r\nMessage du jour : \r\n \r\n" + Motd();
        }
    }
}