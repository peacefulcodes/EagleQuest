namespace EagleQuest.Interfaces
{
    // VIVA: ICollectible is a second interface.
    // Only food items and power-ups implement this.
    // When the eagle touches them, Collect() is called.
    // Having a separate interface makes the design clean and explainable.

    public interface ICollectible
    {
        void Collect(GameObjects.Player player);
    }
}
