using Godot;

namespace Garkanoid.Effects
{
    [GlobalClass] // the effect map is now available as resource in Godot
    public partial class EffectMap: Resource
    {
        [Export] private EffectSceneEntry[] _entries = null;

        public PackedScene GetScene(EffectType effectType)
        {
            foreach (EffectSceneEntry effectSceneEntry in _entries)
            {
                if(effectSceneEntry.EffectType == effectType)
                {
                    return effectSceneEntry.EffectScene;
                }
            }
            return null;
        }
    }
}