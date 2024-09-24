using UnityEngine;
using Random = UnityEngine.Random;

public class BouncingDvd : MonoBehaviour {
    [SerializeField]
    private float speed;

    [SerializeField]
    private RectTransform _img;

    private Vector3 direction;

    private Rect ScreenRect;

    private void Awake() {
        direction = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), 0);
        ScreenRect = new Rect();
    }

    private void Update() {
        AdjustRect();
        Move();
    }

    private void AdjustRect() {
        ScreenRect.x = _img.rect.width / 2;
        ScreenRect.y = _img.rect.height / 2;
        ScreenRect.width = Screen.width - _img.rect.width;
        ScreenRect.height = Screen.height - _img.rect.height;
    }

    private void Move() {
        Vector3 newpos = transform.position + direction * speed * 1000f * Time.deltaTime;

        if (newpos.x > ScreenRect.xMax || newpos.x < ScreenRect.xMin) {
            direction.x *= -1;
        }

        if (newpos.y > ScreenRect.yMax || newpos.y < ScreenRect.yMin) {
            direction.y *= -1;
        }

        newpos.x = Mathf.Clamp(newpos.x, ScreenRect.xMin, ScreenRect.xMax);
        newpos.y = Mathf.Clamp(newpos.y, ScreenRect.yMin, ScreenRect.yMax);
        transform.position = newpos;
    }
}