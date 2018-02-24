using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfiguredLocalizedSystemWatcher.Configuration
{
    public class AppConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("culture")]
        public CultureElement Culture
        {
            get
            {
                return (this["culture"] as CultureElement);
            }
        }

        [ConfigurationProperty("folders")]
        public FolderElementCollection Folders
        {
            get
            {
                return (FolderElementCollection)this["folders"];
            }
        }

        [ConfigurationProperty("rules")]
        public RuleElementCollection Rules
        {
            get
            {
                return (RuleElementCollection)this["rules"];
            }
        }
    }
}
