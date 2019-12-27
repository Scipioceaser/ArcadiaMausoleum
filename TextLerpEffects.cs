using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLerpEffects : MonoBehaviour
{
    private TMP_Text text;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TMP_Text>();
        //text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }

    public IEnumerator LerpText(Vector3 offset, float time, bool fade = false, bool delayWord = false, bool delayCharacter = false, float wordDelayTime = 0.4f)
    {
        for (int i = 0; i < text.textInfo.wordCount; i++)
        {
            StartCoroutine(LerpWord(i, offset, time, fade, delayCharacter, 0.025f));

            if (delayWord)
                yield return new WaitForSeconds(wordDelayTime);
        }

        yield return null;
    }

    public IEnumerator LerpWord(int wordIndex, Vector3 offset, float time, bool fade = false, bool delay = false, float characterDelaySpeed = 0.025f)
    {
        TMP_WordInfo info = text.textInfo.wordInfo[wordIndex];

        for (int i = 0; i < info.characterCount; i++)
        {
            if (fade)
            {
                StartCoroutine(LerpCharacterFrom(info.firstCharacterIndex + i, offset, time));
            }
            else
            {
                StartCoroutine(LerpCharacterTo(info.firstCharacterIndex + i, offset, time));
            }

            if (delay)
                yield return new WaitForSeconds(characterDelaySpeed);
        }

        yield return null;
    }

    public IEnumerator LerpCharacterFrom(int index, Vector3 offset, float time)
    {
        text.ForceMeshUpdate();

        float r = 1.0f / time;
        float t = 0.0f;

        TMP_MeshInfo[] cachedMeshInfo = text.textInfo.CopyMeshInfoVertexData();

        while (t < 1.0f)
        {
            t += Time.deltaTime * r;

            int materialIndex = text.textInfo.characterInfo[index].materialReferenceIndex;
            int vertexIndex = text.textInfo.characterInfo[index].vertexIndex;

            Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;

            Vector3[] destinationVertices = text.textInfo.meshInfo[materialIndex].vertices;

            destinationVertices[vertexIndex] = sourceVertices[vertexIndex] + offset;
            destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] + offset;
            destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] + offset;
            destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] + offset;

            Color32[] newColors = text.textInfo.meshInfo[materialIndex].colors32;
            
            newColors[vertexIndex + 0].a = (byte)Mathf.Lerp(0, 255, Mathf.SmoothStep(0.0f, 1.0f, t));
            newColors[vertexIndex + 1].a = (byte)Mathf.Lerp(0, 255, Mathf.SmoothStep(0.0f, 1.0f, t));
            newColors[vertexIndex + 2].a = (byte)Mathf.Lerp(0, 255, Mathf.SmoothStep(0.0f, 1.0f, t));
            newColors[vertexIndex + 3].a = (byte)Mathf.Lerp(0, 255, Mathf.SmoothStep(0.0f, 1.0f, t));

            destinationVertices[vertexIndex + 0] = Vector3.Lerp(destinationVertices[vertexIndex + 0], sourceVertices[vertexIndex + 0], Mathf.SmoothStep(0.0f, 1.0f, t));
            destinationVertices[vertexIndex + 1] = Vector3.Lerp(destinationVertices[vertexIndex + 1], sourceVertices[vertexIndex + 1], Mathf.SmoothStep(0.0f, 1.0f, t));
            destinationVertices[vertexIndex + 2] = Vector3.Lerp(destinationVertices[vertexIndex + 2], sourceVertices[vertexIndex + 2], Mathf.SmoothStep(0.0f, 1.0f, t));
            destinationVertices[vertexIndex + 3] = Vector3.Lerp(destinationVertices[vertexIndex + 3], sourceVertices[vertexIndex + 3], Mathf.SmoothStep(0.0f, 1.0f, t));

            text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

            for (int i = 0; i < text.textInfo.meshInfo.Length; i++)
            {
                text.textInfo.meshInfo[i].mesh.vertices = text.textInfo.meshInfo[i].vertices;
                text.UpdateGeometry(text.textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }
    }

    public IEnumerator LerpCharacterTo(int index, Vector3 offset, float time)
    {
        text.ForceMeshUpdate();

        float r = 1.0f / time;
        float t = 0.0f;

        TMP_MeshInfo[] cachedMeshInfo = text.textInfo.CopyMeshInfoVertexData();

        while (t < 1.0f)
        {
            t += Time.deltaTime * r;

            int materialIndex = text.textInfo.characterInfo[index].materialReferenceIndex;
            int vertexIndex = text.textInfo.characterInfo[index].vertexIndex;

            Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;

            Vector3[] destinationVertices = text.textInfo.meshInfo[materialIndex].vertices;

            destinationVertices[vertexIndex] = sourceVertices[vertexIndex] + offset;
            destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] + offset;
            destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] + offset;
            destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] + offset;

            Color32[] newColors = text.textInfo.meshInfo[materialIndex].colors32;

            newColors[vertexIndex + 0].a = (byte)Mathf.Lerp(255, 0, Mathf.SmoothStep(0.0f, 1.0f, t));
            newColors[vertexIndex + 1].a = (byte)Mathf.Lerp(255, 0, Mathf.SmoothStep(0.0f, 1.0f, t));
            newColors[vertexIndex + 2].a = (byte)Mathf.Lerp(255, 0, Mathf.SmoothStep(0.0f, 1.0f, t));
            newColors[vertexIndex + 3].a = (byte)Mathf.Lerp(255, 0, Mathf.SmoothStep(0.0f, 1.0f, t));

            destinationVertices[vertexIndex + 0] = Vector3.Lerp(sourceVertices[vertexIndex + 0], destinationVertices[vertexIndex + 0], Mathf.SmoothStep(0.0f, 1.0f, t));
            destinationVertices[vertexIndex + 1] = Vector3.Lerp(sourceVertices[vertexIndex + 1], destinationVertices[vertexIndex + 1], Mathf.SmoothStep(0.0f, 1.0f, t));
            destinationVertices[vertexIndex + 2] = Vector3.Lerp(sourceVertices[vertexIndex + 2], destinationVertices[vertexIndex + 2], Mathf.SmoothStep(0.0f, 1.0f, t));
            destinationVertices[vertexIndex + 3] = Vector3.Lerp(sourceVertices[vertexIndex + 3], destinationVertices[vertexIndex + 3], Mathf.SmoothStep(0.0f, 1.0f, t));

            text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

            for (int i = 0; i < text.textInfo.meshInfo.Length; i++)
            {
                text.textInfo.meshInfo[i].mesh.vertices = text.textInfo.meshInfo[i].vertices;
                text.UpdateGeometry(text.textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }
    }
}
