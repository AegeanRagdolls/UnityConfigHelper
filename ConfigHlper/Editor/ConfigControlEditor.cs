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
            ConfigData configData = new ConfigData();
            configData.configFile = (TextAsset)DragAndDrop.objectReferences[i];
            configData.path = DragAndDrop.paths[i];
            int index = DragAndDrop.paths[i].LastIndexOf("/");
            configData.fileName = configData.path.Substring(index + 1);

            switch (configData.fileName.ToLower().Substring(configData.fileName.LastIndexOf('.') + 1))
            {
              case "json":
                configData.configFileFormatType = ConfigFileFormatType.Json;
                break;
              case "csv":
                configData.configFileFormatType = ConfigFileFormatType.CSV;
                break;
            }
            ConfigData[] tmp = this.cc.configsList.Where(v => v.configFile == configData.configFile).ToArray();
            if (tmp.Length == 0)
            {
              this.cc.configsList.Add(configData);
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
