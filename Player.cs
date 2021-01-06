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
    public float power = 1; // �Ŀ��� ���̸� �� ū �Ѿ��� ������
    public float maxShotDelay; // ���� ������ �ð�
    public float curShotDelay; // �ѹ� ��� �� ���� �����Ǵ� �ð�

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
        Reload();// ���� �Լ�
    }

    /*
     * ���� ������Ʈ ��ȯ Instantiate(������, ���� ��ġ, ������ ������Ʈ�� ����);
     * AddForce();
     * Collider�� IsTrigger�� ���� �� ã�ƺ��� �����ϱ�
     * GetButton,Down,Up�� ���ؼ� �����صα�
     */
    private void Fire()
    {
        if (!Input.GetButton("Fire1"))
            return;

        if (curShotDelay < maxShotDelay) // ���� �ð��� ������ �ð��� �Ǿ��� �� �Ѿ��� ������!!
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
                GameObject bulletRR = Instantiate(bulletObjA, transform.position + Vector3.right * 0.35f, transform.rotation); // ��� ū �Ѿ˰� �浹�� �� �����Ƿ� 0.1->0.35�� �ø�
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
        

        curShotDelay = 0; // �ѹ� ��� ���� �ٽ� �����ؾ� �ϴϱ� curShotDelay�� 0���� �ʱ�ȭ
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    private void Move() // �� �ൿ���� ���� �Լ��� ���� �����صδ� ���� ����
    {
        /* 
         * GetAixs()�� GetAxisRaw()�� ������ �˾ƺ���
         * GetAixs()�� �Ǽ��� ���� ��ȯ, but GetAxisRaw()�� -1, 0, 1 �� �� ���� ���� ��ȯ 
         */
        // �̵� ������ �Է¹޴� ��
        float h = Input.GetAxisRaw("Horizontal"); // ����
        if ((h == 1 && isTouchRight) || (isTouchLeft && h == -1)) h = 0;

        float v = Input.GetAxisRaw("Vertical"); // ����
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
