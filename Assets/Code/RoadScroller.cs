using UnityEngine;

//
// This class is responsible for animating the road
//
public class RoadScroller : MonoBehaviour
{
    public float speed = 4f; // Road scroll speed

    // Update is called once per frame
    void Update()
    {
        // Move the road segment downwards
        transform.Translate(speed * Time.deltaTime * Vector2.down);

        // When a road segment moves off screen reset it's position to the top
        if (transform.position.y <= -6)
        {
            transform.position = new Vector3(0, 6, 0);
        }
    }
}
