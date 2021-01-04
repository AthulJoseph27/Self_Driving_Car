using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private int index;

    [SerializeField] private CarController carController;
    //[SerializeField] private GameObject _gameObject;

    private void OnTriggerEnter(Collider other)
    {
        if ((!carController.checkPoints[index]) && carController.currentCheckPoint < index)
        {
            carController.checkPoints[index] = true;
            carController.currentCheckPoint = index;
            carController.noOfCheckPointsPassed += 1;
            
            //Debug.Log("Passed Through Checkpoint");
        }
    }
}
