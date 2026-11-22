using System;
using System.Threading.Tasks;
using FMOD.Studio;

namespace Game.Sounds
{
    [System.Serializable]
    public class SoundInstanceHandler
    {
        private EventInstance _instance;
        private bool _isActive;
        private STOP_MODE _stopMode = STOP_MODE.ALLOWFADEOUT;
        
        public SoundInstanceHandler(EventInstance instance)
        {
            _instance = instance;
            
        }

        public void Play()
        {
            _instance.start();
            _instance.setVolume(1);
            _isActive = true;
        }

        public void Stop(STOP_MODE mode = STOP_MODE.IMMEDIATE)
        {
            _instance.stop(mode);
            _isActive = false;
        }
        
        public void StartLoop()
        {
            _isActive = true;
            _instance.setVolume(1);
            LoopSoundInstance();
        }

        public void StopLoop(STOP_MODE mode = STOP_MODE.IMMEDIATE)
        {
            _isActive = false;
            _stopMode = mode;
        }

        public void Release()
        {
            _instance.release();
        }

        public async void LoopSoundInstance()
        {
            while (_isActive)
            {
                PLAYBACK_STATE playbackState;
                _instance.getPlaybackState(out playbackState);

                if (playbackState == PLAYBACK_STATE.STOPPED)
                {
                    _instance.start();
                }

                await Task.Yield();
            }

            _instance.stop(_stopMode);
        }

        public void SetParameters(string name, float value)
        {
            _instance.setParameterByName(name, value);
        }

        public async void FadeOut(float seconds)
        {
            int totalMilliseconds = (int) TimeSpan.FromSeconds(seconds).TotalMilliseconds;
            int milliseconds = totalMilliseconds;

            _isActive = false;
            
            while (milliseconds >= 0)
            {
                await Task.Delay(20);

                _instance.setVolume((float) milliseconds/totalMilliseconds);

                milliseconds -= 20;
            }
            
            Stop(STOP_MODE.IMMEDIATE);
        }

        public bool IsPlaying()
        {
            return _isActive;
        }
        
    }
}