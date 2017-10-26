using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    private List<Vector2> path;
    private int currentLocation=0;
    public float moveTime = .5f;
    private float timer = 0;
    private void Update()
    {
        if(path!=null&&currentLocation<path.Count)
        {
            timer = timer + Time.deltaTime;
            float maxDistance = 1.0f / moveTime * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(path[currentLocation].x, path[currentLocation].y, transform.position.z),maxDistance);

            if (timer > moveTime)
            {
                timer -= moveTime;
                currentLocation++;
            }
        }
    }

    public void SetPath(List<Vector2> list)
    {
        path = list;
        timer = 0;
        currentLocation = 0;
    }
}
