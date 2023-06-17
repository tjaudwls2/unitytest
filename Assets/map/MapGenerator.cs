using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Color ColorFloor     = Color.white;
    public Color ColorWall      = Color.red;
    public Color ColorCurveWall = Color.green;
    public Color ColorEdgeWalle = Color.blue;

    public Color ColorResponse = new Color(64,128,128);

    public Transform Terrain;
    public Texture2D MapInfo;
    public float tileSize = 4f;
    private int mapWidth ;
    private int mapHeight;
    public GameObject Floor;
    public GameObject Wall;
    public GameObject CurveWall;
    public GameObject EdgeWalle;
    public GameObject Floor_Response;
   
    public void BuildGenerator()
    {
        GenerateMap();
    }
    private void GenerateMap()
    {
        mapWidth = MapInfo.width;
        mapHeight = MapInfo.height;
        Debug.Log(MapInfo.width);
        Debug.Log(MapInfo.height);

        Color[] pixels = MapInfo.GetPixels();

        for(int i=0; i< mapHeight; i++)
        {
            for(int j=0; j<mapWidth; j++)
            {
                Color pixelColor = pixels[i * mapHeight + j];
                if(pixelColor == Color.white)
                {
                    GameObject floor = GameObject.Instantiate(Floor,Terrain);
                    floor.transform.position = new Vector3(j*tileSize,0,i*tileSize);

                }
                if (pixelColor == Color.red)
                {
                    GameObject wall = GameObject.Instantiate(Wall, Terrain);
                    wall.transform.position = new Vector3(j * tileSize, 0, i * tileSize);
                    wall.transform.Rotate(new Vector3(0,GetWallRot(pixels,i,j),0),Space.Self);
                }
                if (pixelColor == Color.green)
                {
                    GameObject green = GameObject.Instantiate(CurveWall, Terrain);
                    green.transform.position = new Vector3(j * tileSize, 0, i * tileSize);
                    green.transform.Rotate(new Vector3(0, GetCurveWallRot(pixels, i, j), 0), Space.Self);
                }
                if (pixelColor == Color.blue)
                {
                    GameObject edgeWall = GameObject.Instantiate(EdgeWalle, Terrain);
                    edgeWall.transform.position = new Vector3(j * tileSize, 0, i * tileSize);
                    edgeWall.transform.Rotate(new Vector3(0, GetEdgeWall(pixels, i, j), 0), Space.Self);
                }
                if(pixelColor == ColorResponse)
                {
                    GameObject floor = GameObject.Instantiate(Floor_Response, Terrain);
                    floor.transform.position = new Vector3(j * tileSize, 0, i * tileSize);

                }



            }


        }


    }

    private float GetWallRot(Color[] pixels,int i, int j)
    {



        float rot = 0;

        if(i-1>=0&&(pixels[(i-1)*mapHeight + j]==Color.black || pixels[(i - 1) * mapHeight + j] == Color.cyan))
        {
            rot = 90f;
        }
        if (j - 1 >= 0 && (pixels[i * mapHeight + (j - 1)] == Color.black || pixels[i * mapHeight + (j - 1)] == Color.cyan))
        {
            rot = 180f;
        }
        if (i + 1 < mapHeight && (pixels[(i + 1) * mapHeight + j] == Color.black || pixels[(i - 1) * mapHeight + j] == Color.cyan))
        {
            rot = -90f;
        }
        return rot;

    }

    private float GetCurveWallRot(Color[] pixels, int i, int j)
    {
        float rot = 0f;

        if(((pixels[i*mapHeight +j -1]==Color.black) ||
            pixels[i*mapHeight + j-1]==Color.cyan)
            &&((pixels[(i-1) * mapHeight + j] ==Color.black)||
            (pixels[(i-1)*mapHeight+j] == Color.cyan)))
        {

            rot = 180f;
        }
        if (((pixels[i * mapHeight + j-1] == Color.black) ||
            pixels[i * mapHeight + i - 1] ==Color.cyan) 
            && ((pixels[(i + 1) * mapHeight + j] == Color.black) ||
         (pixels[(i + 1) * mapHeight + j] == Color.cyan)))
        {
            rot = -90f;
        }
        if (((pixels[i *  mapHeight + j +1] == Color.black) || 
            pixels[i * mapHeight + j + 1] ==Color.cyan)
            && ((pixels[(i - 1) *  mapHeight + j] == Color.black) ||
             (pixels[(i - 1) *  mapHeight + j] == Color.cyan)))
        {
            rot = 90f;
        }
        return rot;

    }

    private float GetEdgeWall(Color[] pixels, int i, int j)
    {

        float rot = 0f;
        if (i - 1 >= 0 && j + 1 < mapWidth &&
         (pixels[(i - 1) * mapHeight + (j + 1)] == Color.black ||
          pixels[(i - 1) * mapHeight + (j + 1)] == Color.cyan))
         {
        
             rot = 90f;
         }
        if (i - 1 >= 0 && j - 1 >= 0 &&
          (pixels[(i - 1) * mapHeight + (j - 1)] == Color.black ||
           pixels[(i - 1) * mapHeight + (j - 1)] == Color.cyan))
        {

            rot = 180f;
        }
        if (i + 1 < mapHeight && j - 1 >= 0 &&
        (pixels[(i + 1) * mapHeight + (j - 1)] == Color.black ||
         pixels[(i + 1) * mapHeight + (j - 1)] == Color.cyan))
        {

            rot = -90f;
        }
        return rot;



    }











}
