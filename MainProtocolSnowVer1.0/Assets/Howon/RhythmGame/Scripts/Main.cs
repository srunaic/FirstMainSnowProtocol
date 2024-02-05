using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Howon.RhythmGame;

namespace Howon.RhythmGame
{
    [System.Serializable]
    public struct KeyButton
    {
        public SpriteRenderer button; // ���ε� Ű �̹���(D, F, spacebar, J, K)
        public ParticleSystem particle;
        public KeyCode keyCode; // ���� Ű���� ���
        [HideInInspector] public bool isJudge;
    }

    public enum EKey
    {
        D = 0,
        F,
        Space,
        J,
        K
    }

    public enum ENoteType
    {
        Normal = 0, // �Ϲݳ�Ʈ
        Long        // �ճ�Ʈ(���� ��Ʈ)
    }

    public enum EJudgeTiming
    {
        Good = 0,
        Great,
        Bad,
        Miss
    }

    public enum EGrade // ������ ���� ���
    {
        S = 0,
        A,
        B,
        C,
        D,
        F
    }

    public struct ResultData
    {
        public int totalScore;
        public int stageScore;
        public EGrade eGrade;
        public int numGood;
        public int numGreat;
        public int numBad;
        public int numMiss;
    }

    public class Main : MonoBehaviour
    {
        [SerializeField] KeyButton[] _keyButtons; // Ű ����
        private SheetMusic _sheet;
        private GameObject _note;
        private SpriteRenderer _judgeTextSR;
        private SpriteRenderer _renderVideoPlayer;

        private MusicPlayer _musicPlayer;
        private VideoController _videoController;

        private Coroutine _coFlowNote = null;

        [SerializeField] private Sprite[] _spriteJudgeText; // 0:good, 1:great, 2:bad, 3:miss
        [SerializeField] private float _judgeTime = 0.1f;
        [SerializeField] private int _recoverGood = 8;
        [SerializeField] private int _recoverGreat = 20;
        [SerializeField] private int _recoverBad = 3;
        [SerializeField] private int _damage = 20;

        [SerializeField] private int _scoreGood = 7;
        [SerializeField] private int _scoreGreat = 10;
        [SerializeField] private int _scoreBad = 3;

        [SerializeField] private float _fadeoutSoundTime = 2f;

        private int _key = 0; // ������ �ʱ�ȭ ��Ű�� ���� �ӽú���
        private readonly float _judgePosY = 9.2f; // �������� ���� y��ǥ

        private AsyncOperationHandle _asyncNoteHandle;
        private AsyncOperationHandle _asyncScriptableHandle;

        private bool _isGameoverRoutine = false;

        private bool _isStartGame = false;

        private void Awake()
        {
            _videoController = transform.Find("VideoPlayer").GetComponent<VideoController>();
            _musicPlayer = transform.Find("MusicPlayer").GetComponent<MusicPlayer>();
            _judgeTextSR = transform.Find("ImgJudgeText").GetComponent<SpriteRenderer>();

            //EventManager.instance.onLoadScriptableData = ScriptableDataLoaded;

            for (int i = 0; i < _keyButtons.Length; i++)
            {
                _keyButtons[i].button.enabled = false;
                _keyButtons[i].isJudge = false;
            }
        }

        private void OnEnable()
        {
            if (!_isStartGame) return;

            Init();
        }

        private void Start()
        {
            if (!_isStartGame) _isStartGame = true;

            Init();
        }


        // ����� ���� ���� (Ÿ�̹��� ��¦ �ȸ���)
        void ScriptableDataLoaded(AsyncOperationHandle handle)
        {
            if (handle.Result != null)
            {
                _asyncScriptableHandle = handle;
                _sheet = (SheetMusic)_asyncScriptableHandle.Result;
                //_videoController.Play("GreenShining");
                _videoController.Play("MoonBackground");
                _musicPlayer.Play(_sheet.titleName);


                EventManager.instance.onJudgement = JudgeNote;
                EventManager.instance.onPreProcessGameOver = GameOver;

                for (int i = 0; i < _keyButtons.Length; i++)
                {
                    StartCoroutine(Input(i));
                }

                _coFlowNote = StartCoroutine(FlowNote());
            }
            else
            {
                // �ε忡 ������ ����� ó��
            }
        }

