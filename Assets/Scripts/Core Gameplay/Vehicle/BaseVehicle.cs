using UnityEngine;
using UnityEngine.AI;

public abstract class BaseVehicle : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent navMeshAgent;

    public abstract void Park();
}
