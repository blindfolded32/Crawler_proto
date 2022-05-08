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
            var formationObject = Object.Instantiate((GameObject)Resources.Load("FormationObject"));
            
            var inputController = new InputController(gameData);
            
            var selectObjectsController = new SelectObjectsController(allSelectableUnits);

            var formationController = new FormationController(inputController, formationObject);

            var moveUnitsController = new MoveUnitsController(selectObjectsController, inputController, formationController);

            controllersManager.Add(inputController);
            controllersManager.Add(selectObjectsController);
        }
    }
}
