using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMaskScript : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    public float Distance = 5;
    public Transform MaskTransform;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        _spriteRenderer.GetPropertyBlock(mpb);

        mpb.SetFloat("_RenderDistance", MaskTransform.localScale.x);
        mpb.SetFloat("_MaskTargetX", MaskTransform.position.x);
        mpb.SetFloat("_MaskTargetY", MaskTransform.position.y);
        mpb.SetFloat("_MaskType", 0);

        _spriteRenderer.SetPropertyBlock(mpb);
    }
}
