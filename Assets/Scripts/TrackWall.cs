using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackWall : MonoBehaviour
{
    [SerializeField] CarController carController;

    private void OnTriggerEnter(Collider other)
    {
        carController.noOfCollisions += 1;
    }
}
