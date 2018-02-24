using ConfiguredLocalizedSystemWatcher.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using notifications = ConfiguredLocalizedSystemWatcher.Notifications;

namespace ConfiguredLocalizedSystemWatcher
{
    class Program
    {
        private const string defaultFolderPath = @"D:\TargetFolders\DefaultFolder\";
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var manager = AppConfigurationManager.GetManager;

            List<string> foldersPath = manager.GetTrackedFoldersPathList();

            FileSystemWatcher[] watchers = new FileSystemWatcher[foldersPath.Count];

            List<RuleElement> rules = manager.GetRulesList();

            foreach (var rule in rules)
            {
                if (!Directory.Exists(rule.DestinationPath))
                {
                    Directory.CreateDirectory(rule.DestinationPath);
                }
            }

            if (!Directory.Exists(nameof(defaultFolderPath)))
            {
                Directory.CreateDirectory(nameof(defaultFolderPath));
            }

            for (var i = 0; i < foldersPath.Count; i++)
            {
                if (!Directory.Exists(foldersPath[i]))
                {
                    Directory.CreateDirectory(foldersPath[i]);
                }

                var handler = new FileHandler();
                watchers[i] = new FileSystemWatcher();
                watchers[i].Path = foldersPath[i];
                watchers[i].NotifyFilter = NotifyFilters.FileName;
                watchers[i].Filter = "";
                watchers[i].Created += handler.Handle;
                watchers[i].EnableRaisingEvents = true;
            }

            while (true) { }
        }
    }
}
