using System.Collections.Generic;
using EagleQuest.GameObjects;
using EagleQuest.Interfaces;

namespace EagleQuest.Managers
{
    // VIVA: CollisionManager separates collision logic from everything else.
    // It uses Bounds.IntersectsWith() — the teacher's preferred collision method.
    // It casts objects to ICollidable to call OnCollision — this is INTERFACE usage.
    // Game class just calls collisionManager.CheckAll(gameObjects) each tick.

    public class CollisionManager
    {
        public void CheckAll(List<GameObject> gameObjects)
        {
            // Compare every object with every other object
            for (int i = 0; i < gameObjects.Count; i++)
            {
                for (int j = i + 1; j < gameObjects.Count; j++)
                {
                    GameObject a = gameObjects[i];
                    GameObject b = gameObjects[j];

                    // Only check alive objects
                    if (!a.IsAlive || !b.IsAlive) continue;

                    // Use HitBox (smaller than full Bounds) so collisions happen
                    // only when the visible parts of sprites actually overlap.
                    // VIVA: HitBox is a virtual property on GameObject — each class
                    // can override it. CollisionManager doesn't need to know the type.
                    if (a.HitBox.IntersectsWith(b.HitBox))
                    {
                        // Cast to ICollidable and notify both objects
                        // VIVA: We cast to interface — we don't need to know
                        // the specific type, just that it can handle collisions
                        if (a is ICollidable)
                        {
                            ICollidable collidableA = (ICollidable)a;
                            collidableA.OnCollision(b);
                        }

                        if (b is ICollidable)
                        {
                            ICollidable collidableB = (ICollidable)b;
                            collidableB.OnCollision(a);
                        }
                    }
                }
            }
        }
    }
}
