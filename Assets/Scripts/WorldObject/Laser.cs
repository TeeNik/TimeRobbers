using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _enableDuration = 0.35f;
    [SerializeField] private float _disableDuration = 1.25f;

    private BoxCollider2D _boxCollider;
    private float _time = 0;
    private bool _isAble = false;

    void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _time = _isAble ? _enableDuration : _disableDuration;
    }

    void FixedUpdate()
    {
        _time -= Time.fixedDeltaTime;

        if (_time <= 0)
        {
            _isAble = !_isAble;
            _time = _isAble ? _enableDuration : _disableDuration;
            _boxCollider.enabled = _isAble;
        }
    }

}
