using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dreamteck.Splines;
using PrimeTween;
using UnityEngine;
using static GameEnum;

public class PassengerQueue : MonoBehaviour
{
    [SerializeField] private GameObject passengerPrefab;
    [SerializeField] private Transform container;

    [Header("COUPLING")]
    [SerializeField] private ParkingSlotManager parkingSlotManager;

    private Passenger[] _passengerPool;
    private Queue<Passenger> _passengers;

    [Header("VEHICLE")]
    private List<GameFaction> _passengerFactionPool;
    private List<GameFaction> _remainingPassengersFaction;


    [SerializeField] private SplineComputer pathSpline;

    [SerializeField] private int maxPassenger;

    #region ACTION
    public static event Action<int, GameFaction> setPassengerFactionEvent;
    public static event Action<GameFaction> noPassengerLeftForFactionEvent;
    public static event Action<int, CharacterAnimationState> changeAnimationEvent;
    #endregion

    private void Awake()
    {
        BaseVehicle.vehicleReachParkingSlotEvent += OnVehicleArrivedParkingSlot;
        VehicleFaction.vehicleFactionSetEvent += AddPassengerFactionPool;
        PassengerCollider.passengerGotInVehicleEvent += OnPassengerGotInVehicle;

        _passengerPool = new Passenger[maxPassenger];
        _passengers = new Queue<Passenger>();
        _passengerFactionPool = new List<GameFaction>();
        _remainingPassengersFaction = new List<GameFaction>();

        DelayInit();
    }

    private void OnDestroy()
    {
        BaseVehicle.vehicleReachParkingSlotEvent -= OnVehicleArrivedParkingSlot;
        VehicleFaction.vehicleFactionSetEvent -= AddPassengerFactionPool;
        PassengerCollider.passengerGotInVehicleEvent -= OnPassengerGotInVehicle;
    }

    private async void DelayInit()
    {
        await Task.Delay(1000);

        SpawnPassengers();

        await Task.Delay(2000);

        MoveToPosititon();
    }

    private void SpawnPassengers()
    {
        for (int i = 0; i < maxPassenger; i++)
        {
            if (i >= _passengerFactionPool.Count)
            {
                return;
            }

            GameObject passenger = Instantiate(passengerPrefab, container);

            passenger.name = $"Passenger {i}";

            passenger.transform.position = new Vector3(0, 0, 100 + 5 * i);

            Passenger passengerComponent = passenger.GetComponent<Passenger>();

            passengerComponent.SetPathToFollow(pathSpline);

            setPassengerFactionEvent?.Invoke(passenger.GetInstanceID(), GetPassengerFactionFromPool());

            _passengerPool[i] = passengerComponent;
            _passengers.Enqueue(passengerComponent);

            // passenger.gameObject.SetActive(false);
        }
    }

    private void MoveToPosititon()
    {
        Passenger[] passengersArray = _passengers.ToArray();

        for (int i = 0; i < passengersArray.Length; i++)
        {
            int index = i;

            Passenger passenger = passengersArray[i];

            changeAnimationEvent?.Invoke(passenger.gameObject.GetInstanceID(), CharacterAnimationState.Walking);

            float time = 0.3f + (2 + 0.2f * index) * (1 - passenger.CurrentPathPercent);

            Tween.Custom(passenger.CurrentPathPercent, 1 - 0.05f * index, duration: time, ease: Ease.Linear, onValueChange: newVal =>
            {
                passenger.PathFollower.SetPercent(newVal);
            })
            .OnComplete(() =>
            {
                passenger.CurrentPathPercent = 1 - 0.05f * index;

                changeAnimationEvent?.Invoke(passenger.gameObject.GetInstanceID(), CharacterAnimationState.Idle);
            });
        }
    }

    private async void OnVehicleArrivedParkingSlot()
    {
        while (_passengers.Count > 0)
        {
            bool IsFound = false;

            foreach (var parkingSlot in parkingSlotManager.ParkingSlots)
            {
                if (parkingSlot.ParkedVehicle == null)
                {
                    continue;
                }

                GameFaction faction = parkingSlot.ParkedVehicle.GetVehicleFaction();

                if (faction == _passengers.Peek().GetFaction())
                {
                    _passengers.Dequeue().GetInVehicle(parkingSlot.ParkedVehicle);

                    IsFound = true;

                    await Task.Delay(300);

                    break;
                }
            }

            if (!IsFound)
            {
                // MoveToPosititon();

                break;
            }
        }

        while (_passengerFactionPool.Count > 0)
        {
            bool isPassengerAvailable = false;

            for (int i = 0; i < _passengerPool.Length; i++)
            {
                if (!_passengerPool[i].gameObject.activeSelf)
                {
                    setPassengerFactionEvent?.Invoke(_passengerPool[i].gameObject.GetInstanceID(), GetPassengerFactionFromPool());

                    _passengers.Enqueue(_passengerPool[i]);

                    _passengerPool[i].gameObject.SetActive(true);

                    _passengerPool[i].Reset();

                    isPassengerAvailable = true;

                    break;
                }
            }

            if (!isPassengerAvailable)
            {
                break;
            }
        }

        MoveToPosititon();
    }

    private void AddPassengerFactionPool(GameFaction faction, int numberSeat)
    {
        for (int i = 0; i < numberSeat; i++)
        {
            _passengerFactionPool.Add(faction);
            _remainingPassengersFaction.Add(faction);
        }
    }

    private GameFaction GetPassengerFactionFromPool()
    {
        int index = UnityEngine.Random.Range(0, _passengerFactionPool.Count);

        GameFaction faction = _passengerFactionPool[index];

        _passengerFactionPool.RemoveAt(index);

        return faction;
    }

    private void OnPassengerGotInVehicle(BaseVehicle vehicle)
    {
        GameFaction faction = vehicle.GetVehicleFaction();

        _remainingPassengersFaction.Remove(faction);

        if (!_remainingPassengersFaction.Contains(faction))
        {
            noPassengerLeftForFactionEvent?.Invoke(faction);
        }
    }
}
