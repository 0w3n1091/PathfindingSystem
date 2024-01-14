namespace Pathfinding.Scripts.ReactiveProperty
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A generic implementation of a reactive property with read and write capabilities.
    /// </summary>
    /// <typeparam name="T">The type of the reactive property.</typeparam>
    public class ReactiveProperty<T> : IReadonlyReactiveProperty<T>, IWriteableReactiveProperty<T>
    {
        private readonly List<Action<T>> _actions = new List<Action<T>>();
        
        public T Property { get; private set; }
        
        public void Subscribe(Action<T> action) => _actions.Add(action);
        
        public void Dispose(Action<T> action) => _actions.Remove(action);
        
        public void Set(T value)
        {
            Property = value;
            NotifySubscribers(value);
        }

        private void NotifySubscribers(T value)
        {
            foreach (Action<T> action in _actions)
                action?.Invoke(value);
        }
    }
}