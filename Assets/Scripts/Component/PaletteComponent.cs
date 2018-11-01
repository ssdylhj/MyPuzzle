using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyPuzzle;

public class PaletteComponent
    : MonoBehaviour
{
    [SerializeField] private GameObject PaletteColorTemplate;
    [HideInInspector] public MyColor CurrentColor { get; private set; }

    private MyColor DefaultColor;
    private List<GameObject> colors = new List<GameObject>();
    public void Init(List<MyColor> myColors)
    {
        this.ResetPalette();

        foreach(var c in myColors)
        {
            var go = Instantiate(this.PaletteColorTemplate);
            go.transform.SetParent(this.transform, false);
            go.GetComponent<Image>().color = c.ToColor();
            go.GetComponent<Button>().onClick.AddListener(() => this.CurrentColor = c);

            this.DefaultColor = this.DefaultColor == MyColor.None ? c : this.DefaultColor;
        }

        this.CurrentColor = this.DefaultColor;
    }

    private void ResetPalette()
    {
        this.DefaultColor = MyColor.None;
        foreach (var go in this.colors)
            GameObject.Destroy(go);

        this.colors.Clear();
    }
}
