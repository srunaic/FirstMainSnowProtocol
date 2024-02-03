using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;

namespace Jinsol.RunGame
{
    [RequireComponent(typeof(SphereCollider))]
    public class Lane : MonoBehaviour
    {
        // restricts notes to certain key as notes = lanes in game
        public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
        public GameObject[] notePrefabs;
        List<Note> notes = new List<Note>();
        public List<double> timeStamps = new List<double>();
        // keeps track of which timestamps needs to be spawned/detected
        int spawnIndex = 0;

        void Update()
        {

            if (spawnIndex < timeStamps.Count)
            {
                if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
                {
                    int i = Random.Range(0, notePrefabs.Length);
                    var note = Instantiate(notePrefabs[i], transform);
                    notes.Add(note.GetComponent<Note>());
                    note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                    spawnIndex++;
                }
            }
        }

        public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
        {
            foreach (var note in array)
            {
                if (note.NoteName == noteRestriction)
                {
                    var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                    timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
                }
            }
        }
    }
}
