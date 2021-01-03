using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class GroundCollider : MonoBehaviour
{
    public LayerMask WhatIsGround;

    private CircleCollider2D _collider;

    public bool IsTouchingGround
    {
        get { return _collider.IsTouchingLayers(WhatIsGround); }
    }

    void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
    }

}
