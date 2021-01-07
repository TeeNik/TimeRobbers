using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitView : MonoBehaviour
{

    [SerializeField] private Image _portrait;
    [SerializeField] private GameObject _border;

    public void Init(Color color)
    {
        _portrait.color = color;
    }

    public void SetActive(bool isActive)
    {
        _border.SetActive(isActive);
    }

}
