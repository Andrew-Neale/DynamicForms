using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using DynamicForms.Models.Xml;
using DynamicForms.UIControls;
using Newtonsoft.Json;

namespace DynamicForms.Models.Io
{
    public class FileReaderWriter : IFileReaderWriter
    {

        public void SaveForm(List<BaseControl> form, string filename)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
			JsonSerializerSettings settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All
			};
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(form, settings);
            System.IO.File.WriteAllText(filePath, json);
        }

        public async Task<List<BaseControl>> LoadForm(string filename)
        {
            var savedForm = new List<BaseControl>();

            await Task.Run(() =>
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, filename);

                if (File.Exists(filePath))
                {
                    var formData = System.IO.File.ReadAllText(filePath);
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    };
                    savedForm = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BaseControl>>(formData, settings);
                }
            });
                    
            return savedForm;

        }

        public async Task<List<Form>> LoadResource<T>(string resourcePrefix)
        {
            List<Form> formPages = new List<Form>();

            await Task.Run(() =>
             {
                 var assembly = typeof(T).GetTypeInfo().Assembly;
                 var stream = assembly.GetManifestResourceStream(resourcePrefix);

                 using (var reader = new System.IO.StreamReader(stream))
                 {
                     var document = XDocument.Load(reader);
                     var forms = new Forms(document);
                     formPages.AddRange(forms.FormViews);
                 }
             });

            return formPages;
        }
    }
}
