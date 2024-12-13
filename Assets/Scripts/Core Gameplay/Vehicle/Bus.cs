using System;
using System.Collections;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class Bus : BaseVehicle
{
    [SerializeField] private VehicleFaction vehicleFaction;

    #region PRIVATE FIELD
    private ParkingSlotManager _parkingSlotManager;
    private ParkingSlot _parkingSlot;
    #endregion

    #region ACTION
    public static event Action<BaseVehicle> vehicleLeftParkingSlotEvent;
    #endregion

    private void Awake()
    {
        PassengerQueue.noPassengerLeftForFactionEvent += MoveOut;
        ParkingSlotManager.bindParkingSlotManagerEvent += BindParkingSlotManager;

        _numberSeatLeft = numberSeat;
    }

    private void OnDestroy()
    {
        PassengerQueue.noPassengerLeftForFactionEvent -= MoveOut;
        ParkingSlotManager.bindParkingSlotManagerEvent -= BindParkingSlotManager;
    }

    private void BindParkingSlotManager(ParkingSlotManager parkingSlotManager)
    {
        _parkingSlotManager = parkingSlotManager;
    }

    public override GameFaction GetVehicleFaction()
    {
        return vehicleFaction.Faction;
    }

    public override void Park()
    {
        ParkingSlot emptyParkingSlot = _parkingSlotManager.GetEmptyParkingSlot();

        if (emptyParkingSlot != null)
        {
            _parkingSlotManager.ParkVehicle(this);

            navMeshAgent.destination = emptyParkingSlot.transform.position;

            _parkingSlot = emptyParkingSlot;

            StartCoroutine(CheckingReachingParkingSlot());
        }
    }

    private IEnumerator CheckingReachingParkingSlot()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.2f);

        yield return new WaitForSeconds(1);

        while (!NavMeshUtil.IsReachedDestination(navMeshAgent))
        {
            yield return waitForSeconds;
        }

        Tween.Custom(transform.eulerAngles.y, _parkingSlot.transform.eulerAngles.y, duration: 0.5f, onValueChange: newVal =>
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, newVal, transform.eulerAngles.z);
        });

        InvokeVehicleReachParkingSlotEvent();

        _isParked = true;
    }

    private void MoveOut(GameFaction faction)
    {
        if (faction == GetVehicleFaction())
        {
            navMeshAgent.destination = transform.position + new Vector3(50, 0, 0);

            vehicleLeftParkingSlotEvent?.Invoke(this);
        }
    }
}
