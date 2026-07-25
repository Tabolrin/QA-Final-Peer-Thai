using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script defines the borders of ‘Player’s’ movement. Depending on the chosen handling type, it moves the ‘Player’ together with the pointer.
/// </summary>

[System.Serializable]
public class Borders
{
    [Tooltip("offset from viewport borders for player's movement")]
    public float minXOffset = 1.5f, maxXOffset = 1.5f, minYOffset = 1.5f, maxYOffset = 1.5f;
    [HideInInspector] public float minX, maxX, minY, maxY;
}

public class PlayerMoving : MonoBehaviour {

    [Tooltip("offset from viewport borders for player's movement")]
    public Borders borders;

    [Tooltip("half-width of the actual designed play field (enemy wave paths span roughly -8.8..8.2) - " +
             "caps horizontal movement so it never exceeds this even on a wide/landscape viewport")]
    public float contentHalfWidth = 9.5f;

    [Tooltip("Accessibility: keyboard movement speed, for players who have difficulty holding the " +
             "mouse button down continuously - arrow keys / WASD move the ship as an alternative to the mouse")]
    public float keyboardMoveSpeed = 12f;

    Camera mainCamera;
    bool controlIsActive = true; 

    public static PlayerMoving instance; //unique instance of the script for easy access to the script

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        ResizeBorders();                //setting 'Player's' moving borders deending on Viewport's size
    }

    private void Update()
    {
        if (controlIsActive)
        {
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBGL    //if the current platform is not mobile, setting mouse handling

            Vector2 keyboardInput = GetKeyboardInput();
            if (keyboardInput != Vector2.zero)
            {
                // Accessibility: keyboard is an alternative to the mouse, not a replacement for it -
                // some players find holding a mouse button down and tracking the cursor difficult;
                // discrete key presses give them the same movement without that requirement.
                Vector3 keyboardTarget = transform.position + (Vector3)(keyboardInput * keyboardMoveSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, keyboardTarget, 30 * Time.deltaTime);
            }
            else if (Input.GetMouseButton(0)) //if mouse button was pressed
            {
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition); //calculating mouse position in the worldspace
                mousePosition.z = transform.position.z;
                transform.position = Vector3.MoveTowards(transform.position, mousePosition, 30 * Time.deltaTime);
            }
#endif

#if UNITY_IOS || UNITY_ANDROID //if current platform is mobile, 

            if (Input.touchCount == 1) // if there is a touch
            {
                Touch touch = Input.touches[0];
                Vector3 touchPosition = mainCamera.ScreenToWorldPoint(touch.position);  //calculating touch position in the world space
                touchPosition.z = transform.position.z;
                transform.position = Vector3.MoveTowards(transform.position, touchPosition, 30 * Time.deltaTime);
            }
#endif
            transform.position = new Vector3    //if 'Player' crossed the movement borders, returning him back 
                (
                Mathf.Clamp(transform.position.x, borders.minX, borders.maxX),
                Mathf.Clamp(transform.position.y, borders.minY, borders.maxY),
                0
                );
        }
    }

    //accessibility: arrow keys and WASD as an alternative movement input to the mouse
    Vector2 GetKeyboardInput()
    {
        float horizontal = 0f;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) horizontal -= 1f;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) horizontal += 1f;

        float vertical = 0f;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) vertical -= 1f;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) vertical += 1f;

        return new Vector2(horizontal, vertical);
    }

    //setting 'Player's' movement borders according to Viewport size and defined offset,
    //capped to the actual play field so a wide viewport can't extend past where enemies fly
    void ResizeBorders()
    {
        float viewportMinX = mainCamera.ViewportToWorldPoint(Vector2.zero).x + borders.minXOffset;
        float viewportMaxX = mainCamera.ViewportToWorldPoint(Vector2.right).x - borders.maxXOffset;
        borders.minX = PlayFieldBounds.ClampMin(viewportMinX, contentHalfWidth);
        borders.maxX = PlayFieldBounds.ClampMax(viewportMaxX, contentHalfWidth);
        borders.minY = mainCamera.ViewportToWorldPoint(Vector2.zero).y + borders.minYOffset;
        borders.maxY = mainCamera.ViewportToWorldPoint(Vector2.up).y - borders.maxYOffset;
    }
}
