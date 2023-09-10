using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Transform PlayerGeneral;

    private int CameraDirection = 0;
    [SerializeField] private int cameraSpeed = 5;
    private float cameraAcceleration = 0;

    [SerializeField] private float mapWidth;

    public GameObject FrontGround;
    public float FGSpeed;
    public GameObject CBG;
    public float CBGSpeed;
    public GameObject DBG;
    public float DBGSpeed;

    //public float magnitude;
    //public float duration;

    // Start is called before the first frame update
    void Start()
    {
        CameraDirection = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Could add a check if the player is actually in game to be able to move around

        CameraDirection = 0;

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space)) && PlayerGeneral!=null) {
            MoveCamera((PlayerGeneral.position.x - transform.position.x) * 3 * Time.deltaTime);
        }

        else if (Input.GetMouseButton(1)) {

            float MouseX = (Input.mousePosition.x - Screen.width / 2) / Screen.width;
            //Debug.Log("Mouse is at: " + MouseX);
            MoveCamera(MouseX * cameraSpeed * Time.deltaTime);
            
        }

        

        


        //I have three different ways to control the camera and dont like any of them

        else if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            CameraDirection = -1;
        }
        else if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.RightArrow))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            CameraDirection = 1;
        }

        if (CameraDirection != 0)
        {
            MoveCamera(CameraDirection * (cameraSpeed + cameraAcceleration) * Time.deltaTime);
           
            cameraAcceleration += (10f * Time.deltaTime); 
        }
        else {
            cameraAcceleration = 0;
        }
    }

    public void MoveCamera(float X) {

        if (Mathf.Abs(transform.position.x + X) > mapWidth / 2 && Mathf.Sign(transform.position.x) * Mathf.Sign(X) > 0)     return;

        transform.position += new Vector3(X, 0, 0);
        if (FrontGround != null)
        {
            FrontGround.transform.position += new Vector3((X * FGSpeed), 0, 0);
            CBG.transform.position += new Vector3((X * CBGSpeed), 0, 0);
            DBG.transform.position += new Vector3((X * DBGSpeed), 0, 0);
        }
    }

    public void CallShake(float magnitude, float duration) {
        StartCoroutine(ScreenShake(magnitude, duration));
    }
    IEnumerator ScreenShake(float magnitude, float duration) {
        
        //Vector3 changes = new Vector3(0, 0, 0);
        
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float change = magnitude - (magnitude * Mathf.Abs(duration / 2 - elapsed));
            float x = Random.Range(-1f, 1f) * change;
            float y = Random.Range(-1f, 1f) * change;

            transform.position += new Vector3(x, y, 0);

            //changes += new Vector3(x, y, 0);

            //Debug.Log("Moving to " + transform.position);
            elapsed += Time.deltaTime;
            
            yield return 0;
            transform.position -= new Vector3(x, y, 0);
        }
        //transform.position -= changes;

    }
}
