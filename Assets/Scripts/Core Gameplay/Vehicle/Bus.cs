using System;
using System.Collections;
using UnityEngine;
using static GameEnum;

public class Bus : BaseVehicle
{
    [SerializeField] private ParkingSlotManager parkingSlotManager;

    [SerializeField] private VehicleFaction vehicleFaction;

    #region ACTION
    public static event Action<BaseVehicle> vehicleLeftParkingSlotEvent;
    #endregion

    private void Awake()
    {
        PassengerQueue.noPassengerLeftForFactionEvent += MoveOut;
    }

    public override GameFaction GetVehicleFaction()
    {
        return vehicleFaction.Faction;
    }

    public override void Park()
    {
        ParkingSlot emptyParkingSlot = parkingSlotManager.GetEmptyParkingSlot();

        if (emptyParkingSlot != null)
        {
            parkingSlotManager.ParkVehicle(this);

            navMeshAgent.destination = emptyParkingSlot.transform.position;

            StartCoroutine(CheckingReachingParkingSlot());
        }
    }

    private void OnDestroy()
    {
        PassengerQueue.noPassengerLeftForFactionEvent -= MoveOut;
    }

    private IEnumerator CheckingReachingParkingSlot()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.2f);

        yield return new WaitForSeconds(1);

        while (!NavMeshUtil.IsReachedDestination(navMeshAgent))
        {
            yield return waitForSeconds;
        }

        InvokeVehicleReachParkingSlotEvent();
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
