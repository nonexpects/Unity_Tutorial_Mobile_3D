using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMove : MonoBehaviour
{
    float h, v;
    float speed = 3f;

    bool jumping;
    float lastTime;

    Animator mAvatar;

    public Transform missile_pos;
    public GameObject Hero_Missile;

    public GameObject missile_effect;

    public void OnTouchValueChanged(Vector2 stickPos)
    {
        h = stickPos.x;
        v = stickPos.y;
    }

    private void Start()
    {
        mAvatar = GetComponent<Animator>();
    }

    public void OnJumpBtnDown()
    {
        jumping = true;
        StartCoroutine(JumpHero());
    }
    public void OnJumpBtnUp()
    {
        jumping = false;
    }

    IEnumerator JumpHero()
    {
        if(Time.time - lastTime > 1f)
        {
            lastTime = Time.time;
            while(jumping)
            {
                mAvatar.SetTrigger("JUMP");
                jumping = false; // while 구문이 계속 실행되는 것을 막음
                yield return new WaitForSeconds(1f);
            }
        }
    }

    public void OnMissileShootDown()
    {
        GameObject imsy = Instantiate(missile_effect, missile_pos.position, missile_pos.rotation);
        Destroy(imsy, 0.5f);
    }

    public void OnMissileShootUp()
    {
        Instantiate(Hero_Missile, missile_pos.position, missile_pos.rotation);
    }


    void Update()
    {
        mAvatar.SetFloat("Speed", (h * h + v * v));

        if (h != 0f && v != 0f)
        {
            transform.Rotate(0, h, 0);

            transform.Translate(0, 0, v * speed * Time.deltaTime);
        }
    }
}
