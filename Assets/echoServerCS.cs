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

	float accTime = 0f;
	private bool isRunning = false;
	private bool isReading = false;

	void Start() {
//		StartCoroutine ("SerialCoroutine");
	}

	void Update () {
		accTime += Time.deltaTime;
		if (accTime < 0.3f) {
			return;
		}
		accTime = 0f;

		if (doStart) {
			doStart = false;
			isRunning = true;
			bool res = MyRs232cUtil.Open ("COM3", out mySP);
//			mySP.DataReceived += new SerialDataReceivedEventHandler(SPdatarcv);
			if (res == false) {
				T_status.text = "open fail";
				return;
			}
			mySP.Write("hello");
		}
		if (doStop) {
			doStop = false;
			isRunning = false;
			MyRs232cUtil.Close(ref mySP);
			T_status.text = "closed";
		}
		if (mySP.IsOpen) {
//			if (mySP.BytesToRead > 0 && isReading == false) {
			if (isReading == false) {
				T_status.text = "reading";
				isReading = true;
				byte [] data = new byte[100];
				int len = mySP.Read(data, 0, 100);
				string str = System.Text.Encoding.ASCII.GetString(data);
				T_status.text = "has read:" + str;
//				isReading = false;
			}
		}
	}

//	IEnumerator SerialCoroutine() 
//	{
//		string rcvd;
//		while (true) {
//			if (mySP.IsOpen && mySP.BytesToRead > 0) {
//				rcvd = mySP.ReadLine();
//				mySP.WriteLine(rcvd);
//			}
//			yield return new WaitForSeconds(0.1f);
//		}
//	}

//	private static void SPdatarcv(object sender, SerialDataReceivedEventArgs e)
//	{
//		SerialPort port = (SerialPort)sender;
//		byte[] buf = new byte[20];
//		int len = port.Read (buf, 0, 10);
//		string rcvd = System.Text.Encoding.ASCII.GetString (buf, 0, len);
//		port.WriteLine (rcvd);
//	}

	public static void SetStart() {
		doStart = true;
	}
	public static void SetStop() {
		doStop = true;
	}
	
}
