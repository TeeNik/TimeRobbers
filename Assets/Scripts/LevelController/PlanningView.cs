using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlanningView : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private TMP_Text _numOfRecords;

    [Header("Portraits")]
    [SerializeField] private List<Color> _characterColors;
    [SerializeField] private Transform _portraitParent;
    [SerializeField] private PortraitView _portraitView;

    private List<PortraitView> _portraits = new List<PortraitView>();

    void Start()
    {
        foreach (var color in _characterColors)
        {
            var item = Instantiate(_portraitView, _portraitParent);
            item.Init(color);
            _portraits.Add(item);
        }
    }

    public void SetVisibility(bool isVisible)
    {
        _menu.SetActive(isVisible);
    }

    public void SetNumberOfRecords(int count)
    {
        _numOfRecords.text = "Records: " + count;
    }

}
