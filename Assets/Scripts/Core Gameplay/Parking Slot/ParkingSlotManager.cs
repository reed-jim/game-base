using UnityEngine;

public class ParkingSlotManager : MonoBehaviour
{
    #region PRIVATE FIELD
    [SerializeField] private ParkingSlot[] _parkingSlots;
    private ParkingSlot _emptyParkingSlot;
    #endregion

    public ParkingSlot[] ParkingSlots
    {
        get => _parkingSlots;
        set => _parkingSlots = value;
    }

    public ParkingSlot GetEmptyParkingSlot()
    {
        _emptyParkingSlot = null;

        foreach (var parkingSlot in ParkingSlots)
        {
            if (parkingSlot.IsEmpty)
            {
                _emptyParkingSlot = parkingSlot;
            }
        }

        return _emptyParkingSlot;
    }

    public void ParkVehicle(BaseVehicle vehicle)
    {
        if (_emptyParkingSlot != null)
        {
            _emptyParkingSlot.ParkedVehicle = vehicle;
        }
    }
}