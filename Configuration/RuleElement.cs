using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfiguredLocalizedSystemWatcher.Configuration
{
    public class RuleElement : ConfigurationElement
    {
        [ConfigurationProperty("pattern", IsKey = true)]
        public string RegexPattern
        {
            get { return this["pattern"] as string; }
        }

        [ConfigurationProperty("destinationPath")]
        public string DestinationPath
        {
            get { return this["destinationPath"] as string; }
        }

        [ConfigurationProperty("needAddNumber")]
        public bool NeedAddNumber
        {
            get { return (bool)this["needAddNumber"]; }
        }

        [ConfigurationProperty("needAddDate")]
        public bool NeedAddDate
        {
            get { return (bool)this["needAddDate"]; }
        }
    }
}
