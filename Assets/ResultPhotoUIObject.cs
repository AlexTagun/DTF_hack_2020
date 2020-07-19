using UnityEngine;

public class ResultPhotoUIObject : MonoBehaviour
{
    public void show(Texture2D inBestPhotoImageTexture, int inResultScores) {
        gameObject.SetActive(true);
        _bestPhotoImage.texture = inBestPhotoImageTexture;
        _bestScoresText.text = inResultScores.ToString();
    }

    //Fields
    [SerializeField] private UnityEngine.UI.RawImage _bestPhotoImage = null;
    [SerializeField] private UnityEngine.UI.Text _bestScoresText = null;
}
