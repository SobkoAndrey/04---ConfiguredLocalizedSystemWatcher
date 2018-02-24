using ConfiguredLocalizedSystemWatcher.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using notifications = ConfiguredLocalizedSystemWatcher.Notifications;

namespace ConfiguredLocalizedSystemWatcher
{
    public class FileHandler
    {
        public void Handle(object sender, FileSystemEventArgs e)
        {
            List<RuleElement> rules = AppConfigurationManager.GetManager.GetRulesList();

            Thread.CurrentThread.CurrentUICulture = AppConfigurationManager.GetManager.GetCulture();
            var counter = 1;
            string copy = "copy";

            Console.WriteLine(Path.GetDirectoryName(e.FullPath) + notifications.FileAdded + e.Name + " : " + DateTimeOffset.Now.ToString("f", Thread.CurrentThread.CurrentUICulture.DateTimeFormat));

            bool hasAnyMatch = rules.Any(_ => Regex.IsMatch(e.Name, _.RegexPattern));
            if (!hasAnyMatch)
            {
                Console.WriteLine(notifications.NoMatchFound);
                CheckAndFormFileName(e.Name, e.FullPath, "", @"D:\TargetFolders\DefaultFolder\", counter, copy);
            }
            else
            {
                foreach (var rule in rules)
                {
                    var result = CheckAndFormFileName(e.Name, e.FullPath, rule.RegexPattern, rule.DestinationPath, counter, copy);
                    if (result)
                    {
                        break;
                    }
                }
            }
        }
        private bool CheckAndFormFileName(string fileOldName, string fileFullPath, string pattern, string targetPath, int counter, string copy)
        {
            var result = false;
            while (true)
            {
                bool hasMatch = Regex.IsMatch(fileOldName, pattern);
                if (hasMatch)
                {
                    if (!File.Exists(targetPath + fileOldName))
                    {
                        Console.WriteLine(notifications.MatchFound);
                        File.Move(fileFullPath, targetPath + fileOldName);
                        Console.WriteLine(notifications.FileTransferred + fileOldName + " : " + targetPath);
                        result = true;
                        break;
                    }
                    else
                    {
                        bool hasDone = false;
                        while (!hasDone)
                        {
                            var fileName = Path.GetFileNameWithoutExtension(targetPath + fileOldName);
                            var fileExtension = Path.GetExtension(targetPath + fileOldName);

                            string newFullName = string.Empty;
                            bool isDefault = false;
                            if (AppConfigurationManager.GetManager.CheckNumberRenameByPattern(pattern) && AppConfigurationManager.GetManager.CheckDateTimeRenameByPattern(pattern))
                            {
                                var dateTime = DateTime.Now.ToString("G", new CultureInfo("us-US").DateTimeFormat).Replace('/', '-').Replace(':', '-');
                                newFullName = targetPath + fileName + dateTime + " (" + counter + ")" + fileExtension;
                            }
                            else if (AppConfigurationManager.GetManager.CheckNumberRenameByPattern(pattern))
                            {
                                newFullName = targetPath + fileName + counter + fileExtension;
                            }
                            else if (AppConfigurationManager.GetManager.CheckDateTimeRenameByPattern(pattern))
                            {
                                var dateTime = DateTime.Now.ToString("G", new CultureInfo("us-US").DateTimeFormat).Replace('/', '-').Replace(':', '-');
                                newFullName = targetPath + fileName + dateTime + fileExtension;
                            }
                            else
                            {
                                isDefault = true;
                                newFullName = targetPath + fileName + copy + fileExtension;
                            }

                            var newName = Path.GetFileName(newFullName);

                            if (File.Exists(newFullName))
                            {
                                if (isDefault)
                                    copy += "(copy)";

                                if (AppConfigurationManager.GetManager.CheckNumberRenameByPattern(pattern))
                                    counter++;

                                continue;
                            }
                            else
                            {
                                Thread.Sleep(100);
                                Console.WriteLine(notifications.MatchFound);
                                File.Move(fileFullPath, newFullName);
                                Console.WriteLine(notifications.FileTransferred + newName + " : " + targetPath);

                                if (AppConfigurationManager.GetManager.CheckNumberRenameByPattern(pattern))
                                    counter++;

                                result = true;
                                hasDone = true;
                                break;
                            }
                        }
                        break;
                    }
                }
                return false;
            }
            return result;
        }
    }
}