        void Init()
        {
            _asyncNoteHandle = ResourceManager.instance.LoadPrefab<GameObject>("Note");
            _note = (GameObject)_asyncNoteHandle.Result;
            _asyncScriptableHandle = ResourceManager.instance.LoadScriptableData<SheetMusic>(ShareDataManager.instance.Title);
            _sheet = (SheetMusic)_asyncScriptableHandle.Result;
            //_videoController.Play("GreenShining");
            _videoController.Play("MoonBackground");
            _musicPlayer.Play(_sheet.titleName);

            EventManager.instance.onJudgement = JudgeNote;
            EventManager.instance.onPreProcessGameOver = GameOver;

            for (int i = 0; i < _keyButtons.Length; i++)
            {
                StartCoroutine(Input(i));
            }

            _coFlowNote = StartCoroutine(FlowNote());
        }

        void GameOver()
        {
            StopCoroutine(_coFlowNote);
            SetGrade();
            if (!_isGameoverRoutine)
                StartCoroutine(FadeOutAndGameOverScene());
        }

        void SetGrade()
        {
            ResultData data = ShareDataManager.instance.RetData;

            int beatCount = _sheet.beats.Count;
            float convertedScore = (float)data.stageScore / (float)(_scoreGreat * beatCount); // ȯ������

            Debug.Log("convertedScore : " + convertedScore);
            if (convertedScore >= .95f) ShareDataManager.instance.Grade = EGrade.S;
            else if (convertedScore >= .75f) ShareDataManager.instance.Grade = EGrade.A;
            else if (convertedScore >= .65f) ShareDataManager.instance.Grade = EGrade.B;
            else if (convertedScore >= .55f) ShareDataManager.instance.Grade = EGrade.C;
            else if (convertedScore >= .45f) ShareDataManager.instance.Grade = EGrade.D;
            else ShareDataManager.instance.Grade = EGrade.F;
        }

        IEnumerator FadeOutAndGameOverScene()
        {
            _isGameoverRoutine = true;

            yield return new WaitForSeconds(1f);

            float currentTime = 0;
            float startVolume = _musicPlayer.GetVolume();

            while (currentTime < _fadeoutSoundTime)
            {
                currentTime += Time.deltaTime;
                _musicPlayer.SetVolume(Mathf.Lerp(startVolume, 0, currentTime / _fadeoutSoundTime));
                yield return null;
            }

            _musicPlayer.Stop();

            EventManager.instance.onCloseGameMain();
            _isGameoverRoutine = false;
        }

        private void OnDisable()
        {
            ReleaseAssets();
        }

        private void ReleaseAssets()
        {
            //ResourceManager.instance.ReleaseAsset(_asyncNoteHandle);
            ResourceManager.instance.ReleaseAsset(_asyncScriptableHandle);
            _musicPlayer.ReleaseMusic();
            _videoController.ReleaseVideo();
        }

        private void PlayParticle(EKey eKey)
        {
            _keyButtons[(int)eKey].particle.Play();
        }

        void JudgeNote(GameObject obj, EKey eKey, ENoteType eNoteType, EJudgeTiming eJudgeTiming)
        {
            if (eNoteType == ENoteType.Long &&
                _keyButtons[(int)eKey].button.enabled) // ��Ÿ���� ��Ʈ�� Ű��ư�� enabled�� �������� �Ǵ�
            {
                Destroy(obj);
                StartCoroutine(ShowJudgeText(EJudgeTiming.Great));
                PlayParticle(eKey);

                if (eJudgeTiming == EJudgeTiming.Good || // ��Ÿ���̸� good�� great�� ���� �������� ��.
                    eJudgeTiming == EJudgeTiming.Great)
                {
                    ShareDataManager.instance.TotalScore += _scoreGreat;
                    ShareDataManager.instance.StageScore += _scoreGreat;
                    ShareDataManager.instance.NumGreat++;
                    EventManager.instance.onRecover(_recoverGreat);
                }
                else if (eJudgeTiming == EJudgeTiming.Bad)
                {
                    ShareDataManager.instance.TotalScore += _scoreBad;
                    ShareDataManager.instance.StageScore += _scoreBad;
                    ShareDataManager.instance.NumBad++;
                    //EventManager.instance.onRecover(_recoverBad);
                }
            }
            else if (eNoteType == ENoteType.Normal &&
                    _keyButtons[(int)eKey].isJudge) // �븻 Ÿ���� ��Ʈ�� isJudge�� �������� �Ǵ�
            {
                _keyButtons[(int)eKey].isJudge = false;
                Destroy(obj);
                StartCoroutine(ShowJudgeText(eJudgeTiming));
                PlayParticle(eKey);

                if (eJudgeTiming == EJudgeTiming.Good)
                {
                    ShareDataManager.instance.TotalScore += _scoreGood;
                    ShareDataManager.instance.StageScore += _scoreGood;
                    ShareDataManager.instance.NumGood++;
                    //EventManager.instance.onRecover(_recoverGood);
                }
                else if (eJudgeTiming == EJudgeTiming.Great)
                {
                    ShareDataManager.instance.TotalScore += _scoreGreat;
                    ShareDataManager.instance.StageScore += _scoreGreat;
                    ShareDataManager.instance.NumGreat++;
                    EventManager.instance.onRecover(_recoverGreat);
                }
                else if (eJudgeTiming == EJudgeTiming.Bad)
                {
                    ShareDataManager.instance.TotalScore += _scoreBad;
                    ShareDataManager.instance.StageScore += _scoreBad;
                    ShareDataManager.instance.NumBad++;
                    //EventManager.instance.onRecover(_recoverBad);
                }
            }
        }

