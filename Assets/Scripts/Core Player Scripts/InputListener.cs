using UnityEngine;

namespace Assets.Scripts.Player_Character_System
{
    public class InputListener : MonoBehaviour
    {
        /* Input Values - for other scripts to read from */

        public float Mouse_X { get; private set; }
        public float Mouse_Y { get; private set; }

        public bool Pressing_Mouse_LeftClick { get; private set; }
        public bool PressedDown_Mouse_LeftClick { get; private set; }
        public bool PressedUp_Mouse_LeftClick { get; private set; }

        public bool Pressing_Mouse_RightClick { get; private set; }
        public bool PressedDown_Mouse_RightClick { get; private set; }
        public bool PressedUp_Mouse_RightClick { get; private set; }

        /* Listen and Update Input Values - once per physics frame (0, 1, or 2+ calls per render frame, depends on physis) */

        private void Update() //Trying out Update, but was using FixedUpdate ()
        {
            // Mouse Movement
            Mouse_X = Input.GetAxis("Mouse X");
            Mouse_Y = Input.GetAxis("Mouse Y");

            // Mouse Click Buttons
            Pressing_Mouse_LeftClick = Input.GetButton("Mouse-Left");
            PressedDown_Mouse_LeftClick = Input.GetButtonDown("Mouse-Left");
            PressedUp_Mouse_LeftClick = Input.GetButtonUp("Mouse-Left");

            Pressing_Mouse_RightClick = Input.GetButton("Mouse-Right");
            PressedDown_Mouse_RightClick = Input.GetButtonDown("Mouse-Right");
            PressedUp_Mouse_RightClick = Input.GetButtonUp("Mouse-Right");
        }
    }
}
