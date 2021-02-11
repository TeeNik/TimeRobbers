using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraImageEffect : MonoBehaviour
{
    public Material MaterialEffect;

    public Transform MaskTransform;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Vector3 screenPos = GetComponent<Camera>().WorldToScreenPoint(MaskTransform.position);
        Vector3 viewportPos = GetComponent<Camera>().WorldToViewportPoint(MaskTransform.position);
        MaterialEffect.SetVector("_MaskTransform", viewportPos);

        Graphics.Blit(source, destination, MaterialEffect);
    }

}
