using UnityEngine;
using System.Collections;

using System.IO.Ports; // for RS-232C

public class rs232cSendCS : MonoBehaviour {

	void Test_rs232c() {
		SerialPort sp = new SerialPort ();
		//      string[] portname = SerialPort.GetPortNames ();
		//      sp.PortName = portname[0];
		sp.PortName = "COM3";
		sp.BaudRate = 9600;
		sp.DataBits = 8;
		sp.Parity = Parity.None;
		sp.StopBits = StopBits.One;
		sp.Handshake = Handshake.None;
		sp.Encoding = System.Text.Encoding.ASCII;
		
		sp.Open ();
		string msg = "hello";
		sp.Write (msg);
		sp.Close ();
		sp.Dispose ();
	}

	void Start () {
		Test_rs232c (); 	
	}
	
	void Update () {
	
	}
}
