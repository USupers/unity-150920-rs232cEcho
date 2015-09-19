using UnityEngine;
using System.Collections;

using System.IO.Ports; // for RS-232C

using NS_MyRs232cUtil;

public class rs232cSendCS : MonoBehaviour {

	void Test_rs232c() {
		SerialPort sp;

		bool res = MyRs232cUtil.Open ("COM3", out sp);
		if (res == false) {
			return; // fail
		}

		string msg = "hello";
		sp.Write (msg);

		MyRs232cUtil.Close (ref sp);
	}

	void Start () {
		Test_rs232c (); 	
	}
	
	void Update () {
	
	}
}
