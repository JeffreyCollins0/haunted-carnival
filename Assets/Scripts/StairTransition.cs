using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTransition : MonoBehaviour
{
    [SerializeField] private Transform endpoint;

    public Transform getEndpoint(){
        return endpoint;
    }
}
