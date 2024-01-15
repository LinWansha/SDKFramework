using Habby.CNUser;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserGachaData 
{
    public List<UserDataEntry> gachaCount;

    public UserGachaData()
    {
        gachaCount = new List<UserDataEntry>();
    }

    public void Refresh()
    {
        gachaCount.Clear();
    }

    public void Add(string name, int value)
    {
        if (name == null || name.Length == 0) return;
        string key = string.Format(name, DateTime.Now.DayOfYear);

        for (int i = 0; i < gachaCount.Count; ++i) {
            if (key.Equals(gachaCount[i].name)) {
                gachaCount[i].value += value;
                return;
            }
        }

        UserDataEntry data = new UserDataEntry {
            name = key,
            value = value
        };
        gachaCount.Add(data);
    }

    public int Get(string name)
    {
        if (name == null || name.Length == 0) return 0;
        string key = string.Format(name, DateTime.Now.DayOfYear);
        for (int i = 0; i < gachaCount.Count; ++i) {
            if (key.Equals(gachaCount[i].name)) {
                return gachaCount[i].value;
            }
        }

        return 0;
    }
}
