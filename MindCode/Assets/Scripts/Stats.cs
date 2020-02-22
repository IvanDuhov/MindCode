using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Stats : MonoBehaviour
{
    public Text allTimeLabel;
    public Text percentage;
    public Text averageTimes;

    void Start()
    {
        StatObject so = MultiplayerManager.ReadLocalDBJson(MultiplayerManager.localDBPath);

        float winrate;

        if (so.totalGames != 0)
        {
            winrate = so.wonGames / so.totalGames * 100;
        }
        else
        {
            winrate = 0;
        }

        if (winrate < 50)
        {
            percentage.color = Color.red;
        }

        percentage.text = string.Format("{0:00.00}%", winrate);

        float averageTime = so.totalSeconds / so.wonGames;
        float averageOrders = so.totalOrders / so.wonGames;

        if (PlayerPrefs.GetString("English") == "true")
        {
            allTimeLabel.text = "Win rate:";
            averageTimes.text = string.Format("Average time needed to provide a solution: {0:0.00}s\nAverage orders needed to provide a solution: {1:0}", averageTime, averageOrders);
        }
        else
        {
            allTimeLabel.text = "Отношение печалба:";
            averageTimes.text = string.Format("Средно време за решение: {0:0.00}s\nСреден брой команди: {1:0}", averageTime, averageOrders);
        }
    }
}
