using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ConfigHelper
{
  public partial class CSVMapper
  {
    #region Object To String

    /// <summary>
    /// 将CSV对象列表转换为CSV字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objectList">CSV对象列表</param>
    /// <returns></returns>
    public string ToCSV<T>(List<T> objectList) where T : class
    {
      StringBuilder stringBuilder = new StringBuilder();
      FieldInfo[] fi = objectList[0].GetType().GetFields();
      for (int i = 0; i < fi.Length; i++)
      {
        var item = fi[i];
        stringBuilder.Append(item.Name);
        if (i == fi.Length - 1) stringBuilder.Append("\n");
        else stringBuilder.Append(",");
      }
      for (int i = 0; i < objectList.Count; i++)
      {
        var item = objectList[i];
        var tmp = item.GetType().GetFields();
        for (int j = 0; j < tmp.Length; j++)
        {
          stringBuilder.Append(tmp[j].GetValue(item).ToString());
          if (j == tmp.Length - 1) stringBuilder.Append("\n");
          else stringBuilder.Append(",");
        }
      }
      return stringBuilder.ToString();
    }

    #endregion

  }
}
