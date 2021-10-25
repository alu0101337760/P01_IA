using UnityEngine.UI;
using UnityEngine;

namespace IA_sim
{
    public class SliderScript : MonoBehaviour
    {
        public bool isTimelineSlider;
        private void Start()
        {
            if (isTimelineSlider)
            {                
                gameObject.GetComponent<Slider>().onValueChanged.AddListener(delegate
                {
                    PathfindingManager.instance.DrawExploredNodes((int)gameObject.GetComponent<Slider>().value);
                });
            }
            else
            {
                gameObject.GetComponent<Slider>().onValueChanged.AddListener(delegate
                {
                    PlacementManager.instance.SetObstaclePercentage((int)gameObject.GetComponent<Slider>().value);
                });
            }
        }

        public void SetMaxValue(int value)
        {
            gameObject.GetComponent<Slider>().maxValue = value;
        }
    }
}