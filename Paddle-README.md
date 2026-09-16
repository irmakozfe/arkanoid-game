I used a CollisionShape2D for the collision and then rotated it 90 degrees. The paddle sprite has round ends, and a capsule matches this shape, I thought. A RectangleShape2D would have sharp corners that do not match the sprite, so the collision would not look right at the edges of the paddle. Maybe, the CollisionPolygon2D would be more suitable for the paddle, since the paddle has not a direct oval shape. If the collision shape and the sprite do not match well, the ball can look like it bounces in the air, away from the picture. What additionally could be a problem is that the paddle can move outside the screen if I do not add a limit to its position. 

For the movement, I would choose a classic keyboard set 'A' and 'D'.

A classic power-up would be doubling the balls, so they hit the blocks faster together. 
Another could be a power-up that lets the ball break through blocks, so it destroys the block in front of it.
 