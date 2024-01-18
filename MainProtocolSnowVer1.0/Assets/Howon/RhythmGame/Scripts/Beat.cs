using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howon.RhythmGame
{
    [System.Serializable]
    public class Beat
    {
        public float ariavalTime; // 각 노트가 판정바 도달 시간(ex. 1.5초 : 1.5, 30초 : 30)
        public EKey eKey = EKey.Space;
        public ENoteType eNoteType = ENoteType.Normal;
    }
}

