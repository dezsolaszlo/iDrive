using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    public Rigidbody body;
    
    public float maxSpeed;

    public float forwardAcceleration = 8f, reverseAcceleration = 4f;
    private float speedInput;

    public float turnStrength = 180f;
    private float turnInput;

    private bool onGround;

    public Transform groundRayPoint, groundRayPoint1;
    public LayerMask whatIsGround;
    public float groundRayLength = 0.75f;

    public float dragOnGround;
    public float gravityMod = 10f;

    public Transform frontLeftWheel, frontRightWheel;
    public float maxWheelTurn = 25f;

    public ParticleSystem[] dustTrail;
    public float maxEmission = 25f, emissionFadeSpeed = 50f;
    public float emissionRate;

    public AudioSource engineSound;

    public AudioSource tireSqueal;
    public float soundFadeSpeed;

    private int nextCheckpoint = 0;
    public int currentLap = 0;

    public float lapTime, bestLapTime;

    public bool isAI;

    public int currentTarget;
    private Vector3 targetPoint;
    public float aiAccelerateSpeed = 1f, aiTurnSpeed = .8f, aiReachPointRange = 5f, aiPointVariance = 3f, aiMaxTurn = 15f;
    private float aiSpeedInput;

    // Start is called before the first frame update
    void Start()
    {
        body.transform.parent = null;

        dragOnGround = body.drag;

        tireSqueal.volume = 0f;

        if (isAI) {
            targetPoint = RaceManager.instance.allCheckpoints[currentTarget].transform.position;
            RandomiseAITarget();
        }

        UIManager.instance.lapCounterText.text = currentLap + "/" + RaceManager.instance.totalLaps;
    }

    // Update is called once per frame
    void Update()
    {
        lapTime += Time.deltaTime;

        if (!isAI)
        {

            var ts = System.TimeSpan.FromSeconds(lapTime);

            UIManager.instance.currentLapTimeText.text = string.Format("{0:00}m{1:00}.{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds);

            speedInput = 0f;
            if (Input.GetAxis("Vertical") > 0)
            {
                speedInput = Input.GetAxis("Vertical") * forwardAcceleration;
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                speedInput = Input.GetAxis("Vertical") * reverseAcceleration;
            }

            turnInput = Input.GetAxis("Horizontal");
            /*if (onGround && Input.GetAxis("Vertical") != 0)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f,turnInput*turnStrength*Time.deltaTime*Mathf.Sign(speedInput) * (body.velocity.magnitude / maxSpeed),0f)); // Quaternion.Euler() 3 ertekes koordinatat alakit 4 ertekesse
                //Time.deltaTime gordulekeny legyen minden gepen
                //Mathf.Sign = 1 ha a szam>0 es =-1 ha a szam<0
                //body.velocity.magnitude megadja a test sebesseget
            }*/

        }
        else { 
            targetPoint.y = transform.position.y;

            if (Vector3.Distance(transform.position, targetPoint) < aiReachPointRange)
            {
                currentTarget++;
                if (currentTarget >= RaceManager.instance.allCheckpoints.Length) {
                    currentTarget = 0;
                }

                targetPoint = RaceManager.instance.allCheckpoints[currentTarget].transform.position;
                RandomiseAITarget();
            }

            Vector3 targetDir = targetPoint - transform.position;
            float angle = Vector3.Angle(targetDir, transform.forward);

            Vector3 localPos = transform.InverseTransformPoint(targetPoint);

            if (localPos.x < 0f) {
                angle = -angle;
            }

            turnInput = Mathf.Clamp(angle / aiMaxTurn, -1f, 1f); //-1 es 1 kozott tartja az erteket

            aiSpeedInput = 1f;
            speedInput = aiSpeedInput * forwardAcceleration;
        }

        //kerekek forgatasa
        frontLeftWheel.localRotation = Quaternion.Euler(frontLeftWheel.localRotation.eulerAngles.x,(turnInput * maxWheelTurn) - 180, frontLeftWheel.localRotation.eulerAngles.z);
        frontRightWheel.localRotation = Quaternion.Euler(frontRightWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn), frontRightWheel.localRotation.eulerAngles.z);

        //transform.position = body.position;

        //particle emission's control
        emissionRate = Mathf.MoveTowards(emissionRate, 0f, emissionFadeSpeed * Time.deltaTime);

        if (onGround && (Mathf.Abs(turnInput)>.5f || (body.velocity.magnitude < 10f && body.velocity.magnitude !=0))){
            emissionRate = maxEmission;
        }

        if (Mathf.Abs(body.velocity.magnitude) < .5f) {
            emissionRate = 0f;
        }

        for (int i = 0; i < 4; i++) {
            var emission = dustTrail[i].emission;
            emission.rateOverTime = emissionRate;
        }

        if (engineSound != null) {
            engineSound.pitch = 1f + ((body.velocity.magnitude / maxSpeed)*1.8f);
        }

        if (tireSqueal != null) {
            if (Mathf.Abs(turnInput) > 0.5f)
            {
                tireSqueal.volume = 1f;
            }
            else {
                tireSqueal.volume = Mathf.MoveTowards(tireSqueal.volume, 0f, soundFadeSpeed * Time.deltaTime);
            }
        }
    }

    private void FixedUpdate()
    {
        onGround = false;

        RaycastHit hit;

        Vector3 normalTarget = Vector3.zero;

        //megnezi, hogy a foldon van-e a jarmu
        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            onGround = true;

            normalTarget = hit.normal;
        }

        if (Physics.Raycast(groundRayPoint1.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            onGround = true;

            normalTarget = (normalTarget + hit.normal)/2f;
        }

        //auto szoget allitja
        if (onGround)
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, normalTarget) * transform.rotation;  
        }

        //gyorsitja az autot
        if (onGround)
        {
            body.AddForce(transform.forward * speedInput * 1000f);

            body.drag = dragOnGround;
        }
        else
        {
            body.drag = .1f;

            body.AddForce(Vector3.up * -gravityMod * 100f);
        }
        //new Vector3(,,) 3 ertekes koordinata vektor
        //transform.forward megad egy Vector3()at ami megadja, hogy merre nez a kocsi orra

        if (body.velocity.magnitude > maxSpeed)
        {
            body.velocity = body.velocity.normalized * maxSpeed;

            //body.velocity.normalized=1
        }

        //Debug.Log(body.velocity.magnitude);

        transform.position = body.position;
        if (onGround && Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Mathf.Sign(speedInput) * (body.velocity.magnitude / maxSpeed), 0f)); // Quaternion.Euler() 3 ertekes koordinatat alakit 4 ertekesse
            //Time.deltaTime gordulekeny legyen minden gepen
            //Mathf.Sign = 1 ha a szam>0 es =-1 ha a szam<0
            //body.velocity.magnitude megadja a test sebesseget
        }

    }

    public void CheckpointHit(int cpNumber)
    {
        if (cpNumber == nextCheckpoint)
        {
            nextCheckpoint++;
            if (nextCheckpoint == RaceManager.instance.allCheckpoints.Length)
            {
                nextCheckpoint = 0;
                LapCompleted();
            }
        }
    }

    public void LapCompleted() {
        currentLap++;

        if (lapTime < bestLapTime || bestLapTime == 0) { 
            bestLapTime = lapTime;
        }

        lapTime = 0f;

        if (!isAI)
        {
            var ts = System.TimeSpan.FromSeconds(bestLapTime);

            UIManager.instance.bestLapTimeText.text = string.Format("{0:00}m{1:00}.{2:000}", ts.Minutes, ts.Seconds, ts.Milliseconds);


            UIManager.instance.lapCounterText.text = currentLap + "/" + RaceManager.instance.totalLaps;
        }
    }

    public void RandomiseAITarget()
    {
        targetPoint += new Vector3(Random.Range(-aiPointVariance, aiPointVariance),0f, Random.Range(-aiPointVariance, aiPointVariance));
    }
}
