using UnityEngine;
using System.Collections;

public class ControllerInputManager : MonoBehaviour {

	[SerializeField]
	private int _controller = 1;

    #if UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
    public enum Button
    {
        A = 0,
        B = 1,
        X = 2,
        Y = 3,
		SELECT = 6,
        START = 7
    }
    #elif UNITY_STANDALONE_OSX
    public enum Button
    {
        A = 16,
        B = 17,
        X = 18,
        Y = 19,
		SELECT = 10,
        START = 9,
    }
    #endif

    public string GetButtonString(Button button)
    {
        int buttonNum = (int)button;
        return "joystick " + _controller.ToString() + " button " + buttonNum.ToString();
    }

    public float GetLeftAngle()
    {
        float horiz = Input.GetAxis("P" + _controller.ToString() + "_LeftX");
        // Multiply by -1 to have the positive angle going upwards
        float vert = Input.GetAxis("P" + _controller.ToString() + "_LeftY") * -1;

        float angle = Mathf.Atan2(vert, horiz) * Mathf.Rad2Deg;

        if( angle < 0 )
            angle += 360;

        return angle;
    }

	public float GetRightAngle()
	{
		#if UNITY_STANDALONE_WIN
			var os = "Win";
		#elif UNITY_STANDALONE_OSX
			var os = "Mac";
		#endif
		float horiz = Input.GetAxis("P" + _controller.ToString() + "_RightX_" + os);
		// Multiply by -1 to have the positive angle going upwards
		float vert = Input.GetAxis("P" + _controller.ToString() + "_RightY_" + os) * -1;

		float angle = Mathf.Atan2(vert, horiz) * Mathf.Rad2Deg;

		if( angle < 0 )
			angle += 360;

		return angle;
	}
}