using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howon.RhythmGame
{
    [System.Serializable]
    public class Beat
    {
        public float ariavalTime; // �� ��Ʈ�� ������ ���� �ð�(ex. 1.5�� : 1.5, 30�� : 30)
        public EKey eKey = EKey.Space;
        public ENoteType eNoteType = ENoteType.Normal;
    }
}

