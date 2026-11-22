using System;

namespace Game.Sounds
{
    public class SoundManager
    {
        public SFXAudioHandler SFXAudioHandler { get; private set; } = new SFXAudioHandler();
        public InitializerAudioHandler FactorySoundAudioHandler { get; private set; } = new InitializerAudioHandler();
    }
}