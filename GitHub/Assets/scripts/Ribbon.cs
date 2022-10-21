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

    private List<Vector2> StartEnd = new List<Vector2>();

    [SerializeField] private int boingNumber;
    [SerializeField] private float boingDistance = 0.5f;
    [SerializeField] private float boingTime = 0.05f;

    private bool disapeared;
    



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
        {
            StartDrawing();
        }

        else if (Input.GetMouseButtonUp(0))
        {
            StopDrawing();
            CreateSimpleLine();
        }


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

            //mid
            float x = start.x + (end.x  - start.x) / 2;
            float y = start.y + (end.y - start.y) / 2;
            
            Vector2 mid = new Vector2(x, y);

            //start mid
            float j = start.x + (mid.x - start.x) / 2;
            float k = start.y + (mid.y - start.y) / 2;

            Vector2 startMid = new Vector2(j, k);

            //mid end
            float a = mid.x + (end.x - mid.x) / 2;
            float b = mid.y + (end.y - mid.y) / 2;

            Vector2 midEnd = new Vector2(a, b);

            StartEnd = new List<Vector2>();

            StartEnd.Add(start);
            StartEnd.Add(startMid);
            StartEnd.Add(mid);
            StartEnd.Add(midEnd);
            StartEnd.Add(end);

            lineRenderer.positionCount = 5;

            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, startMid);
            lineRenderer.SetPosition(2, mid);
            lineRenderer.SetPosition(3, midEnd);
            lineRenderer.SetPosition(4, end);

            edgeCollider.enabled = true;
            edgeCollider.points = StartEnd.ToArray();
        }
    }

    private IEnumerator BoingAnim ()
    {
        for (int i = 0; i < boingNumber; i++)
        {
            boingDistance = -boingDistance;

            // direction of your line
            var direction = (StartEnd[4] - StartEnd[0]).normalized;
            // perpendicular direction
            var perpendicularDirection = Vector2.Perpendicular(direction).normalized;
            // and finally the point C is starting from the center point go in perpendicular direction
            Vector2 a = StartEnd[2] + perpendicularDirection * boingDistance;
            Vector2 b = StartEnd[1] + perpendicularDirection * (boingDistance / 1.4f);
            Vector2 c = StartEnd[3] + perpendicularDirection * (boingDistance / 1.4f);

            lineRenderer.SetPosition(2, a);
            lineRenderer.SetPosition(1, b);
            lineRenderer.SetPosition(3, c);

            yield return new WaitForSeconds(boingTime);
        }

        lineRenderer.positionCount = 0;
        edgeCollider.enabled = false;
    }


    Vector3 GetPoint(Vector3 mousePosition)
        => Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -Camera.main.transform.position.z));


    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.rigidbody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
        if (lineRenderer.positionCount > 4)
        {
            StartCoroutine(BoingAnim());
        }
        
    }
}