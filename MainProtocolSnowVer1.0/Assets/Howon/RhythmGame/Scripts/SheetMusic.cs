using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howon.RhythmGame
{
    [CreateAssetMenu(fileName = "SheetMusic",
                 menuName = "Scriptable Object/SheetMusic",
                 order = int.MaxValue)]
    public class SheetMusic : ScriptableObject // �Ǻ�
    {
        public string titleName;
        public string artist;
        public float playTime; // �� ���� �ð� (�ʴ���)
        public float noteSpeed = 5f; // �ʴ� ���ִ��� �ӵ� (ex. 5f : �ʴ� 5���� ������)
        public List<Beat> beats = new List<Beat>(); // ���� �� ��Ʈ(��Ʈ��)
    }
}
