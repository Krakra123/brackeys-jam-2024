using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlaySound(GameSound.BGM);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
