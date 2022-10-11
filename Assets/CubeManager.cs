using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeManager : MonoBehaviour
{
    public float speed;
    Rigidbody rb;
    Vector3 startpos, endpos;
    float mag;
    private Vector3 final;
    private float length = 0;
    private bool isTouch = false;

    public GameObject pointPrefab;
    public GameObject[] points;
    public int pointsNumber;
    public float maxforce;
    public Text score;
    public CubeGeneration cubegenerator;
    private bool isGrounded=true;
    public Transform dieLine;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        points = new GameObject[pointsNumber];
        for (int i = 0; i < pointsNumber; i++)
        {
            points[i] = Instantiate(pointPrefab, transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isGrounded)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                final = Vector3.zero;
                length = 0;
                isTouch = false;
                Vector2 touchDeltaPosition = Input.GetTouch(0).position;
                startpos = new Vector3(touchDeltaPosition.x, touchDeltaPosition.y, 0);


            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                isTouch = true;
                Vector2 touchPosition = Input.GetTouch(0).position;
                endpos = new Vector3(touchPosition.x, touchPosition.y, 0);
                final = (endpos - startpos).normalized;
                var vt = new Vector3(-final.x, -final.y*2, 0) * speed*1.5f;
                for (int i = 0; i < pointsNumber; i++)
                {
                    points[i].transform.position = PointsPosition(vt, i * .2f);
                }
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Canceled)
            {
                isTouch = false;
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                isTouch = false;
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (isTouch)
                {
                    Vector2 touchPosition = Input.GetTouch(0).position;
                    endpos = new Vector3(touchPosition.x, touchPosition.y, 0);
                    final = endpos - startpos;
                    length = final.magnitude;
                    if (length > maxforce)
                        final *= (maxforce / length);
                    rb.AddForce((new Vector3(-final.x, -final.y * 2, 0)) * speed * Time.fixedDeltaTime, ForceMode.Impulse);
                }
            }
        }
    }

    Vector2 PointsPosition(Vector3 vt, float t)
    {
        Vector2 current = transform.position + vt * speed * t +  0.5f * Physics.gravity * t * t;
        return current;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "cube")
        {
            GameManager.instance.score += 100;
            score.text = "Scoer: "+GameManager.instance.score.ToString();
            cubegenerator.NextCube();
            isGrounded = true;
            dieLine.transform.position = new Vector3(transform.position.x, 0, 0);

        }
        else if (collision.gameObject.tag == "dead")
        {
            GameManager.instance.gameOver.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "cube")
        {
            isGrounded = false;
        }
    }
}   
