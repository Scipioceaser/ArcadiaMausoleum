using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextLerp : MonoBehaviour
{
    private TMP_Text text;

    public float delayCharacterTime = 0.05f;
    public float delayWordTime = 0.25f;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TMP_Text>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
    }

    private void Start()
    {
        StartCoroutine(SplitText());
    }
    
    // Have to make this an IEnumerator so that it can be called at start
    private IEnumerator SplitText()
    {
        bool hasSplitText = false;
        
        while (!hasSplitText)
        {
            if (text.textInfo.characterCount == 0 || text.textInfo.wordCount == 0)
            {
                yield return new WaitForSeconds(0.05f);
                continue;
            }
            for (int j = 0; j < text.textInfo.wordCount; j++)
            {
                TMP_WordInfo word = text.textInfo.wordInfo[j];

                GameObject p = new GameObject(word.GetWord());

                Vector3 center, left, right;
                left = GetCenter(word.firstCharacterIndex);
                right = GetCenter(word.firstCharacterIndex + (word.characterCount - 1));
                center = left;
                float w = right.x - left.x;
                center.x += w / 2;

                RectTransform r = p.AddComponent<RectTransform>();
                r.SetParent(transform, false);
                r.position = center;
                r.localScale = Vector3.one;

                if (j == text.textInfo.wordCount - 1)
                {
                    for (int i = word.firstCharacterIndex; i < text.textInfo.characterCount; i++)
                    {
                        if (text.textInfo.characterInfo[i].isVisible)
                        {
                            TMP_CharacterInfo c = text.textInfo.characterInfo[i];

                            Vector3 centerCharacter = GetCenter(i);

                            GameObject go = new GameObject("" + c.character);
                            RectTransform rc = go.AddComponent<RectTransform>();
                            go.transform.SetParent(p.transform, false);
                            rc.SetParent(p.transform, false);
                            rc.position = centerCharacter;
                            rc.localScale = Vector3.one;
                            go.AddComponent<CanvasRenderer>();
                            TextMeshProUGUI textMeshPro = go.AddComponent<TextMeshProUGUI>();
                            textMeshPro.color = text.color;
                            textMeshPro.autoSizeTextContainer = true;
                            textMeshPro.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                            textMeshPro.alignment = TextAlignmentOptions.Bottom;
                            textMeshPro.fontSize = GetComponent<TextMeshProUGUI>().fontSize;
                            textMeshPro.enableKerning = false;
                            textMeshPro.SetText("" + c.character);
                        }
                    }
                }
                else if (word.firstCharacterIndex != 0 && j == 0)
                {
                    for (int i = 0; i < word.characterCount + word.firstCharacterIndex + 1; i++)
                    {
                        if (text.textInfo.characterInfo[i].isVisible)
                        {
                            TMP_CharacterInfo c = text.textInfo.characterInfo[i];

                            Vector3 centerCharacter = GetCenter(i);

                            GameObject go = new GameObject("" + c.character);
                            RectTransform rc = go.AddComponent<RectTransform>();
                            go.transform.SetParent(p.transform, false);
                            rc.SetParent(p.transform, false);
                            rc.position = centerCharacter;
                            rc.localScale = Vector3.one;
                            go.AddComponent<CanvasRenderer>();
                            TextMeshProUGUI textMeshPro = go.AddComponent<TextMeshProUGUI>();
                            textMeshPro.color = text.color;
                            textMeshPro.autoSizeTextContainer = true;
                            textMeshPro.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                            textMeshPro.alignment = TextAlignmentOptions.Bottom;
                            textMeshPro.fontSize = GetComponent<TextMeshProUGUI>().fontSize;
                            textMeshPro.enableKerning = false;
                            textMeshPro.SetText("" + c.character);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < word.characterCount + 1; i++)
                    {
                        if (text.textInfo.characterInfo[word.firstCharacterIndex + i].isVisible)
                        {
                            TMP_CharacterInfo c = text.textInfo.characterInfo[word.firstCharacterIndex + i];

                            Vector3 centerCharacter = GetCenter(word.firstCharacterIndex + i);

                            GameObject go = new GameObject("" + c.character);
                            RectTransform rc = go.AddComponent<RectTransform>();
                            go.transform.SetParent(p.transform, false);
                            rc.SetParent(p.transform, false);
                            rc.position = centerCharacter;
                            rc.localScale = Vector3.one;
                            go.AddComponent<CanvasRenderer>();
                            TextMeshProUGUI textMeshPro = go.AddComponent<TextMeshProUGUI>();
                            textMeshPro.color = text.color;
                            textMeshPro.autoSizeTextContainer = true;
                            textMeshPro.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                            textMeshPro.alignment = TextAlignmentOptions.Bottom;
                            textMeshPro.fontSize = GetComponent<TextMeshProUGUI>().fontSize;
                            textMeshPro.enableKerning = false;
                            textMeshPro.SetText("" + c.character);
                        }
                    }
                }
            }

            hasSplitText = true;
            Destroy(text);

            yield return null;
        }
    }

    //Credit: https://forum.unity.com/threads/possible-to-get-size-and-position-of-a-link.608131/
    private Vector3 GetCenter(int index)
    {
        Vector3 bottomLeft = Vector3.zero;
        Vector3 topRight = Vector3.zero;

        float maxAscender = -Mathf.Infinity;
        float minDescender = Mathf.Infinity;
        
        TMP_CharacterInfo currentCharInfo = text.textInfo.characterInfo[index];

        maxAscender = Mathf.Max(maxAscender, currentCharInfo.ascender);
        minDescender = Mathf.Min(minDescender, currentCharInfo.descender);

        bottomLeft = new Vector3(currentCharInfo.bottomLeft.x, currentCharInfo.descender, 0);

        bottomLeft = transform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
        topRight = transform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

        float width = topRight.x - bottomLeft.x;
        float height = topRight.y - bottomLeft.y;

        Vector2 centerPosition = bottomLeft;
        centerPosition.x += width / 2;
        centerPosition.y += height / 2;

        return centerPosition;
    }

    private void Update()
    {
        //if (Input.GetKeyUp(KeyCode.Space)) StartCoroutine(LerpText(Vector3.up * 75f, 1.0f, false, true, false));
        //if (Input.GetKeyUp(KeyCode.LeftAlt)) StartCoroutine(LerpText((Vector3.up + Vector3.left) * 75f, 1.0f, true, true, false));
    }

    public IEnumerator LerpText(Vector3 offset, float time, bool fade, bool delayWords, bool delayCharacters)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            StartCoroutine(LerpWord(i, offset, time, fade, delayCharacters));

            if (delayWords)
                yield return new WaitForSeconds(delayWordTime);
        }
    }

    public IEnumerator LerpWord(int wordIndex, Vector3 offset, float time, bool fade, bool delayCharacters)
    {
        Transform word = transform.GetChild(wordIndex);

        for (int i = 0; i < word.childCount; i++)
        {
            StartCoroutine(LerpCharacter(wordIndex, i, offset, time, fade));

            if (delayCharacters)
                yield return new WaitForSeconds(delayCharacterTime);
        }
    }

    public IEnumerator LerpCharacter(int wordIndex, int characterIndex, Vector3 offset, float time, bool fade)
    {
        Transform character = transform.GetChild(wordIndex).GetChild(characterIndex);
        TextMeshProUGUI textMeshPro = character.gameObject.GetComponent<TextMeshProUGUI>();
        Color originalColor = textMeshPro.color;
        Color newColor = originalColor;
        
        newColor.a = fade ? 0 : 1;
        
        float r = 1.0f / time;
        float t = 0.0f;

        Vector3 origin = character.position;
        Vector3 destination = character.position;

        if (!fade)
            origin -= offset;

        if (fade)
            destination += offset;

        while (t < 1.0f)
        {
            t += Time.deltaTime * r;

            if (fade)
            {
                //newColor.a = Mathf.Lerp(255, 0, Mathf.SmoothStep(0.0f, 1.0f, t));
                character.position = Vector3.Lerp(origin, destination, Mathf.SmoothStep(0.0f, 1.0f, t));
                character.rotation = Quaternion.FromToRotation(origin, destination);
            }
            else
            {
                //newColor.a = Mathf.Lerp(0, 1, Mathf.SmoothStep(0.0f, 1.0f, t));
                character.position = Vector3.Lerp(origin, destination, Mathf.SmoothStep(0.0f, 1.0f, t));
                character.rotation = Quaternion.FromToRotation(character.position, destination);

            }

            //textMeshPro.color = newColor;
            textMeshPro.color = Color.Lerp(originalColor, newColor, Mathf.SmoothStep(0.0f, 1.0f, t));

            yield return null;
        }
    }
}
