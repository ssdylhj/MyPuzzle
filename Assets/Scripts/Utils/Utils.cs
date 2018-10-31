﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyPuzzle
{
    public enum MyColor
    {
        None = 0,
        Red = 1,
        Blue = 2,
        Green = 3,
        Yellow = 4,
    }

    public enum Direction
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,
    }

    public class Pos
    {
        public int R;
        public int C;

        public Pos(int r, int c)
        {
            this.R = r;
            this.C = c;
        }

        public void Copy(Pos pos)
        {
            this.R = pos.R;
            this.C = pos.C;
        }
    }

    public class Utils
    {
        public static string ColorToString(MyColor color)
        {
            switch (color)
            {
                case MyColor.Red: return "r";
                case MyColor.Blue: return "b";
                case MyColor.Green: return "g";
                case MyColor.Yellow: return "y";
                default: return "n";
            }
        }

        public static MyColor StringToMyColor(string str)
        {
            switch (str)
            {
                case "r": return MyColor.Red;
                case "b": return MyColor.Blue;
                case "g": return MyColor.Green;
                case "y": return MyColor.Yellow;
                default: return MyColor.None;
            }
        }

        public static Color MyColorToColor(MyColor color)
        {
            switch (color)
            {
                case MyColor.Red: return Color.red;
                case MyColor.Blue: return Color.blue;
                case MyColor.Green: return Color.green;
                case MyColor.Yellow: return Color.yellow;
                default: return Color.white;
            }
        }
    }

}
