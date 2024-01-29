using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Howon.RhythmGame
{
    public class GameOverPanel : MonoBehaviour
    {
        private Text _txtGreat;
        private Text _txtGood;
        private Text _txtBad;
        private Text _txtMiss;
        private Text _txtScore;
        private Image _imgGrade;
        private Button _btnStart;
        private Button _btnTerminate;

        [SerializeField] private AssetReferenceSprite[] _imgGrades;
        private AsyncOperationHandle<Sprite> _asyncImgHandle;
        private int _numGrade = -1;

        private ResultData _resultData;

        private void Awake()
        {
            _txtGreat = transform.Find("ResultPanel/TxtGreat").GetComponent<Text>();
            _txtGood = transform.Find("ResultPanel/TxtGood").GetComponent<Text>();
            _txtBad = transform.Find("ResultPanel/TxtBad").GetComponent<Text>();
            _txtMiss = transform.Find("ResultPanel/TxtMiss").GetComponent<Text>();
            _txtScore = transform.Find("ResultPanel/TxtScore").GetComponent<Text>();
            _imgGrade = transform.Find("ImgGrade").GetComponent<Image>();
            _btnStart = transform.Find("ButtonPanel/BtnStart").GetComponent<Button>();
            _btnTerminate = transform.Find("ButtonPanel/BtnTerminate").GetComponent<Button>();

            _resultData = ShareDataManager.instance.RetData;

            _txtGreat.text = $"Great : {_resultData.numGreat}";
            _txtGood.text = $"Good  : {_resultData.numGood}";
            _txtBad.text = $"Bad   : {_resultData.numBad}";
            _txtMiss.text = $"Miss  : {_resultData.numMiss}";
            _txtScore.text = $"Score : {_resultData.stageScore}";

            _numGrade = (int)_resultData.eGrade;
            _imgGrades[_numGrade].LoadAssetAsync<Sprite>().Completed += LoadImage;
            _btnStart.onClick.AddListener(OnGotoStartScene);
            _btnTerminate.onClick.AddListener(OnTerminate);
        }

        private void ResetResultData() // 토탈 스코어는 초기화하지 않음
        {
            ShareDataManager.instance.StageScore = 0;
            ShareDataManager.instance.Grade = EGrade.F;
            ShareDataManager.instance.NumGreat = 0;
            ShareDataManager.instance.NumGood = 0;
            ShareDataManager.instance.NumBad = 0;
            ShareDataManager.instance.NumMiss = 0;
        }

        private void OnDestroy()
        {
            _imgGrades[_numGrade].ReleaseAsset();
        }

        void OnGotoStartScene()
        {
            ResetResultData();
            SceneManager.LoadScene("SelectMusicScene");
        }

        void OnTerminate()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        void LoadImage(AsyncOperationHandle<Sprite> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Sprite loadedSprite = handle.Result;
                _imgGrade.sprite = loadedSprite;
            }
            else
            {
                Debug.LogError("이미지 로드 실패");
            }
        }
    }
}

