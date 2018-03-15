using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class LineInfo
{
    public Vector2 StartPoint
    { get; set; }
    public Vector2 EndPoint
    { get; set; }
    public Guid ID
    { get; set; }
    public int BrushWidth
    { get; set; }

    public Color Color
    { get; set; }
    public bool Clear
    { get; set; }

}
