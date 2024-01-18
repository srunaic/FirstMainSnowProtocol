using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howon.RhythmGame
{
    public class ShareDataManager
    {
        public static readonly ShareDataManager instance = new ShareDataManager();

        private ShareDataManager() { }

        private string title = string.Empty;
        private ResultData resultData;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public ResultData RetData { get { return resultData; } }

        public int Score
        {
            get { return resultData.score; }
            set { resultData.score = value; }
        }

        public EGrade Grade
        {
            get { return resultData.eGrade; }
            set { resultData.eGrade = value; }
        }

        public int NumGood
        {
            get { return resultData.numGood; }
            set { resultData.numGood = value; }
        }
        public int NumGreat
        {
            get { return resultData.numGreat; }
            set { resultData.numGreat = value; }
        }

        public int NumBad
        {
            get { return resultData.numBad; }
            set { resultData.numBad = value; }
        }

        public int NumMiss
        {
            get { return resultData.numMiss; }
            set { resultData.numMiss = value; }
        }
    }
}
