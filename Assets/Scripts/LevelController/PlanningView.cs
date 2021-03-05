using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlanningView : MonoBehaviour
{
    [SerializeField] private GameObject _menu;


    [Header("Portraits")]
    [SerializeField] private List<Color> _characterColors;
    [SerializeField] private Transform _portraitParent;
    [SerializeField] private PortraitView _portraitView;

    [Header("History")]
    [SerializeField] private Transform _historyParent;
    [SerializeField] private PortraitView _historyItemView;

    [Header("History")]
    [SerializeField] private Material _material;
    [SerializeField] private Transform _cameraMask;


    private List<PortraitView> _portraits = new List<PortraitView>();
    private List<PortraitView> _historyItems = new List<PortraitView>();

    private CharacterType[] _allowedCharacters;
    private Action<int> _onCharacterSelected;
    private Action<int> _onHistoryItemDeleted;

    private int _selectedCharacter;
    private int _selectedHistoryItem;

    private SpriteRenderer _characterView;

    public void Init(Action<int> onCharacterSelected, Action<int> onHistoryItemDeleted)
    {
        _onCharacterSelected = onCharacterSelected;
        _onHistoryItemDeleted = onHistoryItemDeleted;
    }

    public void InitLevel(Level level)
    {
        _characterView = level.CharacterView;
        _cameraMask.transform.position = level.CharacterView.transform.position;

        _allowedCharacters = level.AllowedCharacters;
        foreach (var portraitView in _portraits)
        {
            Destroy(portraitView.gameObject);
        }
        _portraits.Clear();
        for (var i = 0; i < _allowedCharacters.Length; i++)
        {
            var item = Instantiate(_portraitView, _portraitParent);
            item.Init(_characterColors[(int)_allowedCharacters[i]]);
            _portraits.Add(item);
        }
        if (_allowedCharacters.Length > 1)
        {
            SetVisibility(true);
        }
        else
        {
            _onCharacterSelected((int)_allowedCharacters[0]);
        }
    }

    void Update()
    {
        if (_menu.gameObject.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _onCharacterSelected.Invoke(_selectedCharacter);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                _selectedCharacter = _selectedCharacter == 0 ? _portraits.Count - 1 : _selectedCharacter - 1;
                SetActiveCharacter(_selectedCharacter);
                PlayChromaticAberration();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _selectedCharacter = _selectedCharacter == _portraits.Count - 1 ? 0 : _selectedCharacter + 1;
                SetActiveCharacter(_selectedCharacter);
                PlayChromaticAberration();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                _selectedHistoryItem = _selectedHistoryItem == 0 ? _historyItems.Count - 1 : _selectedHistoryItem - 1;
                SetActiveHistoryItem(_selectedHistoryItem);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                _selectedHistoryItem = _selectedHistoryItem == _historyItems.Count - 1 ? 0 : _selectedHistoryItem + 1;
                SetActiveHistoryItem(_selectedHistoryItem);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                _onHistoryItemDeleted.Invoke(_selectedHistoryItem);
                DeleteSelectedHistoryItem(_selectedHistoryItem);
            }
        }
    }

    private void PlayChromaticAberration()
    {
        _material.SetFloat("_Amount", 0.0f);
        _material.DOFloat(0.01f, "_Amount", 0.1f).SetLoops(2, LoopType.Yoyo);
    }

    public void SetActiveCharacter(int index)
    {
        for (int i = 0; i < _portraits.Count; ++i)
        {
            _portraits[i].SetActive(i == index);
        }

        _characterView.color = _characterColors[index];
    }

    public void SetActiveHistoryItem(int index)
    {
        for (int i = 0; i < _historyItems.Count; ++i)
        {
            _historyItems[i].SetActive(i == index);
        }
    }

    public void CreateHistoryItem(int characterType)
    {
        var item = Instantiate(_historyItemView, _historyParent);
        item.Init(_characterColors[characterType]);
        _historyItems.Add(item);
    }

    public void DeleteSelectedHistoryItem(int index)
    {
        var itemToRemove = _historyItems[index];
        _historyItems.Remove(itemToRemove);
        Destroy(itemToRemove.gameObject);
        _selectedHistoryItem = Mathf.Max(0, _historyItems.Count - 1);
        SetActiveHistoryItem(_selectedHistoryItem);
    }

    public void SetVisibility(bool isVisible)
    {
        _menu.SetActive(isVisible);
        _selectedCharacter = 0;
        _selectedHistoryItem = _historyItems.Count - 1;
        SetActiveCharacter(0);
        SetActiveHistoryItem(_selectedHistoryItem);

        _characterView.gameObject.SetActive(isVisible);

        if (isVisible)
        {
            _cameraMask.DOMoveZ(150, 0.5f).SetEase(Ease.InCubic);
        }
        else 
        {
            _cameraMask.DOMoveZ(2000, 0.35f).SetEase(Ease.InCubic);
        }
    }
}
