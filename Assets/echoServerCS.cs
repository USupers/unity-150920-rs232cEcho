using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Threading; // for Thread
using System.IO.Ports; // for RS-232C

using NS_MyRs232cUtil;

/*
 * v0.2 2015/09/23
 *   - work in thread
 * v0.1 2015/09/22
 *   - can echo back
 */ 

public class echoServerCS : MonoBehaviour {

	private static bool doStart = false;
	private static bool doStop = false;
	private static bool rcvdCRLF = false;
	private Thread rcvThr;
	
	public InputField IF_comname;
	public Text T_status;
	private SerialPort mySP;
	private string accRcvd = "";
	private string statusText = "";

	void Update () {
		T_status.text = statusText;

		if (doStart) {
			doStart = false;
			startThread();
		}
	}

	public static void SetStart() {
		doStart = true;
	}
	public static void SetStop() {
		doStop = true;
	}

	private void startThread() {
		rcvThr = new Thread (new ThreadStart (FuncEcho));
		rcvThr.Start ();
	}
	private void OnApplicationQuit() {
		doStop = true;
		if (rcvThr != null) {
			rcvThr.Abort ();
		}
	}

	private bool rcvAndEcho(ref SerialPort mySP) {
		byte rcv;
		char tmp;
		bool hasRcvd = false;

		try {
			rcv = (byte)mySP.ReadByte();
			if (rcv != 255) {
				hasRcvd = true;

				tmp = (char)rcv;
				if (tmp != 0x0d && tmp != 0x0a) { // not CRLF
					accRcvd = accRcvd + tmp.ToString();
				}
				if (tmp == 0x0d) { // CR
					mySP.WriteLine(accRcvd);
					rcvdCRLF = true;
				}
			}
		} catch (System.Exception) {
		}

		return hasRcvd;
	}

	private void FuncEcho() 
	{
		Debug.Log ("func echo start");
		bool res = MyRs232cUtil.Open (IF_comname.text, out mySP);
		mySP.ReadTimeout = 1;
		if (res == false) {
			statusText = "open fail";
			return;
		}
		statusText = IF_comname.text + " : open";
		mySP.Write (">");

		while (doStop == false) {
			if (mySP != null && mySP.IsOpen) {
				if (rcvAndEcho(ref mySP)) {
					statusText = "has received: " + accRcvd;
					if (rcvdCRLF) {
						rcvdCRLF = false;
						accRcvd = "";
					}
				}
			}
			Thread.Sleep(20); // without this app may freeze
		}
		Debug.Log ("func echo stop");
		MyRs232cUtil.Close(ref mySP);
		statusText = IF_comname.text + " : closed";
		doStop = false;
	}
	
}
