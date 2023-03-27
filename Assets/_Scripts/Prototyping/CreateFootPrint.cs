using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class CreateFootPrint : MonoBehaviour
{
    [SerializeField] Transform _moveDirectionIndicator;
    [SerializeField] GameObject _footPrintPrefab;
    [SerializeField] GameObject _trackerPrintPrefab;
    [SerializeField] LayerMask _noFpLayer;
    [SerializeField] float _raycastDistance;
    [SerializeField] float _delayBetweenCasts;
    [SerializeField] float _startDelay;
    [SerializeField] Transform spawner1;
    [SerializeField] Transform spawner2;
    private Object_ID _id;


    public bool ready = false;
    float _timeSinceLastRc = 0;
    bool firstSpawner = true;
    // Start is called before the first frame update
    void Start()
    {
        _id = GetComponent<Object_ID>();
        StartCoroutine(MakeReady());
    }

    // Update is called once per frame
    void Update()
    {
        if (ready) _timeSinceLastRc += Time.deltaTime;

        if (_timeSinceLastRc >= _delayBetweenCasts && ready)
        {
            _timeSinceLastRc = 0;

            if (firstSpawner)
            {
                firstSpawner = false;
                var track1 =Instantiate(_footPrintPrefab, new Vector3(spawner1.position.x, .1f, spawner1.position.z)
                , Quaternion.LookRotation(_moveDirectionIndicator.forward));

                var track2 = Instantiate(_trackerPrintPrefab, new Vector3(spawner1.position.x, .15f, spawner1.position.z)
                , Quaternion.LookRotation(_moveDirectionIndicator.forward));

                track1.GetComponent<Object_ID>().CreateID(_id);
                track2.GetComponent<Object_ID>().CreateID(_id);
            }
            else
            {
                firstSpawner = true;
                var track1 = Instantiate(_footPrintPrefab, new Vector3(spawner2.position.x, .1f, spawner2.position.z)
                , Quaternion.LookRotation(_moveDirectionIndicator.forward));

                var track2 = Instantiate(_trackerPrintPrefab, new Vector3(spawner2.position.x, .15f, spawner2.position.z)
                , Quaternion.LookRotation(_moveDirectionIndicator.forward));

                track1.GetComponent<Object_ID>().CreateID(_id);
                track2.GetComponent<Object_ID>().CreateID(_id);
            }



            //Debug.Log("Tring Raycast");
            //Ray ray = new Ray(transform.position, Vector3.down);
            //RaycastHit hit;
            //if(Physics.Raycast(ray,out hit, _raycastDistance, _noFpLayer))
            //{
            //    Debug.Log("Success");
            //    var fp = Instantiate(_footPrintPrefab, hit.point + (Vector3.up * 0.01f) , Quaternion.LookRotation(hit.normal));
            //    Destroy(fp, _footPrintLifetime);
            //}
        }

    }

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Ray r = new Ray(transform.position, Vector3.down * _raycastDistance);
        Gizmos.DrawRay(r);

    }
    IEnumerator MakeReady()
    {
        yield return new WaitForSeconds(_startDelay);
        ready = true;
    }

}
