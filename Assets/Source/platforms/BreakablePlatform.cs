using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    public float _timeDelayBreak = 0;
    public float _timeDelayReset = 0;
    [SerializeField] private GameObject _effectBreak;
    private float _timeBreak = 0;
    private float _timeReset = 0;
    private bool _break = false;
    private bool _crack = false;

    private GameObject _sprite;
    private void Start()
    {
        _sprite = transform.Find("Sprite").gameObject;
        _timeBreak = _timeDelayReset;
        _timeReset = _timeDelayReset;
    }

    private void Update()
    {
        Timer();
        if (IsCollide() == true)
        {
            _crack = true;
            _timeBreak = _timeDelayBreak;
        }
    }

    void Timer()
    {
        if (_crack == true) 
        { 
            if (_timeBreak > 0)
            {
                gameObject.GetComponent<Animator>().Play("Base Layer.shake");
                _timeBreak -= Time.deltaTime;
            }
            else if(_break == false)
            {
                _timeBreak = 0;
                _timeReset = _timeDelayReset;
                Break();
            }

            if (_timeReset > 0)
            {
                _timeReset -= Time.deltaTime;
                if (_timeReset <= 0.3f)
                {
                    _sprite.SetActive(true);
                    GetComponent<Animator>().Play("Base Layer.reset");
                }
            }
            else if (_break == true)
            {
                reset();
                _timeReset = 0;
            }
        }
    }

    bool IsCollide()
    {
        bool Hit = false;
        if (_crack == false)
        {
            Vector2 RayPos = new Vector2(transform.position.x - 0.4f, transform.position.y + 0.52f);
            RaycastHit2D RayHit = Physics2D.Raycast(RayPos, Vector2.right, 0.8f);
            Debug.DrawRay(RayPos, Vector2.right * 0.8f,Color.green);
            if (RayHit.collider != null)
            {
                Hit = true;
            }
        }
        return Hit;
    }
    private void reset()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<Animator>().Play("Base Layer.default");
        _break = false;
        _crack = false;
    }

    private void Break()
    {
        Instantiate(_effectBreak, transform.position, Quaternion.identity);
        GetComponent<BoxCollider2D>().enabled = false;
        _break = true;
        _sprite.SetActive(false);
    }
}
