using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howon.RhythmGame
{
    public class GameOverPanel : MonoBehaviour
    {
        void Start()
        {
            Debug.Log($"score : {ShareDataManager.instance.Score}");
            Debug.Log($"grade : {ShareDataManager.instance.Grade}");
            Debug.Log($"numGood : {ShareDataManager.instance.NumGood}");
            Debug.Log($"numGreat : {ShareDataManager.instance.NumGreat}");
            Debug.Log($"numBad : {ShareDataManager.instance.NumBad}");
            Debug.Log($"numMiss : {ShareDataManager.instance.NumMiss}");
        }

        void Update()
        {

        }
    }
}

