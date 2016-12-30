using UnityEngine;
using strange.extensions.command.api;
using strange.extensions.context.api;
using strange.extensions.command.impl;

namespace strange.extensions.context.impl
{
    /// <summary>
    /// Basic Extension of MVCSContext so that signals are the default command dispatcher.
    /// </summary>
    public class MVCSSignalsContext : MVCSContext
    {
        public MVCSSignalsContext(MonoBehaviour view) : base(view) { }

        public MVCSSignalsContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags) { }

        // Unbind the default EventCommandBinder and rebind the SignalCommandBinder
        protected override void addCoreComponents()
        {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

        // Override Start so that we can fire the StartSignal 
        override public IContext Start()
        {
            base.Start();
            StartSignal startSignal = injectionBinder.GetInstance<StartSignal>();
            startSignal.Dispatch();
            return this;
        }
    }
}