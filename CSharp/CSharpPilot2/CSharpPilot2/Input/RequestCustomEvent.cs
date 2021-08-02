using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPilot2.Input
{
    record RequestCustomEvent(InputValidator validator, EventHandler<InputInfo> handler);
}
