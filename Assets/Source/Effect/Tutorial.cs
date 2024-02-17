using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float reading_distance;
    [SerializeField] private float _distance;
    private float _transparent;

    private void Update()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);
        if (_distance <= reading_distance)
        {
            _transparent += Time.deltaTime * 4;
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, _transparent);
        }
        else
        {
            _transparent -= Time.deltaTime * 4;
            _transparent = Mathf.Clamp(_transparent, 0, 1);
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, _transparent);
        }
    }

}
