using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public static class EventBus
{
    public static Action OnSessionStarted;

    public static Action OnRunStarted;
    public static Action OnRunPaused;
    public static Action OnRunEnded;

    public static Action OnPlayerDied;

}
