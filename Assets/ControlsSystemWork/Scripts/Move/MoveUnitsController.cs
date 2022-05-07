using Selection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserInput;

public class MoveUnitsController
{
    private List<ISelectableUnit> _selectedUnits;
    private Camera _camera;
    public MoveUnitsController(SelectObjectsController selectObjectsController, InputController inputController)
    {
        _selectedUnits = selectObjectsController.UnitsSelected;
        _camera = Camera.main;
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
                Move(hits.point);
            }
        }
    }

    private void Move(Vector3 point)
    {
        for (int i = 0; i < _selectedUnits.Count; i++)
        {
            _selectedUnits[i].NavMeshAgent.SetDestination(point);
        }
    }
}
