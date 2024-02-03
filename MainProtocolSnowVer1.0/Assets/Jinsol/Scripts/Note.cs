using UnityEngine;

namespace Jinsol.RunGame
{
    public class Note : MonoBehaviour
    {
        double timeInstantiated;
        public float assignedTime;
        public ScoreManager scoreManager;
        public SongManager songManager;

        AudioSource audioSource;
        public AudioClip audioClip;

        protected virtual void Awake()
        {
            timeInstantiated = SongManager.GetAudioSourceTime();
            scoreManager = FindObjectOfType<ScoreManager>();
            audioSource = (AudioSource)GetComponent("AudioSource");
        }

        protected virtual void Update()
        {
            // when to spawn
            double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
            float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));

            if (t > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.localPosition = Vector3.Lerp(Vector3.forward * SongManager.Instance.noteSpawnZ, Vector3.forward * SongManager.Instance.noteDespawnZ, t);
                //GetComponent<MeshRenderer>().enabled = true;
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                audioSource.PlayOneShot(audioClip, 0.3f);
                scoreManager.Hit();
                this.WaitForIt(0.21f, () => { Destroy(gameObject); });
            }
        }
    }
}
