using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraHandler : MonoBehaviour
{
    //Camera Variables
    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;
    private Transform mTransform;
    private Vector3 cameraTransfromPosition;
    private LayerMask ignoreLayers;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    public static CameraHandler singleton;

    public float lookSpeed = 0.1f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;

    private float targetPosition;
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;
    [SerializeField] float minimumPivot = -35;
    [SerializeField] float maximumPivot = 35;

    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffSet = 0.2f;
    public float minimumCollisionOffset = 0.2f;


    private void Awake()
    {
        singleton = this;
        mTransform = transform;
        defaultPosition = cameraTransform.localPosition.z;
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        targetTransform = FindObjectOfType<PlayerManager>().transform;
    }
    private void Start()
    {

    }

    private void Update()
    {

    }

    public void OnMouseMove(InputAction.CallbackContext ctx)
    {
        Vector2 mouseVector = ctx.ReadValue<Vector2>();
        //mouseX = mouseVector.x;
        //mouseY = mouseVector.y;
    }

    private void LateUpdate()
    {
    }

    
    //Follow player
    public void FollowTarget(float delta)
    {
        Vector3 targetPosition = Vector3.SmoothDamp(mTransform.position, targetTransform.position,ref cameraFollowVelocity, delta / followSpeed);
        mTransform.position = targetPosition;
        HandlerCameraCollisions(delta);
    } 

    public void HandleCameraRotation(float delta, float mouseXInput,float mouseYInput)
    {
        lookAngle += (mouseXInput * lookSpeed)  * delta;
        pivotAngle -= (mouseYInput * pivotSpeed) * delta;
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        mTransform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivotTransform.localRotation= targetRotation;
    }
    private void HandlerCameraCollisions(float delta)
    {
        targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        direction.Normalize();

        if(Physics.SphereCast(cameraPivotTransform.position,cameraSphereRadius,direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
        {
            float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetPosition = -(dis - cameraCollisionOffSet);
        }

        if(Mathf.Abs(targetPosition)< minimumCollisionOffset)
        {
            targetPosition = -minimumCollisionOffset;
        }

        cameraTransfromPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
        cameraTransform.localPosition = cameraTransfromPosition;

 
    }
    //Rotate with mouse input

    //Find possible enemy targets

    //Camera collisions

   /* private void HandleCameraCollisions(Vector3 targetPosition)
    {

        RaycastHit hit;
        Vector3 direction = cameraObject.transform.position - playerCameraPivot.transform.position;
        direction.Normalize();

        if (Physics.SphereCast(playerCameraPivot.transform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(defaultPosition), ignoreLayers)){
        }

    }*/

    private void OnDrawGizmos()
    {
        

    }
}
