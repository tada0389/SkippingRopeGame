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
        private int pointNumHalf = 20;

        [SerializeField]
        private Vector3 centerPosition = new Vector3(0f, 0f, 0f);

        [SerializeField]
        private Vector3 pointDistance = new Vector3(1f, 0f, 0f);

        private List<Rigidbody> ropePoints;

        private LineRenderer line;

        // Start is called before the first frame update
        void Start()
        {
            UnityEngine.Assertions.Assert.IsTrue(pointNumHalf >= 1, "質点の数が足りません");

            line = GetComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Unlit/Color"));
            line.positionCount = pointNumHalf * 2 + 1;

            ropePoints = new List<Rigidbody>(pointNumHalf * 2 + 1);

            // 左側の質点
            {
                Rigidbody point = Instantiate(ropePointPrefab, transform);
                point.transform.localPosition = centerPosition - pointDistance * pointNumHalf;
                ropePoints.Add(point);
            }
            for(int i = 1; i < pointNumHalf; ++i)
            {
                Rigidbody point = Instantiate(ropePointPrefab, transform);
                point.transform.localPosition = centerPosition - pointDistance * (pointNumHalf - i);
                // つなげる
                point.GetComponent<HingeJoint>().connectedBody = ropePoints[i - 1];
                ropePoints.Add(point);
            }
            // 中央の質点
            {
                Rigidbody point = Instantiate(ropePointPrefab, transform);
                point.transform.localPosition = centerPosition;
                ropePoints.Add(point);
            }

            // 右側の質点
            {
                Rigidbody point = Instantiate(ropePointPrefab, transform);
                point.transform.localPosition = centerPosition + pointDistance;
                ropePoints.Add(point);
            }
            for (int i = 1; i < pointNumHalf; ++i)
            {
                Rigidbody point = Instantiate(ropePointPrefab, transform);
                point.transform.localPosition = centerPosition + pointDistance * (i + 1);
                // つなげる
                ropePoints[pointNumHalf + i].GetComponent<HingeJoint>().connectedBody = point;
                ropePoints.Add(point);
            }

            // 中央の質点から左右につなげる
            {
                ropePoints[pointNumHalf].gameObject.AddComponent<HingeJoint>();
                var joints = ropePoints[pointNumHalf].GetComponents<HingeJoint>();
                joints[0].connectedBody = ropePoints[pointNumHalf - 1];
                joints[1].connectedBody = ropePoints[pointNumHalf + 1];
            }

            // 端点を特別にする
            {
                Rigidbody point = ropePoints[0];
                point.isKinematic = true;
                point.useGravity = false;
                point.gameObject.AddComponent<RopeTest>();
            }
            {
                Rigidbody point = ropePoints[pointNumHalf * 2];
                point.isKinematic = true;
                point.useGravity = false;
                point.gameObject.AddComponent<RopeTest>();
            }

            foreach (var point in ropePoints) point.GetComponent<MeshRenderer>().enabled = false;
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