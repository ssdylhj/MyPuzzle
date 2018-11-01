using System;
using UnityEngine;
using UnityEngine.UI;
using MyPuzzle;

public class ResultComponent
    : MonoBehaviour
{
    public Action OnBack;
    public Action OnNext;

    [SerializeField] private Button BackButton;
    [SerializeField] private Button NextButton;

    private void Awake()
    {
        this.BackButton.onClick.AddListener(() => this.OnBack.SafeInvoke());
        this.NextButton.onClick.AddListener(() => this.OnNext.SafeInvoke());
    }

}
