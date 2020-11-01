using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAndBowScripts : MonoBehaviour
{
    private Rigidbody myBody;
    [SerializeField]
    public float speed = 30.0f;
    [SerializeField]
    public float deactivate_Timer = 3.0f;
    [SerializeField]
    public float damage = 15f;

    void Awake()
    {
        myBody = GetComponent<Rigidbody>();

    }
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeactivateGameObject", deactivate_Timer); //每三秒调用一次
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Launch(Camera mainCamera)
    {
        myBody.velocity = mainCamera.transform.forward * speed;
        transform.LookAt(transform.position + myBody.velocity);

    }
    void DeactivateGameObject()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }


    }
    void OnTriggerEnter(Collider target)
    {
        //当这个箭遇到一个物体 target 的时候 会调用这个函数
    }
}
