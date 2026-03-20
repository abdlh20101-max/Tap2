using UnityEngine;

/// <summary>
/// BackgroundController: التحكم بالخلفية 3D المتحركة
/// توفر حركة هادئة وجذابة للخلفية
/// </summary>
public class BackgroundController : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeedX = 0.5f;
    [SerializeField] private float rotationSpeedY = 0.3f;
    [SerializeField] private float rotationSpeedZ = 0.2f;

    [Header("Scale Settings")]
    [SerializeField] private float scaleSpeed = 0.5f;
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;

    [Header("Color Settings")]
    [SerializeField] private bool enableColorAnimation = true;
    [SerializeField] private Color colorA = Color.cyan;
    [SerializeField] private Color colorB = Color.magenta;
    [SerializeField] private float colorSpeed = 1f;

    private float colorTimer = 0f;

    private void Update()
    {
        // تدوير الخلفية
        RotateBackground();

        // تغيير حجم الخلفية
        ScaleBackground();

        // تغيير لون الخلفية
        if (enableColorAnimation)
            AnimateColor();
    }

    private void RotateBackground()
    {
        transform.Rotate(
            rotationSpeedX * Time.deltaTime,
            rotationSpeedY * Time.deltaTime,
            rotationSpeedZ * Time.deltaTime
        );
    }

    private void ScaleBackground()
    {
        float scale = Mathf.Lerp(minScale, maxScale, 
            (Mathf.Sin(Time.time * scaleSpeed) + 1f) / 2f);
        
        transform.localScale = new Vector3(scale, scale, scale);
    }

    private void AnimateColor()
    {
        colorTimer += Time.deltaTime * colorSpeed;
        
        Color newColor = Color.Lerp(colorA, colorB, 
            (Mathf.Sin(colorTimer) + 1f) / 2f);
        
        // تطبيق اللون على جميع الـ Renderers
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.material.color = newColor;
        }
    }

    public void SetRotationSpeed(float x, float y, float z)
    {
        rotationSpeedX = x;
        rotationSpeedY = y;
        rotationSpeedZ = z;
    }

    public void SetScaleRange(float min, float max)
    {
        minScale = min;
        maxScale = max;
    }

    public void SetColors(Color a, Color b)
    {
        colorA = a;
        colorB = b;
    }
}
