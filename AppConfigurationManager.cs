using ConfiguredLocalizedSystemWatcher.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfiguredLocalizedSystemWatcher
{
    public class AppConfigurationManager
    {
        private static AppConfigurationManager manager;
        private static AppConfigurationSection config;
        private AppConfigurationManager()
        {
            config = (AppConfigurationSection)ConfigurationManager.GetSection("appConfigurationSection");
        }

        public static AppConfigurationManager GetManager
        {
            get
            {
                if (manager == null)
                    manager = new AppConfigurationManager();
                return manager;
            }
        }

        public List<string> GetTrackedFoldersPathList()
        {
            List<string> foldersPath = new List<string>();
            foreach (var folder in config.Folders)
            {
                if (folder is FolderElement)
                    foldersPath.Add((folder as FolderElement).Path);
            }

            return foldersPath;
        }

        public List<RuleElement> GetRulesList()
        {
            List<RuleElement> rules = new List<RuleElement>();

            foreach (var rule in config.Rules)
            {
                if (rule is RuleElement)
                    rules.Add(rule as RuleElement);
            }

            return rules;
        }

        public CultureInfo GetCulture()
        {
            return new CultureInfo(config.Culture.Value);
        }

        public bool CheckNumberRenameByPattern(string pattern)
        {
            foreach (var rule in config.Rules)
            {
                if (rule is RuleElement)
                {
                    if ((rule as RuleElement).RegexPattern == pattern)
                        return (rule as RuleElement).NeedAddNumber;
                }
            }
            return false;
        }

        public bool CheckDateTimeRenameByPattern(string pattern)
        {
            foreach (var rule in config.Rules)
            {
                if (rule is RuleElement)
                {
                    if ((rule as RuleElement).RegexPattern == pattern)
                        return (rule as RuleElement).NeedAddDate;
                }
            }
            return false;
        }
    }
}
