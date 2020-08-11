using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class RopeTest : MonoBehaviour
    {
        private float radius = 1.4f;
        private float speed = 0.2f;
        private float x;
        private float angle = 0;

        // Start is called before the first frame update
        void Start()
        {
            x = transform.position.x;
        }

        // Update is called once per frame
        void Update()
        {
            angle += speed;

            float y = 1.2f * radius * Mathf.Sin(-angle * Mathf.Deg2Rad);
            float z = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            transform.position = new Vector3(x, y, z);
        }
    }
}