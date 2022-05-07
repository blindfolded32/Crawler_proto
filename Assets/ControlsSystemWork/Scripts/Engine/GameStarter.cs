using UnityEngine;
using System.Linq;
using Units;

namespace Engine
{
    public class GameStarter : MonoBehaviour
    {   

        private ControllersManager _controllersManager;

        private void Start()
        {
            _controllersManager = new ControllersManager();
            var gamedata = (GameData)Resources.Load("Gamedata");

            var allSelectableUnits = FindObjectsOfType<Unit>().OfType<ISelectableUnit>().ToList();

            new GameInitializator(_controllersManager, gamedata, allSelectableUnits);

            _controllersManager.Initialization();
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            _controllersManager.LocalUpdate(deltaTime);
        }

        private void LateUpdate()
        {
            var deltaTime = Time.deltaTime;
            _controllersManager.LocalLateUpdate(deltaTime);
        }

        private void FixedUpdate()
        {
            var fixedDeltaTime = Time.fixedDeltaTime;
            _controllersManager.LocalFixedUpdate(fixedDeltaTime);
        }

        private void OnGUI()
        {
            _controllersManager.LocalOnGUI();
        }

        private void OnDestroy()
        {
            _controllersManager.CleanUp();
        }

    }
}

