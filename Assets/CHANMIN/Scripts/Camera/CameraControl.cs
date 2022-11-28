using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float TouchSensitivity_x = 10f;
    public float TouchSensitivity_y = 10f;
    public CinemachineFreeLook vCam;

    [Range(10f,20f)] 
    public float zoomRange;

    public virtual void Start()
    {
        CinemachineCore.GetInputAxis = this.DragCameraControl;
        vCam = GetComponent<CinemachineFreeLook>();
        vCam.m_CommonLens = true;
    }

    private void Update()
    {
        Zoom();
    }

    public virtual float DragCameraControl(string axisName)
    {
        switch (axisName)
        {
            case "Mouse X":
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    return Input.GetAxis("Mouse X");
                }

                else if (Input.GetAxis("Horizontal") != 0)
                    return Input.GetAxis("Horizontal") * 0.2f;
                break;

            case "Mouse Y":
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    return Input.GetAxis(axisName);
                }

                break;
            default:
                break;
        }

        return 0f;
    }

    public virtual void Zoom()
    {
        float y = Input.GetAxis("Mouse ScrollWheel") * -1 * 10f;

        if (y == 0) return;

       vCam.m_Lens.FieldOfView += y;
       vCam.m_Lens.FieldOfView = Mathf.Clamp(vCam.m_Lens.FieldOfView, 10f, 70f);
    }
}
