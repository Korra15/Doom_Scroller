using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private enum LaserState
    {
        CoolDown,
        Charging,
        Firing 
    }
    SpriteRenderer spriteRenderer;

    //Laser state Logic
    public float coolDown;
    public float chargeTime;
    public float laserDuration;
    private LaserState state = LaserState.CoolDown;
    private float currentTime;
    public SpriteRenderer canonCharge;
    public GameObject laserCharge;
    public Vector3 laserStartSize;
    public Vector3 laserEndSize;
    public ParticleSystem chargeParticles;
    private float initialRadius = 0f;
    public float targetRadius = 2f;
    public Sprite[] sprites;

    //Laser shooting 
    public Transform firePoint;
    [SerializeField] private float defDistanceRay = 100;
    public LineRenderer lineRenderer;
    Transform laserTransfrom;

    //Player Reference
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log(player.transform.position);

        currentTime = coolDown;
        spriteRenderer = GetComponent<SpriteRenderer>();
        laserTransfrom = GetComponent<Transform>();

        initialRadius = chargeParticles.shape.radius;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        if(currentTime<=0)
        {
            StateChange();
        }
        if(state == LaserState.Firing)
        {
            FireLaser();
        }
    }

    void StateChange ()
    {
        switch (state)
        {
            case LaserState.CoolDown:
                //done cooling down;
                LaserCharge();
                chargeParticles.Play();
                currentTime = chargeTime;
                // spriteRenderer.sprite = sprites[1];
                spriteRenderer.color = Color.yellow;
                state = LaserState.Charging;
                break;
            case LaserState.Charging:
                //Done chargeing
                currentTime = laserDuration;
                spriteRenderer.color = Color.red;

                // spriteRenderer.sprite = sprites[2];
                state = LaserState.Firing;
                break;
            case LaserState.Firing:
                //Done shooting
                chargeParticles.Stop();
                currentTime = coolDown;
                spriteRenderer.color = Color.blue;
                Draw2DRay(transform.position, transform.position);
                //spriteRenderer.sprite = sprites[0];
                state = LaserState.CoolDown;
                break;
            default:
                break;
        }
    }

    public void FireLaser()
    {

        //if (gameObject.transform.position.x - player.transform.position.x < AudioManager.instance.playerLaserDistance)
        //{
        //    //AudioManager.instance.getSfXSource().loop = true;
        //    AudioManager.instance.PlaySFX(AudioManager.instance.getLoopSource(), AudioManager.instance.Laser);
        //}
        //else
        //{
        //    AudioManager.instance.StopSFX(AudioManager.instance.getLoopSource());
        //}

        if (Physics2D.Raycast(transform.position, transform.right))
        {
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, transform.right);
            Draw2DRay(laserTransfrom.position, _hit.point);
            if(_hit.transform.gameObject.CompareTag("Player"))
            {
                _hit.transform.gameObject.GetComponent<PlayerCollisionManager>().GotHit(5);
            }

        }
        else
        {
            //if nothing hit go as far as defult length
            Draw2DRay(laserTransfrom.position, firePoint.transform.right * defDistanceRay);

        }

        laserCharge.SetActive(false);
        initialRadius = 0f;
        
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

    }

    public void LaserCharge()
    {
        laserCharge.SetActive(true);
        // chargeParticles.Play();

        Transform laserChargeScale = laserCharge.GetComponent<Transform>();
        if (laserChargeScale.localScale.x < 0.5 || laserChargeScale.localScale.y < 0.5)
        {
            // laserChargeScale.localScale = Vector3.Lerp(laserStartSize, laserEndSize, chargeTime);
            laserChargeScale.localScale += new Vector3(1, 1, 0) * Time.deltaTime * 0.2f;
        }
        
        if (chargeParticles.shape.radius < targetRadius)
        {

            // Calculate the new radius based on linear interpolation
            float newRadius = Mathf.Lerp(initialRadius, targetRadius, chargeTime);

            // Update particle shape radius
            var shape = GetComponent<ParticleSystem>().shape;
            shape.radius = newRadius;
        }
    }
}
