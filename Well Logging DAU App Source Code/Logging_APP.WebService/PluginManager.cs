using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
namespace Logging_App.WebService
{
    public class PluginManager
    {
        public static readonly string RootPath = AppDomain.CurrentDomain.BaseDirectory + "plugins\\";
        public static List<PluginInfo> PluginsInfo = null;
        public static object lockobj = new object();
        static PluginManager()
        {
            getPluginInfo();
            var watcher = new FileSystemWatcher(RootPath, "Version");
            watcher.Changed += new FileSystemEventHandler(watcher_Changed);
            watcher.EnableRaisingEvents = true;
        }
        private static DateTime lastWriteTimeVersion;
        static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            var time = File.GetLastWriteTime(e.FullPath);
            if (time != lastWriteTimeVersion)
            {
                lastWriteTimeVersion = time;
                getPluginInfo();
            }
        }

        private static void getPluginInfo()
        {
            lock (lockobj)
            {
                var list = new List<PluginInfo>();
                foreach (var dir in new DirectoryInfo(RootPath).GetDirectories())
                {
                    string pluginPath = RootPath + "\\" + dir.Name + "\\";
                    string pluginConfig = pluginPath + "config.xml";
                    if (!File.Exists(pluginConfig)) continue;
                    if (dir.Name.Length < 3) continue;
                    int lastIndex = dir.Name.LastIndexOf('.');
                    if (lastIndex < 1 || lastIndex == dir.Name.Length - 1) continue;
                    if (!File.Exists(pluginPath + dir.Name.Substring(lastIndex + 1) + ".dll")) continue;
                    var pluginInfo = new PluginInfo();
                    pluginInfo.Name = dir.Name;
                    using (var reader = new XmlTextReader(pluginConfig))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                switch (reader.Name)
                                {
                                    case "DisplayName":
                                        pluginInfo.DisplayName = reader.ReadElementContentAsString().Trim();
                                        break;
                                    case "Index":
                                        decimal dec;
                                        if (decimal.TryParse(reader.ReadElementContentAsString().Trim(), out dec))
                                            pluginInfo.Index = dec;
                                        break;
                                }
                            }
                        }
                        if (pluginInfo.DisplayName.Length < 1) continue;
                    }
                    var itemsInfo = new List<PluginItemInfo>();
                    foreach (var file in new DirectoryInfo(pluginPath).GetFiles())
                    {
                        if (file.Name.ToLower() == "config.xml") continue;
                        itemsInfo.Add(new PluginItemInfo { Name = file.Name, LastWriteTime = file.LastWriteTime });
                    }
                    pluginInfo.ItemsInfo = itemsInfo;
                    list.Add(pluginInfo);
                }
                PluginsInfo = list;
            }
        }

        public class PluginInfo
        {
            public string Name { set; get; }
            public string DisplayName { set; get; }
            private decimal _index = 999;
            public decimal Index
            {
                set { _index = value; }
                get { return _index; }
            }
            public List<PluginItemInfo> ItemsInfo { get; set; }
        }

        public class PluginItemInfo
        {
            public string Name { set; get; }
            public DateTime LastWriteTime { get; set; }
        }
    }
}