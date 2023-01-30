using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public int cameraSpeed = 5;
    private bool border = false;

    
    // Start is called before the first frame update
    void Start()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector3(width, height, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0) {
            if (!border || (transform.position.x * Input.GetAxisRaw("Horizontal") < 0)){ 
                transform.position += new Vector3(Input.GetAxisRaw("Horizontal") * cameraSpeed * Time.deltaTime, 0, 0);
                border = false;
            }
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
