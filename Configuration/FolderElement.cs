using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfiguredLocalizedSystemWatcher.Configuration
{
    public class FolderElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsKey = true)]
        public string Path
        {
            get { return this["path"] as string; }
        }
    }
}
