using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

        public List<BaseControl> LoadForm(string filename)
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
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<BaseControl>>(formData, settings);
            }

            return new List<BaseControl>();

        }

        public List<Form> LoadResource<T>(string resourcePrefix)
        {
            List<Form> formPages = new List<Form>();

			var assembly = typeof(T).GetTypeInfo().Assembly;
			var stream = assembly.GetManifestResourceStream(resourcePrefix);

			using (var reader = new System.IO.StreamReader(stream))
			{
				var document = XDocument.Load(reader);
				var forms = new Forms(document);
				formPages.AddRange(forms.FormViews);
			}

            return formPages;
        }
    }
}
