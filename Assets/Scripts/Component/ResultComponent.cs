using System;
using UnityEngine;
using UnityEngine.UI;
using MyPuzzle;

public class ResultComponent
    : MonoBehaviour
{
    public Action OnNext;

    [SerializeField] private Button NextButton;

    private void Awake()
    {
        this.NextButton.onClick.AddListener(() => this.OnNext.SafeInvoke());
    }

}
