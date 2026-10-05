using System.Collections;
using System.Collections.Generic;
using Godot;

namespace GArkanoid.Common
{
	public static class NodeExtensions
	{
		public static T GetNode<T>(this Node node, bool recursive = false)
			where T : Node
		{
			int childCount = node.GetChildCount();
			for (int i = 0; i < childCount; ++i)
			{
				Node child = node.GetChild(i);

				if (child is T result)
				{
					return result;
				}

				if (recursive && child.GetChildCount() > 0)
				{
					T recursiveResult = GetNode<T>(child, recursive);
					if (recursiveResult != null)
					{
						return recursiveResult;
					}
				}
			}

			return null;
		}

		public static IList<T> GetNodes<T>(this Node node, bool recursive = false)
			where T : Node
		{
			List<T> results = new List<T>();
			int childCount = node.GetChildCount();

			for (int i = 0; i < childCount; ++i)
			{
				Node child = node.GetChild(i);

				if (child is T result)
				{
					results.Add(result);
				}

				if (recursive && child.GetChildCount() > 0)
				{
					results.AddRange(GetNodes<T>(child, recursive));
				}
			}

			return results;
		}

		public static Rect2 GetBoundingBox(this Sprite2D sprite)
		{
			if (sprite == null || sprite.Texture == null)
			{
				return default(Rect2);
			}

			Vector2 textureSize = sprite.Texture.GetSize();
			Vector2 scaledSize = textureSize * sprite.Scale;
			Vector2 offset = sprite.Offset;

			Vector2 topLeft = sprite.GlobalPosition - (scaledSize * (sprite.Centered ? 0.5f : 0f)) + offset;

			return new Rect2(topLeft, scaledSize);
		}

		public static Vector2 GetExtents(this Rect2 rectangle)
		{
			return rectangle.Size / 2f;
		}
	}
}