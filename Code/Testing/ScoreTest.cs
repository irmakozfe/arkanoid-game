using System;
using GArkanoid.Systems;
using Godot;

namespace Garkanoid.Testing
{
    public partial class ScoreTest: Node
    {
        public override void _EnterTree()
        {
            base._EnterTree();
            CallDeferred(nameof(Subscribe));
        }

        private void Subscribe()
        {
            GameManager.Instance.ScoreChanged += OnScoreChanged;              
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            GameManager.Instance.ScoreChanged -= OnScoreChanged;
        }

         private void OnScoreChanged(int score)
        {
            GD.Print($"Current Score: {score}");
        }

        

       
    }
}