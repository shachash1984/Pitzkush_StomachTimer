using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    const string DAYS = "Days";
    const string RUNNING = "הפסק";
    const string STOPPED = "התחל";
    Timer currentTimer = default;
    Timer dailyTimer = default;

    [SerializeField] TextMeshProUGUI currentTimerText = default;
    [SerializeField] TextMeshProUGUI dailyTimerText = default;
    [SerializeField] TextMeshProUGUI timerButtonText = default;

    DayDataCollection dayDatas = default;

    [SerializeField] Transform contentWindow = default;
    [SerializeField] GameObject dayDataLinePrefab = default;
    

    private void Awake()
    {
        currentTimer = new Timer();
        dailyTimer = new Timer();
        currentTimerText.text = currentTimer.ToString();
        dayDatas = new DayDataCollection();
        if (PlayerPrefs.HasKey(DAYS))
        {
            RefreshTable();
            foreach (DayData dayData in dayDatas.days)
            {
                if(dayData.day == DateTime.Today.Day)
                {
                    dailyTimer.Tick(dayData.stomachTime);
                }
            }
            
        }
        else
        {
            dailyTimer.ResetTimer();
        }
        dailyTimerText.text = dailyTimer.ToString();
        
        
    }

    private void Update()
    {
        if (currentTimer.IsRunning)
        {
            currentTimer.Tick(Time.deltaTime);
            currentTimerText.text = currentTimer.ToString();
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerPrefs.DeleteAll();
        }
#endif

    }

    public void ToggleTimer()
    {
        bool on = !currentTimer.IsRunning;
        if(on)
        {
            currentTimer.Run();
            timerButtonText.text = RUNNING;
        }
        else
        {
            currentTimer.Stop();
            AcumluateTime();
            currentTimer.ResetTimer();
            currentTimerText.text = currentTimer.ToString();
            timerButtonText.text = STOPPED;
        }
    }

    private void AcumluateTime()
    {
        dailyTimer.Tick(currentTimer.Elapsed);
        PlayerPrefs.SetFloat(System.DateTime.Today.ToString(), dailyTimer.Elapsed);
        dailyTimerText.text = dailyTimer.ToString();
        DayData dayData = new DayData();
        dayData.day = DateTime.Today.Day;
        dayData.month = DateTime.Today.Month;
        dayData.year = DateTime.Today.Year;
        dayData.stomachTime = dailyTimer.Elapsed;
        if (dayDatas.Add(dayData))
        {
            PlayerPrefs.SetString(DAYS, dayDatas.Save());
        }
        RefreshTable();
    }

    public void RefreshTable()
    {
        dayDatas.Load(PlayerPrefs.GetString(DAYS));
        for (int i = 0; i < contentWindow.childCount; i++)
        {
            Destroy(contentWindow.GetChild(i).gameObject);
        }

        foreach (DayData dayData in dayDatas.days)
        {
            //Add line to table
            TextMeshProUGUI t = Instantiate(dayDataLinePrefab, contentWindow).GetComponentInChildren<TextMeshProUGUI>();
            t.text = dayData.GetData();
        }
    }
}
