using ConfigHelper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoad : MonoBehaviour
{

  public ConfigControl cf;

  private void Awake()
  {
    //CSVMapper.Instance.ToObject<TestCSV>(Application.dataPath, "Test.csv");
    //foreach (var items in CSVMapper.Instance.dataList)
    //{
    //  foreach (var item in items)
    //  {
    //    Debug.Log(item);
    //  }
    //}

    //List<TestCSV> tmp = CSVMapper.Instance.ToObject<TestCSV>("ID,Name,Level,Attack,Power,Agility,Stamina\n 1, 木刀, 0, 1, 1, 0, 0.1\n 2, 铜剑, 5, 8, 3, 4, 1.5\n 3, 铁剑, 10, 12, 2, 8, 2.1\n4, 黑铁剑, 15, 20, 10, 2, 2.2");
    //Debug.Log(CSVMapper.Instance.ToCSV(tmp));
    //foreach (var item in tmp)
    //{
    //  Debug.Log($"{item.ID},{item.Name},{item.Level},{item.Agility},{item.Power},{item.Agility},{item.Stamina}");
    //}
    List<TestCSV> tmpList = null;
    foreach (var item in cf.configsList)
    {
      switch (item.configFileFormat)
      {
        case ConfigFileFormat.Json:
          break;
        case ConfigFileFormat.CSV:
          Debug.Log(item.configFile.text);
          //tmpList = CSVMapper.Instance.ToObject<TestCSV>(item.configFile.text);

          tmpList = CSVMapper.Instance.LoadToObject<TestCSV>(item.path);


          break;
      }
    }

    if (tmpList != null)
    {
      foreach (var item in tmpList)
      {
        Debug.Log($"{item.id},{item.name},{item.level},{item.attack},{item.power},{item.agility},{item.stamina}");
      }
    }
  }

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
}
