// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections.Generic;

public class Painter : MonoBehaviour
{


     InteractionManager manager = null;
    public Texture2D baseTex;
    private Vector2 dragStart;
    private Vector2 dragEnd;
    public enum Tool
    {
        None,
        Line,
        Brush,
        Eraser,
        Vector
    }
    public List<LineInfo> Lines = new List<LineInfo>();
    private int tool2 = 3;
    public Samples AntiAlias = Samples.Samples4;
    public Tool tool = Tool.Brush;
    public Texture[] toolimgs;
    public Texture2D colorCircle;
    public float lineWidth = 1;
    public float strokeWidth = 1;
    public Color col = Color.white;
    public Color col2 = Color.white;
    public GUISkin gskin;
    public LineTool lineTool = new LineTool();
    public BrushTool brush = new BrushTool();
    public EraserTool eraser = new EraserTool();
    public Stroke stroke = new Stroke();
    public int zoom = 1;
    public BezierPoint[] BezierPoints;
   
    private Vector2 preDrag;
    Color32[] colors = null;
    void Start()
    {
        var mat = gameObject.GetComponent<Renderer>().materials[0];
        baseTex = new Texture2D(960, 720, TextureFormat.RGBA32, false);
        colors = new Color32[baseTex.width * baseTex.height];       
        mat.mainTexture = baseTex;
        Clear();
        Lines.Clear();
        manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[0];
        
    }
    void OnDestroy()
    {
        
        Debug.Log("Destroy paint");
    }
    public void Clear()
    {
        var c = new Color32(180, 180, 180, 180);
        for (var i = 0; i < colors.Length; i++)
        {
            colors[i] = c;
        }

        baseTex.SetPixels32(0, 0, baseTex.width, baseTex.height, colors);
        baseTex.Apply();
        Lines.Add(new LineInfo() { Clear = true });
    }
    bool startDrag = false;
    System.Guid currentID = System.Guid.NewGuid();
    public LineInfo currentInfo = new LineInfo();
    void Update()
    {

        if ((Input.GetMouseButton(0)
            && manager.PrimaryHandEvent == InteractionManager.HandEventType.None) ||
            manager.PrimaryHandEvent == InteractionManager.HandEventType.Grip)
        {
            RaycastHit hit;
            Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
            Ray cursorRay = Camera.main.ScreenPointToRay(cursorPos);
            if (Physics.Raycast(cursorRay, out hit) && hit.transform.gameObject == gameObject)
            {
                Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
                Rect imgRect = new Rect(0, 0, baseTex.width, baseTex.height);
                Vector2 mouse = pixelUV;
                mouse.x *= baseTex.width;
                mouse.y *= baseTex.height;
                mouse.y = baseTex.height - mouse.y;
                mouse = new Vector2((int)mouse.x, (int)mouse.y);
                //mouse.y = Screen.height - mouse.y;


                if (imgRect.Contains(mouse))
                {
                    if (!startDrag)
                    {
                        startDrag = true;
                    }
                    dragStart = mouse - new Vector2(imgRect.x, imgRect.y);
                    dragStart.y = imgRect.height - dragStart.y;
                    dragStart.x = Mathf.Round(dragStart.x / zoom);
                    dragStart.y = Mathf.Round(dragStart.y / zoom);
                    //LineStart (mouse - Vector2 (imgRect.x,imgRect.y));

                    dragEnd = mouse - new Vector2(imgRect.x, imgRect.y);
                    dragEnd.x = Mathf.Clamp(dragEnd.x, 0, imgRect.width);
                    dragEnd.y = imgRect.height - Mathf.Clamp(dragEnd.y, 0, imgRect.height);
                    dragEnd.x = Mathf.Round(dragEnd.x / zoom);
                    dragEnd.y = Mathf.Round(dragEnd.y / zoom);
                }
                else
                {
                    dragStart = Vector3.zero;
                }


                if (dragStart == Vector2.zero)
                {
                    return;
                }
                dragEnd = mouse - new Vector2(imgRect.x, imgRect.y);
                dragEnd.x = Mathf.Clamp(dragEnd.x, 0, imgRect.width);
                dragEnd.y = imgRect.height - Mathf.Clamp(dragEnd.y, 0, imgRect.height);
                dragEnd.x = Mathf.Round(dragEnd.x / zoom);
                dragEnd.y = Mathf.Round(dragEnd.y / zoom);

                if (tool == Tool.Brush)
                {

                    Brush(dragEnd, preDrag);
                }
                if (tool == Tool.Eraser)
                {
                    Eraser(dragEnd, preDrag);
                }
                preDrag = dragEnd;
            }
        }
        else
        {
            startDrag = false;
            dragStart = Vector3.zero;
            preDrag = Vector3.zero;
        }
    }

    
    
    void Brush(Vector2 p1, Vector2 p2)
    {
       
        if (p2 == Vector2.zero)
        {
            p2 = p1;
        }
        if (currentInfo.StartPoint == p1 && currentInfo.EndPoint == p2 && currentInfo.Color == ColorPicker.selectedColor)
            return;
        else
            if (p1 != p2)
            {
                Drawing.NumSamples = AntiAlias;
                Lines.Add(new LineInfo() { Color = ColorPicker.selectedColor, ID = currentID, StartPoint = p1, EndPoint = p2, BrushWidth = brush.width });
                Drawing.PaintLine(p1, p2, brush.width, ColorPicker.selectedColor, brush.hardness, baseTex);
                currentInfo.Color = ColorPicker.selectedColor;
                currentInfo.StartPoint = p1;
                currentInfo.EndPoint = p2;
                baseTex.Apply();
            }
    }

    void Eraser(Vector2 p1, Vector2 p2)
    {
        Drawing.NumSamples = AntiAlias;
        if (p2 == Vector2.zero)
        {
            p2 = p1;
        }
        Drawing.PaintLine(p1, p2, eraser.width, Color.white, eraser.hardness, baseTex);
        baseTex.Apply();
    }

    void test()
    {
        float startTime = Time.realtimeSinceStartup;
        var w = 100;
        var h = 100;
        var p1 = new BezierPoint(new Vector2(10, 0), new Vector2(5, 20), new Vector2(20, 0));
        var p2 = new BezierPoint(new Vector2(50, 10), new Vector2(40, 20), new Vector2(60, -10));
        var c = new BezierCurve(p1.main, p1.control2, p2.control1, p2.main);
        p1.curve2 = c;
        p2.curve1 = c;
        Vector2 elapsedTime = new Vector2((Time.realtimeSinceStartup - startTime) * 10, 0);
        float startTime2 = Time.realtimeSinceStartup;
        for (var i = 0; i < w * h; i++)
        {
            Mathfx.IsNearBezier(new Vector2(Random.value * 80, Random.value * 30), p1, p2, 10);
        }

        Vector2 elapsedTime2 = new Vector2((Time.realtimeSinceStartup - startTime2) * 10, 0);
        Debug.Log("Drawing took " + elapsedTime.ToString() + "  " + elapsedTime2.ToString());

    }

    public class LineTool
    {
        public int width = 1;
    }
    public class EraserTool
    {
        public int width = 1;
        public int hardness = 1;
    }
    public class BrushTool
    {
        public int width = 20;
        public int hardness = 50;
        public int spacing = 10;
    }
    public class Stroke
    {
        public bool enabled = false;
        public int width = 1;
    }
}