using CustomClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerMovement : NetworkBehaviour
{

    [Header("Movement Properties")]
    [SerializeField] float _startSpeed = 1.0f;
    [SerializeField] float _maxSpeed;
    public float _currentSpeed = 8.0f;
    [SerializeField] float _timeToAchieveMaxSpeed;
    private Vector2 moveInputVector;
    [SerializeField] KeyCode _dodgeKey;

    [Header("Visuals")]
    public GameObject movementDirectionIndicator;
    public GameObject marker;

    [Header("Dodge Values")]
    public int _maxDodgeCharges = 2;
    public float _dodgeRechargeCooldown;
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float dodgeDuration;
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] private GameObject _rollingSmoke;
    private float _timeSinceLastDodge;
    public int _currentDodgeCharges;

    [Header("Mouse")]
    [SerializeField] private LayerMask layerMask;

    [Header("Script and Component References")]
    private Animator anim;
    private ITime iTime;
    private CreateFootPrint _footPrinter;
    Rigidbody rb;
    private Object_ID _id;

    [Header("Runtime Variables")]
    private float vel = 0;
    bool _sleepAnimFloat = false;
    bool _canMove = true;
    Vector3 moveDirection;
    float accelaration;
    
    [Header("Perk Values")]
    private float _perkSpeedModifier = 0;
    public bool _prowlerActive = false;
    public bool _trackSpeedUp = false;
    [HideInInspector] public float _trackSpeed;

    //READ ONLY
    public float Speed => _currentSpeed;
    public float MinSpeed => _startSpeed;
    public float MaxSpeed => _maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Collider>().enabled = false;
        Invoke("TempFix", 1);
        ComponentSetup();
        accelaration = (_maxSpeed - _startSpeed) / _timeToAchieveMaxSpeed;
        _currentSpeed = _startSpeed;
        _currentDodgeCharges = _maxDodgeCharges;
    }

    void TempFix()
    {
        transform.position += Vector3.up * 50;
        transform.GetComponent<Collider>().enabled = true;
    }

    void ComponentSetup()
    {
        _footPrinter = GetComponent<CreateFootPrint>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        iTime = GetComponent<ITime>();
        _id = GetComponent<Object_ID>();
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();
        //networkInputData.rotationInput = viewInputVector.x;
        networkInputData.movementInput = moveInputVector;

        return networkInputData;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!this.HasInputAuthority) return;
        RechargeDodge();
        float xMov = Input.GetAxis("Horizontal");
        float zMov = Input.GetAxis("Vertical");
        moveInputVector.x = xMov;
        moveInputVector.y = zMov;
        Vector3 mousePos = MousePosition();

        moveDirection = new Vector3(xMov, 0f, zMov).normalized;


        if (Input.GetKeyDown(_dodgeKey))
        {
            Dodge();
        }

        if (_canMove) transform.rotation = Quaternion.LookRotation(mousePos);
        if (moveDirection.magnitude != 0.0f && _canMove)
        {
            _currentSpeed += accelaration * Time.deltaTime;
            _currentSpeed = Mathf.Clamp(_currentSpeed, _startSpeed, _maxSpeed);
            _footPrinter.ready = true;
            movementDirectionIndicator.GetComponent<Animator>().SetBool("On", true);
            movementDirectionIndicator.transform.rotation = Quaternion.LookRotation(moveDirection);
            CalculateAnimatorMovementValue(moveDirection);

            
        }
        else
        {
            _currentSpeed = _startSpeed;
            _footPrinter.ready = false;
            movementDirectionIndicator.GetComponent<Animator>().SetBool("On", false);
            //anim.SetBool("Moving", false);
            float angle = Mathf.SmoothDamp(anim.GetFloat("MovBlend"), 1000, ref vel, 1f);
            anim.SetFloat("MovBlend", 1000);
        }      
    }
    public override void FixedUpdateNetwork()
    {
        //if (!this.HasStateAuthority) return;
        if (GetInput(out NetworkInputData networkInputData))
        {
            //Vector3 moveDirection = transform.forward * networkInputData.movementInput.y + transform.right * networkInputData.movementInput.x;
            Vector3 moveDirection = new Vector3(networkInputData.movementInput.x, 0, networkInputData.movementInput.y);
            moveDirection.Normalize();

            if (moveDirection.magnitude != 0.0f && _canMove)
            {
                rb.MovePosition(transform.position + (1 + _perkSpeedModifier) * _currentSpeed * iTime.personalTimeScale * Runner.DeltaTime * moveDirection);
                //anim.SetBool("Moving", true);  
            }

            //Animation

            float zVelocity = Vector3.Dot(moveDirection, transform.forward);
            float xVelocity = Vector3.Dot(moveDirection, transform.right);
            float movSpeedMult = _currentSpeed / _maxSpeed;

            if (!_sleepAnimFloat)
            {
                anim.SetFloat("zMov", zVelocity, 0.1f, Runner.DeltaTime);
                anim.SetFloat("xMov", xVelocity, 0.1f, Runner.DeltaTime);
                anim.SetFloat("movSpeedMult", movSpeedMult);
            }

        }
    }

    private void RechargeDodge()
    {
        if (_currentDodgeCharges == _maxDodgeCharges) return;

        _timeSinceLastDodge += Time.deltaTime;

        if(_timeSinceLastDodge >= _dodgeRechargeCooldown) 
        {
            _currentDodgeCharges++;
            _timeSinceLastDodge = 0;
        }
    }

    public void Dodge()
    {
        if (_currentDodgeCharges <= 0) return;
        anim.Play("Rollin Blend Tree");
        _timeSinceLastDodge = 0;
        _currentDodgeCharges--;
        StartCoroutine(DodgeMove());
    }
    IEnumerator DodgeMove()
    {
        _sleepAnimFloat = true;
        _canMove = false;
        //_rollingSmoke?.SetActive(true);
        float startTime = Time.time;
        while (Time.time < startTime + dodgeDuration)
        {
            rb.velocity = (speedCurve.Evaluate(Time.time - startTime) / dodgeDuration) 
                * dodgeSpeed * iTime.personalTimeScale * moveDirection;

            yield return null;
        }
        _sleepAnimFloat = false;
        _canMove = true;
        //_rollingSmoke?.SetActive(false);
    }

    public Vector3 MousePosition()
    {
        Camera camera = Camera.main;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            return new Vector3(hit.point.x - transform.position.x, 0, hit.point.z - transform.position.z);

        }
        else return Vector3.zero;
    }

    private void CalculateAnimatorMovementValue(Vector3 moveDir)
    {
        float value = /*Mathf.Sin(*/Vector3.Angle(transform.forward, moveDir)/*)*/;
        if (moveDir.x > 0) value += 180;
        anim.SetFloat("MovBlend", value);
    }


    #region Perk Effects On Movement
    public void PerkModifySpeed(float _multiplier, bool add = true)
    {
        _perkSpeedModifier += _multiplier;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        if (other.gameObject.layer != LayerMask.NameToLayer("TrackerPrint") || other.GetComponent<Object_ID>().my_ID == _id.my_ID) return;
        Debug.Log("On Tracks");
        PerkModifySpeed(_trackSpeed);
        _trackSpeedUp = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("TrackerPrint") || other.GetComponent<Object_ID>().my_ID == _id.my_ID) return;
        Debug.Log("Off Tracks");
        PerkModifySpeed(-_trackSpeed);
        _trackSpeedUp = false;
    }


    #endregion

}













