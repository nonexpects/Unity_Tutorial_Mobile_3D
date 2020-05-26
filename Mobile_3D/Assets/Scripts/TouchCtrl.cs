using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchCtrl : MonoBehaviour
{
    public HeroMove player;

    RectTransform touchCtrl;
    private int touchID = -1;

    private Vector2 startPos = Vector2.zero;

    public float dragRadius = 50f;
    private bool btnPressed = false;

    void Start()
    {
        // TouchCtrl오브젝트 RectTransform 컴포넌트 가져오기
        touchCtrl = GetComponent<RectTransform>();

        // 콘트롤러의 처음 위치
        startPos = touchCtrl.position;
    }
    //Event Trigger에서 처리 (터치가 될 경우 실행)
    public void TouchDown()
    {
        Debug.Log("TouchDown");
        btnPressed = true;
    }

    public void TouchUp()
    {
        Debug.Log("TouchUp");
        btnPressed = false;
        //터치한 손을 놓으면 콘트롤러를 원래 지점으로 복귀시키기
        SendInputValue(startPos);
    }

    private void FixedUpdate()
    {
        HandleTouchPhase();

        //전처리기 사용해서 모바일 아닌 경우(Unity Editor, 웹플레이)에서는 마우스로 입력받기 설정
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER ||  UNITY_STANDALONE_OSX 

        SendInputValue(Input.mousePosition);

#endif
    }

    void HandleTouchPhase()
    {
        int i = 0;
        if(Input.touchCount > 0)
        {
            // 인자로 들어온 iterable-item의 내부 인덱스 끝까지 알아서 순환해주는 반복문
            foreach(Touch touch in Input.touches)
            {
                i++;
                Vector3 touchPos = new Vector3(touch.position.x, touch.position.y);
                // touch.phase -> 손가락 터치한 상태
                switch (touch.phase)
                {
                    // 화면에 터치가 시작된 상태
                    case TouchPhase.Began:
                        if(touch.position.x <= (startPos.x + dragRadius))
                        {
                            touchID = i;
                        }
                        break;
                        // 손가락으로 터치한 상태로 움직인 상태 
                    case TouchPhase.Moved:
                        if(touchID == i)
                        {
                            SendInputValue(touchPos);
                        }
                        break;
                        // 터치를 하고 움직이지 않고 가만히 있는 상태
                    case TouchPhase.Stationary:
                        if (touchID == i)
                        {
                            SendInputValue(touchPos);
                        }
                        break;
                        // 사용자가 화면에서 손가락을 뗀 상태
                    case TouchPhase.Ended:
                        if (touchID == i)
                        {
                            touchID = -1;
                        }
                        break;
                }
            }
        }
    }

    void SendInputValue(Vector2 inputPos)
    {
        if(btnPressed)
        {
            // 컨트롤러의 기준좌표로부터 입력받은 좌표사이의 거리 구하기
            Vector2 gabPos = (inputPos - startPos);

            // 입력 지점이 기준 좌표로부터 일정 거리 안에 있으면
            if(gabPos.sqrMagnitude <= dragRadius * dragRadius)
            {
                // 현재 터치 좌표로 방향키 이동하기
                touchCtrl.position = inputPos;
            }
            else //입력 지점이 일정 기준 좌표보다 크다면
            {
                // 벡터크기 정규화하기
                gabPos.Normalize();
                touchCtrl.position = startPos + gabPos * dragRadius;
            }
        }
        else
        {
            // 터치한 손을 놓으면 컨트롤러를 초기 위치로 설정
            touchCtrl.position = startPos;
        }

        Vector2 touchPosXY = new Vector3(touchCtrl.position.x, touchCtrl.position.y);
        //Debug.Log(touchCtrl.position.x);
        //Debug.Log(touchCtrl.position.y);

        Vector2 diff = touchPosXY - startPos;
        Vector2 normDiff = new Vector2(diff.x / dragRadius, diff.y / dragRadius);

        if(player != null)
        {
            // HeroMove.cs 스크립트 안에 있는 OnTouchValueChanged() 메소드 호출
            player.OnTouchValueChanged(normDiff);
        }
    }
    
}
