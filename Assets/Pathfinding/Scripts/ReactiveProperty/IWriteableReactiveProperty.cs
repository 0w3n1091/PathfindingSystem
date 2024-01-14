namespace Pathfinding.Scripts.ReactiveProperty
{
    /// <summary>
    /// An interface representing a writeable reactive property of type T.
    /// </summary>
    /// <typeparam name="T">The type of the reactive property.</typeparam>
    public interface IWriteableReactiveProperty<T>
    {
        /// <summary>
        /// Sets the value of the writeable reactive property.
        /// </summary>
        /// <param name="value">The new value to set.</param>
        public void Set(T value);
    }
}