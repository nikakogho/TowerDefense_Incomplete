using UnityEngine;

public class CameraController : MonoBehaviour {
    public float speed = 15;
    float speededBy = 1;

    public Vector3 min = new Vector3(0, 20, 0);
    Vector3 max;

    void Awake()
    {
        max = new Vector3(250, 300, 250);
    }

    void Update()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float upDown = 0;

        if (Input.GetKey(KeyCode.DownArrow)) upDown = 1;
        else if (Input.GetKey(KeyCode.UpArrow)) upDown = -1;

        speededBy = Input.GetKey(KeyCode.LeftShift) ? 4 : 1;

        if (h != 0 || v != 0) transform.position += new Vector3(h, upDown, v) * speed * Time.deltaTime * speededBy;

        transform.position = transform.position.Clamp(min, max);
    }
}
