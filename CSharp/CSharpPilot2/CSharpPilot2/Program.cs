using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharpPilot2.Gameplay;
using CSharpPilot2.IO;
using CSharpPilot2.Locales;

namespace CSharpPilot2
{
    partial class Program
    {
        static void Main()
        {
            Encoding encoding = Encoding.UTF8;
            Performer performer = new(new ConsoleInputSource(encoding), new ConsoleOutputTarget(encoding));

            Locale locale = new(StringTable.Russian);

            App app = new(performer, locale);

            Game game = new(app, GameRules.Default);

            app.Start();
        }
    }
}
