using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxMultiplier;
    private Transform cameraTransform;
    private Vector3 lastCameraPos;

    private Sprite sprite;
    private Texture2D texture;
    private float textureUnitSizeX;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPos = cameraTransform.position;
        sprite = GetComponent<SpriteRenderer>().sprite;
        texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        float xMovement = cameraTransform.position.x - lastCameraPos.x;
        transform.position += new Vector3(xMovement * parallaxMultiplier, 0, 0);
        lastCameraPos = cameraTransform.position;

        //if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        //{
        //    float offsetPosX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
        //    transform.position = new Vector3(cameraTransform.position.x, transform.position.y);
        //}
    }
}
