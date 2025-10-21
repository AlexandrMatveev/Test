using System;
using Test.Core.Data;
using UnityEngine;

namespace Core.MVC
{
    /// <summary>
    /// Basic common implementation MVC's controller part
    /// </summary>
    public abstract class Controller<T, K> : IDisposable where T : Model where K : Component
    {
        private T model;
        private K view;

        protected Controller(T model, GameObject view)
        {
            this.model = model;
            this.view = view.GetComponent<K>();
            this.model.OnModelChanged += ModelChangeHandler;
        }

        public T Model => model;

        public K View => view;

        protected virtual void ModelChangeHandler(ObjectIdentifier identifier)
        {
            UpdateView();
        }

        protected abstract void UpdateView();

        public virtual void Dispose()
        {
            model.OnModelChanged -= ModelChangeHandler;
        }
    }
}