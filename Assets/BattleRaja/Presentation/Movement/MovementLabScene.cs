using UnityEngine;

namespace BattleRaja.Presentation.Movement
{
    public sealed class MovementLabScene : MonoBehaviour
    {
        [SerializeField] private MovementPlayerAgent player;
        [SerializeField] private TopDownCameraController cameraController;

        public MovementPlayerAgent Player => player;
        public TopDownCameraController CameraController => cameraController;
    }
}
