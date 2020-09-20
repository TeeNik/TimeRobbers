using TMPro;
using UnityEngine;

public class PlanningView : MonoBehaviour
{

    [SerializeField] private TMP_Text _numOfRecords;

    public void SetNumberOfRecords(int count)
    {
        _numOfRecords.text = "Records: " + count;
    }

}
