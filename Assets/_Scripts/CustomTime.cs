using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomClasses
{

    public interface ITime
    {
       public float personalTimeScale { get; set; }
    }
    public class CustomTime 
    {
        private static float areaTimeScale = Time.timeScale;

        public static float deltaTime => Time.deltaTime * areaTimeScale;
        public static float timeScale => areaTimeScale;
        public static float time  => Time.time;

        //Make The Rest as needed
    
        public void SetTimeScale(float newTimeScale)
        {
            areaTimeScale = newTimeScale;
        }

        public float GetTimeScale(ITime personalTime)
        {
            return timeScale * personalTime.personalTimeScale;
        }
    
    }
}
   
 