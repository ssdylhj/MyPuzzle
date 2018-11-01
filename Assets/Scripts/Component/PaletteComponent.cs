using UnityEngine;
using MyPuzzle;

public class PaletteComponent
    : MonoBehaviour
{
    [HideInInspector]
    public MyColor CurrentColor { get; private set; }

    private void Start()
    {
        var colors = this.GetComponentsInChildren<PaletteColorComponent>();
        foreach(var c in colors)
        {
            c.OnColorChange = this.OnColorChange;
        }
    }

    private void OnColorChange(MyColor c)
    {
        this.CurrentColor = c;
    }
}
