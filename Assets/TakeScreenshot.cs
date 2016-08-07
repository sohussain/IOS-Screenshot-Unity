using UnityEngine;
using System.Collections;

public class TakeScreenshot : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void screenshotButtonClicked() {

	}

	IEnumerator ScreenshotEncode()
	{
		yield return new WaitForEndOfFrame();
		//		moreDetailsCanvas.SetActive (false);
		Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

		texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		texture.Apply();
		//		moreDetailsCanvas.SetActive (true);
		yield return 0;

		byte[] bytes = texture.EncodeToPNG();
		string root = null;

		#if UNITY_IPHONE
		root = Application.persistentDataPath();
		#elif UNITY_ANDROID
		AndroidJavaClass Environment = new AndroidJavaClass("android.os.Environment");
		AndroidJavaObject picturesDirectory = Environment.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", new object[] { Environment.GetStatic<string>("DIRECTORY_DCIM") });
		root = picturesDirectory.Call<string>("getPath");
		string fullFileAddress = root + "/HistoricalAR-" + DateTime.Now.ToString ("dd-MM-yyyy-HH:mm:ss") + ".png";
		File.WriteAllBytes(fullFileAddress, bytes);
		#endif

//		count++;
		#if UNITY_ANDROID
		AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject file = new AndroidJavaObject("java.io.File", new object[1]{fullFileAddress});
		AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", new object[2]{intentClass.GetStatic<string>("ACTION_MEDIA_SCANNER_SCAN_FILE"), classUri.CallStatic<AndroidJavaObject>("fromFile", new object[]{file})});
		objActivity.Call ("sendBroadcast", objIntent);
		#elif UNITY_IPHONE
		new CaptureScreenshot ().CaptureScreenShot ();
		#endif

		DestroyObject(texture);
	}
}

