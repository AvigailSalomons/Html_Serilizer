using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace practicode_2
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _htmlHelper = new HtmlHelper();
        public static HtmlHelper htmlHelper => _htmlHelper;
        public string[] j1;
        public string[] j2;
        private HtmlHelper()
        {
            string jsonFilePath1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HtmlVoidTags.json");
            string jsonString1 = File.ReadAllText(jsonFilePath1);
            j1 = Deserialize<string[]>(jsonString1);

            // השורה הזו מאפשרת להשתמש בנתיב מלא לקובץ HtmlTags.json
            string jsonFilePath2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HtmlTags.json");
            string jsonString2 = File.ReadAllText(jsonFilePath2);
            j2 = Deserialize<string[]>(jsonString2);

        }
        private T Deserialize<T>(string jsonString)
        {
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }
}
