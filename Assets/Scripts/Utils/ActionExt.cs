﻿using System;
using UnityEngine;

namespace MyPuzzle
{
    public static class ActionExt
    {
        public static void SafeInvoke(this Action action)
        {
            if (action == null)
                return;

            try
            {
                action();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        public static void SafeInvoke<T>(this Action<T> action, T t)
        {
            if (action == null)
                return;

            try
            {
                action(t);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        public static void SafeInvoke<T1, T2>(this Action<T1, T2> action, T1 t1, T2 t2)
        {
            if (action == null)
                return;

            try
            {
                action(t1, t2);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
        {
            if (action == null)
                return;

            try
            {
                action(t1, t2, t3);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        public static void SafeInvoke<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            if (action == null)
                return;

            try
            {
                action(t1, t2, t3, t4);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
    }
}
