using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howon.RhythmGame
{
    public class ShareDataManager
    {
        public static readonly ShareDataManager instance = new ShareDataManager();

        private ShareDataManager() { }

        private string title = string.Empty; // 노래 타이틀
        private int noteSpeedTimes = 1; // 노트 배속
        private ResultData resultData;  // 게임 결과 데이터

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public int NoteSpeedTimes
        {
            get { return noteSpeedTimes; }
            set { noteSpeedTimes = value; }
        }

        public ResultData RetData { get { return resultData; } }

        public int TotalScore
        {
            get { return resultData.totalScore; }
            set { resultData.totalScore = value; }
        }

        public int StageScore
        {
            get { return resultData.stageScore; }
            set { resultData.stageScore = value; }
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
