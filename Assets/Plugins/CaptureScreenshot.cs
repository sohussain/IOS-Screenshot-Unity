using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class CaptureScreenshot : MonoBehaviour {

	[DllImport("__Internal")]
	private static extern void _PlaySystemShutterSound ();

	[DllImport("__Internal")]
	private static extern void _GetTexture (byte[] textureByte, int length);

	public void CaptureScreenShot (Texture2D texture) {
		_PlaySystemShutterSound ();
//		Handheld.SetActivityIndicatorStyle(UnityEngine.iOS.ActivityIndicatorStyle.Gray);
//		Handheld.StartActivityIndicator ();
		StartCoroutine (TakeScreenshot (texture));
	}

	private IEnumerator TakeScreenshot(Texture2D texture) {
		yield return new WaitForEndOfFrame();
		
		var width = Screen.width;
		var height = Screen.height;
//		var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

//		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
//		tex.Apply();
		byte[] screenshot = texture.EncodeToPNG();

		_GetTexture (screenshot, screenshot.Length);
//		Handheld.StopActivityIndicator ();
		}

	void DidImageWriteToAlbum (string errorDescription) {
//		Handheld.StopActivityIndicator ();
		}
}