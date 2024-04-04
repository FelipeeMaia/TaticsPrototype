using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int movement;
    public Tile tile;
    public GridBehaviour grid;

    [SerializeField] Animator _anim;

    //Recieve a new path
    public void MoveUnit(List<Tile> path)
    {
        StartCoroutine(MoveThroughPath(path));
    }

    //Move unit through each tile of the path
    public IEnumerator MoveThroughPath(List<Tile> path)
    {
        _anim.SetTrigger("Run");

        while(path.Count > 1)
        {
            Vector3 startPos = path[0].transform.position;
            Vector3 endPos = path[1].transform.position;
            float lerpTime = 0;

            StartCoroutine(RotateTowards(startPos, endPos));

            while (lerpTime < 1)
            {
                lerpTime += 0.05f;
                transform.position = Vector3.Lerp(startPos, endPos, lerpTime);
                yield return new WaitForFixedUpdate();
            }

            path.RemoveAt(0);
        }

        tile = path[0];
        transform.position = tile.transform.position;
        _anim.SetTrigger("Idle");
    }

    private IEnumerator RotateTowards(Vector3 startPos, Vector3 endPos)
    {
        Vector3 dist = endPos - startPos;
        Quaternion startRotation = transform.rotation;
        Quaternion desiredRotation = Quaternion.LookRotation(dist);

        if (startRotation == desiredRotation) yield break;

        float lerpTime = 0;

        while (lerpTime < 1)
        {
            lerpTime += 0.1f;
            transform.rotation = Quaternion.Lerp(startRotation, desiredRotation, lerpTime);
            yield return new WaitForFixedUpdate();
        }
    }

    public void Start()
    {
        tile = GameObject.Find("Tile: 0-0").GetComponent<Tile>();
    }
}
