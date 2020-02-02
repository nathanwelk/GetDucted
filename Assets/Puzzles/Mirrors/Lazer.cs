using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour {

    [SerializeField] int maxLazerReflections = 10;

    LineRenderer lr;
    LazerGame miniGame;

    void reflectLoop(List<Vector3> positions) {
        RaycastHit2D hit;

        Vector3 rayStart = transform.position;
        Vector3 rayDirection = transform.TransformDirection(Vector3.up);

        for (int i = 0; i < maxLazerReflections; i++) {

            hit = Physics2D.Raycast(rayStart, rayDirection, 9999, 1 << LayerMask.NameToLayer("LazerGame"));

            if (hit) {
                Vector3 pnt = new Vector3(hit.point.x, hit.point.y, rayStart.z);

                positions.Add(pnt);

                GameObject hitObject = hit.transform.gameObject;

                if (hitObject.tag == "Mirror") {
                    // set start point and end point 
                    rayStart = pnt;
                    rayDirection = Vector3.Reflect(rayDirection, hit.normal);
                } else if(hitObject.tag == "Reciever") {
                    // end minigame
                    miniGame.isComplete = true;
                    return;
                } else {
                    // stop reflecting
                    return;
                }

            } else {
                positions.Add(rayStart + rayDirection * 20);
                return;
            }
        }
    }

    void drawLazer() {
        List<Vector3> pos = new List<Vector3>();
        pos.Add(new Vector3(transform.position.x, transform.position.y, transform.position.z));

        reflectLoop(pos);

        lr.positionCount = pos.Count;
        lr.SetPositions(pos.ToArray());
    }

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
        lr = GameObject.Find("LazerLine").GetComponent<LineRenderer>();
        miniGame = transform.parent.gameObject.GetComponent<LazerGame>();
    }

    void Update() {
        drawLazer();
    }
}
