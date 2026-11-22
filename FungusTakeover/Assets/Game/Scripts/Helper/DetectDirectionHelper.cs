
using UnityEngine;

namespace Helper
{
	public static class DetectDirectionHelper
	{
		public static Vector2 Get8DirectionVector(Vector2 input)
		{
			Vector2 result = new Vector2(0,0);

			if (input.x > 0)
			{
				result.x = 1;
			}
			else if (input.x < 0)
			{
				result.x = -1;
			}

            if (input.y > 0)
            {
                result.y = 1;
            }
            else if (input.y < 0)
            {
                result.y = -1;
            }


            return result;
		}

        public static Vector2Int Get4DirectionVector(Vector2 input)
        {
            Vector2Int result = new Vector2Int(0, 0);

            Vector2 temp = new Vector2 (Mathf.Abs(input.x), Mathf.Abs(input.y));

            if (input.magnitude <= 0)
                return result;

            if (temp.x > 0 && temp.x >= temp.y)
            {
                if (input.x > 0)
                {
                    result.x = 1;
                }
                else if (input.x < 0)
                {
                    result.x = -1;
                }
            }
            else if (temp.y > 0 && temp.y >= temp.x)
            {
                if (input.y > 0)
                {
                    result.y = 1;
                }
                else if (input.y < 0)
                {
                    result.y = -1;
                }
            }
            
            return result;
        }

    }
}