using UnityEngine;

public class MultiDisplayController : MonoBehaviour
{
    [SerializeField]
    Camera left, middle, right;

    void Awake()
    {
        var availableDisplays = Display.displays;
        for (int i = 1; i < availableDisplays.Length; i++)
        {
            availableDisplays[i].Activate();
        }
    }

    void Start()
    {
        left.targetDisplay = 0;
        middle.targetDisplay = 1;
        right.targetDisplay = 2;

        var horizontalFOV = Mathf.Rad2Deg * 2 * Mathf.Atan(Mathf.Tan(middle.fieldOfView * Mathf.Deg2Rad / 2) * middle.aspect);
        print(horizontalFOV);

        left.transform.localEulerAngles = Vector3.up * horizontalFOV;
        right.transform.localEulerAngles = Vector3.up * -horizontalFOV;

        left.fieldOfView = right.fieldOfView = middle.fieldOfView;


    }
}
