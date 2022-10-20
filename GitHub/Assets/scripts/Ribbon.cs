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

    [SerializeField] private float boingIntensity;

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
            StartDrawing();
        else if (Input.GetMouseButtonUp(0))
        {
            StopDrawing();
            CreateSimpleLine();
            StartCoroutine(FadeAnim());
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
        lineRenderer.SetPosition(2, new Vector2(StartEnd[2].x + 2, StartEnd[2].y + 2));

        yield return new WaitForEndOfFrame();
    }

    private IEnumerator FadeAnim()
    {
            yield return new WaitForSeconds(1.5f);
            lineRenderer.positionCount = 0;
            edgeCollider.enabled = false;
    }


    Vector3 GetPoint(Vector3 mousePosition)
        => Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -Camera.main.transform.position.z));


    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.rigidbody.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
    }
}