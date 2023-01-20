using UnityEngine;

public class Item : MonoBehaviour
{
    private bool isPickedUp = false;
    private float shakeAmount = 0.1f;
    private float shakeDuration = 0.5f;
    private float shakeTimer = 0f;

    public bool IsPickedUp { get { return isPickedUp; } }

    private void Update()
    {
        if (!isPickedUp)
        {
            shakeTimer += Time.deltaTime;
            if (shakeTimer >= shakeDuration)
            {
                shakeTimer = 0f;
            }
            float shake = shakeAmount * Mathf.Sin(shakeTimer / shakeDuration * Mathf.PI * 2);
            transform.position += new Vector3(shake, 0f, 0f);
        }
    }

    public void PickUp()
    {
        isPickedUp = true;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    public void Use()
    {
        // This method should be overridden by child classes to implement specific use behavior
    }
}
