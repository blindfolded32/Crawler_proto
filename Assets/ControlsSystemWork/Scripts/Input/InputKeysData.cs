using UnityEngine;

namespace UserInput
{
    [CreateAssetMenu(menuName = "DataBase/InputKeysData", fileName = nameof(InputKeysData))]
    public sealed class InputKeysData : ScriptableObject
    {        
        [SerializeField] private KeyCode _save;
        [SerializeField] private KeyCode _load;
                
        public KeyCode Save => _save;
        public KeyCode Load => _load;
    }
}