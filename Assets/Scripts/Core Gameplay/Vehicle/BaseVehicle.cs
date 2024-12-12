using System;
using UnityEngine;
using UnityEngine.AI;
using static GameEnum;

public abstract class BaseVehicle : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent navMeshAgent;

    public static event Action<BaseVehicle> vehicleReachParkingSlotEvent;

    public abstract void Park();
    public abstract GameFaction GetVehicleFaction();

    protected void InvokeVehicleReachParkingSlotEvent()
    {
        vehicleReachParkingSlotEvent?.Invoke(this);
    }
}
