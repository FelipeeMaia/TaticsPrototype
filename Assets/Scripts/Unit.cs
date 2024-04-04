using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int movement;
    public Tile tile;
    public GridBehaviour grid;

    //Recieve a new path
    public void MoveUnit(List<Tile> path)
    {
        StartCoroutine(MoveThroughPath(path));
    }

    //Move unit through each tile of the path
    public IEnumerator MoveThroughPath(List<Tile> path)
    {
        while(path.Count > 1)
        {
            Vector3 startPos = path[0].transform.position;
            Vector3 endPos = path[1].transform.position;
            float lerpTime = 0;

            while (lerpTime < 1)
            {
                lerpTime += 0.1f;
                transform.position = Vector3.Lerp(startPos, endPos, lerpTime);
                yield return new WaitForFixedUpdate();
            }

            path.RemoveAt(0);
        }

        tile = path[0];
        transform.position = tile.transform.position;
    }

    public void Start()
    {
        tile = GameObject.Find("Tile: 0-0").GetComponent<Tile>();
    }
}
