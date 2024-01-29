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

        // ����ȭ ���
        public AsyncOperationHandle LoadPrefab<T>(string name)
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(_myFolderPath + $"Prefabs/{name}.prefab");
            Debug.Log(_myFolderPath + $"Prefabs/{name}.prefab");
            handle.WaitForCompletion(); // ���������� �۾��� ��ٸ�.

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle;
            }
            else
            {
                Debug.LogError($"������ �ε� ���� : " + name);
                return default;
            }
            //return Resources.Load<T>($"Prefabs/{path}");
        }

        // ����ȭ ���
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
                Debug.LogError($"����� ���� �ε� ���� : " + name);
                return default;
            }
        }

        // ����ȭ ���
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
                Debug.LogError($"���� ���� �ε� ���� : {name}");
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
                Debug.LogError($"�̹��� ���� �ε� ���� : {name}");
                return default;
            }
        }

        // ����ȭ ���
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
                Debug.LogError($"��ũ���ͺ� ������Ʈ �ε� ���� : " + name);
                return default;
            }
        }

        // �񵿱�ȭ ���
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
                    Debug.LogError($"��ũ���ͺ� ������Ʈ �ε� ���� : " + name);
                }
            };
        }

        public void ReleaseAsset(AsyncOperationHandle handle)
        {
            Addressables.Release(handle);
        }
    }
}
