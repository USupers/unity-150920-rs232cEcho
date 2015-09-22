using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Threading; // for Thread
using System.IO.Ports; // for RS-232C

using NS_MyRs232cUtil;

/*
 * v0.1 2015/09/22
 *   - can echo back
 */ 

public class echoServerCS : MonoBehaviour {

	private static bool doStart = false;
	private static bool doStop = false;
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
		if (mySP != null && mySP.IsOpen) {
			byte rcv;
			char tmp;
			try {
				rcv = (byte)mySP.ReadByte();
				if (rcv != 255) {
					tmp = (char)rcv;
					if (tmp != 0x0d && tmp != 0x0a) { // not CRLF
						accRcvd = accRcvd + tmp.ToString();
					}
					if (tmp == 0x0d) { // CR
						mySP.WriteLine(accRcvd);
						T_status.text = "has read:" + accRcvd;
						accRcvd = "";
					}
				}
			} catch (System.Exception) {
			}
		}
		Thread.Sleep (20); // without this app will freeze 
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


			Thread.Sleep(20); // without this app may freeze
		}
		Debug.Log ("func echo stop");
		MyRs232cUtil.Close(ref mySP);
		statusText = IF_comname.text + " : closed";
	}
	
}
