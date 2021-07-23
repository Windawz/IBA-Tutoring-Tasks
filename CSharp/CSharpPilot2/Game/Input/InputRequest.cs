using System;
using System.Timers;

/*  TODO: READ ME
 *  
 *  The current plan is:
 *     +-----------------------+
 *     |    InputRequest       |
 *     |          v            |    Input                     Input
 *     | ValidatedInputRequest |  <--------  InputMediator <----------  InputSource
 *     |          v            |
 *     |   TimedInputRequest   |
 *     +-----------------------+
 *     
 *     The InputSource is an object whose sole job is to create brand-new InputInfo objects.
 *     
 *     The InputRequest family is a set of classes whose job is to request input from the user
 *     in form of dialogue. It can optionally display a message.
 *     
 *     ValidatedInputRequest will keep requesting the input until the validator function deems it
 *     as true. Such repeated requests until input is true are called input attempts.
 *     It can optionally display a retry message every time a new attempt starts after the
 *     last one fails.
 *     
 *     TimedInputRequest will accumulate time taken by each attempt into the final
 *     InputInfo object. It can be given a time threshold. It can also append a
 *     (hardcoded, for now) string to messages and retry messages that shows the difference
 *     between the threshold and the accumulated time.
 *     This will allow to show how much time a player has left to enter a word.
 *     
 *     The InputMediator's role is not yet completely defined. It will receive InputInfo from
 *     InputSource and will optionally perform additional logic based on the contents (probably
 *     will have to rename it to something else).
 *     This will allow to respond, for example, to commands starting with '/'.
 */

namespace CSharpPilot2.Input {
    /// <summary>
    /// Represents an input request from the specified source. The input info will
    /// belong to the specified player.
    /// </summary>
    class InputRequest {
        public InputRequest(InputSource source) =>
            Source = source;

        public InputSource Source { get; }

        public event EventHandler? RequestStarted;
        public event EventHandler<InputInfo>? InputInfoReceived;

        public InputInfo Perform() {
            OnRequestStarted();
            return RequestInputInfo();
        }
        protected virtual InputInfo RequestInputInfo() =>
            Source(); // no idea how to enforce the usage
        protected virtual void OnInputInfoReceived(InputInfo inputInfo) =>
            InputInfoReceived?.Invoke(this, inputInfo);
        protected virtual void OnRequestStarted() =>
            RequestStarted?.Invoke(this, EventArgs.Empty);
        private InputInfo ReadInputInfo() {
            double time = 0.0;

            var timer = new Timer(100);
            timer.Elapsed += (sender, e) => time += 0.1;

            timer.Start();
            string text = Console.ReadLine() ?? "";
            timer.Stop();

            var inputInfo = new InputInfo(text, time);
            OnInputInfoReceived(inputInfo);

            return inputInfo;
        }
    }
}
