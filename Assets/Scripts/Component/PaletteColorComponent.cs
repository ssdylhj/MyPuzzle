using System;
using UnityEngine;
using UnityEngine.UI;
using MyPuzzle;

public class PaletteColorComponent
    : MonoBehaviour
{
    [SerializeField]
    private MyColor Color;

    private Image Image;
    private Button Button;
    private void Awake()
    {
        this.Image = this.GetComponent<Image>();
        this.Image.color = this.Color.ToColor();

        this.Button = this.GetComponent<Button>();
        this.Button.onClick.AddListener(this.OnClick);
    }

    public Action<MyColor> OnColorChange;
    private void OnClick()
    {
        this.OnColorChange.SafeInvoke(this.Color);
    }
}
