using UnityEngine;
using UnityEngine.UI;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;

using Object = UnityEngine.Object;

namespace MyPuzzle
{
    public class DoPuzzle : MonoBehaviour
    {
        Puzzle puzzle;
        GameObject[] rowNums;
        GameObject[] colNums;
        GameObject[,] cubes;

        void Awake()
        {
            //string config = "4,4|r,3,2,3,2,4,3,3,0|b,0,0,2,2,0,0,2,2|2,2,r,b,r,b";
            string config = Config.GetQuiz("Normal", 7);
            puzzle = new Puzzle(config);

            init();
        }

        private void init()
        {
            GameObjectRef refs = GetComponent<GameObjectRef>();
            GameObject datum = refs["Datum"];
            GameObject cube = refs["Cube"];
            GameObject rowNum = refs["RowNum"];
            GameObject colNum = refs["ColNum"];

            rowNums = new GameObject[puzzle.Config.Row];
            colNums = new GameObject[puzzle.Config.Col];
            cubes = new GameObject[puzzle.Config.Row, puzzle.Config.Col];

            for (int r = 0; r < puzzle.Config.Row; r++)
            {
                for (int c = 0; c < puzzle.Config.Col; c++)
                {
                    GameObject newCube = Instantiate<GameObject>(cube);
                    newCube.name = string.Format("cube({0},{1})", r, c);
                    newCube.transform.SetParent(datum.transform);
                    newCube.transform.localPosition = new Vector3(c * 30, r * -30, 0);
                    newCube.SetActive(true);

                    cubes[r, c] = newCube;

                    GameObjectRef cubeRefs = cubes[r, c].GetComponent<GameObjectRef>();
                    cubeRefs["Up"].GetComponent<Image>().color = Utils.MyColorToColor(this.puzzle.Cubes[r, c].UpColor);
                    cubeRefs["Down"].GetComponent<Image>().color = Utils.MyColorToColor(this.puzzle.Cubes[r, c].DownColor);
                    cubeRefs["Left"].GetComponent<Image>().color = Utils.MyColorToColor(this.puzzle.Cubes[r, c].LeftColor);
                    cubeRefs["Right"].GetComponent<Image>().color = Utils.MyColorToColor(this.puzzle.Cubes[r, c].RightColor);
                }
            }

            for (int r = 0; r < puzzle.Config.Row; r++)
            {
                GameObject newRowNum = Instantiate<GameObject>(rowNum);
                newRowNum.name = string.Format("rowNum({0})", r);
                newRowNum.transform.SetParent(datum.transform);
                newRowNum.transform.localPosition = new Vector3(-15 - 50, r * -30, 0);
                newRowNum.SetActive(true);

                rowNums[r] = newRowNum;
            }

            for (int c = 0; c < puzzle.Config.Col; c++)
            {
                GameObject newColNum = Instantiate<GameObject>(colNum);
                newColNum.name = string.Format("colNum({0})", c);
                newColNum.transform.SetParent(datum.transform);
                newColNum.transform.localPosition = new Vector3(c * 30, 15 + 50, 0);
                newColNum.SetActive(true);

                colNums[c] = newColNum;
            }

            foreach (var kvp in this.puzzle.Config.TagNums)
            {
                MyColor color = kvp.Key;
                List<int> nums = kvp.Value;

                for (int i = 0; i < rowNums.Length; i++)
                    rowNums[i].GetComponent<Text>().text += " " + nums[i].ToString();

                for (int i = 0; i < colNums.Length; i++)
                    colNums[i].GetComponent<Text>().text += " " + nums[rowNums.Length + i].ToString();
            }
        }
    }
}
