﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Audio;

namespace Tankz_2023
{
    class RandomizeSoundEmitter : Component
    {
        AudioSource source;
        List<AudioClip> clips;

        public float Volume { get { return source.Volume; } set { source.Volume = value; } }

        public RandomizeSoundEmitter(GameObject owner) : base(owner)
        {
            source = new AudioSource();
            clips = new List<AudioClip>();
        }

        public void AddClip(string clipName)
        {
            AudioClip clip = GfxMngr.GetClip(clipName);
            if (clip != null)
            {
                clips.Add(clip);
            }
        }

        public void Play()
        {
            RandomizePitch();
            source.Play(GetRandomClip());
        }

        public void Play(float volume)
        {
            source.Volume = volume;
            Play();
        }


        protected void RandomizePitch()
        {
            source.Pitch = RandomGenerator.GetRandomFloat() * 0.4f + 0.8f;//0.8f => 1.2f
        }

        protected AudioClip GetRandomClip()
        {
            return clips[RandomGenerator.GetRandomInt(0, clips.Count)];
        }
    }
}
