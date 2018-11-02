using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyPuzzle;

public class PaletteComponent
    : MonoBehaviour
{
    [SerializeField] private GameObject PaletteColorTemplate;
    [HideInInspector] public MyColor CurrentColor { get; private set; }

    private float Height;
    private void Awake()
    {
        this.Height = this.PaletteColorTemplate.GetComponent<RectTransform>().rect.height;
    }

    private MyColor DefaultColor;
    private List<GameObject> colors = new List<GameObject>();
    public void Init(List<MyColor> myColors)
    {
        this.ResetPalette();

        var pos = Vector2.zero;
        foreach(var c in myColors)
        {
            pos.y -= this.Height;

            var go = Instantiate(this.PaletteColorTemplate);
            go.transform.SetParent(this.transform, false);
            go.transform.localPosition = pos;
            go.GetComponent<Image>().color = c.ToColor();
            go.GetComponent<Button>().onClick.AddListener(() => this.CurrentColor = c);
            this.colors.Add(go);

            this.DefaultColor = this.DefaultColor == MyColor.None ? c : this.DefaultColor;
        }

        this.CurrentColor = this.DefaultColor;
    }

    public void ResetPalette()
    {
        this.DefaultColor = MyColor.None;
        foreach (var go in this.colors)
            GameObject.Destroy(go);

        this.colors.Clear();
    }
}
