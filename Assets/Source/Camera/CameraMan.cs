using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class CameraMan : MonoBehaviour
{
    private GameObject _player;

    [Header("Camera Following")]
    [SerializeField]
    private float _leftXBoundOffset;
    [SerializeField]
    private float _rightXBoundOffset;
    [SerializeField]
    private float _botYBoundOffset;
    [SerializeField]
    private float _topYBoundOffset;
    [SerializeField]
    private float _camaraFollowSmoothness;

    [SerializeField]
    private Vector2 _idealPlayerOffset;

    [SerializeField]
    private Transform[] _cameraBounds = new Transform[2];

    private bool _followingXAxis;
    private bool _followingYAxis;

    private float ShakePowter;
    private float TimeShake;
    private bool _shake;
    private float TimeDelayShake;
    private const float SPEED_SHAKE = 0.08f;
    private float TIME_SHAKE;

    // Update is called once per frame

    private void Start()
    {
        _player = GameManager.Instance.playerManager.gameObject;
    }

    private void Update()
    {
        Timer();

        FollowPlayerHandle();
    }

    void ShakeCamera()
    {
        Vector2 _offset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * ShakePowter;
        Vector2 _pos = _offset * Mathf.Pow(TimeShake / TIME_SHAKE,2) + new Vector2(transform.position.x, transform.position.y);
        Camera.main.transform.position = new Vector3(_pos.x, _pos.y,-10);
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
            Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
    }

    private void FollowPlayerHandle()
    {
        Vector2 newCameraPosition = transform.position;
        Vector2 deltaWithPlayer = (Vector2)transform.position - ((Vector2)_player.transform.position - _idealPlayerOffset);

        const float SNAPDISTANCE = .1f;

        if (_followingXAxis)
        {
            float target = _player.transform.position.x - _idealPlayerOffset.x;
            newCameraPosition.x = Mathf.Lerp(
                newCameraPosition.x, 
                target, 
                _camaraFollowSmoothness
            );

            if (Mathf.Abs(newCameraPosition.x - target) < SNAPDISTANCE) 
            {
                newCameraPosition.x = target;
                _followingXAxis = false;
            }
        }
        else if (deltaWithPlayer.x < _leftXBoundOffset || deltaWithPlayer.x > _rightXBoundOffset)
        {
            _followingXAxis = true;
        }
        
        if (_followingYAxis)
        {
            float target = _player.transform.position.y - _idealPlayerOffset.y;
            newCameraPosition.y = Mathf.Lerp(
                newCameraPosition.y, 
                target, 
                _camaraFollowSmoothness
            );

            if (Mathf.Abs(newCameraPosition.y - target) < SNAPDISTANCE) 
            {
                newCameraPosition.y = target;
                _followingYAxis = false;
            }
        }
        else if (deltaWithPlayer.y < _botYBoundOffset || deltaWithPlayer.y > _topYBoundOffset)
        {
            _followingYAxis = true;
        }

        newCameraPosition.x = Mathf.Clamp(newCameraPosition.x, _cameraBounds[0].position.x, _cameraBounds[1].position.x);
        newCameraPosition.y = Mathf.Clamp(newCameraPosition.y, _cameraBounds[0].position.y, _cameraBounds[1].position.y);

        transform.position = newCameraPosition;
    }

    public void Shake(float _powter, float _timeShake)
    {
        ShakePowter = _powter;
        TimeShake = _timeShake;
        TIME_SHAKE = _timeShake;
    }
}
