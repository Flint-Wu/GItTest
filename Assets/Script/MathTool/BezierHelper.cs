using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 贝塞尔曲线 动态.
/// </summary>
public class BezierHelper{

 // 一阶贝塞尔曲线，参数P0、P1、t对应上方原理内的一阶曲线参数.
    public static Vector3 Bezier(Vector3 p0, Vector3 p1, float t)
    {
        return (1 - t) * p0 + t * p1;
    }

    // 二阶贝塞尔曲线，参数对应上方原理内的二阶曲线参数.
    Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        Vector3 p0p1 = (1 - t) * p0 + t * p1;
        Vector3 p1p2 = (1 - t) * p1 + t * p2;
        Vector3 temp = (1 - t) * p0p1 + t * p1p2;
        return temp;
    }
    
    // 三阶贝塞尔曲线，参数对应上方原理内的三阶曲线参数.
    public static  Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 temp;
        Vector3 p0p1 = (1 - t) * p0 + t * p1;
        Vector3 p1p2 = (1 - t) * p1 + t * p2;
        Vector3 p2p3 = (1 - t) * p2 + t * p3;
        Vector3 p0p1p2 = (1 - t) * p0p1 + t * p1p2;
        Vector3 p1p2p3 = (1 - t) * p1p2 + t * p2p3;
        temp = (1 - t) * p0p1p2 + t * p1p2p3;
        return temp;
    }

    // 多阶贝塞尔曲线，使用递归实现.
    public static Vector3 Bezier(float t, List<Vector3> p)
    {
        if (p.Count < 2)
            return p[0];
        List<Vector3> newp = new List<Vector3>();
        for (int i = 0; i < p.Count - 1; i++)
        {
            Debug.DrawLine(p[i], p[i + 1]);
            
            Vector3 p0p1 = (1 - t) * p[i] + t * p[i + 1];
            newp.Add(p0p1);
        }
        return Bezier(t, newp);
    }
}