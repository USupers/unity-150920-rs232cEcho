using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Threading; // for Thread
using System.IO.Ports; // for RS-232C

using NS_MyRs232cUtil;

public class echoServerCS : MonoBehaviour {

	private static bool doStart = false;
	private static bool doStop = false;
	private Thread rcvThr;
	private bool stopThr = false;

	public Text T_status;
	private string lastRcvd;

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
		if (lastRcvd.Length > 0) {
			T_status.text = lastRcvd;
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
		SerialPort sp;
		bool res = MyRs232cUtil.Open ("COM3", out sp);
		if (res == false) {
			Debug.Log("exit thread");
			return; // fail
		}

		while (stopThr == false) {
			if (sp.BytesToRead == 0) {
				Thread.Sleep(100);
			}
			string rcvd;
			rcvd = sp.ReadLine();
			lastRcvd = rcvd;

			sp.Write(rcvd);

			Thread.Sleep(20);
		}

		sp.Write ("hello");

//		Debug.Log ("exit while");

		MyRs232cUtil.Close (ref sp);

	}

}
