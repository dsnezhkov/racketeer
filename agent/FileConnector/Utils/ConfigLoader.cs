using FileConnector.Models;
using FileConnectorCommon;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Web.Script.Serialization;

namespace FileConnector.Utils
{
    class ConfigLoader
    {
        private Logger log;
        public ConfigLoader()
        {
            log = Config.ConfigLog.getLog();
        }
        public bool Load()
        {
            Models.DriverConfig configTree = new Models.DriverConfig();
            string config = String.Empty;

            switch (Config.ConfigFile.location)
            {
                case Location.External:
                    log.Debug($"Loading config from file: {Config.ConfigFile.cfgFile}");
                    try
                    {

                        config = System.IO.File.ReadAllText(Config.ConfigFile.cfgFile);
                        Config.ConfigFile.setCfg(deserCfg(config));
                    }catch (Exception e)
                    {
                        log.Error($"Config file error: {e.Message} {e.InnerException?.Message}");
                        return false;
                    }
                    break;
                case Location.Embedded:

                    // _dumpEmbRes();
                    log.Debug("Loading config from embedded resources");
                    var assembly = Assembly.GetExecutingAssembly();
                    var resourceName = Config.ConfigFile.cfgFile;

                    try
                    {
                        Stream stream = assembly.GetManifestResourceStream(resourceName);
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            config = reader.ReadToEnd();
                        }
                        Config.ConfigFile.setCfg(deserCfg(config));
                    }catch(Exception e)
                    {
                        log.Error($"Resource file error: {e.Message}");
                        _dumpEmbRes();
                        Config.ConfigFile.setCfg(null);
                        return false;
                    }
                    break;
                case Location.Network:
                    log.Debug("Loading config from network");
                    Config.ConfigFile.setCfg(deserCfg(Config.ConfigFile.cfgFile));
                    break;
                default:
                    Config.ConfigFile.setCfg(null);
                    log.Error($"Unknown type of config file location");
                    break;
            }
            return true;

        }
        private void _dumpEmbRes()
        {
            log.Debug("Getting sesources");
            var resources = this.GetType().Assembly.GetManifestResourceNames();
            foreach (var res in resources)
            {
               log.Debug($" - > Resource: {res}");
            }
        }

        private DriverConfig deserCfg(string config)
        {
            Models.DriverConfig configTree;

            try
            {
                log.Debug("Deserializing config");
                // configTree = new JavaScriptSerializer().Deserialize<Models.DriverConfig>(config);
                configTree = JsonConvert.DeserializeObject<DriverConfig>(config,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        Formatting = Formatting.Indented
                    });

                log.Debug($"Config tree populated");
            }
            catch (Exception e)
            {
                log.Error($"Unable to load configuration: {e.Message} : {e}");
                return null;
            }
            return configTree;
        }
    }
}
