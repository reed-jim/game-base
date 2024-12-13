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
    protected int _numberSeatFilled;
    protected int _confirmedNumberSeatFilled;
    protected bool _isParked;
    #endregion

    public static event Action vehicleReachParkingSlotEvent;
    public static event Action<int> getInVehicleEvent;

    public int NumberSeat
    {
        get => numberSeat;
        set => numberSeat = value;
    }

    public int NumberSeatFilled
    {
        get => _numberSeatFilled;
        set => _numberSeatFilled = value;
    }

    public int ConfirmedNumberSeatFilled
    {
        get => _confirmedNumberSeatFilled;
        set => _confirmedNumberSeatFilled = value;
    }

    public bool IsParked
    {
        get => _isParked;
        set => _isParked = value;
    }

    public abstract void Park();
    public abstract void FillSeat();
    public abstract void GetInVehicle();
    public abstract GameFaction GetVehicleFaction();

    protected void InvokeVehicleReachParkingSlotEvent()
    {
        vehicleReachParkingSlotEvent?.Invoke();
    }

    protected void InvokeGetInVehicleEvent()
    {
        getInVehicleEvent?.Invoke(gameObject.GetInstanceID());
    }

    public bool IsFull()
    {
        return _numberSeatFilled == numberSeat;
    }
}
