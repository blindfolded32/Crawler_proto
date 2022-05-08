using Selection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserInput;

public class MoveUnitsController
{
    private List<ISelectableUnit> _selectedUnits;
    private Camera _camera;
    private FormationController _formationController;
    public MoveUnitsController(SelectObjectsController selectObjectsController, InputController inputController, FormationController formationController)
    {
        _selectedUnits = selectObjectsController.UnitsSelected;
        _camera = Camera.main;
        _formationController = formationController;
        inputController.OnClickUpRMB += CheckPosition;
    }

    public void CheckPosition()
    {
        if (_selectedUnits.Count == 0) return;

        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hits))
        {
            if (hits.collider.TryGetComponent(out ISelectableUnit selectableUnit))
            {
                Debug.Log("Недостижимая область");
            }
            else
            {
                var direction = hits.point - _formationController.GetFormationObjectPosition();
                Move(hits.point, direction);
            }
        }
    }

    private void Move(Vector3 point, Vector3 direction)
    {
        var positions = _formationController.GetFormationPoints(point, direction);

        for (int i = 0; i < _selectedUnits.Count; i++)
        {
            _selectedUnits[i].NavMeshAgent.SetDestination(positions[i].position);
        }
    }
}
