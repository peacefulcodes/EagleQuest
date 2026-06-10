namespace EagleQuest.Interfaces
{
    // VIVA: An interface is a contract.
    // Any class that implements ICollidable MUST provide an OnCollision method.
    // This is how CollisionManager can call OnCollision on ANY game object
    // without knowing the specific type — it just knows it is ICollidable.

    public interface ICollidable
    {
        void OnCollision(GameObjects.GameObject other);
    }
}
