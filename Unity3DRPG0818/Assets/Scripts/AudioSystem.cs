using UnityEngine;

namespace re
{
    /// <summary>
    /// 音效系統
    /// 提供窗口給要播放音效的物件
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioSystem : MonoBehaviour
    {
        private AudioSource aud;

        private void Awake()
        {
            aud = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClip sound)
        {
            aud.PlayOneShot(sound);
        }

        public void PlaySoundRandomVolume(AudioClip sound)
        {
            float volume = Random.Range(0.7f, 1.2f);
            aud.PlayOneShot(sound, volume);
        }
    }
}