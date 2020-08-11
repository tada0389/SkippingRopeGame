using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class RopeCreator : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody ropePointPrefab;

        [SerializeField]
        private int pointNum = 20;

        [SerializeField]
        private Vector3 beginPosition = new Vector3(-10f, 0f, 0f);

        [SerializeField]
        private Vector3 pointDistance = new Vector3(1f, 0f, 0f);

        private List<Rigidbody> ropePoints;

        private LineRenderer line;

        // Start is called before the first frame update
        void Start()
        {
            line = GetComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Unlit/Color"));
            line.positionCount = pointNum;

            ropePoints = new List<Rigidbody>(pointNum);
            for (int i = 0; i < pointNum; ++i)
            {
                Rigidbody point = Instantiate(ropePointPrefab, transform);
                point.transform.localPosition = beginPosition + pointDistance * i;

                //point.GetComponent<MeshRenderer>().enabled = false;
                point.useGravity = false;
                //Destroy(point.GetComponent<HingeJoint>());
                ropePoints.Add(point);
                if (i >= 1)
                {
                    //ropePoints[i - 1].GetComponent<HingeJoint>().connectedBody = point;
                }
            }

            // 両端を固定する
            if (pointNum != 0)
            {
                var rb = ropePoints[0];
                rb.isKinematic = true;
                rb.useGravity = false;
                //rb.gameObject.AddComponent<RopeTest>();
            }
            if (pointNum >= 2)
            {
                var rb = ropePoints[pointNum - 1];
                //Destroy(rb.GetComponent<HingeJoint>());
                //rb.isKinematic = true;
                //rb.useGravity = false;
                //rb.gameObject.AddComponent<RopeTest>();
            }
        }

        private void Update()
        {
            for(int i = 0, n = ropePoints.Count; i < n; ++i)
            {
                line.SetPosition(i, ropePoints[i].transform.position);
            }
        }
    }
}