//using CustomClasses;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class QuickMove : MonoBehaviour
//{
//    public float speed;
//    public GameObject movementDirectionIndicator;
//    private Animator anim;
//    public GameObject bullet;
//    public GameObject muzzleFlash;
//    public Transform firePoint;
//    public GameObject mouseTracker;
//    private float vel = 0;
//    private ITime iTime;

//    bool _sleepAnimFloat = false;
//    bool _canMove = true;
//    Rigidbody rb;
//    [SerializeField] private LayerMask layerMask;

//    [Header("Dodge Values")]
//    [SerializeField] private float dodgeSpeed;
//    [SerializeField] private float dodgeDuration;
//    [SerializeField] private AnimationCurve speedCurve;

//    // Start is called before the first frame update
//    void Start()
//    {
//        anim = GetComponent<Animator>();
//        rb = GetComponent<Rigidbody>();
//        iTime = GetComponent<ITime>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        float xMov = Input.GetAxis("Horizontal");
//        float zMov = Input.GetAxis("Vertical");
//        Vector3 mousePos = MousePosition();

//        Vector3 moveDirection = new Vector3(xMov, 0f, zMov).normalized;


//        //Animation

//        float zVelocity = Vector3.Dot(moveDirection, transform.forward);
//        float xVelocity = Vector3.Dot(moveDirection, transform.right);


//        if (!_sleepAnimFloat)
//        {
//            anim.SetFloat("zMov", zVelocity, 0.1f, Time.deltaTime);
//            anim.SetFloat("xMov", xVelocity, 0.1f, Time.deltaTime);
//        }



//        if (moveDirection.magnitude != 0.0f && _canMove)
//        {
//            movementDirectionIndicator.GetComponent<Animator>().SetBool("On", true);
//            movementDirectionIndicator.transform.rotation = Quaternion.LookRotation(moveDirection);
//            transform.position += moveDirection * speed * iTime.personalTimeScale * Time.deltaTime;
//            //anim.SetBool("Moving", true);

//            if (Input.GetKeyDown(KeyCode.LeftShift))
//            {
//                anim.Play("Rollin Blend Tree");
//                Dodge();
//            }
//            CalculateAnimatorMovementValue(moveDirection);
//        }
//        else
//        {
//            movementDirectionIndicator.GetComponent<Animator>().SetBool("On", false);
//            //anim.SetBool("Moving", false);
//            float angle = Mathf.SmoothDamp(anim.GetFloat("MovBlend"), 1000, ref vel, 1f);
//            anim.SetFloat("MovBlend", 1000);

//        }
//        if (_canMove) transform.rotation = Quaternion.LookRotation(mousePos);

//    }

//    public void Dodge()
//    {
//        StartCoroutine(DodgeMove());
//    }
//    IEnumerator DodgeMove()
//    {
//        _sleepAnimFloat = true;
//        _canMove = false;
//        float startTime = Time.time;
//        while (Time.time < startTime + dodgeDuration)
//        {
//            transform.position += movementDirectionIndicator.transform.forward * dodgeSpeed * iTime.personalTimeScale
//                * (speedCurve.Evaluate(Time.time - startTime) / dodgeDuration)
//                * Time.deltaTime;

//            yield return null;
//        }
//        _sleepAnimFloat = false;
//        _canMove = true;
//    }

//    private Vector3 MousePosition()
//    {
//        Camera camera = Camera.main;
//        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
//        RaycastHit hit;

//        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
//        {
//            mouseTracker.transform.position = hit.point;
//            return new Vector3(hit.point.x - transform.position.x, 0, hit.point.z - transform.position.z);

//        }
//        else return Vector3.zero;
//    }

//    private void CalculateAnimatorMovementValue(Vector3 moveDir)
//    {
//        float value = /*Mathf.Sin(*/Vector3.Angle(transform.forward, moveDir)/*)*/;
//        if (moveDir.x > 0) value += 180;
//        anim.SetFloat("MovBlend", value);
//    }
//}


