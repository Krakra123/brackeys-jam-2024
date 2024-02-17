using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeStink : MonoBehaviour
{
    public bool stinkDisable { get; set; }

    private void Start()
    {
        stinkDisable = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!stinkDisable && other.tag == "Player")
        {
            GameManager.Instance.Restart();
        }
    }
}
