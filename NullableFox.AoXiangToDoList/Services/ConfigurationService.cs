using NullableFox.AoXiangToDoList.Models;
using NullableFox.AoXiangToDoList.Services.Interfaces;
using NullableFox.AoXiangToDoList.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Management.Core;
using Windows.Storage;

namespace NullableFox.AoXiangToDoList.Services
{
    internal class ConfigurationService : IConfigurationService
    {
        readonly string configurationFileName = "ApplicationConfiguration.json";
        string ConfigurationFullPath
        {
            get
            {
                if (Package.Current is null)
                {
                    return Path.GetDirectoryName(Environment.ProcessPath) + @$"\{configurationFileName}";
                }
                else
                {
                    return ApplicationDataManager.CreateForPackageFamily(Package.Current.Id.FamilyName).LocalCacheFolder.Path + $@"\{configurationFileName}";
                }
            }
        }

        public async Task<AppConfiguration> LoadAsync()
        {
            try
            {
                string json = await File.ReadAllTextAsync(ConfigurationFullPath).ConfigureAwait(false);
                var obj = JsonHelper.ObjectFromJsonString<AppConfiguration>(json);
                return obj;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{nameof(ConfigurationService)}.{nameof(LoadAsync)}] 加载配置文件失败，创建默认实例（{ex.Message}）。");
                return new AppConfiguration();
            }
        }

        public async Task SaveAsync(AppConfiguration config)
        {
            string text = config.ToJsonString();
            await File.WriteAllTextAsync(ConfigurationFullPath, text).ConfigureAwait(false);
        }
    }
}
