
using FMODUnity;
using UnityEngine;

namespace Game.Sounds
{
    public class SFXAudioHandler
    {
        public void PlayOneShot(EventReference sound, Vector3 worldPos)
        {
            RuntimeManager.PlayOneShot(sound,  worldPos);
        }

        public void PlayFollowedShot(EventReference sound, GameObject gameObject)
        {
            RuntimeManager.PlayOneShotAttached(sound, gameObject);
        }
    }
}