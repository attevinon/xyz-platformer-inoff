using System;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class PlayerData
    {
        public int Coins;
        public int Health;
        public bool IsArmed;
        public int Projectiles;

        public PlayerData Clone()
        {
            var jsonClone = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(jsonClone);
        }
    }
}

