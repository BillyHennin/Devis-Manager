// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

using System;

namespace MANAGER.Table
{
    public class Category
    {
        static Category()
        {
            TableName = "CATEGORIE";
            ID = String.Format("ID_{0}", TableName);
            Title = "LIBELLE";
        }

        public static string ID { get; set; }
        public static string TableName { get; set; }
        public static string Title { get; set; }

        public void Construction(string ID, string TableName, string Title)
        {
            Category.ID = ID;
            Category.TableName = TableName;
            Category.Title = Title;
        }
    }
}