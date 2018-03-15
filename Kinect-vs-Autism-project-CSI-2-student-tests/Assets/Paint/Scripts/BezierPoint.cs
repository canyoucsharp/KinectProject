using System;
using UnityEngine;
public class BezierPoint
{
    public Vector2 main;
    public Vector2 control1;//Think of as left
    public Vector2 control2;//Right
    //Rect rect;
    public BezierCurve curve1;//Left
    public BezierCurve curve2;//Right

   public  BezierPoint(Vector2 m, Vector2 l, Vector2 r)
    {
        main = m;
        control1 = l;
        control2 = r;

        /*Vector2 topleft = Vector2(Mathf.Infinity,Mathf.Infinity);
        Vector2 bottomright = Vector2(Mathf.NegativeInfinity,Mathf.NegativeInfinity);
		
        topleft.x = Mathf.Min (topleft.x,main.x);
        topleft.x = Mathf.Min (topleft.x,control1.x);
        topleft.x = Mathf.Min (topleft.x,control2.x);
		
        topleft.y = Mathf.Min (topleft.y,main.y);
        topleft.y = Mathf.Min (topleft.y,control1.y);
        topleft.y = Mathf.Min (topleft.y,control2.y);
		
        bottomright.x = Mathf.Max (bottomright.x,main.x);
        bottomright.x = Mathf.Max (bottomright.x,control1.x);
        bottomright.x = Mathf.Max (bottomright.x,control2.x);
		
        bottomright.y = Mathf.Max (bottomright.y,main.y);
        bottomright.y = Mathf.Max (bottomright.y,control1.y);
        bottomright.y = Mathf.Max (bottomright.y,control2.y);
		
        rect = new Rect(topleft.x,topleft.y,bottomright.x-topleft.x,bottomright.y-topleft.y);*/
    }
}