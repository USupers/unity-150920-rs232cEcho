using UnityEngine;
using System.Collections;

public class ButtonsCS : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartButtonClick() {
		echoServerCS.SetStart ();
	}
	public void StopButtonClick() {
		echoServerCS.SetStop ();
	}
}
