using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// \file
public class FollowObject : MonoBehaviour
{
    [SerializeField] private GameObject gameObj; ///< get referece to a game obj
    // Update is called once per frame
    void Update()
    {
        transform.position = gameObj.transform.position; ///this object poisiton equals the refenced objects position
    }
}
