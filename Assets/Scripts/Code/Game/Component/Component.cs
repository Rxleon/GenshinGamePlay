﻿using System;

namespace TaoTie
{
    /// <summary>
    /// 所有组件都是从池子中取的，回收时一定要通过Destroy方法将数据清掉
    /// </summary>
    public abstract class Component : IDisposable
    {
        [Timer(TimerType.RemoveComponent)]
        public class RemoveComponent: ATimer<Component>
        {
            public override void Run(Component c)
            {
                try
                {
                    c.parent.RemoveComponent(c);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }
        protected Entity parent { get; private set; }
        public long Id => parent != null ? parent.Id : 0;
        private long timerId;
        public void BeforeInit(Entity entity)
        {
            IsDispose = false;
            parent = entity;
        }
        public void AfterInit()
        {
            if(this is IUpdate updater)
                timerId = GameTimerManager.Instance.NewFrameTimer(TimerType.ComponentUpdate, updater);
            if (this is IFixedUpdate fixedUpdate)
                CodeLoader.Instance.FixedUpdate += fixedUpdate.FixedUpdate;
            if (this is ILateUpdate lateUpdate)
                CodeLoader.Instance.LateUpdate += lateUpdate.LateUpdate;
        }
        public void BeforeDestroy()
        {
            IsDispose = true;
            if (this is IFixedUpdate fixedUpdate)
                CodeLoader.Instance.FixedUpdate -= fixedUpdate.FixedUpdate;
            if (this is ILateUpdate lateUpdate)
                CodeLoader.Instance.LateUpdate -= lateUpdate.LateUpdate;
            GameTimerManager.Instance?.Remove(ref timerId);
        }
        public void AfterDestroy()
        {
            parent = null;
            ObjectPool.Instance.Recycle(this);
        }

        public bool IsDispose { get; private set; }

        public void Dispose()
        {
            if (IsDispose) return;
            BeforeDestroy();
            (this as IComponentDestroy)?.Destroy();
            if (parent != null)
            {
                parent.RemoveComponent(GetType());
            }
            AfterDestroy();
        }

        public T GetParent<T>() where T : Entity
        {
            return parent as T;
        }

        public T GetComponent<T>() where T : Component, IComponentDestroy
        {
            return parent?.GetComponent<T>();
        }
    }
}