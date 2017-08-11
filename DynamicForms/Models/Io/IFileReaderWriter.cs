﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DynamicForms.Models.Xml;
using DynamicForms.UIControls;

namespace DynamicForms.Models.Io
{
    public interface IFileReaderWriter
    {
        void SaveForm(List<BaseControl> form, string filename);

        List<BaseControl> LoadForm(string filename);

        List<Form> LoadResource<T>(string resourcePrefix);

    }
}