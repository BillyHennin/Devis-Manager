// This program is a private software, based on c# source code.
// To sell or change credits of this software is forbidden,
// except if someone approve it from MANAGER INC. team.
//  
// Copyrights (c) 2014 MANAGER INC. All rights reserved.

namespace MANAGER.Classes.Table
{
    public class SQL_Category
    {
        static SQL_Category()
        {
            TableName = "CATEGORIE";
            ID = $"ID_{TableName}";
            Title = "LIBELLE";
        }

        public static string ID { get; set; }
        public static string TableName { get; set; }
        public static string Title { get; set; }

        public void Construction(string id, string tableName, string title)
        {
            ID = ID;
            TableName = TableName;
            Title = Title;
        }
    }
}