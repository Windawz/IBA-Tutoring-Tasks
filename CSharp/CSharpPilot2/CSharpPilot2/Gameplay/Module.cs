using System;

namespace CSharpPilot2.Gameplay
{
    abstract class Module
    {
        protected Module(App app)
        {
            App = app;
            App.AddModule(this);
        }

        public App App { get; }

        public void Init()
        {
            InitImpl();
        }
        public void Act()
        {
            ActImpl();
        }
        public void Finish()
        {
            FinishImpl();
        }

        protected virtual void InitImpl() { }
        protected virtual void ActImpl() { }
        protected virtual void FinishImpl() { }
    }
}
