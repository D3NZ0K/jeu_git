using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]
public class Ribbon : MonoBehaviour
{
    public float distance = 0.05f;
    public Material lineMat;
    public int ribbonLength = 20;

    LineRenderer lineRenderer;
    List<Vector3> positions = new List<Vector3>();
    float sqrMagnitude;
    bool mouseDown, draw;

    EdgeCollider2D edgeCollider;
    

    void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineRenderer.endWidth = 0.1f;
        lineRenderer.material = lineMat;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            StartDrawing();
        else if (Input.GetMouseButtonUp(0))
            StopDrawing();
            CreateSimpleLine();
            

        if (mouseDown)
        {
            DrawUpdate();
            RibbonUpdate();
            Render();
        }
        
    }

    void StartDrawing()
    {
        mouseDown = true;
        draw = true;
        positions.Clear();
        positions.Add(GetPoint(Input.mousePosition));
    }

    void StopDrawing()
    {
        mouseDown = false;
        draw = false;
    }

    void DrawUpdate()
    {
        if (!draw) return;

        Vector3 worldMousePosition = GetPoint(Input.mousePosition);

        sqrMagnitude = (worldMousePosition - positions[positions.Count - 1]).sqrMagnitude;

        if ((sqrMagnitude > distance * distance) &&
            positions.Count < ribbonLength)
            positions.Add(worldMousePosition);

        if (positions.Count == ribbonLength)
            draw = false;
    }

    void RibbonUpdate()
    {
        if (draw) return;

        Vector3 worldMousePosition = GetPoint(Input.mousePosition);

        sqrMagnitude = (worldMousePosition - positions[positions.Count - 1]).sqrMagnitude;

        if (sqrMagnitude > distance * distance)
        {
            for (int i = 0; i < positions.Count - 1; i++)
                positions[i] = positions[i + 1];
            positions[positions.Count - 1] = (worldMousePosition);
        }
    }

    void Render()
    {
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    void CreateSimpleLine ()
    {
        if (positions.Count == lineRenderer.positionCount && positions.Count > 0)
        {
            Vector2 start = lineRenderer.GetPosition(0);
            Vector2 end = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

            List<Vector2> StartEnd = new List<Vector2>();

            StartEnd.Add(start);
            StartEnd.Add(end);

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);

            edgeCollider.points = StartEnd.ToArray();
        }
    }

    Vector3 GetPoint(Vector3 mousePosition)
        => Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -Camera.main.transform.position.z));
}