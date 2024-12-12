using UnityEngine;

public class ParkingSlot : MonoBehaviour
{
    private BaseVehicle _parkedVehicle;

    public BaseVehicle ParkedVehicle
    {
        get => _parkedVehicle;
        set => _parkedVehicle = value;
    }

    public bool IsEmpty
    {
        get => _parkedVehicle == null;
    }
}
