using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

using UnityEngine;

namespace FishRunner.Systems
{
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
            #if UNITY_ANDROID
            var channel = new AndroidNotificationChannel
            {
                Id = CHANNEL_ID,
                Name = "Inactivity Notifications",
                Importance = Importance.Default,
                Description = "Notifications about inactivity"
            };

            AndroidNotificationCenter.RegisterNotificationChannel(channel);
            #endif
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
            #if UNITY_ANDROID
            if (_lastTimeActive - DateTime.Now < TimeSpan.FromMinutes(15))
                AndroidNotificationCenter.CancelAllScheduledNotifications();
            #endif
        }
        private void ScheduleNotification()
        {
            #if UNITY_ANDROID
            CancelNotification();

            var notification = new AndroidNotification
            {
                Title = "Ты где?",
                Text = "Давно тебя не было в рыбьих гонках, заходи!",
                FireTime = DateTime.Now.AddMinutes(15)
            };

            AndroidNotificationCenter.SendNotification(notification, CHANNEL_ID);
            #endif
        }

        private void CancelNotification()
        {
            #if UNITY_ANDROID
            AndroidNotificationCenter.CancelAllScheduledNotifications();
            #endif
        }
    }
}