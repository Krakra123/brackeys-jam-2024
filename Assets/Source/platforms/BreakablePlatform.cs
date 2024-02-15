using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    public float TimeDelayBreak = 0;
    public float TimeDelayReset = 0;
    [SerializeField] private GameObject EffectBreak;
    private float TimeBreak = 0;
    private float TimeReset = 0;
    private bool _break = false;
    private bool _crack = false;

    private GameObject Sprite;
    private void Start()
    {
        Sprite = transform.Find("Sprite").gameObject;
        TimeBreak = TimeDelayReset;
        TimeReset = TimeDelayReset;
    }

    private void Update()
    {
        Timer();
        if (IsCollide() == true)
        {
            _crack = true;
            TimeBreak = TimeDelayBreak;
        }
    }

    void Timer()
    {
        if (_crack == true) 
        { 
            if (TimeBreak > 0)
            {
                gameObject.GetComponent<Animator>().Play("Base Layer.shake");
                TimeBreak -= Time.deltaTime;
            }
            else if(_break == false)
            {
                TimeBreak = 0;
                TimeReset = TimeDelayReset;
                Break();
            }

            if (TimeReset > 0)
            {
                TimeReset -= Time.deltaTime;
            }
            else if (_break == true)
            {
                reset();
                TimeReset = 0;
            }
        }
    }

    bool IsCollide()
    {
        bool Hit = false;
        if (_crack == false)
        {
            Vector2 RayPos = new Vector2(transform.position.x - 0.4f, transform.position.y + 0.51f);
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
        Sprite.SetActive(true);
        _break = false;
        _crack = false;
    }

    private void Break()
    {
        Instantiate(EffectBreak, transform.position, Quaternion.identity);
        GetComponent<BoxCollider2D>().enabled = false;
        _break = true;
        Sprite.SetActive(false);
    }
}
