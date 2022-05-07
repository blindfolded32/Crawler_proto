using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Units
{
    public class Unit : MonoBehaviour, ISelectableUnit
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Transform _unitTransform;

        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        public MeshRenderer MeshRenderer => _meshRenderer;
        public Transform UnitTransform => _unitTransform;
    }
}