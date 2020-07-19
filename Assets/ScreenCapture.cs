using UnityEngine;

public class ScreenCapture : MonoBehaviour
{
    public System.Collections.IEnumerator doCapture(Camera inCamera, System.Action<Texture2D> inResultCallback) {
        Texture2D theResultTexture = null;

        Camera.CameraCallback captureLambda = (Camera inRenderedCamera) => {
            if (inCamera != inRenderedCamera) return;

            Rect theCapturingRect = worldRectToScreenRect(captureZoneWorldRect, inCamera);

            Vector2Int theCapturingSizeInt = Vector2Int.CeilToInt(theCapturingRect.size);
            theResultTexture = new Texture2D(
                theCapturingSizeInt.x, theCapturingSizeInt.y, TextureFormat.RGB24, false);
            theResultTexture.ReadPixels(theCapturingRect, 0, 0, false);
            theResultTexture.Apply();
        };

        Camera.onPostRender += captureLambda;
        while (!theResultTexture) {
            yield return null;
        }
        Camera.onPostRender -= captureLambda;

        inResultCallback.Invoke(theResultTexture);
    }

    static private RenderTexture createCaptureTexture(Camera inCamera)
    {
        int width = Mathf.Max(8, inCamera.pixelWidth) / 2 * 2;
        int height = Mathf.Max(8, inCamera.pixelHeight) / 2 * 2;

        RenderTextureFormat theFormat = inCamera.allowHDR ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.Default;
        var theResult = new RenderTexture(width, height, 24, theFormat);
        theResult.antiAliasing = 1;//inCamera.allowMSAA ? QualitySettings.antiAliasing : 1;
        return theResult;
    }

    private void OnDrawGizmos() {
        Color theColor = Color.red;

        Rect theWorldRect = captureZoneWorldRect;

        Vector2 thePointA = theWorldRect.position;
        Vector2 thePointB = theWorldRect.position;

        thePointB += new Vector2(_size.x, 0f);
        Debug.DrawLine(thePointA, thePointB, theColor);
        thePointA = thePointB;

        thePointB += new Vector2(0f, _size.y);
        Debug.DrawLine(thePointA, thePointB, theColor);
        thePointA = thePointB;

        thePointB += new Vector2(-_size.x, 0f);
        Debug.DrawLine(thePointA, thePointB, theColor);
        thePointA = thePointB;

        thePointB += new Vector2(0f, -_size.y);
        Debug.DrawLine(thePointA, thePointB, theColor);
    }

    private static Vector2 worldToScreenSize(Vector2 inSize, Camera inCamera) {
        Vector2 theSizeNotCorrected = inCamera.WorldToScreenPoint(inSize);
        Vector2 theCameraViewCenterScreenPosition = inCamera.WorldToScreenPoint(inCamera.transform.position);
        return theSizeNotCorrected - theCameraViewCenterScreenPosition;
    }

    private static Rect worldRectToScreenRect(Rect inWorldRect, Camera inCamera) {
        Vector2 thePosition = inCamera.WorldToScreenPoint(inWorldRect.position);
        Vector2 theSize = worldToScreenSize(inWorldRect.size, inCamera);
        return new Rect(thePosition, theSize);
    }

    public Rect captureZoneWorldRect {
        get {
            Vector2 theCenterPosition = transform.position;
            return new Rect(theCenterPosition - _size/2, _size);
        }
    }

    //Fields
    [SerializeField] Vector2 _size = Vector2.one;
}
