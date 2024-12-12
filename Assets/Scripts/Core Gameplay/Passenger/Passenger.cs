using System.Threading.Tasks;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.AI;
using static GameEnum;

public class Passenger : MonoBehaviour
{
    [SerializeField] private PassengerFaction passengerFaction;

    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private SplineFollower pathFollower;

    public NavMeshAgent NavMeshAgent
    {
        get => navMeshAgent;
    }

    public SplineFollower PathFollower
    {
        get => pathFollower;
        set => pathFollower = value;
    }

    private void Awake()
    {
        pathFollower.follow = false;

        navMeshAgent.isStopped = true;
    }

    private void OnDestroy()
    {
        
    }

    public void SetPathToFollow(SplineComputer spline)
    {
        pathFollower.spline = spline;
    }

    public GameFaction GetFaction()
    {
        return passengerFaction.Faction;
    }

    public async void GetInVehicle(BaseVehicle vehicle)
    {
        navMeshAgent.isStopped = false;

        navMeshAgent.destination = vehicle.transform.position;
    }
}
