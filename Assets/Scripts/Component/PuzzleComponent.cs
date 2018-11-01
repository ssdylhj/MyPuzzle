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
    [SerializeField] private Button ResetButton;
    [SerializeField] private Text WinText;

    private GameObject[] rowNums;
    private GameObject[] colNums;

    private Puzzle Puzzle;

    void Awake()
    {
        string config = "4,4|r,3,2,3,2,4,3,3,0|b,0,0,2,2,0,0,2,2|2,2,r,b,r,b";
        //string config = Config.GetQuiz("Normal", 7);
        this.Puzzle = new Puzzle(config);
        this.InitPuzzle();

        this.ResetButton.onClick.AddListener(() => { this.Puzzle.Reset(); });
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
                newCube.transform.localPosition = new Vector3(c * this.CellWidth, -r * this.CellHeigh, 0);
                newCube.SetActive(true);

                var CubeComponent = newCube.GetComponent<CubeComponent>();
                CubeComponent.Setup(this.Puzzle.Cubes[r, c], r, c);
                CubeComponent.OnDraw = this.DrawLine;
                CubeComponent.OnDown = this.HandleOnDown;
                CubeComponent.OnUp = this.HandleOnUp;
            }
        }

        for (int r = 0; r < this.Puzzle.Config.Row; r++)
        {
            //GameObject newRowNum = Instantiate<GameObject>(rowNum);
            GameObject newRowNum = new GameObject();
            newRowNum.name = string.Format("rowNum({0})", r);
            newRowNum.transform.SetParent(datum.transform);
            newRowNum.transform.localPosition = new Vector3(-this.CellWidth, -r * this.CellHeigh, 0);
            newRowNum.SetActive(true);

            rowNums[r] = newRowNum;
        }

        for (int c = 0; c < this.Puzzle.Config.Col; c++)
        {
            //GameObject newColNum = Instantiate<GameObject>(colNum);
            var newColNum = new GameObject();
            newColNum.name = string.Format("colNum({0})", c);
            newColNum.transform.SetParent(datum.transform);
            newColNum.transform.localPosition = new Vector3(c * this.CellWidth, 0, 0);
            newColNum.SetActive(true);

            colNums[c] = newColNum;
        }

        var rowPos = Vector2.zero;
        var colPos = Vector2.zero;
        colPos.x = -16;
        colPos.y = 16;
        foreach (var kvp in this.Puzzle.Config.TagNums)
        {
            MyColor color = kvp.Key;
            List<int> nums = kvp.Value;

            rowPos.x -= 16;
            for (int i = 0; i < rowNums.Length; i++)
            {
                if (nums[i] == -1)
                    continue;

                var num = Instantiate(rowNum);
                num.transform.SetParent(rowNums[i].transform, false);
                num.transform.localPosition = rowPos;
                Debug.Log(string.Format("i = {0} Pos: {1}/{2}", i, rowPos, num.transform.position));

                var text = num.GetComponent<Text>();
                text.text = nums[i].ToString();
                text.color = color.ToColor();

                num.SetActive(true);
            }

            colPos.y += 16;
            for (int i = 0; i < colNums.Length; i++)
            {
                if (nums[rowNums.Length + i] == -1)
                    continue;

                var num = Instantiate(rowNum);
                num.transform.SetParent(colNums[i].transform);
                num.transform.localPosition = colPos;

                var text = num.GetComponent<Text>();
                text.color = color.ToColor();
                text.text = nums[rowNums.Length + i].ToString();

                num.SetActive(true);
            }
        }
    }

    private bool DrawFlag = false;
    private void DrawLine(int r, int c, Direction direction)
    {
        if (!this.DrawFlag)
            return;
        Debug.Log(string.Format("DrawLine: r:{0} c: {1} Direction: {2}",
            r, c,
            direction));
        this.Puzzle.DrawLine(r, c, direction, this.PaletteComponent.CurrentColor);
    }

    private void HandleOnDown()
    {
        Debug.Log("Mouse Down!");
        this.DrawFlag = true;
    }

    private void HandleOnUp()
    {
        Debug.Log("Mouse Up!");
        this.DrawFlag = false;

        if (this.Puzzle.CheckResult())
        {
            this.WinText.gameObject.SetActive(true);
        }
    }
}
