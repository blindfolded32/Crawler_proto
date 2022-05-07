using Selection;
using System.Collections.Generic;
using UnityEngine;
using UserInput;

namespace Engine
{
    public sealed class GameInitializator
    {
        public GameInitializator(ControllersManager controllersManager, GameData gameData, List<ISelectableUnit> allSelectableUnits)
        {
            
            var inputController = new InputController(gameData);

            
            var selectObjectsController = new SelectObjectsController(allSelectableUnits);

            var moveUnitsController = new MoveUnitsController(selectObjectsController, inputController);

            controllersManager.Add(inputController);
            controllersManager.Add(selectObjectsController);
        }
    }
}
