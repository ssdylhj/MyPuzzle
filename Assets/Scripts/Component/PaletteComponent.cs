using UnityEngine;
using MyPuzzle;

public class PaletteComponent
    : MonoBehaviour
{
    [SerializeField] private MyColor DefaultColor;
    [HideInInspector] public MyColor CurrentColor { get; private set; }

    private void Start()
    {
        var colors = this.GetComponentsInChildren<PaletteColorComponent>();
        foreach(var c in colors)
        {
            c.OnColorChange = this.OnColorChange;
        }

        this.CurrentColor = this.DefaultColor;
    }

    private void OnColorChange(MyColor c)
    {
        this.CurrentColor = c;
    }
}
