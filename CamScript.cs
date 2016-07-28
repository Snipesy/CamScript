using UnityEngine;
using System.Collections;

public class CamScript : MonoBehaviour
{
    // The focus of the camera, used for translational
    private Vector3 focusPosition;
    private Quaternion focusRotation;

    public Transform trackTarget;

    // The rotational focus of the camera
    private Vector3 rotationFocus;

    public float maxOrbitDistance = 3;
    public float minOrbitDistance = 2;
    public float orbitDistance = 3;
    public float angle = 53;
    public float trackingSpeed = 5;

    [HideInInspector]
    public int selection = 2;

    // Use this for initialization
    void Start()
    {
        focusRotation = Quaternion.Euler(angle, 0,0);
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.O))
        {
            if (selection == 1)
                selection = 2;
            else if (selection == 2)
                selection = 1;
        }
        switch (selection)
        {
            // Track a target
            case 0:
                Destroy(gameObject);
                break;
            case 1:
                

                if (trackTarget != null)
                {

                    float calc = Vector3.Distance(focusPosition, trackTarget.position);



                    focusPosition = Vector3.Lerp(focusPosition, trackTarget.position,
                        Time.deltaTime * (trackingSpeed * calc / orbitDistance));
                    rotationFocus = focusPosition;

                    focusRotation = Quaternion.Lerp(focusRotation, Quaternion.Euler(angle,
                        focusRotation.eulerAngles.y, 0), calc * Time.deltaTime * (trackingSpeed));
                }
                break;
            // Track a target and match its rotation
            case 2:
                if (trackTarget != null)
                {

                    float calc = Vector3.Distance(focusPosition, trackTarget.position);



                    focusPosition = Vector3.Lerp(focusPosition, trackTarget.position,
                        Time.deltaTime * (trackingSpeed * calc / orbitDistance));
                    rotationFocus = focusPosition;

                    focusRotation = Quaternion.Lerp(focusRotation, Quaternion.Euler(angle, 
                        trackTarget.rotation.eulerAngles.y, 0), calc * Time.deltaTime * (trackingSpeed));

                }

                break;
            default:
                
                break;
        }

        var a = Input.GetAxis("Mouse ScrollWheel");
        if (a != 0)
        {
            orbitDistance += a * 20f;
            if (orbitDistance < minOrbitDistance)
                orbitDistance = minOrbitDistance;
            if (orbitDistance > maxOrbitDistance)
                orbitDistance = maxOrbitDistance;
        }

        TransUpdate();
        LookAt();
        if (selection == 1)
            Orbit();


    }

    private void TransUpdate()
    {
        // Unit vector of focus
        if (orbitDistance <= 0)
            return;
        Vector3 offset = -(focusRotation * Vector3.forward) * orbitDistance;

        // Update camera position
        gameObject.transform.position = focusPosition + offset;
    }

    private void LookAt()
    {
        transform.LookAt(rotationFocus);
    }

    // Orbits camera by modifying focusRotation
    private void Orbit()
    {
   
        if (Input.GetKey(KeyCode.Q))
            gameObject.transform.RotateAround(focusPosition, Vector3.up, -100f * Time.deltaTime);
        if (Input.GetKey(KeyCode.E))
            gameObject.transform.RotateAround(focusPosition, Vector3.up, 100f * Time.deltaTime);
        focusRotation = gameObject.transform.rotation;
    }
#if UNITY_EDITOR
    void OnGUI()
    {
        GUILayout.Label("Current Camera Mode: " + selection);
        GUILayout.Label("Current Camera Theta: " + angle);
        GUILayout.Label("Current Camera Distance: " + orbitDistance);
    }

#endif
}
