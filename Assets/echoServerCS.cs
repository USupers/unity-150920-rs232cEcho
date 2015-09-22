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

	void Update () {
		if (doStart) {
			doStart = false;
			bool res = MyRs232cUtil.Open ("COM3", out mySP);
			if (res == false) {
				T_status.text = "open fail";
				return;
			}
			mySP.Write("hello");
		}
		if (doStop) {
			doStop = false;
			MyRs232cUtil.Close(ref mySP);
			T_status.text = "closed";
		}
		if (mySP.IsOpen) {
			byte [] data = new byte[100];
			int len = mySP.Read(data, 0, 100);
			string str = System.Text.Encoding.ASCII.GetString(data);
			T_status.text = "has read:" + str;
		}
	}

	public static void SetStart() {
		doStart = true;
	}
	public static void SetStop() {
		doStop = true;
	}
	
}
