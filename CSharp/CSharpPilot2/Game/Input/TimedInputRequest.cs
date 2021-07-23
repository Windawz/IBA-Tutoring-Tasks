using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Input {
    class TimedInputRequest : ValidatedInputRequest {
        public TimedInputRequest(InputSource source, InputValidator validator) : base(source, validator) =>
            InputInfoReceived += (sender, e) => SecondsPassed += e.Seconds;

        protected double SecondsPassed { get; private set; } = 0.0;

        protected override InputInfo RequestInputInfo() =>
            base.RequestInputInfo() with { Seconds = SecondsPassed };
    }
}
