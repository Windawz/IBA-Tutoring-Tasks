using System;

namespace CSharpPilot2.Input
{
    internal class ValidatedRequest : Request
    {
        public ValidatedRequest(InputValidator validator, InputExceptor exceptor) : base() =>
            (Validator, Exceptor) = (validator, exceptor);

        protected InputValidator Validator { get; }
        protected InputExceptor Exceptor { get; }

        public event EventHandler<InputInfo>? InputInfoValid;
        public event EventHandler<InputInfo>? InputInfoInvalid;

        protected virtual void OnInputInfoValid(InputInfo inputInfo) =>
            InputInfoValid?.Invoke(this, inputInfo);
        protected virtual void OnInputInfoInvalid(InputInfo inputInfo) =>
            InputInfoInvalid?.Invoke(this, inputInfo);
        protected override InputInfo PerformImpl(InputSource source)
        {
            while (true)
            {
                InputInfo? inputInfo = base.PerformImpl(source);
                bool isValid = Validator(inputInfo);
                bool isExcepted = Exceptor(inputInfo);

                if (isValid)
                {
                    OnInputInfoValid(inputInfo);
                    return inputInfo;
                }
                else
                {
                    if (!isExcepted)
                    {
                        OnInputInfoInvalid(inputInfo);
                    }
                }
            }
        }
    }
}
