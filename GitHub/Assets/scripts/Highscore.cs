using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    private RectTransform rectT;

    private void Start()
    {
        rectT = GetComponent<RectTransform>();
    }

    private void Update()
    {
        textMesh.text =  rectT.position.y.ToString("F0");
    }
}
