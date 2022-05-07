using UserInput;
using UnityEngine;

namespace Engine
{
    [CreateAssetMenu(menuName = "DataBase/GameData", fileName = nameof(GameData))]
    public sealed class GameData : ScriptableObject
    {
        [SerializeField] private InputKeysData _inputKeysData;
        


        public InputKeysData InputKeysData => _inputKeysData;        
    }
}
