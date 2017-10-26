using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public Texture2D map;

    public ColorToPrefab[] colorMappings;

    private ClickableTile[,] tileMap;

    public GameObject[] selectedCharacters;

	// Use this for initialization
	void Start () {
        GenerateLevel();
	}

    void GenerateLevel()
    {
        tileMap = new ClickableTile[map.width, map.height];
        for (int x = 0; x < map.width; x++)
        {
            for(int y = 0; y < map.height; y++)
            {
                GenerateTile(x, y);
            }
        }
    }

    void GenerateTile(int x, int y)
    {
        Color pixelColor = map.GetPixel(x, y);

        if (pixelColor.a == 0)
        {
            //if pixel is transparent ignore it
            return;
        }

        foreach(ColorToPrefab colorMapping in colorMappings)
        {
            if (colorMapping.color.Equals(pixelColor))
            {
                Vector3 position = new Vector3(x, y, 0.0f);
                GameObject objectA = Instantiate(colorMapping.prefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                objectA.transform.parent = this.transform;
                objectA.name = "Tile at " + x + ", " + y;
                tileMap[x,y] = objectA.GetComponent<ClickableTile>();
                tileMap[x,y].setCoords(x, y);
            }
            
        }

    }


    public void displayPath(int x, int y)
    {
        List < Vector2 > path = findPathForPerson((int)selectedCharacters[0].transform.position.x, (int)selectedCharacters[0].transform.position.y, x, y);
        selectedCharacters[0].GetComponent<Character>().SetPath(path);
        foreach(Vector2 v in path)
        {
            print(v.ToString());
        }

    }


    public List<Vector2> findPathForPerson(int xOrigin, int yOrigin, int xDestination, int yDestination)
    {

        List<Vector2> open = new List<Vector2>();
        List<Vector2> closed = new List<Vector2>();
        List<Vector2> path = new List<Vector2>();
        if (tileMap[xDestination,yDestination].getFloor()<=0)
        {
            print("target presently not accessable");
            return path;
        }
        Vector2 target = new Vector2(xDestination, yDestination);
        Vector2 origin = new Vector2(xOrigin, yOrigin);
        float[,] fCost = new float[map.width, map.height];
        int[,] gCost = new int[map.width, map.height];
        float[,] hCost = new float[map.width, map.height];
        Vector2[,] parents = new Vector2[map.width, map.height];
        bool found = false;
        bool timedOut = false;
        open.Add(origin);
        fCost[xOrigin, yOrigin] = 1;
        hCost[xOrigin, yOrigin] = 0;
        Vector2 lowestFCostNode = origin;
        int safety = 1;
        while (found == false)
        {
            Vector2 current;
            float lowestFCost = 9999;
            //print ("work for" + fCost [(int)open[0].x, (int)open[0].y]);
            foreach (Vector2 node in open)
            {
                if (fCost[(int)node.x, (int)node.y] < lowestFCost)
                {
                    //print ("set proper node");
                    lowestFCost = fCost[(int)node.x, (int)node.y];
                    lowestFCostNode = node;
                }
            }
            current = lowestFCostNode;
            open.Remove(lowestFCostNode);
            closed.Add(lowestFCostNode);



            if (current == target)
            {
                print("found path");
                found = true;
                break;
            }

            for (int xOff = -1; xOff <= 1; xOff++)
            {
                for (int yOff = -1; yOff <= 1; yOff++)
                {
                    if (closed.Contains(new Vector2(current.x + xOff, current.y + yOff)) == true|| tileMap[(int)current.x + xOff, (int)current.y + yOff].getFloor() <= 0||(xOff==-1&&yOff==-1)|| (xOff == -1 && yOff == 1)|| (xOff == 1 && yOff == -1)|| (xOff == 1 && yOff == 1))
                    {
                        continue;
                    }
                    else
                    {
                        gCost[(int)current.x + xOff, (int)current.y + yOff] = System.Math.Abs((int)target.x - ((int)current.x + xOff)) + System.Math.Abs((int)target.y - ((int)current.y + yOff));
                        hCost[(int)current.x + xOff, (int)current.y + yOff] = System.Math.Abs((int)current.x - ((int)current.x + xOff)) + System.Math.Abs((int)current.y - ((int)current.y + yOff));
                        fCost[(int)current.x + xOff, (int)current.y + yOff] = gCost[(int)current.x + xOff, (int)current.y + yOff] + hCost[(int)current.x + xOff, (int)current.y + yOff];
                        if (open.Contains(new Vector3(current.x + xOff, current.y + yOff)) != true)
                        {
                            open.Add(new Vector3(current.x + xOff, current.y + yOff));
                            print("We are in open");
                            //parentX [current.x + xOff] [current.y + yOff] [current.z + zOff] = current.x;
                            //parentY [current.x + xOff] [current.y + yOff] [current.z + zOff] = current.y;
                            //parentX [current.x + xOff] [current.y + yOff] [current.z + zOff] = current.z;
                            parents[(int)current.x + xOff, (int)current.y + yOff] = new Vector2(current.x, current.y);
                        }
                    }
                }
            }
            safety++;
            if (safety > 190)
            {
                print("Timed out");
                found = true;
                timedOut = true;
            }
        }
        found = false;
        if (timedOut == false)
        {
            Vector2 node = target;
            while (found == false)
            {
                path.Add(node);
                print("We should have path");
                node = parents[(int)node.x, (int)node.y];
                if (node == origin)
                {
                    found = true;
                }
            }
        }
        path.Reverse();
        return path;
    }
}
