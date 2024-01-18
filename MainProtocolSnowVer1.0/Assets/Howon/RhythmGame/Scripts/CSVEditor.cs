using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Howon.RhythmGame;
using static UnityEngine.EventSystems.EventTrigger;

namespace Howon.RhythmGame
{
#if UNITY_EDITOR
    public class CSVEditor : EditorWindow
    {
        private string _fileName;
        private string _csvPath;
        private string _scriptablePath;

        public readonly string _myFolderPath = "Assets/Howon/RhythmGame/GameResources";
        public readonly string _arrivalTime = "ArrivalTime";
        public readonly string _key = "Key";
        public readonly string _noteType = "NoteType";

        private List<Dictionary<string, object>> _list = new List<Dictionary<string, object>>();

        [MenuItem("MyMenu/CsvToScriptableObject")]
        private static void OpenWindow()
        {
            EditorWindow.GetWindow<CSVEditor>().Show();
        }

        void OnGUI()
        {
            GUILayout.Label("CsvToScriptableObject", EditorStyles.boldLabel);
            _fileName = EditorGUILayout.TextField("CSV 파일이름", _fileName);

            _csvPath = $"{_myFolderPath}/CsvFile/{_fileName}.csv";
            _scriptablePath = $"{_myFolderPath}/ScriptableData/{_fileName}.asset";

            if (GUILayout.Button("변환"))
            {
                CsvToScriptableObject(_csvPath);
            }
        }

        private void CsvToScriptableObject(string path)
        {
            Addressables.LoadAssetAsync<TextAsset>(path).Completed += OnLoadDone;
        }

        private void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<TextAsset> operation)
        {
            _list = CSVReader.Parse(operation.Result);

            SheetMusic sheet = ScriptableObject.CreateInstance<SheetMusic>();

            sheet.titleName = _fileName;
            sheet.artist = String.Empty;
            sheet.playTime = 0f;
            sheet.noteSpeed = 5f;

            string compKey;
            string compNoteType;

            for (int i = 0; i < _list.Count; i++)
            {
                Beat beat = new Beat();

                beat.ariavalTime = float.Parse(_list[i][_arrivalTime].ToString());
                compKey = _list[i][_key].ToString();
                compNoteType = _list[i][_noteType].ToString();

                Debug.Log(compKey + " " + compNoteType);

                if (compKey.Equals("D"))
                {
                    beat.eKey = EKey.D;
                }
                else if (compKey.Equals("F"))
                {
                    beat.eKey = EKey.F;
                }
                else if (compKey.Equals("Space"))
                {
                    beat.eKey = EKey.Space;
                }
                else if (compKey.Equals("J"))
                {
                    beat.eKey = EKey.J;
                }
                else if (compKey.Equals("K"))
                {
                    beat.eKey = EKey.K;
                }
                else
                {
                    Debug.Log("키버튼 할당 문자열 오류입니다.");
                }

                if (compNoteType.Equals("Normal"))
                {
                    beat.eNoteType = ENoteType.Normal;
                }
                else if (compNoteType.Equals("Long"))
                {
                    beat.eNoteType = ENoteType.Long;
                }
                else
                {
                    Debug.Log("노트 타입 할당 문자열 오류입니다.");
                }
                sheet.beats.Add(beat);
            }

            AssetDatabase.CreateAsset(sheet, _scriptablePath);
            AssetDatabase.SaveAssets();
        }
    }

#endif
}
