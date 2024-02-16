using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour
{
    public Vector2 Offset = Vector2.zero;
    public bool followPlayer = true;
    [SerializeField]
    private float smoothness = 0.5f;
    private GameObject Camera;
    public GameObject Player;
    private float ShakePowter;
    private float TimeShake;
    private bool _shake;
    private float TimeDelayShake;
    private const float SPEED_SHAKE = 0.08f;
    private float TIME_SHAKE;

    // Update is called once per frame

    private void Start()
    {
        Camera = transform.Find("Camera").gameObject;
    }
    void Update()
    {
        Timer();
    }
    private void FixedUpdate()
    {
        if (followPlayer == true)
        {
            FollowPlayer();
        }
    }

    void ShakeCamera()
    {
        Vector2 _offset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * ShakePowter;
        Vector2 _pos = _offset * Mathf.Pow(TimeShake / TIME_SHAKE,2) + new Vector2(transform.position.x, transform.position.y);
        Camera.transform.position = new Vector3(_pos.x, _pos.y,-10);
    }

    private void Timer()
    {
        if (TimeShake > 0)
        {
            _shake = true;
            TimeShake -= Time.deltaTime;
        }
        else
        {
            _shake = false;
            TimeShake = 0;
        }

        if (_shake)
        {
            if (TimeDelayShake <= 0)
            {
                ShakeCamera();
            }
            else
            {
                TimeDelayShake -= Time.deltaTime;
            }
        }
        else
        {
            Camera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }

    void FollowPlayer()
    {
        if (Player == null)
        {
            Debug.LogError("WibuKa : không tìm thấy object player {Player = null}");
        }
        transform.position = Vector2.Lerp(transform.position, Player.transform.position + new Vector3(Offset.x, Offset.y,0), smoothness);
    }

    void Shake(float _powter, float _timeShake)
    {
        ShakePowter = _powter;
        TimeShake = _timeShake;
        TIME_SHAKE = _timeShake;
    }
}
