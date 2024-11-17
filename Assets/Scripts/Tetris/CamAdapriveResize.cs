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
        // �������� ������� ����������� ������
        float screenRatio = (float)Screen.height / Screen.width;

        // ���������� ������� ����������� ������
        float targetRatio = aspectRatioHeight / aspectRatioWidth;

        if (screenRatio >= targetRatio)
        {
            // ���� ����� ���� / ����� �������� �����������, ���������� �� ������
            cam.orthographicSize = aspectRatioWidth;
        }
        else
        {
            // ���� ����� ��� �������� �����������, ���������� �� ������
            cam.orthographicSize = (targetRatio) * (Screen.width);
        }
    }
}
