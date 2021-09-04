using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CSharpPilot2.IO {
    sealed record Request {
        public SayDo? Before { get; init; }
        public Predicate<Input>? Condition { get; init; }
        public SayDo<Input>? Anyway { get; init; }
        public SayDo<Input>? Matched { get; init; }
        public SayDo<Input>? MatchedNot { get; init; }
    }
}
