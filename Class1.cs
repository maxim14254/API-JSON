
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;


namespace WindowsFormsApp1
{
    static class Class1// класс для возврата готового массива данных полученных из json 
    {
        public static List<DataJson> GetDataJsons(DataContractJsonSerializer json)
        {
            using (var file = new FileStream("datajson.json", FileMode.OpenOrCreate))
            {
                List<DataJson> newDataJson = json.ReadObject(file) as List<DataJson>;
                return newDataJson;
            }
        }
    }
}
