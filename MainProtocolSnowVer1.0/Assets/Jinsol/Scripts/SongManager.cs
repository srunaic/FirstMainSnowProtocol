using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

namespace Jinsol.RunGame
{
    public class SongManager : MonoBehaviour
    {
        public GameObject player;

        public static SongManager Instance;
        public AudioSource audioSource;
        public float songDelayInSeconds;
        public AudioClip musicList;
        public Lane[] lanes;

        public double marginOfError;
        public int inputDelayMillisec;

        public string fileLocation = "MidiMap.mid"; // 노트 생성을 위한 미디맵 위치. 루트는 Assets/StreamingAssets 고정
        public float noteTime; // 노트 유지 시간
        public float noteSpawnZ;
        public float noteGetZ;

        private void Awake()
        {
            audioSource = (AudioSource)GetComponent("AudioSource");
            audioSource.playOnAwake = false;
            marginOfError = -0.5f;
            noteTime = 1;
            noteSpawnZ = 25.3f;
            noteGetZ = -19.96f;
        }

        public float noteDespawnZ
        {
            get
            {
                return noteGetZ - (noteSpawnZ - noteGetZ);
            }
        }

        public static MidiFile midiFile;

        void Start()
        {
            Instance = this;
            ReadFromFile();
            //StartCoroutine(Victory());
        }

        void ReadFromFile()
        {
            midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
            GetDataFromMidi();
        }

        public void GetDataFromMidi()
        {
            var notes = midiFile.GetNotes();
            var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
            notes.CopyTo(array, 0);

            foreach (var lane in lanes) lane.SetTimeStamps(array);

            Invoke(nameof(StartSong), songDelayInSeconds);
        }

        void StartSong()
        {
            audioSource.Play();
        }

        // Guess the audio source time
        public static double GetAudioSourceTime()
        {
            // Time samples divided by the audio source's sample rates
            return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
        }

        /*IEnumerator Victory()
        {
            player.GetComponent<Animator>().applyRootMotion = true;
            yield return new WaitForSeconds(46.2f);
            GetComponent<AudioSource>().clip = musicList;
            GetComponent<AudioSource>().Play();
            player.GetComponent<Animator>().SetTrigger("Victory");
            yield return new WaitForSeconds(4.5f); //변경금지
            SceneManager.LoadScene("BossCutScene0", LoadSceneMode.Single);
        }*/
    }
}
