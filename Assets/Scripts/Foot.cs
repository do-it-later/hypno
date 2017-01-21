using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Hole")
		{
			Destroy(transform.parent.gameObject);
		}
	}

}
