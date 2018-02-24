using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfiguredLocalizedSystemWatcher.Configuration
{
    public class CultureElement : ConfigurationElement
    {
        [ConfigurationProperty("value")]
        public string Value
        {
            get { return this["value"] as string; }
        }
    }
}
