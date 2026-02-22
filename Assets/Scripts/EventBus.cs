using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public static class EventBus
{
    //Session
    public static Action OnSessionStarted;

    //GameStates
    public static Action OnRunStarted;
    public static Action OnRunPaused;
    public static Action<int,int> OnRunEnded;

    //PlayerStates
    public static Action<int, bool> OnHealthChanged;
    public static Action<int> OnPointsChanged;
    public static Action<string, string> ChangeSkeletonAnim;
}
