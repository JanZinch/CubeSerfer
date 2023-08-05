using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "mover_config", menuName = "Application/MoverConfig", order = 0)]
    public class MoverConfig : ScriptableObject
    {
        [field: SerializeField] public float ForwardSpeed { get; private set; } = 3.0f;
        [field: SerializeField] public float MaxHorizontalSpeed { get; private set; } = 6.0f;

        [field: SerializeField] public float LeftMovementConstraint { get; private set; } = -2.0f;
        [field: SerializeField] public float RightMovementConstraint { get; private set; } = 2.0f;
        
    }
}