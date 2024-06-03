using System;
using UnityEngine;

namespace PixelCrew.Utils
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField] private float value;

        private float _timesUp;
        public void Reset()
        {
            _timesUp = Time.time + value;
        }

        public bool IsReady => Time.time >= _timesUp;
}
}
