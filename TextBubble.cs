using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rect))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(TextLerp))]
public class TextBubble : MonoBehaviour
{
    private TextLerp textEffects;
    private RectTransform rect;

    private LineRenderer line;

    public Vector2[] linePoints;
    private int pointCount = 10;
    [Range(0.01f, 1.0f)]
    public float width = 0.25f;
    public float timeToWaitBetweenPointsDraw = 0.15f;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        //line.useWorldSpace = false;
        line.startWidth = 0.25f;
        line.endWidth = 0.25f;
        line.material = new Material(Shader.Find("Sprites/Default"));
        textEffects = GetComponent<TextLerp>();
        rect = GetComponent<RectTransform>();
        linePoints = new Vector2[pointCount];
    }

    /*
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) CreateBubble();
        if (Input.GetKeyUp(KeyCode.D)) DestroyBubble();
    }
    */

    public void CreateBubble()
    {
        textEffects.StartCoroutine(textEffects.LerpText(Vector3.up * 75f, 1.0f, false, true, false));
        StartCoroutine(DrawLine(Vector3.zero, timeToWaitBetweenPointsDraw));
    }

    public void DestroyBubble()
    {
        textEffects.StartCoroutine(textEffects.LerpText((Vector3.up + Vector3.left) * 75f, 1.0f, true, true, false));
        StartCoroutine(FadeLine(1.0f));
    }

    // At the moment this only works with the Sprite default shader
    private IEnumerator FadeLine(float fadeTime = 1.0f)
    {
        float r = 1.0f / fadeTime;
        float t = 0.0f;

        Color originalColorStart = line.startColor;
        Color newColorStart = originalColorStart;
        newColorStart.a = 0;

        Color originalColorEnd = line.endColor;
        Color newColorEnd = originalColorEnd;
        newColorEnd.a = 0;

        while (t < 1.0f)
        {
            t += Time.deltaTime * r;
            
            line.material.SetColor("_Color", Color.Lerp(originalColorStart, newColorStart, Mathf.SmoothStep(0.0f, 1.0f, t)));

            yield return null;
        }
    }
    
    // Need to add in player position
    private IEnumerator DrawLine(Vector3 source, float time = 0.05f)
    {
        Vector3 offset = new Vector3(rect.sizeDelta.x / 2, -(rect.sizeDelta.y / 4), 0f);
        Vector3 rectPosition = rect.transform.position + offset;
        //print(rect.sizeDelta.x);
        Vector3 p;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, rectPosition, Camera.main, out p);

        Vector3 v = new Vector3(p.x * 0.25f, p.y * 0.75f, p.z);
        CalculateCurve(v, p, pointCount);
        //line.positionCount = pointCount;
        
        Vector3 prev = linePoints[0];
        for (int i = 0; i < linePoints.Length; i++)
        {
            line.positionCount++;
            Vector3 cur = linePoints[i];
            line.SetPosition(i, cur);
            prev = linePoints[i];
            yield return new WaitForSeconds(time);
        }
    }

    public void CalculateCurve(Vector3 origin, Vector3 target, int numPoints, float waitTime = 0.05f)
    {
        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (numPoints - 1f);
            Vector3 p1 = new Vector3(target.x / 2, target.y / 2, target.z);
            Vector3 v = (1.0f - t) * (1.0f - t) * origin + 2.0f * (1.0f - t) * t * p1 + t * t * target;
            linePoints[i] = v;
        }
    }
}
