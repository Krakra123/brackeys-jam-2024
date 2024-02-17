using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    [SerializeField] private GameObject _ghost;
    [SerializeField] private GameObject _sprite;
    private float _time_run_FX = 0;
    private float _time_spawn_FX = 0;
    void Update()
    {
        if (_time_run_FX > 0)
        {
            _time_run_FX -= Time.deltaTime;
            _time_spawn_FX += Time.deltaTime;
            if (_time_spawn_FX >= 0.08f)
            {
                _time_spawn_FX = 0;
                Spawm_FX();
            }
        }
        else
        {
            _time_spawn_FX = 0.05f;
        }
    }
    public void Run(float _time)
    {
        _time_run_FX = _time;
    }

    void Spawm_FX()
    {
        GameObject _Ghost = Instantiate(_ghost, transform.position, Quaternion.identity);
        _Ghost.GetComponent<SpriteRenderer>().sprite = _sprite.GetComponent<SpriteRenderer>().sprite;
        _Ghost.transform.localScale = _sprite.transform.localScale;
    }
}
