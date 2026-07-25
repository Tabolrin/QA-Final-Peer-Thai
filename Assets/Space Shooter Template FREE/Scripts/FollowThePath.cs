using UnityEngine;

/// <summary>
/// This script moves the ‘Enemy’ along the defined path.
/// </summary>
public class FollowThePath : MonoBehaviour {
        
    [HideInInspector] public Transform [] path; //path points which passes the 'Enemy' 
    [HideInInspector] public float speed; 
    [HideInInspector] public bool rotationByPath;   //whether 'Enemy' rotates in path direction or not
    [HideInInspector] public bool loop;         //if loop is true, 'Enemy' returns to the path starting point after completing the path
    float currentPathPercent;               //current percentage of completing the path
    Vector3[] pathPositions;                //path points in vector3
    [HideInInspector] public bool movingIsActive;   //whether 'Enemy' moves or not
    [HideInInspector] public bool movingForward = true;   //when looping, whether currently retracing the path backward or forward

    //setting path parameters for the 'Enemy' and sending the 'Enemy' to the path starting point
    public void SetPath()
    {
        currentPathPercent = 0;
        movingForward = true;
        pathPositions = new Vector3[path.Length];       //transform path points to vector3
        for (int i = 0; i < pathPositions.Length; i++)
        {
            pathPositions[i] = path[i].position;
        }
        transform.position = NewPositionByPath(pathPositions, 0); //sending the enemy to the path starting point
        if (!rotationByPath)
            transform.rotation = Quaternion.identity;
        movingIsActive = true;
    }

    private void Update()
    {
        if (movingIsActive)
        {
            currentPathPercent += (movingForward ? 1 : -1) * speed / 100 * Time.deltaTime;     //every update calculating current path percentage according to the defined speed

            //clamp/reverse BEFORE using the value to compute position - otherwise a percent
            //that overshoots past 0 while reversing gets used for one frame while still
            //negative, which throws IndexOutOfRangeException inside Interpolate
            currentPathPercent = PathLoopMotion.ClampAndReverseIfNeeded(currentPathPercent, loop, ref movingForward, out bool shouldDestroy);
            if (shouldDestroy)
            {
                Destroy(gameObject);
                return;
            }

            transform.position = NewPositionByPath(pathPositions, currentPathPercent); //moving the 'Enemy' to the path position, calculated in method NewPositionByPath
            if (rotationByPath)                            //rotating the 'Enemy' in path direction, if set 'rotationByPath'
            {
                float lookAheadPercent = Mathf.Clamp01(currentPathPercent + (movingForward ? 0.01f : -0.01f));
                transform.right = CatmullRomPath.Interpolate(CatmullRomPath.CreatePoints(pathPositions), lookAheadPercent) - transform.position;
                transform.Rotate(Vector3.forward * 90);
            }
        }
    }

    Vector3 NewPositionByPath(Vector3 [] pathPos, float percent)
    {
        return CatmullRomPath.Interpolate(CatmullRomPath.CreatePoints(pathPos), percent);
    }
}
