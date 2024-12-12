using com.unity3d.mediation;
using UnityEngine;

public class PassengerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        BaseVehicle vehicle = other.GetComponentInParent<BaseVehicle>();

        if (vehicle != null)
        {
            gameObject.SetActive(false);
        }
    }
}
