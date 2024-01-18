using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Howon.RhythmGame
{
    public class Note : MonoBehaviour
    {
        private float _speed = 10f; // 1초에 설정한 유닛만큼 이동
        private EKey _eKey = EKey.Space;
        private ENoteType _eNoteType = ENoteType.Normal;

        private readonly Vector3[] _initPositions =
        {
        new Vector3(-4, 5, 0), new Vector3(-2, 5, 0),
        new Vector3(0, 5, 0),  new Vector3(2, 5, 0),
        new Vector3(4, 5, 0)
    };

        public void InitAndGo(EKey eKey, ENoteType eNoteType, float speed)
        {
            _eKey = eKey;
            _eNoteType = eNoteType;
            _speed = speed;
            transform.position = _initPositions[(int)_eKey];

            StartCoroutine(Move());
        }

        IEnumerator Move()
        {
            while (true)
            {
                transform.position += Vector3.down * _speed * Time.deltaTime;
                yield return null;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("JudgementBarGood"))
            {
                EventManager.instance.onJudgement(gameObject, _eKey, _eNoteType, EJudgeTiming.Good);
            }
            else if (collision.CompareTag("JudgementBarGreat"))
            {
                EventManager.instance.onJudgement(gameObject, _eKey, _eNoteType, EJudgeTiming.Great);
            }
            else if (collision.CompareTag("JudgementBarBad"))
            {
                EventManager.instance.onJudgement(gameObject, _eKey, _eNoteType, EJudgeTiming.Bad);
            }
        }
    }
}
