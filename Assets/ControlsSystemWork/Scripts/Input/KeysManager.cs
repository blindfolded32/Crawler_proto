using System;
using UnityEngine;

namespace UserInput
{
    public sealed class KeysManager
    {       
        public void GetKeyDownSave(InputKeysData _inputKeysData, Action action)
        {
            if (Input.GetKeyDown(_inputKeysData.Save)) action?.Invoke();
        }

        public void GetKeyDownLoad(InputKeysData _inputKeysData, Action action)
        {
            if (Input.GetKeyDown(_inputKeysData.Load)) action?.Invoke();
        }

        public void GetKeyDownLMB(Action action)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) action?.Invoke();
        }

        public void GetKeyUpLMB(Action action)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0)) action?.Invoke();
        }

        public void GetKeyDownRMB(Action action)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1)) action?.Invoke();
        }
        public void GetKeyUpRMB(Action action)
        {
            if (Input.GetKeyUp(KeyCode.Mouse1)) action?.Invoke();
        }

    }
}
