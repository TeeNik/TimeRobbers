using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class Forcer : MonoBehaviour
{

    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _endPos;


    private GameObject _target;
    private bool _movingForward;
    private bool _movingBackward;

    private TweenerCore<Vector3, Vector3, VectorOptions> _tween;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_movingForward)
        {
            if (_tween.IsActive())
            {
                _tween.Kill();
            }
            _movingForward = true;
            _target = collision.gameObject;
            _target.transform.parent = transform;

            _tween = transform.DOMove(_endPos.position, .25f).SetEase(Ease.InCubic).OnComplete(() =>
            {
                _movingForward = false;
                if (_target != null)
                {
                    var character = _target.GetComponent<CharacterController>();
                    character.AddJumpForce(3);
                }

                _movingBackward = false;
                _tween = transform.DOMove(_startPos.position, 3).OnComplete(() =>
                {
                    _movingBackward = false;
                });
            });
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (_target != null)
        {
            _target.transform.parent = null;
            _target = null;
        }
    }

}
