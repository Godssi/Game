using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchRight;
    public bool isTouchLeft;

    public float speed;
    public float power = 1; // 파워가 쌓이면 더 큰 총알이 나간다
    public float maxShotDelay; // 실제 딜레이 시간
    public float curShotDelay; // 한발 쏘고 난 뒤의 충전되는 시간

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public int bullet_speed = 10;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Fire();
        Reload();// 장전 함수
    }

    /*
     * 게임 오브젝트 반환 Instantiate(프리팹, 생성 위치, 생성된 오브젝트의 방향);
     * AddForce();
     * Collider의 IsTrigger에 대한 것 찾아보고 정리하기
     * GetButton,Down,Up에 대해서 정리해두기
     */
    private void Fire()
    {
        if (!Input.GetButton("Fire1"))
            return;

        if (curShotDelay < maxShotDelay) // 장전 시간이 딜레이 시간이 되었을 때 총알이 나간다!!
            return;

        switch (power)
        {
            case 1: //Power One
                GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * bullet_speed, ForceMode2D.Impulse);
                break;

            case 2:
                GameObject bulletR = Instantiate(bulletObjA, transform.position + Vector3.right * 0.1f, transform.rotation);
                GameObject bulletL = Instantiate(bulletObjA, transform.position + Vector3.left * 0.1f, transform.rotation);

                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

                rigidR.AddForce(Vector2.up * bullet_speed, ForceMode2D.Impulse);
                rigidL.AddForce(Vector2.up * bullet_speed, ForceMode2D.Impulse);

                break;
            case 3:
                GameObject bulletRR = Instantiate(bulletObjA, transform.position + Vector3.right * 0.35f, transform.rotation); // 가운데 큰 총알과 충돌할 수 있으므로 0.1->0.35로 올림
                GameObject bulletLL = Instantiate(bulletObjA, transform.position + Vector3.left * 0.35f, transform.rotation);
                GameObject bulletCC = Instantiate(bulletObjB, transform.position, transform.rotation);

                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();

                rigidRR.AddForce(Vector2.up * bullet_speed, ForceMode2D.Impulse);
                rigidLL.AddForce(Vector2.up * bullet_speed, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up * bullet_speed, ForceMode2D.Impulse);

                break;
        }
        

        curShotDelay = 0; // 한발 쏘고 나서 다시 장전해야 하니까 curShotDelay를 0으로 초기화
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    private void Move() // 각 행동들을 따로 함수를 만들어서 정리해두는 것이 좋다
    {
        /* 
         * GetAixs()와 GetAxisRaw()의 차이점 알아보기
         * GetAixs()는 실수의 값을 반환, but GetAxisRaw()는 -1, 0, 1 중 한 가지 값을 반환 
         */
        // 이동 방향을 입력받는 것
        float h = Input.GetAxisRaw("Horizontal"); // 수평
        if ((h == 1 && isTouchRight) || (isTouchLeft && h == -1)) h = 0;

        float v = Input.GetAxisRaw("Vertical"); // 수직
        if ((v == 1 && isTouchTop) || (isTouchBottom && v == -1)) v = 0;

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;


        if (Input.GetButtonDown("Horizontal") ||
            Input.GetButtonUp("Horizontal"))
        {
            anim.SetInteger("Input", (int)h);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}
