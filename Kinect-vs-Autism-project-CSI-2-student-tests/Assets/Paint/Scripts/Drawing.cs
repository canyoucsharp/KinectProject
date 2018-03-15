// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections.Generic;

public class Drawing : MonoBehaviour
{
    
    public static Samples NumSamples = Samples.Samples4;

    public static Texture2D DrawLine(Vector2 from, Vector2 to, float w, Color col, Texture2D tex)
    {
        return DrawLine(from, to, w, col, tex, false, Color.black, 0);
    }

    public static Texture2D DrawLine(Vector2 from, Vector2 to, float w, Color col, Texture2D tex, bool stroke, Color strokeCol, float strokeWidth)
    {
        w = Mathf.Round(w);//It is important to round the numbers otherwise it will mess up with the texture width
        strokeWidth = Mathf.Round(strokeWidth);

        var extent = w + strokeWidth;
        var stY = Mathf.Clamp(Mathf.Min(from.y, to.y) - extent, 0, tex.height);//This is the topmost Y value
        var stX = Mathf.Clamp(Mathf.Min(from.x, to.x) - extent, 0, tex.width);
        var endY = Mathf.Clamp(Mathf.Max(from.y, to.y) + extent, 0, tex.height);
        var endX = Mathf.Clamp(Mathf.Max(from.x, to.x) + extent, 0, tex.width);//This is the rightmost Y value

        strokeWidth = strokeWidth / 2;
        var strokeInner = (w - strokeWidth) * (w - strokeWidth);
        var strokeOuter = (w + strokeWidth) * (w + strokeWidth);
        var strokeOuter2 = (w + strokeWidth + 1) * (w + strokeWidth + 1);
        var sqrW = w * w;//It is much faster to calculate with squared values

        var lengthX = endX - stX;
        var lengthY = endY - stY;
        var start = new Vector2(stX, stY);
        Color[] pixels = tex.GetPixels((int)stX, (int)stY, (int)lengthX, (int)lengthY, 0);//Get all pixels

        for (var y = 0; y < lengthY; y++)
        {
            for (var x = 0; x < lengthX; x++)
            {//Loop through the pixels
                var p = new Vector2(x, y) + start;
                var center = p + new Vector2(0.5f, 0.5f);
                float dist = (center - Mathfx.NearestPointStrict(from, to, center)).sqrMagnitude;//The squared distance from the center of the pixels to the nearest point on the line
                if (dist <= strokeOuter2)
                {
                    var samples = Sample(p);
                    var c = Color.black;
                    var pc = pixels[y * (int)lengthX + x];
                    for (var i = 0; i < samples.Length; i++)
                    {//Loop through the samples
                        dist = (samples[i] - Mathfx.NearestPointStrict(from, to, samples[i])).sqrMagnitude;//The squared distance from the sample to the line
                        if (stroke)
                        {
                            if (dist <= strokeOuter && dist >= strokeInner)
                            {
                                c += strokeCol;
                            }
                            else if (dist < sqrW)
                            {
                                c += col;
                            }
                            else
                            {
                                c += pc;
                            }
                        }
                        else
                        {
                            if (dist < sqrW)
                            {//Is the distance smaller than the width of the line
                                c += col;
                            }
                            else
                            {
                                c += pc;//No it wasn't, set it to be the original colour
                            }
                        }
                    }
                    c /= samples.Length;//Get the avarage colour
                    pixels[y * (int)lengthX + x] = c;
                }
            }
        }
        tex.SetPixels((int)stX, (int)stY, (int)lengthX, (int)lengthY, pixels, 0);
        tex.Apply();
        return tex;
    }

    public static Texture2D Paint(Vector2 pos, float rad, Color col, float hardness, Texture2D tex)
    {
        var start = new Vector2(Mathf.Clamp(pos.x - rad, 0, tex.width), Mathf.Clamp(pos.y - rad, 0, tex.height));
        var width = rad * 2;
        var end = new Vector2(Mathf.Clamp(pos.x + rad, 0, tex.width), Mathf.Clamp(pos.y + rad, 0, tex.height));
        var widthX = Mathf.Round(end.x - start.x);
        var widthY = Mathf.Round(end.y - start.y);
        var sqrRad = rad * rad;
        var sqrRad2 = (rad + 1) * (rad + 1);
        Color[] pixels = tex.GetPixels((int)start.x, (int)start.y, (int)widthX, (int)widthY, 0);

        for (var y = 0; y < widthY; y++)
        {
            for (var x = 0; x < widthX; x++)
            {
                var p = new Vector2(x, y) + start;
                var center = p + new Vector2(0.5f, 0.5f);
                float dist = (center - pos).sqrMagnitude;
                if (dist > sqrRad2)
                {
                    continue;
                }
                var samples = Sample(p);
                var c = Color.black;
                for (var i = 0; i < samples.Length; i++)
                {
                    dist = Mathfx.GaussFalloff(Vector2.Distance(samples[i], pos), rad) * hardness;
                    if (dist > 0)
                    {
                        c += Color.Lerp(pixels[y * (int)widthX + x], col, dist);
                    }
                    else
                    {
                        c += pixels[y * (int)widthX + x];
                    }
                }
                c /= samples.Length;

                pixels[y * (int)widthX + x] = c;
            }
        }

        tex.SetPixels((int)start.x, (int)start.y, (int)widthX, (int)widthY, pixels, 0);
        return tex;
    }

