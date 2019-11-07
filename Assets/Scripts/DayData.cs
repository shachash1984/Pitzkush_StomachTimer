using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

[System.Serializable]
public struct DayData 
{
    public int day;
    public int month;
    public int year;
    public float stomachTime;

    public DayData(DayData other)
    {
        day = other.day;
        month = other.month;
        year = other.year;
        stomachTime = other.stomachTime;
    }

    public static string Save(DayData dd)
    {
        return JsonUtility.ToJson(dd);
    }

    public static DayData Load(string data)
    {
        return JsonUtility.FromJson<DayData>(data);
    }

    public string GetData()
    {
        return string.Format("{0}/{1}/{2}: {3}", day, month, year, Timer.ToString(stomachTime));
    }

    public bool Equals(DayData other)
    {
        return other.year == this.year && other.month == this.month && other.day == this.day;
    }
}

[System.Serializable]
public class DayDataCollection
{
    [SerializeField]
    public List<DayData> days = new List<DayData>();

    public string Save()
    {
        string data = JsonUtility.ToJson(this);
        return data;
    }

    public void Load(string data)
    {
        DayDataCollection ddc = JsonUtility.FromJson<DayDataCollection>(data);
        foreach (DayData dd in ddc.days)
        {
             if (!days.Contains(dd))
                days.Add(dd);
        }
    }

    public bool Add(DayData dd)
    {
        if(days.Count == 0)
        {
            days.Add(dd);
            return true;
        }
        else
        {
            foreach (DayData day in days)
            {
                if (day.Equals(dd))
                {
                    if (dd.stomachTime <= day.stomachTime)
                        return false;
                    else
                    {
                        int index = days.IndexOf(day);
                        DayData newData = new DayData(dd);
                        days[index] = newData;
                        return true;
                    }
                }
                    
            }
            days.Add(dd);
            return true;
        }
    }
}
