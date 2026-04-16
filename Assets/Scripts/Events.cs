using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishRunner.Events
{
    //GameStates
    public struct OnRunStarted { };
    public struct OnRunPaused { };
    public struct OnRunUnpaused { };
    public struct OnRunEnded
    {
        public int Distance;
        public int Food;
    };

    //PlayerStates
    public struct OnHealthChanged
    {
        public int HealthPoints;
        public bool InstantDeath;
    };
    public struct OnPointsChanged
    {
        public int Points;
    }

    public struct ChangeSkeletonAnim
    {
        public string Anim1;
        public string Anim2;
    }
}