using UnityEngine;

namespace Start
{
    public class StartGameBehaviour : MonoBehaviour
    {
        public void Awake()
        {
            ScaleResolutionTo16_9();
        }

        private void ScaleResolutionTo16_9()
        {
            //force 16 by 9

            if (Screen.height < (int)((float)Screen.width / 16f) * 9f)
                Screen.SetResolution((int)(Screen.height * (16f / 9f)), Screen.height, true);
            else if (Screen.height > (int)((float)Screen.width / 16f) * 9f)
                Screen.SetResolution(Screen.width, (int)((float)Screen.width * (9f / 16f)), true);
        }
    }
}

