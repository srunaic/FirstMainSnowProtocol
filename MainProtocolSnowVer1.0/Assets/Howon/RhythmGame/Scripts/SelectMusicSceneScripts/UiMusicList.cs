using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Howon.RhythmGame
{
    public class UiMusicList : MonoBehaviour
    {
        private GameObject _listItemView;
        private Transform _contentParent;
        private MusicPlayer _musicPlayer;
        private UiSelectionDisc _selectionDisc;

        public float _stopSoundTime = 5f;
        public float _fadeoutSoundTime = 3f;

        private Coroutine _fadeOutCoroutine;

        private AsyncOperationHandle _asyncItemView;
        public void Init(SheetMusic[] sheets)
        {
            _contentParent = transform.Find("Viewport/Content");
            _musicPlayer = transform.parent.parent.Find("MusicPlayer").GetComponent<MusicPlayer>();
            _selectionDisc = transform.parent.Find("DiscImage").GetComponent<UiSelectionDisc>();

            _asyncItemView = ResourceManager.instance.LoadPrefab<GameObject>("MusicListItem");
            _listItemView = (GameObject)_asyncItemView.Result;

            foreach (var sheet in sheets)
            {
                ItemInfo item = new ItemInfo();
                item.title = sheet.titleName;
                item.artist = sheet.artist;
                item.playTime = sheet.playTime;
                AddItem(item);
            }

            EventManager.instance.onReleaseAsset = ReleaseAssets;
        }

        void AddItem(ItemInfo data)
        {
            var go = Instantiate(_listItemView, _contentParent);
            UiMusicListItem listItem = go.GetComponent<UiMusicListItem>();
            listItem.Init(data);

            listItem._buttonList.onClick.AddListener(() =>
            {

                if (_fadeOutCoroutine != null)
                {
                    StopCoroutine(_fadeOutCoroutine);
                }

                _selectionDisc.Title = data.title;
                _musicPlayer.ResetVolume();
                _musicPlayer.Play(data.title);
                _fadeOutCoroutine = StartCoroutine(FadeOut());
            });
        }

        // 볼륨을 점진적으로 줄이는 코루틴
        IEnumerator FadeOut()
        {
            yield return new WaitForSeconds(_stopSoundTime);

            float currentTime = 0;
            float startVolume = _musicPlayer.GetVolume();

            while (currentTime < _fadeoutSoundTime)
            {
                currentTime += Time.deltaTime;
                _musicPlayer.SetVolume(Mathf.Lerp(startVolume, 0, currentTime / _fadeoutSoundTime));
                yield return null;
            }

            _musicPlayer.Stop();
            _fadeOutCoroutine = null;
        }

        public void ReleaseAssets()
        {
            ResourceManager.instance.ReleaseAsset(_asyncItemView);
            _musicPlayer.ReleaseMusic();
        }
    }
}
