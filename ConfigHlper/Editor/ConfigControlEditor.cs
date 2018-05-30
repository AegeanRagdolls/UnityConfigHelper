using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace ConfigHelper
{
  [CustomEditor(typeof(ConfigControl))]
  public class ConfigControlEditor : Editor
  {
    private ConfigControl cc;
    private Rect rect;
    private string assetObj;
    protected SerializedObject sObj;
    protected SerializedProperty sProp;

    private void OnEnable()
    {
      this.cc = (ConfigControl)target;

      this.sObj = new SerializedObject(cc);
      this.sProp = this.sObj.FindProperty("configsList");
    }

    public override void OnInspectorGUI()
    {
      EditorGUILayout.Space();
      EditorGUILayout.Space();

      cc.configMehtod = (SwitchConfigLoadMethod)EditorGUILayout.EnumPopup("加载方式 : ", this.cc.configMehtod);

      EditorGUILayout.Space();
      EditorGUILayout.Space();

      GUILayout.Label("↓↓ 请将文件拖拽到这里  ↓↓");
      this.rect = EditorGUILayout.GetControlRect(GUILayout.Height(30));
      EditorGUI.TextField(this.rect, assetObj);
      if ((Event.current.type == EventType.DragUpdated
      || Event.current.type == EventType.DragExited)
      && rect.Contains(Event.current.mousePosition))
      {
        //改变鼠标的外表  
        DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
        if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
        {
          for (int i = 0; i < DragAndDrop.paths.Length; i++)
          {
            ConfigFile configFile = new ConfigFile();
            configFile.configFile = (TextAsset)DragAndDrop.objectReferences[i];
            configFile.path = DragAndDrop.paths[i];
            int index = DragAndDrop.paths[i].LastIndexOf("/");
            configFile.fileName = configFile.path.Substring(index + 1);

            switch (configFile.fileName.ToLower().Substring(configFile.fileName.LastIndexOf('.') + 1))
            {
              case "json":
                configFile.configFileFormat = ConfigFileFormat.Json;
                break;
              case "csv":
                configFile.configFileFormat = ConfigFileFormat.CSV;
                break;
            }
            ConfigFile[] tmp = this.cc.configsList.Where(v => v.configFile == configFile.configFile).ToArray();
            if (tmp.Length == 0)
            {
              this.cc.configsList.Add(configFile);
            }
          }

        }
      }

      EditorGUILayout.Space();
      EditorGUILayout.Space();
      this.sObj.Update();

      EditorGUI.BeginChangeCheck();
      EditorGUILayout.PropertyField(this.sProp, true);

      if (EditorGUI.EndChangeCheck())
      {
        this.sObj.ApplyModifiedProperties();
      }



    }



  }
}
