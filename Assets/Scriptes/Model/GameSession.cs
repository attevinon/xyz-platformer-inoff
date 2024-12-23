﻿using UnityEngine;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _save;
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;

        void Awake()
        {
            if(IsSessionExists())
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Save();
                DontDestroyOnLoad(this);
            }
        }

        private bool IsSessionExists()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var session in sessions)
            {
                if (session != this)
                    return true;
            }

            return false;
        }

        public void Save()
        {
            _save = _data.Clone();
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();
        }
    }
}

