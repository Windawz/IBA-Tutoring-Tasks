using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace CSharpPilot2.Input
{
    internal class CommandList : IEnumerable<Command>
    {
        public CommandList(IEnumerable<Command> commands)
        {
            var d = new Dictionary<string, Command>();
            foreach (Command? command in commands)
            {
                d.Add(command.Name, command);
            }
            _commands = d.ToImmutableDictionary();
        }

        private readonly ImmutableDictionary<string, Command> _commands;

        public IEnumerable<Command> Commands =>
            _commands.Values;

        public Command this[string name] =>
            _commands[name];

        public bool TryGet(string name, out Command value) =>
            _commands.TryGetValue(name, out value!);
        public IEnumerator<Command> GetEnumerator() =>
            _commands.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
