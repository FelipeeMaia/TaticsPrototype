using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Tile Stats")]
    public int x = 0;
    public int y = 0;
    public int movementCost = 1;

    [Header("Grid Calculations")]
    public List<Tile> neighbours;
    public int movementNeed;
    public float distanceFromStart;
    public Tile previusTile;

    [Header("References")]
    [SerializeField] GameObject _highlight;

    public void SetPosition(int x, int y, float spacing)
    {
        this.x = x;
        this.y = y;

        gameObject.name = $"Tile: {x}-{y}";
        transform.localPosition = new Vector3(x * spacing, 0, y * spacing);
    }

    public void Highlight()
    {
        _highlight.SetActive(true);
    }

    public void SetDistanceToTarget(Tile target)
    {
        var thisPostition = new Vector2(x, y);
        var targetPostition = new Vector2(target.x, target.y);

        distanceFromStart = Vector2.Distance(thisPostition, targetPostition);
    }

    public void CleanUp()
    {
        _highlight.SetActive(false);
        distanceFromStart = Mathf.Infinity;
        movementNeed = 0;
        previusTile = null;
    }

    private void Start()
    {
        CleanUp();
    }
}