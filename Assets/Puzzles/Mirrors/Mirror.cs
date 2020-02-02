using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour {

    void OnMouseDrag() {
        Camera gameCam = GameObject.Find("LazerGameCamera").GetComponent<Camera>();

        Vector3 mouse_pos = Input.mousePosition;
        Vector3 object_pos = gameCam.WorldToScreenPoint(transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;

        float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        angle -= 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Start() {
    }

    void Update() {
        
    }
}
