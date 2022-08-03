using UnityEngine;

public class Drone : MonoBehaviour
{
    [Header("Drone Settings")]
    [SerializeField]
    protected float maxSpeed = 40;
    [SerializeField]
    protected float minSpeed = 0;
    [SerializeField]
    protected float defaultSpeed = 20;
    protected float speed;
    [SerializeField]
    protected float startLocationX = 0;
    [SerializeField]
    protected float startLocationY = 0;
    [SerializeField]
    protected float startLocationZ = 0;


    [Header("Accel/Rotate Values")]
    [SerializeField]
    protected float accelerateAmount = 15.0f;
    [SerializeField]
    protected float turningForce = 1.5f;


    [Header("Z Rotate Values")]
    [SerializeField]
    protected float zRotateMaxThreshold = 0.3f;
    [SerializeField]
    protected float zRotateAmount = 135;
    [SerializeField]
    protected float zRotateLerpAmount = 1.5f;


    [Header("Collider")]
    [SerializeField]
    protected Main main;
    protected BoxCollider areaCollider;


    // spherical coordinate
    float radius;
    float azimuth;
    float elevation;


    // 3차원 벡터를 입력받아 해당 위치로 이동
    // 입력값이 없을 경우 startLocation으로 이동
    protected void SetLocation(Vector3? location = null)
    {
        if (location == null) transform.position = new Vector3(startLocationX, startLocationY, startLocationZ);

        else transform.position = (Vector3)location;
    }

    // 3차원 벡터를 구 좌표로 변환하여 udp로 전송
    void SendSphericalCoordinate()
    {
        var pos = transform.localPosition;
        radius = pos.magnitude;
        azimuth = Mathf.Atan2(pos.z, pos.x) * Mathf.Rad2Deg;
        elevation = Mathf.Acos(pos.y / radius) * Mathf.Rad2Deg;

        string name = gameObject.name;

        main.SendString(name + ": " + radius + ", " + azimuth + ", " + elevation);
    }

    protected virtual void Start()
    {
        speed = defaultSpeed;
        main = GameObject.Find("Area").GetComponent<Main>();
        areaCollider = main.areaCollider;

        if (main.isSend) InvokeRepeating("SendSphericalCoordinate", 0.0f, main.udpPeriod);
    }
}
