using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScrollRect))]
public class ButtonSelectionController : MonoBehaviour
{
    [SerializeField]
    private float m_lerpTime = 0.1f;
    private ScrollRect m_scrollRect;
    private Button[] m_buttons;
    private int m_index;
    private float m_verticalPosition;
    private bool m_up;
    private bool m_down;


    /*
    public void Start()
    {
        m_scrollRect = GetComponent<ScrollRect>();
        m_buttons = GetComponentsInChildren<Button>();
        Debug.Log(m_buttons[0]);
        Debug.Log(m_buttons.Length);
        m_buttons[m_index].Select();
        m_verticalPosition = 1f - ((float)m_index / (m_buttons.Length - 1));
    }
    */

    public void SetUp() // not using Start() because array of Buttons needs to be intiated before this SetUp() calls.
        //Gets called from ButtonListControl - Script
    {
        m_scrollRect = GetComponent<ScrollRect>();
        m_buttons = GetComponentsInChildren<Button>();
        Debug.Log(m_buttons[0]);
        Debug.Log(m_buttons.Length);
        m_buttons[m_index].Select();
        m_verticalPosition = 1f - ((float)m_index / (m_buttons.Length - 1));
    }

    public void Update()
    {
        m_up = Input.GetKeyDown(KeyCode.UpArrow);
        m_down = Input.GetKeyDown(KeyCode.DownArrow);

        if (m_up ^ m_down)
        {
            if (m_up)
                m_index = Mathf.Clamp(m_index - 1, 0, m_buttons.Length - 1);
            else
                m_index = Mathf.Clamp(m_index + 1, 0, m_buttons.Length - 1);

            m_buttons[m_index].Select();
            m_verticalPosition = 1f - ((float)m_index / (m_buttons.Length - 1));
        }


        m_scrollRect.verticalNormalizedPosition = Mathf.Lerp(m_scrollRect.verticalNormalizedPosition, m_verticalPosition, Time.deltaTime / m_lerpTime);
    }

   
        
}