using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class CustomTextMeshOutline : MonoBehaviour
{
    public float outlineSize = 0.1f;
    public Color outlineColor = Color.black;
    private TextMesh mainTextMesh;
    private TextMesh[] outlines;

    void Start()
    {
        mainTextMesh = GetComponent<TextMesh>();
        CreateOutlines();
        UpdateOutlines();
    }

    void CreateOutlines()
    {
        outlines = new TextMesh[8];

        for (int i = 0; i < 8; i++)
        {
            GameObject outlineObject = new GameObject("Outline");
            outlineObject.transform.SetParent(transform, false);
            outlineObject.transform.localPosition = Vector3.zero;
            outlineObject.transform.localRotation = Quaternion.identity;
            outlineObject.transform.localScale = Vector3.one;

            TextMesh outlineTextMesh = outlineObject.AddComponent<TextMesh>();
            outlineTextMesh.fontSize = mainTextMesh.fontSize;
            outlineTextMesh.font = mainTextMesh.font;
            outlineTextMesh.anchor = mainTextMesh.anchor;
            outlineTextMesh.alignment = mainTextMesh.alignment;
            outlineTextMesh.fontStyle = mainTextMesh.fontStyle;

            outlines[i] = outlineTextMesh;
        }
    }

    void UpdateOutlines()
    {
        Vector3[] directions = new Vector3[]
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.up + Vector3.left,
            Vector3.up + Vector3.right,
            Vector3.down + Vector3.left,
            Vector3.down + Vector3.right
        };

        for (int i = 0; i < 8; i++)
        {
            outlines[i].text = mainTextMesh.text;
            outlines[i].color = outlineColor;
            outlines[i].transform.localPosition = directions[i] * outlineSize;
            outlines[i].transform.localRotation = mainTextMesh.transform.localRotation;
            outlines[i].transform.localScale = mainTextMesh.transform.localScale;
        }
    }

    void LateUpdate()
    {
        UpdateOutlines();
    }
}
