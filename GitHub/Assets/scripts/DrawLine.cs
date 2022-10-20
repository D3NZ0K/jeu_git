using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject currentLine;

    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider;

    public List<Vector2> mousePositions;
    [SerializeField] private float timeBeforeFade = 1;

    public float distance;
    public float maxDistance;

    public int lineLenght;

    private bool draw = false;
    private float sqrtMagnitude;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateLine();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            draw = false;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 tempMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(tempMousePos, mousePositions[mousePositions.Count - 1]) > distance && Vector2.Distance(tempMousePos, mousePositions[mousePositions.Count - 1]) < maxDistance)
            {
                UpdateLine(tempMousePos);
                UpdateNotDraw(tempMousePos);
                Render(tempMousePos);
            }
        }
    }

    void CreateLine()
    {
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);

        lineRenderer = currentLine.GetComponent<LineRenderer>();
        edgeCollider = currentLine.GetComponent<EdgeCollider2D>();
        mousePositions.Clear();
        mousePositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        mousePositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        lineRenderer.SetPosition(0, mousePositions[0]);
        lineRenderer.SetPosition(1, mousePositions[1]);

        edgeCollider.points = mousePositions.ToArray();
        draw = true;
    }

    void UpdateLine(Vector2 newMousePos)
    {
        if (!draw) { return; }

        sqrtMagnitude = (newMousePos - mousePositions[mousePositions.Count - 1]).sqrMagnitude;

        if (sqrtMagnitude > distance * distance && mousePositions.Count < lineLenght)
        {
            mousePositions.Add(newMousePos);
        }

        if (mousePositions.Count == lineLenght)
        {
            draw = false;
        }
    }

    void UpdateNotDraw (Vector2 newMousePos)
    {
        if (draw) { return; }

        sqrtMagnitude = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(mousePositions[mousePositions.Count - 1].x, mousePositions[mousePositions.Count - 1].y, 1)).sqrMagnitude;
        
        if (sqrtMagnitude > distance * distance)
        {
            for (int i = 0; i < mousePositions.Count - 1; i++)
                mousePositions[i] = mousePositions[i + 1];
            mousePositions[mousePositions.Count - 1] = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void Render (Vector2 newMousePos)
    {
        lineRenderer.positionCount = mousePositions.Count;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newMousePos);
        //edgeCollider.points = mousePositions.ToArray();
    }

    private IEnumerator lineDisapear ()
    {
        yield return new WaitForSeconds(timeBeforeFade);
    }
}