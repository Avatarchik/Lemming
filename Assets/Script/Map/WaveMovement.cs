using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaveMovement : MonoBehaviour
{
    private float initialLocalYPositionOfRectTransformPosition = 0f;
    [SerializeField]
    private float maximumTopModificationValue = 30f;
    [SerializeField]
    private float minimumTopModificationValue = -30f;
    [SerializeField]
    private float waveSpeed = 0.5f;
    private float targetLocalYPositionOfRectTransformPosition = 0f;
    void Awake()
    {
        initialLocalYPositionOfRectTransformPosition = gameObject.GetComponent<RectTransform>().localPosition.y;
    }

    void Start()
    {
        SetRandomTargetPosition();
    }

    private void SetRandomTargetPosition()
    {
        var randomModificationValue = Random.Range(minimumTopModificationValue, maximumTopModificationValue);
        targetLocalYPositionOfRectTransformPosition = initialLocalYPositionOfRectTransformPosition + randomModificationValue;
        isRising = targetLocalYPositionOfRectTransformPosition > gameObject.GetComponent<RectTransform>().localPosition.y;
    }

    void Update()
    {
        MoveUpOrDown();
    }

    private bool isRising = false;
    void MoveUpOrDown()
    {
        var position = gameObject.GetComponent<RectTransform>().localPosition;

        if (isRising)
        {
            if (position.y > targetLocalYPositionOfRectTransformPosition)
                SetRandomTargetPosition();
            else 
                gameObject.GetComponent<RectTransform>().localPosition = new Vector2(position.x, position.y + waveSpeed);
        }
        else
        {
            if (position.y < targetLocalYPositionOfRectTransformPosition)
                SetRandomTargetPosition();
            else
                gameObject.GetComponent<RectTransform>().localPosition = new Vector2(position.x, position.y - waveSpeed);
        }
    }
}
