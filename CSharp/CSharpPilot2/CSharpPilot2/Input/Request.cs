using System;

namespace CSharpPilot2.Input
{
    /// <summary>
    /// Represents an input request from the specified source. The input info will
    /// belong to the specified player.
    /// </summary>
    internal class Request
    {
        public Request() { }

        public event EventHandler? RequestStarted;
        public event EventHandler<InputInfo>? RequestEnded;
        public event EventHandler<InputInfo>? InputInfoReceived;

        public InputInfo Perform(InputSource source)
        {
            OnRequestStarted();


            InputInfo? inputInfo = PerformImpl(source: () =>
            {
                InputInfo? inputInfo = source();
                OnInputInfoReceived(inputInfo);
                return inputInfo;
            });

            OnRequestEnded(inputInfo);
            return inputInfo;
        }
        protected virtual InputInfo PerformImpl(InputSource source)
        {
            InputInfo? inputInfo = source();
            return inputInfo;
        }
        protected virtual void OnRequestStarted() =>
            RequestStarted?.Invoke(this, EventArgs.Empty);
        protected virtual void OnRequestEnded(InputInfo inputInfo) =>
            RequestEnded?.Invoke(this, inputInfo);
        protected virtual void OnInputInfoReceived(InputInfo inputInfo) =>
            InputInfoReceived?.Invoke(this, inputInfo);
    }
}
