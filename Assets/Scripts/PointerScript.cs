using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerScript : MonoBehaviour
{
    public float _defaultLength = 5.0f;
    public GameObject dot;
    public VRInputModule m_InputModule;

    public Canvas m_Canvas;

    private LineRenderer m_LineRenderer = null;

    private void Awake()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (m_Canvas.enabled)
        {
            dot.SetActive(true);
            m_LineRenderer.enabled = true;
            UpdateLine();
        }
        else
        {
            dot.SetActive(false);
            m_LineRenderer.enabled = false;
        }
    }

    private void UpdateLine()
    {
        // Use default or distance
        PointerEventData data = m_InputModule.GetData();
        float targetLength = data.pointerCurrentRaycast.distance == 0 ? _defaultLength : data.pointerCurrentRaycast.distance;

        // Raycast
        RaycastHit hit = createRaycast(targetLength);

        // Default end
        Vector3 endPosition = transform.position + transform.forward * targetLength;

        // If hit 
        if (hit.collider != null)
        {
            endPosition = hit.point;
        }

        // Set position of dot
        dot.transform.position = endPosition;

        // Set linerenderer
        m_LineRenderer.SetPosition(0, transform.position);
        m_LineRenderer.SetPosition(1, endPosition);
    }

    private RaycastHit createRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, _defaultLength);

        return hit;
    }
}
