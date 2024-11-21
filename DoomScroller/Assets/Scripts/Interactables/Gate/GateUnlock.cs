using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateUnlock : MonoBehaviour
{
    public GameObject gate;
    public Transform gear;
    public Transform handle;
    public PlayerStateManager playerState;
    public SaveManager save;
    public UserInput input;
    public bool canUnlock = false;
    public bool isOpening = false;
    public float gateSpeed = 1f;
    public float gearRotateSpeed = 20f;
    public float handleRotateSpeed = 30f;
    public float maxGateLift;

    // Update is called once per frame
    void Start()
    {
        if (playerState == null)
        {
            playerState = GameObject.Find("Player").GetComponent<PlayerStateManager>();
        }
        if (save == null)
        {
            save = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        }
        if (input == null)
        {
            input = GameObject.Find("InputManager").GetComponent<UserInput>();
        }
    }
    void Update()
    {
        if (playerState.spawn_gate) Destroy(gate);
        if (canUnlock && input.Interact && !playerState.spawn_gate)
            StartCoroutine(OpenGate());
        if (isOpening)
            StartCoroutine(RotateGear());
        if (playerState.spawn_gate) handle.localRotation = Quaternion.Euler(0, 0, 90f);
    }
    
    public IEnumerator OpenGate()
    {
        if (gate.GetComponent<Transform>().position.y < maxGateLift)
        {
            gate.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, gateSpeed);
        }

        isOpening = true;

        yield return new WaitForSeconds(10f);
        isOpening = false;
        playerState.spawn_gate = true;
        save.Save();
        Destroy(gate);
    }

    public IEnumerator RotateGear()
    {
        gear.Rotate(0, 0, gearRotateSpeed * Time.deltaTime, Space.World);

        if (handle.rotation.eulerAngles.z <= 90f)
        {
            handle.Rotate(0, 0, handleRotateSpeed * Time.deltaTime, Space.World);
        }
        

        yield return new WaitForSeconds(7f);
        gearRotateSpeed = 0f;
        handleRotateSpeed = 0f;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            canUnlock = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            canUnlock = false;
    }
}
