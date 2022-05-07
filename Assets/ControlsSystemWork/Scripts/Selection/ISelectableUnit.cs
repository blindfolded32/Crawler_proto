using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface ISelectableUnit
{
    public NavMeshAgent NavMeshAgent { get; }
    public MeshRenderer MeshRenderer { get; }
    public Transform UnitTransform { get; }
}
