using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Helpers
{
    public class AudioManipulator : MonoBehaviour
    {
        private const float LERP_TIME = 1f;
        
        [SerializeField] private AudioMixerGroup _master;
        [SerializeField] private AudioMixerGroup _music;
        [SerializeField] private AudioMixerGroup _sfx;

        private Coroutine _masterVolumeLerpRoutine;
        private Coroutine _masterPitchLerpRoutine;
        
        private Coroutine _musicVolumeLerpRoutine;
        private Coroutine _musicPitchLerpRoutine;
        
        private Coroutine _sfxVolumeLerpRoutine;
        private Coroutine _sfxPitchLerpRoutine;
        
        public void SetMasterVolume(float value) => _master.audioMixer.SetFloat("MasterVolume", value);
        public void SetMasterPitch(float value) => _master.audioMixer.SetFloat("MasterPitch", value);

        public void LerpMasterVolume(float value)
        {
            if (!_master.audioMixer.GetFloat("MasterVolume", out float currentValue))
                return;
            
            StartCoroutine(LerpFunction(SetMasterVolume, currentValue, value));
        }
        public void LerpMasterPitch(float value)
        {
            if (!_master.audioMixer.GetFloat("MasterPitch", out float currentValue))
                return;
            
            StartCoroutine(LerpFunction(SetMasterPitch, currentValue, value));
        }
        
        public void SetMusicVolume(float value) => _music.audioMixer.SetFloat("MusicVolume", value);
        public void SetMusicPitch(float value) => _music.audioMixer.SetFloat("MusicPitch", value);
        public void LerpMusicVolume(float value)
        {
            if (!_music.audioMixer.GetFloat("MusicVolume", out float currentValue))
                return;
            
            StartCoroutine(LerpFunction(SetMusicVolume, currentValue, value));
        }
        public void LerpMusicPitch(float value)
        {
            if (!_music.audioMixer.GetFloat("MusicPitch", out float currentValue))
                return;
            
            StartCoroutine(LerpFunction(SetMusicPitch, currentValue, value));
        }
        
        public void SetSFXVolume(float value) => _sfx.audioMixer.SetFloat("SFXVolume", value);
        public void SetSFXPitch(float value) => _sfx.audioMixer.SetFloat("SFXPitch", value);
        public void LerpSFXVolume(float value)
        {
            if (!_sfx.audioMixer.GetFloat("SFXVolume", out float currentValue))
                return;
            
            StartCoroutine(LerpFunction(SetSFXVolume, currentValue, value));
        }
        public void LerpSFXPitch(float value)
        {
            if (!_sfx.audioMixer.GetFloat("SFXPitch", out float currentValue))
                return;
            
            StartCoroutine(LerpFunction(SetSFXPitch, currentValue, value));
        }

        private IEnumerator LerpFunction(Action<float> function, float startValue, float targetValue)
        {
            float elapsedTime = 0f;

            while (elapsedTime < LERP_TIME)
            {
                float value = Mathf.Lerp(startValue, targetValue, elapsedTime / LERP_TIME);
                function(value);
                
                elapsedTime += Time.deltaTime;
                
                yield return null;
            }
            
            function(targetValue);
        }
    }
}