        IEnumerator FlowNote()
        {
            int i = 0;
            float startTime = 0;
            float arrivalTime;
            float delayTime;
            float prevTime = 0f;
            EKey eKey = EKey.Space;
            ENoteType eNoteType = ENoteType.Normal;
            float noteSpeed = _sheet.noteSpeed * ShareDataManager.instance.NoteSpeedTimes;

            GameObject newNote = null;
            Note note;

            while (i < _sheet.beats.Count) // ���� ��� ��Ʈ�� ���� ������
            {
                arrivalTime = _sheet.beats[i].ariavalTime;
                if (i == 0) startTime = CalculateStartTime(arrivalTime, _sheet.noteSpeed);

                eKey = _sheet.beats[i].eKey;
                eNoteType = _sheet.beats[i].eNoteType;
                delayTime = arrivalTime - prevTime;

                if (delayTime > 0f)
                {
                    float finalDelayTime = i == 0 ? startTime : delayTime;
                    float elapsed = 0f;

                    yield return new WaitForSeconds(finalDelayTime);
                    /*while (elapsed < finalDelayTime)
                    {
                        elapsed += Time.deltaTime;
                        yield return null;
                    }*/
                }

                newNote = Instantiate(_note, this.transform);
                note = newNote.GetComponent<Note>();
                note.InitAndGo(eKey, eNoteType, noteSpeed);

                prevTime = arrivalTime;
                i++;
            }

            GameOver();
        }

        float CalculateStartTime(float arrivalTime, float speed) // �����ð����� ���۽ð� ����
        {
            return arrivalTime - (_judgePosY / speed);
        }

        IEnumerator ShowJudgeText(EJudgeTiming eJudgeTiming)
        {
            _judgeTextSR.sprite = _spriteJudgeText[(int)eJudgeTiming];
            _judgeTextSR.enabled = true;

            yield return new WaitForSeconds(0.5f);
            _judgeTextSR.enabled = false;
        }

        IEnumerator Input(int key)
        {
            while (true)
            {
                if (UnityEngine.Input.GetKeyDown(_keyButtons[key].keyCode))
                {
                    _key = key;
                    _keyButtons[key].isJudge = true;
                    _keyButtons[key].button.enabled = true;
                    Invoke("JudgeInactive", _judgeTime);
                }
                if (UnityEngine.Input.GetKeyUp(_keyButtons[key].keyCode))
                {
                    _keyButtons[key].isJudge = false;
                    _keyButtons[key].button.enabled = false;
                }
                yield return null;
            }
        }

        void JudgeInactive()
        {
            _keyButtons[_key].isJudge = false;
        }

        private void OnTriggerEnter2D(Collider2D collision) // ��Ʈ�� �����ٸ� �������� ��
        {
            if (collision.CompareTag("Note"))
            {
                StartCoroutine(ShowJudgeText(EJudgeTiming.Miss));
                ShareDataManager.instance.NumMiss++;
                EventManager.instance.onDamage(_damage);
                Destroy(collision.gameObject);
            }
        }
    }
}
