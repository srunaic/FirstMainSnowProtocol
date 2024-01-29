using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Howon.RhythmGame
{
    public class ResourceManager
    {
        public static readonly ResourceManager instance = new ResourceManager();

        public readonly string _myFolderPath = $"Assets/Howon/RhythmGame/GameResources/";
        private ResourceManager() { }

        // 동기화 방식
        public AsyncOperationHandle LoadPrefab<T>(string name)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(_myFolderPath + $"Prefabs/{name}.prefab");
            Debug.Log(_myFolderPath + $"Prefabs/{name}.prefab");
            handle.WaitForCompletion(); // 동기적으로 작업을 기다림.

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle;
            }
            else
            {
                Debug.LogError($"프리팹 로드 실패 : " + name);
                return default;
            }
            //return Resources.Load<T>($"Prefabs/{path}");
        }

        // 동기화 방식
        public AsyncOperationHandle LoadAudioClip<T>(string name)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(_myFolderPath + $"Audio/{name}.mp3");
            handle.WaitForCompletion();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle;
            }
            else
            {
                Debug.LogError($"오디오 파일 로드 실패 : " + name);
                return default;
            }
        }

        // 동기화 방식
        public AsyncOperationHandle LoadVideoClip<T>(string name)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(_myFolderPath + $"Video/{name}.mp4");
            handle.WaitForCompletion();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle;
            }
            else
            {
                Debug.LogError($"비디오 파일 로드 실패 : {name}");
                return default;
            }
        }

        public AsyncOperationHandle LoadImage<T>(string name)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(_myFolderPath + $"Images/{name}.png");
            handle.WaitForCompletion();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle;
            }
            else
            {
                Debug.LogError($"이미지 파일 로드 실패 : {name}");
                return default;
            }
        }

        // 동기화 방식
        public AsyncOperationHandle LoadScriptableData<T>(string name)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(_myFolderPath + $"ScriptableData/{name}.asset");
            handle.WaitForCompletion();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle;
            }
            else
            {
                Debug.LogError($"스크립터블 오브젝트 로드 실패 : " + name);
                return default;
            }
        }

        // 비동기화 방식
        public void LoadScriptableObject<T>(string name)
        {
            string path = _myFolderPath + $"ScriptableData/{name}.asset";
            Debug.Log(path);
            Addressables.LoadAssetAsync<T>(path).Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    EventManager.instance.onLoadScriptableData(handle);
                }
                else
                {
                    Debug.LogError($"스크립터블 오브젝트 로드 실패 : " + name);
                }
            };
        }

        public void ReleaseAsset(AsyncOperationHandle handle)
        {
            Addressables.Release(handle);
        }
    }
}
