using TMPro;
using UnityEngine;

public class PlanningView : MonoBehaviour
{

    [SerializeField] private GameObject _menu;
    [SerializeField] private TMP_Text _numOfRecords;

    public void SetVisibility(bool isVisible)
    {
        _menu.SetActive(isVisible);
    }

    public void SetNumberOfRecords(int count)
    {
        _numOfRecords.text = "Records: " + count;
    }

}
