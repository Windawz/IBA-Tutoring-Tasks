using System.Collections.Generic;
using System.Linq;

using CSharpPilot2.IO;
using CSharpPilot2.Locales;

namespace CSharpPilot2.Gameplay
{
    sealed class App
    {
        public App(IPerformer performer, Locale locale)
        {
            Performer = performer;
            Locale = locale;
        }

        private readonly HashSet<Module> _modules = new();
        public bool Exit { get; set; } = false;
        public IPerformer Performer { get; set; }
        public Locale Locale { get; }

        public void Start()
        {
            if (_modules.Count == 0)
            {
                return;
            }

            foreach (var m in _modules)
            {
                m.Init();
            }
            while (!Exit)
            {
                foreach (var m in _modules)
                {
                    m.Act();
                }
            }
            foreach (var m in _modules)
            {
                m.Finish();
            }
        }
        /// <returns>true on successful addition, false if it already added.</returns>
        public bool AddModule(Module module) =>
            _modules.Add(module);
    }
}
