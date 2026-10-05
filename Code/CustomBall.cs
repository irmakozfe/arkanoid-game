using GArkanoid.Common;
using Godot;
using System;

namespace GArkanoid.Entities
{
    
    public partial class CustomBall : Sprite2D
    {
        // backing fields
        [Export] private Vector2 _direction = new Vector2(1,-1);
        [Export] private float _speed = 100;

        // contains references to all walls in the level
        [Export] private Sprite2D[] _walls=null;
        
        //property: they can be complicated than getter and setters (like adding checks)
        public Vector2 Direction
        {
            get {
             return _direction.IsNormalized() ? _direction : _direction.Normalized();
        }
        }

        public float Speed
        {
            get{return Math.Clamp(_speed, Config.MinSpeed, Config.MaxSpeed);} // px per second
        }

        //not backing field 
        public Vector2 Velocity
        {
            get {return Direction * Speed;}
        }

        public float Radius
        {
            get
            {
                return this.GetBoundingBox().Size.X / 2f;
            }
        }
        public override void _Process(double delta)
        {
            float deltatime = (float) delta;
            // Move the ball here
            Vector2 initialPosition = Position;
            Vector2 movement = Velocity * deltatime;
            Position= ResolveWallCollisions( initialPosition + movement); 
        }

        private Vector2 ResolveWallCollisions(Vector2 newPosition)
        {
            // TODO do collision checks with all walls here and bounce the ball if needed
            // bouncing means that you calcualte the new direction for the ball 
            // take into account how far into the wall the ball ended up and and use that distance in the bounce vecor
            
            if (_walls == null)
            {
                return newPosition;
            }

            foreach(Sprite2D wall in _walls)
            {
                if (wall == null)
                {
                    continue;
                }

                Rect2 wallRect = wall.GetBoundingBox();

                //Additional

                Hit hit = CustomPhysics.Intersects(wallRect, newPosition, Radius);

                if (hit == null)
                {
                    continue;
                }

                // bounce: if the ball is moving to the wall 

                if (Direction.Dot(hit.Normal) < 0)
                {
                    _direction = CustomPhysics.Bounce(Direction, hit.Normal);
                }

                newPosition += hit.Penetration * 2f;
            }

            return newPosition;
        }
    }
}