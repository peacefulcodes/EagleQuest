using System.Collections.Generic;
using EagleQuest.GameObjects;
using EagleQuest.Interfaces;

namespace EagleQuest.Managers
{
    

    public class CollisionManager
    {
        public void CheckAll(List<GameObject> gameObjects)
        {
            
            for (int i = 0; i < gameObjects.Count; i++)
            {
                for (int j = i + 1; j < gameObjects.Count; j++)
                {
                    GameObject a = gameObjects[i];
                    GameObject b = gameObjects[j];

                    // Only check alive objects
                    if (!a.IsAlive || !b.IsAlive) continue;

                    if (a.HitBox.IntersectsWith(b.HitBox))
                    {
                        
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
