using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ConfigHelper
{
  public enum SwitchConfigLoadMethod
  {
    Auto,
    path,
    TextAsset
  }

  public enum ConfigFileFormat
  {
    Json,
    CSV
  }

  public class ConfigControl : MonoBehaviour
  {
    public SwitchConfigLoadMethod configMehtod;

    public List<ConfigFile> configsList = new List<ConfigFile>();

    public List<TextAsset> textAssetList = new List<TextAsset>();

  }


  [Serializable]
  public class ConfigFile
  {
    public TextAsset configFile;

    public string fileName;

    public string path;

    public ConfigFileFormat configFileFormat;

  }

}
