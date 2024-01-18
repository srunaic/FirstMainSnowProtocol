using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace Howon.RhythmGame
{
    public class EventManager
    {
        public static readonly EventManager instance = new EventManager();

        private EventManager() { }

        public Action<GameObject, EKey, ENoteType, EJudgeTiming> onJudgement;
        public Action<int> onDamage;
        public Action<int> onRecover;
        public Action<AsyncOperationHandle> onLoadScriptableData;
        public Action onGameOver;
        public Action onReleaseAsset;
    }
}

