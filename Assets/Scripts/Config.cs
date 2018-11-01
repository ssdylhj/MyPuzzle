using UnityEditor;
using UnityEngine;

using System;
using System.Collections.Specialized;

namespace MyPuzzle
{
    public class Config
    {
        static ConfigIni.ConfigIni configIni = new ConfigIni.ConfigIni(Application.dataPath + "/Config/quiz.ini");

        public static StringCollection GetDiffcultTypes()
        {
            StringCollection difficulties = new StringCollection();
            configIni.ReadSections(difficulties);
            return difficulties;
        }

        public static int GetQuizNum(string difficulty)
        {
            StringCollection quiz = new StringCollection();
            configIni.ReadKeys(difficulty, quiz);
            return quiz.Count;
        }

        public static string GetQuiz(string difficulty, int index)
        {
            return configIni.ReadString(difficulty, index.ToString(), "");
        }
    }
}