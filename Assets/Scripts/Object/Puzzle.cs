using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Linq;

namespace MyPuzzle
{
    public class Puzzle
    {
        public PuzzleConfig Config;
        public Cube[,] Cubes;

        public Puzzle(string config)
        {
            this.Config = new PuzzleConfig(config);
            this.Cubes = new Cube[this.Config.Row, this.Config.Col];

            for (int r = 0; r < this.Cubes.GetLength(0); r++)
                for (int c = 0; c < this.Cubes.GetLength(1); c++)
                    this.Cubes[r, c] = new Cube();

            foreach (var block in  this.Config.Blocks)
            {
                int r = int.Parse(block[0]);
                int c = int.Parse(block[1]);
                MyColor[] colors = new MyColor[] { Utils.StringToMyColor(block[2]), Utils.StringToMyColor(block[3]), Utils.StringToMyColor(block[4]), Utils.StringToMyColor(block[5]) };
                this.Cubes[r, c].SetColors(colors);
                this.Cubes[r, c].SetBlockState(true);
            }
        }

        public void DrawLine(int r, int c, Direction direct, MyColor color)
        {
            if (this.Cubes[r, c] == null)
                this.Cubes[r, c] = new Cube();

            // 已经有该颜色连接该方向。则删除
            if (this.Cubes[r, c].IsConnectTo(direct, color))
                this.Cubes[r, c].Clear(direct);
            else
                this.Cubes[r, c].SetColor(direct, color);

            this.Cubes[r, c].IsDirty = true;
        }

        public bool CheckResult()
        {
            if (! CheckConnection())
                return false;

            foreach (var color in this.Config.TagNums.Keys)
            {
                if (! CheckRowAndColByColor(color))
                    return false;

                if (! CheckCircle(color))
                    return false;
            }

            return true;
        }

        // 检查是否有断口
        // 边角的检查不完全，但最后一步的检查可以弥补，所以就不添加逻辑了
        public bool CheckConnection()
        {
            // 只要检查下方和右方即可
            for (int r = 0; r < this.Config.Row; r++)
            {
                for (int c = 0; c < this.Config.Col; c++)
                {
                    if (r < this.Config.Row - 1)
                    {
                        if (this.Cubes[r, c].DownColor != this.Cubes[r + 1, c].UpColor)
                            return false;
                    }

                    if (c < this.Config.Col - 1)
                    {
                        if (this.Cubes[r, c].RightColor != this.Cubes[r, c + 1].LeftColor)
                            return false;
                    }
                }
            }

            return true;
        }

        // 检查是否符合行列的数字
        public bool CheckRowAndColByColor(MyColor color)
        {
            bool[,] ret = new bool[this.Config.Row, this.Config.Col];

            // 先检查每一格的连接是否合法。并记录合法的连接
            for (int r = 0; r < this.Config.Row; r++)
            {
                for (int c = 0; c < this.Config.Col; c++)
                {
                    int cn = this.Cubes[r, c].ConnectionNum(color);
                    if (cn == 1 || cn == 3)
                        return false;

                    ret[r, c] = (cn != 0);
                }
            }

            // 检查行上的数字
            for (int r = 0; r < this.Config.Row; r++)
            {
                int TagNum = this.Config.TagNums[color][r];
                if (TagNum == -1)
                    continue;

                int num = 0;

                for (int c = 0; c < this.Config.Col; c++)
                {
                    if (ret[r, c])
                        num++;
                }

                if (num != TagNum)
                    return false;
            }

            // 检查列上的数字
            for (int c = 0; c < this.Config.Col; c++) 
            {
                int TagNum = this.Config.TagNums[color][this.Config.Row + c];
                if (TagNum == -1)
                    continue;

                int num = 0;

                for (int r = 0; r < this.Config.Row; r++)
                {
                    if (ret[r, c])
                        num++;
                }

                if (num != TagNum)
                    return false;
            }

            return true;
        }

        // 检查是否只有一个环
        private bool CheckCircle(MyColor color)
        {
            // 现在用的办法：找到一个位置开始一路向前走，直到走回来。看一共经过了几个位置，再看看数量是否和不为0的位置数量相同

            Pos startPos = new Pos(0, 0);
            Pos nowPos = new Pos(0, 0);
            Pos lastPos = new Pos(0, 0);

            Direction direct = Direction.None;  // 记录前进方向

            int wayCount = 0;   // 记录实际走的路径长度
            int lineCount = 0;  // 记录所有标记的路径长度，十字路口算两次

            // 随便找一个初始位置（为了后面的逻辑简单，初始位置不要是十字路口），顺便把不是零的数量记录一下
            for (int r = 0; r < this.Config.Row; r++)
            {
                for (int c = 0; c < this.Config.Col; c++)
                {
                    int cn = this.Cubes[r, c].ConnectionNum(color);
                    if (cn == 2)
                        startPos = new Pos(r, c);

                    // 十字路口会经过两次，其他通路算一次
                    lineCount += cn / 2;
                }
            }

            nowPos.Copy(startPos);

            do
            {
                Cube me = this.Cubes[nowPos.R, nowPos.C];
                lastPos.Copy(nowPos);

                wayCount++;

                // 十字路口全部直行(因为前面已经确定了起点不会选中十字路口，所以不会出现不知道往哪走的情况)
                if (me.IsCross(color))
                {
                    switch (direct)
                    {
                        case Direction.Up: nowPos.R--; break;
                        case Direction.Down: nowPos.R++; break;
                        case Direction.Left: nowPos.C--; break;
                        case Direction.Right: nowPos.C++; break;
                        default:break;
                    }

                    continue;
                }

                // 向上走一步。上一步向下的话这一步就不能向上
                if ((direct != Direction.Down && me.IsConnectTo(Direction.Up, color) && nowPos.R > 0))
                {
                    nowPos.R--;
                    direct = Direction.Up;
                    continue;
                }

                // 向下走一步。上一步向上的话这一步就不能向下
                if (direct != Direction.Up && me.IsConnectTo(Direction.Down, color) && nowPos.R < this.Config.Row - 1)
                {
                    nowPos.R++;
                    direct = Direction.Down;
                    continue;
                }

                // 向左走一步。上一步向右的话这一步就不能向左
                if (direct != Direction.Right && me.IsConnectTo(Direction.Left, color) && nowPos.C > 0)
                {
                    nowPos.C--;
                    direct = Direction.Left;
                    continue;
                }

                // 向右走一步。上一步向左的话这一步就不能向右
                if (direct != Direction.Left && me.IsConnectTo(Direction.Right, color) && nowPos.C < this.Config.Col - 1)
                {
                    nowPos.C++;
                    direct = Direction.Right;
                    continue;
                }

                // 走到这来说明路断了
                return false;
            } while (nowPos != startPos);

            if (wayCount != lineCount)
                return false;

            return true;
        }
    }
}