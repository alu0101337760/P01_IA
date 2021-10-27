using UnityEngine;

namespace IA_sim 
{
    public class CameraController : MonoBehaviour
    {
        public float panSpeed = 20f;
        public float panBorderThickness = 10f;
        public float scrollSpeed = 40f;
        public float minY = 10f;

        void Update()
        {
            Vector3 pos = transform.position;

            if (Input.GetKey("w") /*|| Input.mousePosition.y >= (Screen.height - panBorderThickness)*/)
            {
                pos.z += panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("s") /*|| Input.mousePosition.y <= panBorderThickness*/)
            {
                pos.z -= panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("d") /*|| Input.mousePosition.x >= (Screen.width - panBorderThickness)*/)
            {
                pos.x += panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("a") /*|| Input.mousePosition.x <= panBorderThickness*/)
            {
                pos.x -= panSpeed * Time.deltaTime;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            pos.y -= scroll * scrollSpeed * 300 * Time.deltaTime;

            pos.x = Mathf.Clamp(pos.x, -5, PlacementManager.instance.maxX + 5);
            pos.y = Mathf.Clamp(pos.y, minY, float.MaxValue);
            pos.z = Mathf.Clamp(pos.z, -5, PlacementManager.instance.maxZ + 5);

            transform.position = pos;
        }
    }
}


