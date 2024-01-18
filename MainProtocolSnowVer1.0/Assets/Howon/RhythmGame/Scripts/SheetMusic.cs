using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howon.RhythmGame
{
    [CreateAssetMenu(fileName = "SheetMusic",
                 menuName = "Scriptable Object/SheetMusic",
                 order = int.MaxValue)]
    public class SheetMusic : ScriptableObject // 악보
    {
        public string titleName;
        public string artist;
        public float playTime; // 총 곡의 시간 (초단위)
        public float noteSpeed = 5f; // 초당 유닛단위 속도 (ex. 5f : 초당 5유닛 움직임)
        public List<Beat> beats = new List<Beat>(); // 곡의 총 비트(노트수)
    }
}
