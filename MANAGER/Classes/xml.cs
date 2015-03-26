namespace MANAGER.Classes
{
    public class xml
    {
        private string lang;


        //private static xml Read()
        //{
        //    var reader = new System.Xml.Serialization.XmlSerializer(typeof(xml));
        //    var overview = new xml();
        //    //overview = (xml)reader.Deserialize(new System.IO.StreamReader("Config.xml"));
        //    //return new xml{lang = overview.lang};
        //}

        public static void getLang()
        {
            Transharp.SetCurrentLanguage(Transharp.LangsEnum.English);
            //switch (Read().lang)
            //{
            //    case "French":
            //        Transharp.SetCurrentLanguage(Transharp.LangsEnum.French);
            //        break;
            //    case "English":
            //        Transharp.SetCurrentLanguage(Transharp.LangsEnum.English);
            //        break;
            //    default:
            //        Transharp.SetCurrentLanguage(Transharp.LangsEnum.English);
            //        break;
            //}
        }
    }
}
