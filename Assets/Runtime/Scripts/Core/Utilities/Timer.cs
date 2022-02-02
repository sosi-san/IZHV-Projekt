using System;
using UnityEngine;

namespace Woska.Core
{
    public class Timer
    {
        public float Duration { get; private set; }
        public float RemainingTime { get; private set; }
        public int RemainingTimeCeil=> Mathf.CeilToInt(RemainingTime);
        public bool IsRunning { get; private set; }
        public event Action OnTimerEnd;
    
        public Timer(float duration)
        {
            Duration = duration;
            Start();
        }
        public void Start()
        {
            RemainingTime = Duration;
            IsRunning = true;
        }
        public void Reset()
        {
            RemainingTime = Duration;
        }
        public void Tick(float deltaTime)
        {
            if(!IsRunning) return;

            RemainingTime -= deltaTime;
            CheckForTimerEnd();
        }

        private void CheckForTimerEnd()
        {
            if(RemainingTime > 0f) return;
        
            RemainingTime = 0f;
            IsRunning = false;
        
            OnTimerEnd?.Invoke();
        }
    
    }
}
