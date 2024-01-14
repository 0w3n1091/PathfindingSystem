using System;

namespace Pathfinding.Scripts.ReactiveProperty
{
    /// <summary>
    /// An interface representing a read-only reactive property of type T.
    /// </summary>
    /// <typeparam name="T">The type of the reactive property.</typeparam>
    public interface IReadonlyReactiveProperty<T>
    {
        /// <summary>
        /// Gets the current value of the reactive property.
        /// </summary>
        public T Property { get; }
        
        /// <summary>
        /// Subscribes an action to be executed whenever the value of the reactive property changes.
        /// </summary>
        /// <param name="action">The action to be executed when the value changes.</param>
        public void Subscribe(Action<T> action);
        
        /// <summary>
        /// Disposes a previously subscribed action from receiving updates on property changes.
        /// </summary>
        /// <param name="action">The action to be disposed.</param>
        public void Dispose(Action<T> action);
    }
}