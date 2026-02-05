using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] GameObject player;
    void Start()
    {
        if(player == null)
            this.transform.position = new Vector3(player.transform.position.x + 5, transform.position.y, transform.position.z);
    }
    void Update()
    {
        this.transform.position = new Vector3(player.transform.position.x + 5, transform.position.y, transform.position.z);
    }
}
