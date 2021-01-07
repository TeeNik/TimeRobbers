using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _enableDuration = 0.35f;
    [SerializeField] private float _disableDuration = 1.25f;

    [SerializeField] private Color _enableColor;
    [SerializeField] private Color _disableColor;

    private BoxCollider2D _boxCollider;
    private SpriteRenderer _spriteRenderer;

    private float _time = 0;
    private bool _isAble = false;



    void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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

            _spriteRenderer.DOColor(_isAble ? _enableColor : _disableColor, 0.1f);
            transform.DOScaleY(_isAble ? .5f : .3f, .1f);
        }
    }

}
