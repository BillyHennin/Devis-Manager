// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from SLAM3 INC. team.
//  
// Copyrights (c) 2014 SLAM3 INC. All rights reserved.

using System;

/*
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types; 
using System.Data;
*/

namespace SLAM3.Pages
{
    /// <summary>
    ///     Logique d'interaction pour about.xaml
    /// </summary>
    public partial class about
    {
        public about()
        {
            InitializeComponent();
        }

        private static string motd()
        {
            /*
            string oradb = "Data Source=ORCL;User Id=hr;Password=hr;";
            OracleConnection conn = new OracleConnection(oradb);  // C#, no shit
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT motd FROM MOTD";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();
            conn.Dispose();
            return dr.GetString(0);
            */
            return "sweg";
        }

        private void Label_Initialized(object sender, EventArgs e)
        {
            Text.BBCode = "\r\nBienvenue dans l'application SIO2 - SLAM3 pour la creation et la visualisation de devis."
                          + "\r\n\r\nA propos de l'application : "
                          +
                          "\r\n\r\n\tCette application à été créée dans le cadre d'un projet de SLAM3. Le but était créer un application utilisant une base de données Oracle et de l'exploiter."
                          +
                          "\r\n\tAvec cette application vous serez capable de creer des devis, de visualiser vos devis et de voir la liste de produit que vous disposez."
                          +
                          "\r\nL'application que vous utilisez actuellement est open-source et est disponible [url='https://github.com/BillyHennin/APPSLAM3']ici (GitHub)[/url]."
                          + "\r\n\r\nMessage du jour : \r\n \r\n" + motd();
        }
    }
}