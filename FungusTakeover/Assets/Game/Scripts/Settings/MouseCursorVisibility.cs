
using UnityEngine;
using UnityEngine.InputSystem;

namespace Start
{
	public class MouseCursorVisibility : MonoBehaviour
	{
		[SerializeField] private float _hideDuration;
		private float _time;
		private bool _isShown;
		private bool isShown
		{ 
			get => _isShown;
			set
			{
				_isShown = value;
				Cursor.visible = _isShown;
			}
		}

		public void Update()
		{
			if (Mouse.current.delta.magnitude > 0)
			{
				isShown = true;
				_time = 0;

                return;
			}

			if (_time < _hideDuration)
			{ 
				_time += Time.deltaTime;
				return;
			}

			isShown = false;
        }

	}
}