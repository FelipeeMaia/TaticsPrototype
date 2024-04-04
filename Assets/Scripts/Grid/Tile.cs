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
    [SerializeField] Renderer _renderer;
    [SerializeField] Material _normalMat, _altMat;

    //Setup Tile
    public void SetTile(int x, int y, float spacing)
    {
        this.x = x;
        this.y = y;

        gameObject.name = $"Tile: {x}-{y}";
        transform.localPosition = new Vector3(x * spacing, 0, y * spacing);

        bool isOffset = (x + y) % 2 == 0;
        _renderer.material = isOffset ? _altMat : _normalMat;
    }

    //Turns highlight on
    public void Highlight()
    {
        _highlight.SetActive(true);
    }

    //Set tile to default state
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