    public static Texture2D PaintLine(Vector2 from, Vector2 to, float rad, Color col, float hardness, Texture2D tex)
    {
        var width = rad * 2;

        var extent = rad;
        var stY = Mathf.Clamp(Mathf.Min(from.y, to.y) - extent, 0, tex.height);
        var stX = Mathf.Clamp(Mathf.Min(from.x, to.x) - extent, 0, tex.width);
        var endY = Mathf.Clamp(Mathf.Max(from.y, to.y) + extent, 0, tex.height);
        var endX = Mathf.Clamp(Mathf.Max(from.x, to.x) + extent, 0, tex.width);


        var lengthX = endX - stX;
        var lengthY = endY - stY;



        var sqrRad = rad * rad;
        var sqrRad2 = (rad + 1) * (rad + 1);
        Color[] pixels = tex.GetPixels((int)stX, (int)stY, (int)lengthX, (int)lengthY, 0);
        var start = new Vector2(stX, stY);
        //Debug.Log (widthX + "   "+ widthY + "   "+ widthX*widthY);
        for (var y = 0; y < lengthY; y++)
        {
            for (var x = 0; x < lengthX; x++)
            {
                var c = Color.white;
                var p = new Vector2(x, y) + start;
                var center = p + new Vector2(0.5f, 0.5f);
                float dist = (center - Mathfx.NearestPointStrict(from, to, center)).sqrMagnitude;
                if (dist > sqrRad2)
                {
                    continue;
                }
                dist = Mathfx.GaussFalloff(Mathf.Sqrt(dist), rad) * hardness;
                //dist = (samples[i]-pos).sqrMagnitude;
                if (dist > 0)
                {
                    c = Color.Lerp(pixels[(int)(y * lengthX + x)], col, dist);
                }
                else
                {
                    c = pixels[(int)(y * lengthX + x)];
                }

                pixels[(int)(y * lengthX + x)] = c;
            }
        }
        tex.SetPixels((int)start.x, (int)start.y, (int)lengthX, (int)lengthY, pixels, 0);
        return tex;
    }

    public static void DrawBezier(BezierPoint[] points, float rad, Color col, Texture2D tex)
    {
        rad = Mathf.Round(rad);//It is important to round the numbers otherwise it will mess up with the texture width

        if (points.Length <= 1)
        {
            return;
        }

        Vector2 topleft = new Vector2(Mathf.Infinity, Mathf.Infinity);
        Vector2 bottomright = new Vector2(0, 0);

        for (var i = 0; i < points.Length - 1; i++)
        {
            BezierCurve curve = new BezierCurve(points[i].main, points[i].control2, points[i + 1].control1, points[i + 1].main );
            points[i].curve2 = curve;
            points[i + 1].curve1 = curve;

            topleft.x = Mathf.Min(topleft.x, curve.rect.x);

            topleft.y = Mathf.Min(topleft.y, curve.rect.y);

            bottomright.x = Mathf.Max(bottomright.x, curve.rect.x + curve.rect.width);

            bottomright.y = Mathf.Max(bottomright.y, curve.rect.y + curve.rect.height);
        }

        topleft -= new Vector2(rad, rad);
        bottomright += new Vector2(rad, rad);

        var start = new Vector2(Mathf.Clamp(topleft.x, 0, tex.width), Mathf.Clamp(topleft.y, 0, tex.height));
        var width = new Vector2(Mathf.Clamp(bottomright.x - topleft.x, 0, tex.width - start.x), Mathf.Clamp(bottomright.y - topleft.y, 0, tex.height - start.y));

        Color[] pixels = tex.GetPixels((int)start.x, (int)start.y, (int)width.x, (int)width.y, 0);

        for (var y = 0; y < width.y; y++)
        {
            for (var x = 0; x < width.x; x++)
            {
                var p = new Vector2(x + start.x, y + start.y);
                if (!Mathfx.IsNearBeziers(p, points, rad + 2))
                {
                    continue;
                }

                var samples = Sample(p);
                var c = Color.black;
                var pc = pixels[(int)(y * width.x + x)];//Previous pixel color
                for (var i = 0; i < samples.Length; i++)
                {
                    if (Mathfx.IsNearBeziers(samples[i], points, rad))
                    {
                        c += col;
                    }
                    else
                    {
                        c += pc;
                    }
                }

                c /= samples.Length;

                pixels[(int)(y * width.x + x)] = c;

            }
        }

        tex.SetPixels((int)start.x, (int)start.y, (int)width.x, (int)width.y, pixels, 0);
        tex.Apply();
    }

