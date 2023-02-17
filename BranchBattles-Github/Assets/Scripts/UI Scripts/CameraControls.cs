using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public int cameraSpeed = 5;
    private bool border = false;
    public float cameraAcceleration = 0;

    public float mapWidth;

    public GameObject FrontGround;
    public float FGSpeed;
    public GameObject CBG;
    public float CBGSpeed;
    public GameObject DBG;
    public float DBGSpeed;

    // Start is called before the first frame update
    void Start()
    {
        GameObject mapBorder = new GameObject();
        mapBorder.name = "Map Borders";

        BoxCollider2D colliderL = mapBorder.AddComponent<BoxCollider2D>();
        colliderL.size = new Vector2(1, 10);
        colliderL.offset = new Vector2(mapWidth / -2, 0);
        colliderL.isTrigger = true;
        colliderL.gameObject.tag = "Border";

        BoxCollider2D colliderR = mapBorder.AddComponent<BoxCollider2D>();
        colliderR.size = new Vector2(1, 10);
        colliderR.offset = new Vector2(mapWidth / 2, 0);
        colliderR.isTrigger = true;
        colliderR.gameObject.tag = "Border";


        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector3(width, height, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (!border || (transform.position.x * Input.GetAxisRaw("Horizontal") < 0))
            {
                transform.position += new Vector3(Input.GetAxisRaw("Horizontal") * (cameraSpeed + cameraAcceleration) * Time.deltaTime, 0, 0);
                if (FrontGround != null) {
                    FrontGround.transform.position += new Vector3(Input.GetAxisRaw("Horizontal") * ((cameraSpeed + cameraAcceleration) * FGSpeed) * Time.deltaTime, 0, 0);
                    CBG.transform.position += new Vector3(Input.GetAxisRaw("Horizontal") * ((cameraSpeed + cameraAcceleration) * CBGSpeed) * Time.deltaTime, 0, 0);
                    DBG.transform.position += new Vector3(Input.GetAxisRaw("Horizontal") * ((cameraSpeed + cameraAcceleration) * DBGSpeed) * Time.deltaTime, 0, 0);
                }
                

                border = false;
            }
            cameraAcceleration += (10f * Time.deltaTime); 
        }
        else {
            cameraAcceleration = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Border")) {
            border = true;
            //Debug.Log("Touched border");
        }
    }
}
