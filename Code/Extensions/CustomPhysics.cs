
using Godot;

namespace GArkanoid.Common
{
    
    public class Hit
    {
        // where the collision happened
        public Vector2 Point
        {
            get;
            set;
        }

        public Vector2 Normal
        {
            get;
            set;
        }

        public Vector2 Penetration
        {
            get;
            set;
        }
    }

    public static class CustomPhysics
    {
        // point and rectangle collision
        public static Hit Intersects(Rect2 rect, Vector2 point)
        {
            Vector2 center = rect.GetCenter();
            Vector2 extents = rect.GetExtents();

            // center to the point
            Vector2 delta = point - center;

            float penetrationX = extents.X - Mathf.Abs(delta.X);
            float penetrationY = extents.Y - Mathf.Abs(delta.Y);

            if (penetrationX < 0 || penetrationY < 0)
            {
                return null;
            }

            Vector2 normal, penetration, collisionPoint;

            if (penetrationX < penetrationY)
            {
                float signX = GetSign(delta.X);
                normal = new Vector2(signX,0);
                penetration = new Vector2(penetrationX * signX , 0);
                collisionPoint = new Vector2(center.X + extents.X *signX, point.Y);
            } else
            {
                float signY = GetSign(delta.Y);
                normal = new Vector2(0, signY);
                penetration = new Vector2(0, penetrationY*signY);
                collisionPoint = new Vector2(point.X, center.Y + extents.Y * signY);
            }

            return new Hit
            {
                Point= collisionPoint,
                Normal = normal,
                Penetration = penetration
            };
        }

        private static float GetSign(float value)
		{
			return value < 0 ? -1f : 1f;
		}

        public static Hit Intersects(Rect2 rect, Vector2 circleCenter, float radius)
        {
            Vector2 closest = new Vector2(
                Mathf.Clamp(circleCenter.X, rect.Position.X, rect.End.X),
                Mathf.Clamp(circleCenter.Y, rect.Position.Y, rect.End.Y)
            );

            Vector2 difference = circleCenter - closest;
            float distanceSquared = difference.LengthSquared();

            if(distanceSquared > radius * radius){ return null;}

            // 1
            if(distanceSquared > 0)
            {
                float distance = Mathf.Sqrt(distanceSquared);
                Vector2 normal = difference / distance;

                return new Hit{
                    Point = closest,
                    Normal = normal,
                    Penetration = normal * (radius- distance)
                };
            
            }
            
            Hit pointHit = Intersects(rect,circleCenter);
            // 2
            if(pointHit == null){return null;}

            return new Hit
            {
                Point = pointHit.Point,
                Normal = pointHit.Normal,
                Penetration = pointHit.Penetration + pointHit.Normal* radius
            };
        }

        public static Vector2 Bounce(Vector2 direction, Vector2 normal)
        {
            Vector2 parallelComponent = direction.Dot(normal) * normal;

            Vector2 perpendicularComponent = direction - parallelComponent;

            return perpendicularComponent - parallelComponent;
        }
        
    }
}