    public static Vector2[] Sample(Vector2 p)
    {
        switch (NumSamples)
        {
         
            case Samples.None:
                return new Vector2[] { p + new Vector2(0.5f, 0.5f) };
            case Samples.Samples2:
                return new Vector2[] { p + new Vector2(0.25f, 0.5f), p + new Vector2(0.75f, 0.5f) };
            case Samples.Samples4:
                //return [p+Vector2(0.25f,0.25f),p+Vector2(0.75f,0.25f), p+Vector2(0.25f,0.75f),p+Vector2(0.75f,0.75f)];
                return new Vector2[]
			{
			p+new Vector2(0.25f,0.5f),
			p+new Vector2(0.75f,0.5f),
			p+new Vector2(0.5f,0.25f),
			p+new Vector2(0.5f,0.75f)};
            case Samples.Samples8:
                return
            new Vector2[]
			{
			
			p+new Vector2(0.25f,0.5f),
			p+new Vector2(0.75f,0.5f),
			p+new Vector2(0.5f,0.25f),
			p+new Vector2(0.5f,0.75f),
			
			p+new Vector2(0.25f,0.25f),
			p+new Vector2(0.75f,0.25f),
			p+new Vector2(0.25f,0.75f),
			p+new Vector2(0.75f,0.75f)
			};
            case Samples.Samples16:
                return new Vector2[]
            {
			
			p+new Vector2(0,0),
			p+new Vector2(0.3f,0),
			p+new Vector2(0.7f,0),
			p+new Vector2(1,0),
			
			p+new Vector2(0,0.3f),
			p+new Vector2(0.3f,0.3f),
			p+new Vector2(0.7f,0.3f),
			p+new Vector2(1,0.3f),
			
			p+new Vector2(0,0.7f),
			p+new Vector2(0.3f,0.7f),
			p+new Vector2(0.7f,0.7f),
			p+new Vector2(1,0.7f),
			
			p+new Vector2(0,1),
			p+new Vector2(0.3f,1),
			p+new Vector2(0.7f,1),
			p+new Vector2(1,1)
			};
            case Samples.Samples32:
                return
                    new Vector2[]
                {
			
			p+new Vector2(0,0),
			p+new Vector2(1,0),
			p+new Vector2(0,1),
			p+new Vector2(1,1),
			
			p+new Vector2(0.2f,0.2f),
			p+new Vector2(0.4f,0.2f),
			p+new Vector2(0.6f,0.2f),
			p+new Vector2(0.8f,0.2f),
			
			p+new Vector2(0.2f,0.4f),
			p+new Vector2(0.4f,0.4f),
			p+new Vector2(0.6f,0.4f),
			p+new Vector2(0.8f,0.4f),
			
			p+new Vector2(0.2f,0.6f),
			p+new Vector2(0.4f,0.6f),
			p+new Vector2(0.6f,0.6f),
			p+new Vector2(0.8f,0.6f),
			
			p+new Vector2(0.2f,0.8f),
			p+new Vector2(0.4f,0.8f),
			p+new Vector2(0.6f,0.8f),
			p+new Vector2(0.8f,0.8f),
			
			
			
			p+new Vector2(0.5f,0),
			p+new Vector2(0.5f,1),
			p+new Vector2(0,0.5f),
			p+new Vector2(1,0.5f),
			
			p+new Vector2(0.5f,0.5f)
                };

            case Samples.RotatedDisc:
                return
                    new Vector2[]
                {
			
			p+new Vector2(0,0),
			p+new Vector2(1,0),
			p+new Vector2(0,1),
			p+new Vector2(1,1),
			
			p+new Vector2(0.5f,0.5f)+new Vector2(0.258f,0.965f),//Sin (75°) && Cos (75°)
			p+new Vector2(0.5f,0.5f)+new Vector2(-0.965f,-0.258f),
			p+new Vector2(0.5f,0.5f)+new Vector2(0.965f,0.258f),
			p+new Vector2(0.5f,0.5f)+new Vector2(0.258f,-0.965f)
                };

                break;
            default:
                return new Vector2[] { p + new Vector2(0.5f, 0.5f) };
        }
    }
}