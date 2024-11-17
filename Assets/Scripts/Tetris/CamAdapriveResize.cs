using UnityEngine;

public class CamAdapriveResize : MonoBehaviour
{
    public float aspectRatioWidth = 9f;
    public float aspectRatioHeight = 16f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        SetCameraSize();
    }

    void SetCameraSize()
    {
        // Получаем текущее соотношение сторон
        float screenRatio = (float)Screen.height / Screen.width;

        // Определяем целевое соотношение сторон
        float targetRatio = aspectRatioHeight / aspectRatioWidth;

        if (screenRatio >= targetRatio)
        {
            // Если экран шире / равен целевому соотношению, базируемся на высоте
            cam.orthographicSize = aspectRatioWidth;
        }
        else
        {
            // Если экран уже целевого соотношения, базируемся на ширине
            cam.orthographicSize = (targetRatio) * (Screen.width);
        }
    }
}
