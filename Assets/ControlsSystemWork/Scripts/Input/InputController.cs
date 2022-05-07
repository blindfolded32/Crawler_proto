using Engine;
using System;
using UnityEngine;

namespace UserInput
{
    public sealed class InputController : IUpdatable, IController
    {        
        public event Action OnClickSave;
        public event Action OnClickLoad;
        public event Action OnClickDownLMB;
        public event Action OnClickUpLMB;
        public event Action OnClickDownRMB;
        public event Action OnClickUpRMB;

        private readonly KeysManager _inputKeys;
        private readonly InputKeysData _inputKeysData;

        public InputController(GameData gameData)
        {
            _inputKeys = new KeysManager();
            _inputKeysData = gameData.InputKeysData;
        }

        public void LocalUpdate(float deltaTime)
        {
            if (Time.timeScale == Mathf.Round(0)) return;
                       
            _inputKeys.GetKeyDownSave(_inputKeysData, OnClickSave);
            _inputKeys.GetKeyDownLoad(_inputKeysData, OnClickLoad);
            _inputKeys.GetKeyDownLMB(OnClickDownLMB);
            _inputKeys.GetKeyUpLMB(OnClickUpLMB);
            _inputKeys.GetKeyDownRMB(OnClickDownRMB);
            _inputKeys.GetKeyUpRMB(OnClickUpRMB);
        }        
    }
}
