using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Input {
    class ValidatedInputRequest : InputRequest {
        public ValidatedInputRequest(InputSource source, InputValidator validator) : base(source) =>
            Validator = validator;

        public InputValidator Validator { get; }

        public event EventHandler<InputInfo>? InputInfoInvalidated;

        protected virtual void OnInputInfoInvalidated(InputInfo inputInfo) =>
            InputInfoInvalidated?.Invoke(this, inputInfo);
        protected override InputInfo RequestInputInfo() {
            while (true) {
                InputInfo inputInfo = base.RequestInputInfo();
                bool isValid = Validator(inputInfo);

                if (isValid) {
                    return inputInfo;
                } else {
                    OnInputInfoInvalidated(inputInfo);
                }
            }
        }
    }
}
