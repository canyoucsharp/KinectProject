using UnityEngine;
using System.Collections;

public class objectMaker : MonoBehaviour
{

    // Use this for initialization

    public GameObject Markers;
    public GameObject Image;
    public GameObject WinImage;
    public SpriteRenderer sr;
    public Sprite[] textures;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        textures = Resources.LoadAll<Sprite>("images");
        Debug.Log(transform.childCount);
        int choice = Random.Range(0, textures.Length);
        float xSize;
        float ySize;
        Markers = GameObject.Find("Marker");
        WinImage = GameObject.Find("The Wall");
        float restriction = Markers.GetComponent<Renderer>().bounds.size.x;//how far away from border

        sr = GetComponent<SpriteRenderer>();
        gameObject.GetComponent<SpriteRenderer>().sprite = textures[choice] as Sprite;
        WinImage.GetComponent<SpriteRenderer>().sprite = textures[choice] as Sprite;

        sr = (SpriteRenderer)GetComponent("Renderer");
        if (sr == null)
            return;

        



        if (GameObject.Find("Pink Floyd"))
            Image = GameObject.Find("Pink Floyd");
        else
            Debug.Log("no wall");
     //   transform.localScale = new Vector3(2F, 2, 1);
        WinImage.transform.localScale = transform.localScale;
        xSize = gameObject.GetComponent<Renderer>().bounds.extents.x;
        ySize = gameObject.GetComponent<Renderer>().bounds.extents.y;

        for (int i = 0; i < 9; i++)
        {
            (Instantiate(Markers, Image.transform.position + new Vector3(Random.Range(-xSize + restriction, xSize - restriction), Random.Range(-ySize + restriction, ySize - restriction), Markers.transform.position.z), Quaternion.identity) as GameObject).transform.SetParent(gameObject.transform);

        }
        //Instantiate(Markers, Image.transform.position + new Vector3(12.5f / 2, 9 / 2, Markers.transform.position.z), Quaternion.identity);
        Destroy(Markers);
        /*   for (int i = 0; i < 5; i++)
           {
               for (int j = 0; j < 7; j++)
               {
                   Instantiate(Markers, new Vector3( j*2,+ i*2, Markers.transform.position.z), Quaternion.identity);
               }
           }*/


    }


    // Update is called once per frame
    void Update ()
    {

        if (transform.childCount == 0)
        {
            WinImage.GetComponent<Renderer>().enabled = true;
			Debug.Log(transform.childCount);
        }
    }
}
