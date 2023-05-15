using UnityEngine;

[CreateAssetMenu(fileName = "TrailConfig", menuName = "Guns/Gun Trail Configuration", order = 4)]
public class TrailConfigScriptableObject : ScriptableObject
{
    // How do we show the bullet trail/bullet tracer
    public Material Material;
    public AnimationCurve WidthCurve;
    public float Duration = 0.5f;
    public float MinVertexDistance = 0.1f;
    public Gradient Color;

    public float MissDistance = 100f;
    public float SimulationSpeed = 100f;
    
}
