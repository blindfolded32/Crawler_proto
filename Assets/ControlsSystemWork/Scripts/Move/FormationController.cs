using Engine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserInput;
using System.Linq;

public class FormationController : IUpdatable, IController
{
    private float _timeTillShowFormation = 1f;
    private float _timeTillShowFormationCountDown;
    private bool _isFormationWaiting;
    private bool _isFormationShown;
    private GameObject _formationObject;

    private List<Transform> _transforms;

    //public GameObject FormationObject => _formationObject;

    public FormationController(InputController inputController, GameObject formation)
    {
        _formationObject = formation;
        _transforms = new List<Transform>();
        _transforms.AddRange(_formationObject.GetComponentsInChildren<Transform>().Where(t => t != _formationObject.transform));

        inputController.OnClickDownRMB += StartWaitingFormation;
        //inputController.OnClickUpRMB += ;
    }

    public void LocalUpdate(float deltaTime)
    {
        if (_isFormationWaiting)
        {
            _timeTillShowFormationCountDown -= deltaTime;

            if(_timeTillShowFormationCountDown < 0)
            {
                ShowFormation();
            }
        }
        if (_isFormationShown)
        {
            
        }
    }

    public List<Transform> GetFormationPoints(Vector3 point, Vector3 direction)
    {
        _formationObject.transform.position = point;
        _formationObject.transform.forward = direction;

        return _transforms;
    }

    public Vector3 GetFormationObjectPosition()
    {
        return _formationObject.transform.position;
    }


    private void ShowFormation()
    {
       // _formation.transform.position =
        _formationObject.SetActive(true);
        _isFormationShown = true;
    }

    private void StartWaitingFormation()
    {
        _timeTillShowFormationCountDown = _timeTillShowFormation;
        _isFormationWaiting = true;
    }

    private void StopWaitingFormation()
    {
        _isFormationWaiting = false;
    }


}
