using UnityEngine;

public class AndroidCamera {
	
    AndroidJavaObject camera = null;
    public bool isFlashOn = false;
    public AndroidCamera() {
        WebCamDevice[] devices = WebCamTexture.devices;
        Debug.Log("Camera Name:"+devices[0].name);
		
        Open();
    }

    private void Open()
    {
        if (camera == null) {
#if (UNITY_ANDROID && !UNITY_EDITOR)
			AndroidJavaClass cameraClass = new AndroidJavaClass("android.hardware.Camera");
			camera = cameraClass.CallStatic<AndroidJavaObject>("open");
#endif
        }
    }
	
    public void Release()
    {
        if (camera != null) {
            LedOff();
			
            camera.Call("release");
            camera = null;
        }
    }
	
    public void StartPreview()
    {
        if (camera != null) {
            Debug.Log("AndroidCamera::startPreview()");
            camera.Call("startPreview");
        }
    }
	
    public void StopPreview()
    {
        if (camera != null) {
            Debug.Log("AndroidCamera::stopPreview()");
            LedOff();
            camera.Call("stopPreview");
        }
    }

    // Flashモードの設定	
    private void SetFlashMode(string mode)
    {		
        if (camera != null) {
            AndroidJavaObject cameraParameters = camera.Call<AndroidJavaObject>("getParameters");
            cameraParameters.Call("setFlashMode",mode);			
            camera.Call("setParameters",cameraParameters);
        }
    }
	
    // LED点灯
    public void LedOn() {
        SetFlashMode("torch");
        isFlashOn = true;
        Debug.Log("ON");
    }

    // LED消灯
    public void LedOff() {
        SetFlashMode("off");
        isFlashOn = false;
        Debug.Log("off");
    }
}