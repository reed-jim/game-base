using System;
using com.unity3d.mediation;
using UnityEngine;

public class PassengerCollider : MonoBehaviour
{
    #region ACTION
    public static event Action<BaseVehicle> passengerGotInVehicleEvent;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        BaseVehicle vehicle = other.GetComponentInParent<BaseVehicle>();

        if (vehicle != null)
        {
            gameObject.SetActive(false);

            passengerGotInVehicleEvent?.Invoke(vehicle);
        }
    }
}
