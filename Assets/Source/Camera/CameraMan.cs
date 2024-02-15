using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    public bool followPlayer = true;
    [SerializeField]
    private float smoothness = 0.5f;
    public GameObject Player;

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, Player.transform.position, smoothness);
    }
}
