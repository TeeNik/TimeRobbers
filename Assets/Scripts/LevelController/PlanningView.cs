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
        for (var i = 0; i < _characterColors.Count; i++)
        {
            var item = Instantiate(_portraitView, _portraitParent);
            item.Init(_characterColors[i]);
            _portraits.Add(item);
        }
        SetActiveCharacter(0);
    }

    public void SetActiveCharacter(int index)
    {
        for (int i = 0; i < _portraits.Count; ++i)
        {
            _portraits[i].SetActive(i == index);
        }
    }

    public void SetVisibility(bool isVisible)
    {
        _menu.SetActive(isVisible);
        SetActiveCharacter(0);
    }

    public void SetNumberOfRecords(int count)
    {
        _numOfRecords.text = "Records: " + count;
    }

}
