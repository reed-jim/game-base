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

    private Queue<Passenger> _passengers;

    [Header("VEHICLE")]
    private List<GameFaction> _passengerFactionPool;
    private List<GameFaction> _remainingPassengersFaction;


    [SerializeField] private SplineComputer pathSpline;

    [SerializeField] private int maxPassenger;

    #region ACTION
    public static event Action<int, GameFaction> setPassengerFactionEvent;
    public static event Action<GameFaction> noPassengerLeftForFactionEvent;
    #endregion

    private void Awake()
    {
        BaseVehicle.vehicleReachParkingSlotEvent += OnVehicleArrivedParkingSlot;
        VehicleFaction.vehicleFactionSetEvent += AddPassengerFactionPool;

        _passengers = new Queue<Passenger>();
        _passengerFactionPool = new List<GameFaction>();

        DelayInit();
    }

    private void OnDestroy()
    {
        BaseVehicle.vehicleReachParkingSlotEvent -= OnVehicleArrivedParkingSlot;
        VehicleFaction.vehicleFactionSetEvent -= AddPassengerFactionPool;
    }

    private async void DelayInit()
    {
        await Task.Delay(1000);

        SpawnPassengers();

        MoveToPosititon();
    }

    private void SpawnPassengers()
    {
        int poolPassengerSize = _passengerFactionPool.Count < maxPassenger ? _passengerFactionPool.Count : maxPassenger;

        for (int i = 0; i < poolPassengerSize; i++)
        {
            GameObject passenger = Instantiate(passengerPrefab, container);

            passenger.transform.position = new Vector3(6, 0, 50 + 2 * i);

            Passenger passengerComponent = passenger.GetComponent<Passenger>();

            passengerComponent.SetPathToFollow(pathSpline);

            setPassengerFactionEvent?.Invoke(passenger.GetInstanceID(), GetPassengerFactionFromPool());

            _passengers.Enqueue(passengerComponent);
        }
    }

    private void MoveToPosititon()
    {
        Passenger[] passengersArray = _passengers.ToArray();

        for (int i = 0; i < passengersArray.Length; i++)
        {
            int index = i;

            Passenger passenger = passengersArray[i];

            passenger.PathFollower.follow = true;

            Tween.Custom(0, 1 - 0.05f * i, duration: 1 + 2 / (1 - 0.05f * i), onValueChange: newVal =>
            {
                passenger.PathFollower.SetPercent(newVal);
            })
            .OnComplete(() =>
            {
                passenger.PathFollower.follow = false;
            });
        }
    }

    private async void OnVehicleArrivedParkingSlot(BaseVehicle vehicle)
    {
        while (_passengers.Count > 0)
        {
            GameFaction faction = vehicle.GetVehicleFaction();

            if (faction == _passengers.Peek().GetFaction())
            {
                _passengers.Dequeue().GetInVehicle(vehicle);

                _remainingPassengersFaction.Remove(faction);

                if (!_remainingPassengersFaction.Contains(faction))
                {
                    noPassengerLeftForFactionEvent?.Invoke(faction);
                }

                await Task.Delay(300);
            }
            else
            {
                break;
            }
        }
    }

    private void AddPassengerFactionPool(GameFaction faction)
    {
        _passengerFactionPool.Add(faction);

        _remainingPassengersFaction = _passengerFactionPool;
    }

    private GameFaction GetPassengerFactionFromPool()
    {
        int index = UnityEngine.Random.Range(0, _passengerFactionPool.Count);

        GameFaction faction = _passengerFactionPool[index];

        _passengerFactionPool.RemoveAt(index);

        return faction;
    }
}
