using UnityEngine;
using System.Collections;

using System.Threading; // for Thread

public class echoServerCS : MonoBehaviour {

	private static bool doStart = false;
	private static bool doStop = false;
	private Thread rcvThr;
	private bool stopThr = false;

	void Start () {
	
	}
	
	void Update () {
		if (doStart) {
			doStart = false;
			startThread();
		}
		if (doStop) {
			doStop = false;
			stopThread();
		}
	}

	public static void SetStart() {
		doStart = true;
	}
	public static void SetStop() {
		doStop = true;
	}

	void startThread() {
		Debug.Log ("start Thread");
		rcvThr = new Thread (new ThreadStart (FuncRcvData));
		rcvThr.Start ();
	}
	void stopThread() {
		Debug.Log ("stop Thread");
		stopThr = true;
		rcvThr.Abort ();
	}

	private void FuncRcvData() {
		while (stopThr == false) {


			Thread.Sleep(20);
		}

		Debug.Log ("exit while");
	}

}
