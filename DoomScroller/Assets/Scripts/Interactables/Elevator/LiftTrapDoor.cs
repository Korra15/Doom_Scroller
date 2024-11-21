using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftTrapDoor : MonoBehaviour
{
    public Transform trapDoor;
    public Transform gear;
    public float rotateSpeed = 3f;
    public float doorSpeed = 5f;

    void Update()
    {
            StartCoroutine(TrapDoor());
    }

    public IEnumerator TrapDoor()
    {
        gear.Rotate(0, 0, rotateSpeed * Time.deltaTime, Space.World);
        trapDoor.Translate(doorSpeed * Time.deltaTime, 0, 0, Space.World);
        yield return new WaitForSeconds(1f);
        rotateSpeed = 0f;
        doorSpeed = 0f;
    }
}
