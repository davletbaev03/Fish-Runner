using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    private const string CHANNEL_ID = "inactivity_channel";

    private DateTime _lastTimeActive = DateTime.Now;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        RegisterChannel();
    }

    private void RegisterChannel()
    {
        var channel = new AndroidNotificationChannel
        {
            Id = CHANNEL_ID,
            Name = "Inactivity Notifications",
            Importance = Importance.Default,
            Description = "Notifications about inactivity"
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            ScheduleNotification();
            _lastTimeActive = DateTime.Now;
        }
        else
        {
            CancelNotification();
        }
    }

    private void OnApplicationQuit()
    {
        _lastTimeActive = DateTime.Now;
        ScheduleNotification();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (_lastTimeActive - DateTime.Now < TimeSpan.FromMinutes(15))
            AndroidNotificationCenter.CancelAllScheduledNotifications();
    }
    private void ScheduleNotification()
    {
        CancelNotification();

        var notification = new AndroidNotification
        {
            Title = "Ты где?",
            Text = "Давно тебя не было в рыбьих гонках, заходи!",
            FireTime = DateTime.Now.AddMinutes(15)
        };

        AndroidNotificationCenter.SendNotification(notification, CHANNEL_ID);
    }

    private void CancelNotification()
    {
        AndroidNotificationCenter.CancelAllScheduledNotifications();
    }
}
