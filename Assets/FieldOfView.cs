using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteAlways]
public class FieldOfView : MonoBehaviour
{

	public float viewRadius;
	[Range(0, 360)]
	public float viewAngle;

	public LayerMask targetMask;
	public LayerMask obstacleMask;

	[HideInInspector]
	public List<Transform> visibleTargets = new List<Transform>();

	public float meshResolution;
	public int edgeResolveIterations;
	public float edgeDstThreshold;

	public MeshFilter viewMeshFilter;
	Mesh viewMesh;

	void Start()
	{
		viewMesh = new Mesh();
		viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = viewMesh;

		StartCoroutine("FindTargetsWithDelay", .2f);
	}


	IEnumerator FindTargetsWithDelay(float delay)
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}

	void LateUpdate()
	{
		DrawFieldOfView();
	}

	void FindVisibleTargets()
	{
		visibleTargets.Clear();
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++)
		{
			Transform target = targetsInViewRadius[i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
			{
				float dstToTarget = Vector3.Distance(transform.position, target.position);
				if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
				{
					visibleTargets.Add(target);
				}
			}
		}
	}

	void DrawFieldOfView()
	{
		int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
		float stepAngleSize = viewAngle / stepCount;
		List<Vector3> viewPoints = new List<Vector3>();
		ViewCastInfo oldViewCast = new ViewCastInfo();
		for (int i = 0; i <= stepCount; i++)
		{
			float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			ViewCastInfo newViewCast = ViewCast(angle);

			if (i > 0)
			{
				bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
				if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
				{
					EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
					if (edge.pointA != Vector3.zero)
					{
						viewPoints.Add(edge.pointA);
					}
					if (edge.pointB != Vector3.zero)
					{
						viewPoints.Add(edge.pointB);
					}
				}

			}


			viewPoints.Add(newViewCast.point);
			oldViewCast = newViewCast;
		}

		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];

		vertices[0] = Vector3.zero;
		for (int i = 0; i < vertexCount - 1; i++)
		{
			vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

			if (i < vertexCount - 2)
			{
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}
		}

		viewMesh.Clear();

		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals();
	}


	EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
	{
		float minAngle = minViewCast.angle;
		float maxAngle = maxViewCast.angle;
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;

		for (int i = 0; i < edgeResolveIterations; i++)
		{
			float angle = (minAngle + maxAngle) / 2;
			ViewCastInfo newViewCast = ViewCast(angle);

			bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
			if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
			{
				minAngle = angle;
				minPoint = newViewCast.point;
			}
			else
			{
				maxAngle = angle;
				maxPoint = newViewCast.point;
			}
		}

		return new EdgeInfo(minPoint, maxPoint);
	}


	ViewCastInfo ViewCast(float globalAngle)
	{
		Vector3 dir = DirFromAngle(globalAngle, true);
		RaycastHit hit;

		if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
		{
			return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
		}
		else
		{
			return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
		}
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	public struct ViewCastInfo
	{
		public bool hit;
		public Vector3 point;
		public float dst;
		public float angle;

		public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
		{
			hit = _hit;
			point = _point;
			dst = _dst;
			angle = _angle;
		}
	}

	public struct EdgeInfo
	{
		public Vector3 pointA;
		public Vector3 pointB;

		public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
		{
			pointA = _pointA;
			pointB = _pointB;
		}
	}

}



































//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[ExecuteAlways]
//public class FieldOfView : MonoBehaviour
//{
//    [SerializeField] float viewDistance = 50f;
//    [SerializeField] float startingAngle;
//    [SerializeField] float fov = 90f;
//    [SerializeField] int rayCount = 2;
//    [SerializeField] Vector3 origin = Vector3.zero;
//    [SerializeField] LayerMask layerMask;
//    [SerializeField] Transform player;
//    float currentAngle = 0;

//    float angleIncrease;
//    Mesh mesh;
//    private void Start()
//    {
//       // origin = transform.position;
//        GetComponent<MeshFilter>().mesh = null;
//        mesh = new Mesh();
//        GetComponent<MeshFilter>().mesh = mesh;



//    }

//    private void Update()
//    {
//        transform.position = player.position;
//        angleIncrease = fov / rayCount;
//        CreateMesh(mesh);
//    }

//    private void CreateMesh(Mesh mesh)
//    {
//        currentAngle = startingAngle;
//        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
//        Vector2[] uv = new Vector2[vertices.Length];
//        int[] triangles = new int[rayCount * 3];

//        vertices[0] = origin;

//        int vertexIndex = 1;
//        int triangleIndex = 0;


//        for (int i = 0; i <= rayCount; i++)
//        {
//            Vector3 vertex;

//            RaycastHit hit;
//            Ray ray = new Ray(transform.position, GetVectorFromAngle(currentAngle));

//            if(Physics.Raycast(ray,out hit, viewDistance, layerMask))
//            {
//                vertex = hit.point;
//            }
//            else vertex = origin + GetVectorFromAngle(currentAngle) * viewDistance;

//            //RaycastHit hit;
//            //if( Physics.Raycast(transform.position, GetVectorFromAngle(currentAngle), out hit, viewDistance,layerMask))
//            //{
//            //    vertex = hit.point;
//            //    Debug.Log("Hit" + hit.collider.name);
//            //}
//            //else
//            //{
//            //    vertex = origin + GetVectorFromAngle(currentAngle) * viewDistance;
//            //}


//            vertices[vertexIndex] = vertex;

//            if (i > 0)
//            {
//                triangles[triangleIndex + 0] = 0;
//                triangles[triangleIndex + 1] = vertexIndex - 1;
//                triangles[triangleIndex + 2] = vertexIndex;
//                triangleIndex += 3;
//            }

//            vertexIndex++;
//            currentAngle -= angleIncrease;
//        }


//        mesh.vertices = vertices;
//        mesh.uv = uv;
//        mesh.triangles = triangles;
//    }

//    Vector3 GetVectorFromAngle(float angle)
//    {
//        //float angleRad = angle * (Mathf.PI / 180f);
//        return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
//    }

//    public void SetOrigin(Vector3 origin)
//    {
//        this.origin = origin;
//    }


//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.red;

//        for (int i = 0; i < rayCount; i++)
//        {
//            Ray r = new Ray(transform.position, GetVectorFromAngle(currentAngle) * 300);
//            Gizmos.DrawRay(r);
//        }

//    }


//    //private void OnValidate()
//    //{
//    //    GetComponent<MeshFilter>().mesh = null;
//    //    Mesh mesh = new Mesh();
//    //    GetComponent<MeshFilter>().mesh = mesh;
//    //   // origin = transform.position;
//    //    currentAngle = 0;
//    //    angleIncrease = fov / rayCount;
//    //    CreateMesh(mesh);
//    //}

//}
