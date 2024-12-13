using System;
using UnityEngine;
using UnityEngine.AI;
using static GameEnum;

public abstract class BaseVehicle : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent navMeshAgent;

    [Header("CUSTOMIZE")]
    [SerializeField] protected int numberSeat;

    #region PRIVATE FIELD
    protected int _numberSeatLeft;
    protected bool _isParked;
    #endregion

    public static event Action vehicleReachParkingSlotEvent;

    public int NumberSeat
    {
        get => numberSeat;
        set => numberSeat = value;
    }

    public bool IsParked
    {
        get => _isParked;
        set => _isParked = value;
    }

    public abstract void Park();
    public abstract GameFaction GetVehicleFaction();

    protected void InvokeVehicleReachParkingSlotEvent()
    {
        vehicleReachParkingSlotEvent?.Invoke();
    }
}
