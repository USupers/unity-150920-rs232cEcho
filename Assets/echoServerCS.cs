using UnityEngine;
using System.Collections;

using System.Threading; // for Thread

public class echoServerCS : MonoBehaviour {

	private static bool doStart = false;
	private Thread rcvThr;
	private bool stopThr = false;

	void Start () {
	
	}
	
	void Update () {
	
	}

	public static void SetStart() {
		doStart = true;
	}
	public static void SetStop() {
		doStart = false;
	}

	void startThread() {
		Debug.Log ("init");
		rcvThr = new Thread (new ThreadStart (FuncRcvData));
		rcvThr.Start ();
	}
	void stopThread() {
		stopThr = true;
		rcvThr.Abort ();
	}

	private void FuncRcvData() {
		while (stopThr == false) {


			Thread.Sleep(20);
		}
	}

}
