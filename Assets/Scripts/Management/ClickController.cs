using Tactics.Commands;
using Tactics.Core;
using UnityEngine;

public class ClickController : MonoBehaviour
{
    [SerializeField] CommandManager _commander;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectObject();
        }
    }

    private void SelectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var objectFound = hit.transform.gameObject;
            var tileFound = objectFound.GetComponent<Tile>();

            if (tileFound)
                _commander.PrepareCommand(tileFound);
        }
    }
}
