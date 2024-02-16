using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRemove : MonoBehaviour
{
    [SerializeField] private float _time_auto_remove = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Remove());
    }

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(_time_auto_remove);
        Destroy(gameObject);
    }
}
