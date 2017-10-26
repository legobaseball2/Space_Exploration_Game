using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour {
    private int x;
    private int y;
    public int floor;
    //0 indicates wall

    public void setCoords(int xx, int yy)
    {
        x = xx;
        y = yy;
    }

    private void OnMouseUp()
    {
        if(floor>0)
        {
            print("Floor");
            LevelGenerator pathFinder = this.GetComponentInParent<LevelGenerator>();
            pathFinder.displayPath(x, y);
        }
        else
        {
            print("Wall");
        }
        
    }

    public int getFloor()
    {
        return floor;
    }
}
