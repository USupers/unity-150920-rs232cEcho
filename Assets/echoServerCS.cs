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

	public Text T_status;
	private SerialPort mySP;
	private string accRcvd = "";

	void Update () {
		if (doStart) {
			doStart = false;
			bool res = MyRs232cUtil.Open ("COM3", out mySP);
			mySP.ReadTimeout = 1;
			if (res == false) {
				T_status.text = "open fail";
				return;
			}
			mySP.Write(">");
		}
		if (doStop) {
			doStop = false;
			MyRs232cUtil.Close(ref mySP);
			T_status.text = "closed";
		}
		if (mySP.IsOpen) {
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
	
}
