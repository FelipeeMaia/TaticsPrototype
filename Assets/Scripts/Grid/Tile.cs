using Tactics.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tactics.Core
{
    public class Tile : MonoBehaviour
    {
        [Header("Tile Stats")]
        public int x = 0;
        public int y = 0;
        public int movementCost = 1;
        public Unit unitOnTile = null;

        //[Header("Grid Calculations")]
        [HideInInspector] public List<Tile> neighbours;
        [HideInInspector] public int movementNeed;
        [HideInInspector] public int distanceFromStart;
        [HideInInspector] public Tile previusTile;

        [Header("References")]
        public TileHighlight highlight;
        [SerializeField] Renderer _renderer;
        [SerializeField] Material[] _materials;

        //Setup Tile
        public void SetTile(int x, int y, float spacing)
        {
            this.x = x;
            this.y = y;

            gameObject.name = $"Tile: {x}-{y}";
            transform.localPosition = new Vector3(x * spacing, 0, y * spacing);

            SetMaterial();
            SetRotation();
        }

        private void SetMaterial()
        {
            int offsetID = (x + y) % 2;
            _renderer.material = _materials[offsetID];
        }

        private void SetRotation()
        {
            float x = Random.Range(-2, 3);
            float z = Random.Range(-2, 3);

            Vector3 rotation = new Vector3(x, 0, z);
            transform.rotation = Quaternion.Euler(rotation);
        }

        //Set tile to default state
        public void CleanUp()
        {
            distanceFromStart = 1000;
            movementNeed = 0;
            previusTile = null;

            highlight.Clean();
        }
    }
}