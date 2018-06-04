using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConfigHelper
{
  public enum ConfigFileFormatType
  {
    Json,
    CSV
  }

  public class ConfigControl : MonoBehaviour, ISerializationCallbackReceiver
  {
    public List<ConfigData> configsList = new List<ConfigData>();
    private Dictionary<string, ConfigData> dicJsonConfigData = new Dictionary<string, ConfigData>();
    private Dictionary<string, ConfigData> dicCSVConfigData = new Dictionary<string, ConfigData>();

    public List<ConfigData> GetConfigData(ConfigFileFormatType configType)
    {
      switch (configType)
      {
        case ConfigFileFormatType.Json:
          return this.dicJsonConfigData.Values.ToList();
        case ConfigFileFormatType.CSV:
          return this.dicCSVConfigData.Values.ToList();
        default:
          break;
      }
      return null;
    }

    public ConfigData GetConfigData(ConfigFileFormatType configType, string name)
    {
      string tmpName = $".{configType.ToString().ToLower()}";
      int tmpIndex = name.LastIndexOf(tmpName);
      if (tmpIndex == -1)
      {
        name = name + tmpName;
      }
      ConfigData configData = null;
      switch (configType)
      {
        case ConfigFileFormatType.Json:
          if (this.dicJsonConfigData.TryGetValue(name, out configData))
            return configData;
          break;
        case ConfigFileFormatType.CSV:
          if (this.dicCSVConfigData.TryGetValue(name, out configData))
            return configData;
          break;
        default:
          break;
      }
      return null;
    }

    public void OnAfterDeserialize()
    {
      foreach (var item in configsList)
      {
        switch (item.configFileFormatType)
        {
          case ConfigFileFormatType.Json:
            this.dicJsonConfigData.Add(item.fileName, item);
            break;
          case ConfigFileFormatType.CSV:
            this.dicCSVConfigData.Add(item.fileName, item);
            break;
          default:
            break;
        }
      }
    }

    public void OnBeforeSerialize()
    {

    }
  }


  [Serializable]
  public class ConfigData
  {
    public TextAsset configFile;

    public string fileName;

    public string path;

    public ConfigFileFormatType configFileFormatType;

  }

}
