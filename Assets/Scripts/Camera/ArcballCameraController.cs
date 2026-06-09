using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArcballCameraController : MonoBehaviour
{
    [SerializeField] private RobotController robotController;

    private Vector3 focus;
    private float yaw;
    private float pitch;
    private float distance;
    private Tween moveTween;
    private Tween rotateTween;
    private bool initialized;

    private void OnEnable()
    {
        robotController.RobotChanged += SetFocus;
    }

    private void Start()
    {
        SetFocus(robotController.Robot);
        Vector3 angles = Quaternion.LookRotation(focus - transform.position).eulerAngles;
        yaw = angles.y;
        pitch = Mathf.Clamp(angles.x > 180f ? angles.x - 360f : angles.x, -20f, 80f);
        initialized = true;
        Apply();
    }

    private void OnDisable()
    {
        robotController.RobotChanged -= SetFocus;
        moveTween?.Kill();
        rotateTween?.Kill();
    }

    private void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null || (!mouse.leftButton.isPressed && !mouse.rightButton.isPressed))
        {
            return;
        }

        Vector2 delta = mouse.delta.ReadValue();
        moveTween?.Kill();
        rotateTween?.Kill();
        yaw += delta.x * 0.3f;
        pitch = Mathf.Clamp(pitch - delta.y * 0.3f, -20f, 80f);
        Apply();
    }

    private void SetFocus(RobotBuild robot)
    {
        float height = robot.Legs.Height + robot.Torso.Height + robot.Head.Height;
        focus = new Vector3(0f, height * 0.6f, 0f);
        distance = Mathf.Lerp(3.5f, 5f, Mathf.InverseLerp(1.75f, 2.55f, height));
        if (initialized)
        {
            Apply(0.4f);
        }
    }

    private void Apply(float duration = 0f)
    {
        Vector3 position = focus + Quaternion.Euler(pitch, yaw, 0f) * Vector3.back * distance;
        Quaternion rotation = Quaternion.LookRotation(focus - position);
        if (duration == 0f)
        {
            transform.SetPositionAndRotation(position, rotation);
            return;
        }
        
        moveTween?.Kill();
        rotateTween?.Kill();
        moveTween = transform.DOMove(position, duration).SetEase(Ease.OutQuad);
        rotateTween = transform.DORotateQuaternion(rotation, duration).SetEase(Ease.OutQuad);
    }
}
