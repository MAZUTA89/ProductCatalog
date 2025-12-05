using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalog.Infrastructure.Configuring.Helpers.Json
{
    public class SectionNotFoundException : Exception
    {
        public JsonConfigArgs JsonConfigArgs;
        public SectionNotFoundException(JsonConfigArgs jsonConfigArgs)
            :base($"{jsonConfigArgs.SectionName} section not found at" +
                 $" {jsonConfigArgs.FileName} file.")
        {
             
        }
        
    }
}
