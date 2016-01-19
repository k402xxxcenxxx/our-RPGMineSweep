using System.Collections.Generic;
using UnityEngine;


public class TouchControl : MonoBehaviour
{
    /// <summary>
    /// 縮放時紀錄的兩個tiuch
    /// </summary>
    private Touch pinchFinger1, pinchFinger2;
    /// <summary>
    /// 紀錄單點touch的起始位置
    /// </summary>
    private Vector2 basePosition;
    /// <summary>
    /// 紀錄選取的button
    /// </summary>
    public static btnClone mybtnClone;
    /// <summary>
    /// 要縮放移動的目標
    /// </summary>
    public Canvas targetCanvas;
    public GameObject btnContainer;

    /// <summary>
    /// 縮放最大值
    /// </summary>
    public static float MAP_MAX_SCALE = 15;
    /// <summary>
    /// 縮放最小值
    /// </summary>
    public static float MAP_MIN_SCALE = 1;

    /// <summary>
    /// 移動的x最大值
    /// </summary>
    public static float MAP_MAX_RIGHT = 400;
    /// <summary>
    /// 移動的y最大值
    /// </summary>
    public static float MAP_MAX_TOP = 300;
    /// <summary>
    /// 移動的x最小值
    /// </summary>
    public static float MAP_MAX_LEFT = -400;
    /// <summary>
    /// 移動的y最小值
    /// </summary>
    public static float MAP_MAX_BOTTOM = -300;

    private float currScale;

    void Awake() {
        currScale = 1;
    }
    void Update()
    {
        //==========================================================================================
        //  Touch
        //==========================================================================================

        // -- Pinch縮放
        // ------------------------------------------------
        // Works only with two fingers
        if (Input.touchCount == 2)
        {
            var finger1 = Input.GetTouch(0);
            var finger2 = Input.GetTouch(1);

            if (finger1.phase == TouchPhase.Began && finger2.phase == TouchPhase.Began)
            {
                this.pinchFinger1 = finger1;
                this.pinchFinger2 = finger2;
            }

            // On move, update
            if (finger1.phase == TouchPhase.Moved || finger2.phase == TouchPhase.Moved)
            {
                

                float baseDistance = Vector2.Distance(this.pinchFinger1.position, this.pinchFinger2.position);
                float currentDistance = Vector2.Distance(finger1.position, finger2.position);

                // Purcent
                float currentDistancePurcent = currentDistance / baseDistance;


                // Take the base scale and make it smaller/bigger 
                float nowScale = targetCanvas.scaleFactor;
                if (currentDistancePurcent < 1 && currentDistancePurcent > 0)
                {
                    if (nowScale * (currentDistancePurcent) >= MAP_MIN_SCALE)
                    {
                        Debug.Log("currentDistancePurcent = "+ currentDistancePurcent+",nowScale = "+nowScale);

                        targetCanvas.scaleFactor = nowScale * (currentDistancePurcent);
                        currScale = targetCanvas.scaleFactor;

                        TouchControl.MAP_MAX_RIGHT = currScale * 200;
                        TouchControl.MAP_MAX_LEFT = currScale * -1 * 200;

                        TouchControl.MAP_MAX_TOP = currScale * 100;
                        TouchControl.MAP_MAX_BOTTOM = currScale * -1 * 100;
                    }
                }
                else if (currentDistancePurcent > 1) {
                    if (nowScale * (currentDistancePurcent) <= MAP_MAX_SCALE)
                    {
                        Debug.Log("currentDistancePurcent = " + currentDistancePurcent + ",nowScale = " + nowScale);

                        targetCanvas.scaleFactor = nowScale * (currentDistancePurcent);
                        currScale = targetCanvas.scaleFactor;


                        TouchControl.MAP_MAX_RIGHT = currScale * 200;
                        TouchControl.MAP_MAX_LEFT = currScale * -1 * 200;

                        TouchControl.MAP_MAX_TOP = currScale * 100;
                        TouchControl.MAP_MAX_BOTTOM = currScale * -1 * 100;
                    }
                }
                
            }
        }
        else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            // -- Drag
            // ------------------------------------------------
            if (touch.phase == TouchPhase.Began)
            {
                basePosition = touch.position;
                
            }
            else if (touch.phase == TouchPhase.Moved)
            {

                Vector3 positionDelta = (touch.position - basePosition) / currScale;
                Vector3 resultPosition = btnContainer.transform.localPosition + positionDelta;

                if (resultPosition.x > MAP_MAX_RIGHT || resultPosition.x < MAP_MAX_LEFT)
                    positionDelta.x = 0;
                if (resultPosition.y > MAP_MAX_TOP || resultPosition.y < MAP_MAX_BOTTOM)
                    positionDelta.y = 0;

                btnContainer.transform.localPosition += positionDelta;

                basePosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (mybtnClone != null) {
                    if (basePosition == touch.position) {
                        mybtnClone.leftClickCloneBtn();
                    }
                }
            }
        }

        //==========================================================================================
        //  mouse down
        //==========================================================================================
        if (Input.GetMouseButtonDown(0))//Left Button
        {
            basePosition = (Vector2)Input.mousePosition;
            Debug.Log("Input.GetMouseButtonDown(0) , basePosition = "+ basePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 positionDelta = ((Vector2)Input.mousePosition - basePosition) ;
            Vector3 resultPosition = btnContainer.transform.localPosition + positionDelta;
            
            if (resultPosition.x > MAP_MAX_RIGHT || resultPosition.x < MAP_MAX_LEFT)
                positionDelta.x = 0;
            if (resultPosition.y > MAP_MAX_TOP || resultPosition.y < MAP_MAX_BOTTOM)
                positionDelta.y = 0;
            
            btnContainer.transform.localPosition += positionDelta;

            basePosition = (Vector2)Input.mousePosition;
            Debug.Log("positionDelta = " + positionDelta);
        }

        if (Input.GetMouseButtonUp(0))//Left Button
        {
            if (mybtnClone != null)
            {
                if (basePosition == (Vector2)Input.mousePosition)
                {
                    mybtnClone.leftClickCloneBtn();
                    
                }
            }
        }

        /*
        if (currScale > 3)
            targetCanvas.transform.localScale = new Vector3(3, 3, 3);

        if (currScale < 1)
            targetCanvas.transform.localScale = new Vector3(1, 1, 1);
        
        if (targetCanvas.transform.localPosition.x > MAP_MAX_RIGHT)
            targetCanvas.transform.localPosition = new Vector2(MAP_MAX_RIGHT, targetCanvas.transform.localPosition.y);

        if (targetCanvas.transform.localPosition.x < MAP_MAX_LEFT)
            targetCanvas.transform.localPosition = new Vector2(MAP_MAX_LEFT, targetCanvas.transform.localPosition.y);

        if (targetCanvas.transform.localPosition.y > MAP_MAX_TOP)
            targetCanvas.transform.localPosition = new Vector2(targetCanvas.transform.localPosition.x, MAP_MAX_TOP);

        if (targetCanvas.transform.localPosition.y < MAP_MAX_BOTTOM)
            targetCanvas.transform.localPosition = new Vector2(targetCanvas.transform.localPosition.x, MAP_MAX_BOTTOM);
            */
    }
    public static void setBtnHold(btnClone btnHold) {
        mybtnClone = btnHold;
    }
}