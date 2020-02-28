using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private GameObject gameObj;
    // Update is called once per frame
    void Update()
    {
        transform.position = gameObj.transform.position;
    }
}
