using System;
using System.Collections;
using System.Linq;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class Bus : BaseVehicle
{
    [SerializeField] private VehicleFaction vehicleFaction;

    [SerializeField] private PassengerInVehicle[] passengers;

    #region PRIVATE FIELD
    private ParkingSlotManager _parkingSlotManager;
    private ParkingSlot _parkingSlot;
    private float _initialScale;
    #endregion

    #region ACTION
    public static event Action<BaseVehicle> vehicleLeftParkingSlotEvent;
    #endregion

    private void Awake()
    {
        PassengerQueue.noPassengerLeftForFactionEvent += MoveOut;
        ParkingSlotManager.bindParkingSlotManagerEvent += BindParkingSlotManager;

        _initialScale = transform.localScale.x;
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

        if (CheckFrontObstacle())
        {
            Tween.Position(transform, transform.position + transform.forward, duration: 0.3f, cycles: 2, cycleMode: CycleMode.Yoyo);

            // Tween.Scale(transform, 1.1f * _initialScale, duration: 0.2f, cycles: 6, cycleMode: CycleMode.Yoyo);

            return;
        }

        if (emptyParkingSlot != null)
        {
            _parkingSlotManager.ParkVehicle(this);

            navMeshAgent.destination = emptyParkingSlot.transform.position;

            _parkingSlot = emptyParkingSlot;

            StartCoroutine(CheckingReachingParkingSlot());
        }
    }

    private bool CheckFrontObstacle()
    {
        RaycastHit obstacle;

        Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), transform.forward, out obstacle, 10);

        if (obstacle.collider == null)
        {
            return false;
        }
        else
        {
            return true;
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

        Tween.Custom(transform.eulerAngles.y, _parkingSlot.transform.eulerAngles.y - 25f, duration: 0.5f, onValueChange: newVal =>
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, newVal, transform.eulerAngles.z);
        });

        InvokeVehicleReachParkingSlotEvent();

        _isParked = true;
    }

    private void MoveOut()
    {
        navMeshAgent.destination = transform.position + new Vector3(50, 0, -20);

        vehicleLeftParkingSlotEvent?.Invoke(this);
    }

    private void MoveOut(GameFaction faction)
    {
        if (faction == GetVehicleFaction())
        {
            MoveOut();
        }
    }

    public override void FillSeat()
    {
        _numberSeatFilled++;
    }

    public override void GetInVehicle()
    {
        if (_confirmedNumberSeatFilled == 0)
        {
            directionArrow.SetActive(false);

            InvokeVehicleRoofFillEvent();
        }

        passengers[_confirmedNumberSeatFilled].gameObject.SetActive(true);
        passengers[_confirmedNumberSeatFilled].SetColor(FactionUtility.GetColorForFaction(vehicleFaction.Faction));

        _confirmedNumberSeatFilled++;

        if (_confirmedNumberSeatFilled == numberSeat)
        {
            MoveOut();
        }

        InvokeGetInVehicleEvent();
    }
}
