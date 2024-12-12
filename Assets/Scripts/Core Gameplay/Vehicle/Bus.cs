using System;
using System.Collections;
using UnityEngine;

public class Bus : BaseVehicle
{
    [SerializeField] private ParkingSlotManager parkingSlotManager;

    [SerializeField] private VehicleFaction vehicleFaction;

    public override GameEnum.GameFaction GetVehicleFaction()
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
}
