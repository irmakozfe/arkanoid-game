using System;
using System.Collections.Generic;
using GArkanoid.Systems;
using Godot;

namespace Garkanoid.State
{
    public abstract class GameStateBase
    {

        // type of the game state
        public abstract GameStateType StateType{get;}

// points to the scene resource
        public abstract string ScenePath{get;}

       

        // virtual: theres a base implementation we can override
        public virtual bool IsAdditive => false; // a get accessor that returns false by default 
        
        // a list of possible target states

        private HashSet<GameStateType> _targetStates = new HashSet<GameStateType>();

        private PackedScene _sceneResource = null;

        

        //a reference to the scene associated with this state. 
        private Node _scene = null;

        // constructor 

        protected GameStateBase()
        {
            // preload the scene file and cache the result
            // this implementation consumes a lit bit more memory bcs the scene are preloaded even tho they are not instanciated 

            _sceneResource = GD.Load<PackedScene>(ScenePath); 
             if(_sceneResource == null){
            GD.PushError("Scene not available!");
        }
        }

        protected bool AddTargetState(GameStateType gameStateType)
        {
                return _targetStates.Add(gameStateType);
        }

        protected bool RemoveTargetState(GameStateType gameStateType)
        {
            return  _targetStates.Remove(gameStateType);
        }

        // public bcs the game manager should be able to reach this method 

        public bool CanTransitionTo(GameStateType targetStateType)
        {
            return _targetStates.Contains(targetStateType);
        }


        // possible approach: transitions can only happen after a frame
        public virtual void OnEnter(bool forceLoad = false)
        {
            if (forceLoad || _scene == null)
            {
                if (_scene != null)
                {
                    // TODO: perhaps a bug??
                    _scene.QueueFree();
                }

                

                _scene = _sceneResource.Instantiate();
                // TODO: show what happens if scene addition is not delayed 
                GameManager.Instance.SceneTree.Root.CallDeferred(Node.MethodName.AddChild, _scene);

                // TODO: Threaded level loading (for very large complicated scenes oots) Godot Docs: Background Loading 
            

            }
        }

        // keepLoaded -> true: when the game is paused, the data is saved there , false: we need to unload the scene
        // state deactivation logic. Should be run before a state is deactivated 
        public virtual void OnExit(bool keepLoaded)
        {
            // && -> short circuitting
            if (!keepLoaded && _scene != null)
            {
                // this scene should be unloaded and it is still loaded. Unload it:
                _scene.QueueFree();
                _scene = null;

            }
        }
    }
}