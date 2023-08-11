using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class PlayerData
    {
        public int Coins;
        public int Health;
        public bool IsArmed;

        public PlayerData Clone()
        {
            var jsonClone = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(jsonClone);
        }
    }
}

