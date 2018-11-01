using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyPuzzle;

[RequireComponent(typeof(GameObjectRef))]
public class PuzzleComponent
    : MonoBehaviour
{
    [SerializeField] private int CellWidth;
    [SerializeField] private int CellHeigh;
    [SerializeField] private PaletteComponent PaletteComponent;

    private GameObject[] rowNums;
    private GameObject[] colNums;

    private Puzzle Puzzle;


    void Awake()
    {
        string config = "4,4|r,3,2,3,2,4,3,3,0|b,0,0,2,2,0,0,2,2|2,2,r,b,r,b";
        this.Puzzle = new Puzzle(config);
        this.InitPuzzle();
    }

    private void InitPuzzle()
    {
        GameObjectRef refs = GetComponent<GameObjectRef>();
        GameObject datum = refs["Datum"];
        GameObject cube = refs["Cube"];
        GameObject rowNum = refs["RowNum"];
        GameObject colNum = refs["ColNum"];

        rowNums = new GameObject[this.Puzzle.Config.Row];
        colNums = new GameObject[this.Puzzle.Config.Col];

        for (int r = 0; r < this.Puzzle.Config.Row; r++)
        {
            for (int c = 0; c < this.Puzzle.Config.Col; c++)
            {
                GameObject newCube = Instantiate<GameObject>(cube);
                newCube.name = string.Format("cube({0},{1})", r, c);
                newCube.transform.SetParent(datum.transform);
                newCube.transform.localPosition = new Vector3(c * this.CellWidth, r * this.CellHeigh, 0);
                newCube.SetActive(true);

                var CubeComponent = newCube.GetComponent<CubeComponent>();
                CubeComponent.Setup(this.Puzzle.Cubes[r, c], r, c);
                CubeComponent.OnDraw = this.DrawLine;
            }
        }

        for (int r = 0; r < this.Puzzle.Config.Row; r++)
        {
            GameObject newRowNum = Instantiate<GameObject>(rowNum);
            newRowNum.name = string.Format("rowNum({0})", r);
            newRowNum.transform.SetParent(datum.transform);
            newRowNum.transform.localPosition = new Vector3(-this.CellWidth, r * this.CellHeigh, 0);
            newRowNum.SetActive(true);

            rowNums[r] = newRowNum;
        }

        for (int c = 0; c < this.Puzzle.Config.Col; c++)
        {
            GameObject newColNum = Instantiate<GameObject>(colNum);
            newColNum.name = string.Format("colNum({0})", c);
            newColNum.transform.SetParent(datum.transform);
            newColNum.transform.localPosition = new Vector3(c * this.CellWidth, (this.Puzzle.Config.Row-1) * this.CellHeigh, 0);
            newColNum.SetActive(true);

            colNums[c] = newColNum;
        }

        foreach (var kvp in this.Puzzle.Config.TagNums)
        {
            MyColor color = kvp.Key;
            List<int> nums = kvp.Value;

            for (int i = 0; i < rowNums.Length; i++)
                rowNums[i].GetComponent<Text>().text += " " + nums[i].ToString();

            for (int i = 0; i < colNums.Length; i++)
                colNums[i].GetComponent<Text>().text += " " + nums[rowNums.Length + i].ToString();
        }
    }

    private void DrawLine(int r, int c, Direction direction)
    {
        Debug.Log(string.Format("DrawLine: r:{0} c: {1} Direction: {2}",
            r, c,
            direction));
        this.Puzzle.DrawLine(r, c, direction, this.PaletteComponent.CurrentColor);
    }
}
