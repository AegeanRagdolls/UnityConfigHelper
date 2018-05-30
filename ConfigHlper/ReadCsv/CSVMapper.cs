using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConfigHelper
{
  public partial class CSVMapper
  {
    private static CSVMapper instance;
    public List<string[]> dataList;

    public static CSVMapper Instance
    {
      get
      {
        return instance ?? (instance = new CSVMapper());
      }
    }

    private CSVMapper()
    {
      this.dataList = new List<string[]>();
    }

    #region SCV To Object || SCV To String

    /// <summary>
    /// 通过路径加载SCV并转换至Object
    /// </summary>
    /// <typeparam name="T">转换的类型</typeparam>
    /// <param name="path">加载的路径</param>
    /// <param name="fileName">文件名（需要携带扩展名）</param>
    /// <returns></returns>
    public List<T> LoadToObject<T>(string path, string fileName = null) where T : class
    {
      string tmp = path;
      if (path.ToCharArray()[path.Length - 1] == '/')
      {
        tmp = path.Substring(0, path.Length - 1);
      }
      if (fileName != null)
      {
        this.CSVRead($"{tmp}/{fileName}");
      }
      this.CSVRead($"{tmp}");
      return this.SCVToObject<T>();
    }

    /// <summary>
    /// 将字符串SCV转换至Object
    /// </summary>
    /// <typeparam name="T">转换的类型</typeparam>
    /// <param name="CSVContenter">字符串SCV内容</param>
    /// <returns></returns>
    public List<T> ToObject<T>(string CSVContenter) where T : class
    {
      this.SplitCSVContent(CSVContenter);
      return this.SCVToObject<T>();
    }


    /// <summary>
    /// 获取某个Key下面所有的值
    /// </summary>
    /// <param name="Key"></param>
    /// <param name="csvData"></param>
    /// <returns></returns>
    public string GetAllValue(string Key, List<string[]> csvData)
    {
      string value = null;
      int index = -1;
      for (int i = 0; i < csvData[0].Length; i++)
      {
        string item = csvData[0][i];
        if (item == Key)
        {
          index = i;
        }
      }
      if (index == -1) { Debug.LogError("没有找到这个Key"); return null; }
      for (int i = 1; i < this.dataList.Count; i++)
      {
        value += this.dataList[i][index] + ",";
      }

      return value;
    }

    /// <summary>
    /// 获取某个Key下面所有的值(请确保已经手动加载了SCV)
    /// </summary>
    /// <param name="Key"></param>
    /// <returns></returns>
    public string GetAllValue(string Key)
    {
      if (this.dataList == null || this.dataList.Count == 0)
      {
        Debug.LogError("当前缓存列表中 没有值 请先加载SCV后在进行获取值等操作 : CSVReadPath(string path, string fileName)");
        return null;
      }
      string value = null;
      int index = -1;
      for (int i = 0; i < this.dataList[0].Length; i++)
      {
        string item = this.dataList[0][i];
        if (item == Key)
        {
          index = i;
        }
      }
      if (index == -1) { Debug.LogError("没有找到这个Key"); return null; }
      for (int i = 1; i < this.dataList.Count; i++)
      {
        value += this.dataList[i][index] + ",";
      }

      return value;
    }


    /// <summary>
    /// 手动加载CSV
    /// </summary>
    /// <param name="path">加载的路径</param>
    /// <param name="fileName">文件名（需要携带扩展名</param>
    public List<string[]> CSVRead(string path)
    {
      this.dataList.Clear();
      try
      {
        using (StreamReader sr = new StreamReader($"{path}"))
        {
          string tmp;

          while ((tmp = sr.ReadLine()) != null)
          {
            this.dataList.Add(tmp.Split(','));
          }
        }
      }
      catch (System.Exception e)
      {
        Debug.LogError(e);
      }
      return this.dataList;
    }

    private void SplitCSVContent(string CSVContenter)
    {
      string[] tmpStr = Regex.Split(CSVContenter, "\n");
      for (int i = 0; i < tmpStr.Length; i++)
      {
        tmpStr[i] = tmpStr[i].Replace("\r", "");
      }
      foreach (var item in tmpStr)
      {
        this.dataList.Add(item.Split(','));
      }

    }

    private List<T> SCVToObject<T>() where T : class
    {
      List<T> tmpList = new List<T>();
      for (int j = 1; j < this.dataList.Count; j++)
      {
        T t = Activator.CreateInstance(typeof(T)) as T;
        FieldInfo[] mis = typeof(T).GetFields();
        int i = 0;
        foreach (var item in this.dataList[0])
        {
          List<FieldInfo> str = mis.Where(v => v.Name.ToLower() == item.ToLower()).ToList();
          if (str == null || str.Count == 0)
          {
            Debug.LogError($"转换错误 ： 变量名出线异常 {item}");
            return null;
          }
          if (j > this.dataList.Count) j = this.dataList.Count;
          object obj = Convert.ChangeType(this.dataList[j][i], str[0].FieldType);
          str[0].SetValue(t, obj);
          i++;
        }
        tmpList.Add(t);
      }
      return tmpList;
    }
    #endregion

  }
}

