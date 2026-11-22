using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Game.Sounds
{
    public class InitializerAudioHandler
    {
        private List<SoundInstanceHandler> _instances = new List<SoundInstanceHandler>();
        private List<StudioEventEmitter> _emitters = new List<StudioEventEmitter>();

        public SoundInstanceHandler CreateEventInstance(EventReference eventReference)
        {
            SoundInstanceHandler result = new SoundInstanceHandler(RuntimeManager.CreateInstance(eventReference));
            _instances.Add(result);

            return result;
        }

        public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
        {
            if (emitterGameObject == null)
                return null;

            StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
            emitter.EventReference = eventReference;
            _emitters.Add(emitter);
            return emitter;
        }

        public void StopAllInstances()
        {
            for (int i = 0; i < _instances.Count; i++)
            {
                _instances[i].StopLoop(STOP_MODE.IMMEDIATE);
                _instances[i].Release();
            }
        }

        public void StopAllEmitters()
        {
            for (int i = 0; i < _emitters.Count; i++)
            {
                _emitters[i].Stop();
            }
        }

    }
}