using UnityEngine;

public class Bus : BaseVehicle
{
    [SerializeField] private ParkingSlotManager parkingSlotManager;

    public override void Park()
    {
        ParkingSlot emptyParkingSlot = parkingSlotManager.GetEmptyParkingSlot();

        if (emptyParkingSlot != null)
        {
            parkingSlotManager.ParkVehicle(this);

            navMeshAgent.destination = emptyParkingSlot.transform.position;
        }
    }
}
