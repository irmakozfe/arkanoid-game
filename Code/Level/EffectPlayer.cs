using System;
using Godot;
using GArkanoid.Common;

namespace Garkanoid.Effects
{
    [GlobalClass]
    public partial class EffectPlayer : Node2D
    {
        [Export] private EffectMap _effectMap = null;

        public void PlayEffect(EffectType effectType, Vector2 globalPosition)
        {
            if (_effectMap == null)
            {
                GD.PushError("Effect Map is not assigned!");
                return;
            }

            PackedScene effectScene = _effectMap.GetScene(effectType);

            if(effectScene == null)
            {
                GD.PushError($"Effect scene matching the effect type {effectType} is not found");
                return;
            }

            GpuParticles2D effect = effectScene.Instantiate<GpuParticles2D>();
            AddChild(effect);
            effect.GlobalPosition = globalPosition;

            PlayEffect(effect);


        }

        private void PlayEffect(GpuParticles2D effect)
        {
            int effectsPlaying = 1;
            effect.OneShot = true;
            effect.Emitting = true;

            AudioStreamPlayer2D audio = effect.GetNode<AudioStreamPlayer2D>();

            if(audio != null)
            {
                effectsPlaying =1;
                audio.Play();
                audio.Finished += () =>
                {
                    EffectFinished();
                    
                };
            }

            effect.Finished += () =>
            {
                // Gets called when the signal is emmitted
                EffectFinished();
            };

            // intentionally declared here! Dont move this
            void EffectFinished()
            {
                effectsPlaying--;
                if(effectsPlaying <= 0){
                effect.QueueFree();
                }

                
            }

            
        }

        
    }

}