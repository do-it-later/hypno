﻿using UnityEngine;
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
		START = 7,
		LB = 4,
		RB = 5,
    }

	private readonly string osString = "Win";
    #elif UNITY_STANDALONE_OSX
    public enum Button
    {
		A = 16,
		B = 17,
		X = 18,
		Y = 19,
		SELECT = 10,
		START = 9,
		LB = 13,
		RB = 14,
    }

	private readonly string osString = "Mac";
    #endif

    public string GetButtonString(Button button)
    {
        int buttonNum = (int)button;
        return "joystick " + _controller + " button " + buttonNum;
    }

    public float GetLeftAngle()
    {
        float horiz = Input.GetAxis("P" + _controller + "_LeftX");
        // Multiply by -1 to have the positive angle going upwards
        float vert = Input.GetAxis("P" + _controller + "_LeftY") * -1;

        float angle = Mathf.Atan2(vert, horiz) * Mathf.Rad2Deg;

        if( angle < 0 )
            angle += 360;

        return angle;
    }

	public bool IsLeftStickIdle() 
	{
		float horiz = Input.GetAxis("P" + _controller + "_LeftX");
		// Multiply by -1 to have the positive angle going upwards
		float vert = Input.GetAxis("P" + _controller + "_LeftY") * -1;

		return horiz == 0 && vert == 0;
	}

	public float GetRightAngle()
	{
		float horiz = Input.GetAxis("P" + _controller + "_RightX_" + osString);
		// Multiply by -1 to have the positive angle going upwards
		float vert = Input.GetAxis("P" + _controller + "_RightY_" + osString) * -1;

		float angle = Mathf.Atan2(vert, horiz) * Mathf.Rad2Deg;

		if( angle < 0 )
			angle += 360;

		return angle;
	}

	public bool IsRightStickIdle() 
	{
		float horiz = Input.GetAxis("P" + _controller + "_RightX_" + osString);
		// Multiply by -1 to have the positive angle going upwards
		float vert = Input.GetAxis("P" + _controller + "_RightY_" + osString) * -1;

		return horiz == 0 && vert == 0;
	}

	public Vector2 GetLeftDirections()
	{
		var x = Input.GetAxis("P" + _controller + "_LeftX");
		var y = Input.GetAxis("P" + _controller + "_LeftY") * -1;
		return new Vector2(x, y);
	}

	public Vector2 GetRightDirections()
	{
		var x = Input.GetAxis("P" + _controller + "_RightX_" + osString);
		var y = Input.GetAxis("P" + _controller + "_RightY_" + osString) * -1;
		return new Vector2(x, y);
	}

	// Mac: Returns 0 until used, then [-1, 1] where -1 is unpressed, and 1 is fully pressed.
	// Windows: Returns [0, 1] where 0 is unpressed and 1 is fully pressed.
	public float GetLeftTrigger()
	{
		return Input.GetAxis("P" + _controller + "_LT_" + osString);
	}

	// Mac: Returns 0 until used, then [-1, 1] where -1 is unpressed, and 1 is fully pressed.
	// Windows: Returns [0, 1] where 0 is unpressed and 1 is fully pressed.
	public float GetRightTrigger()
	{
		return Input.GetAxis("P" + _controller + "_RT_" + osString);
	}
}