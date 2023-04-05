using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningGroundScript : MonoBehaviour
{
    [SerializeField] float _effectRadius;
    [SerializeField] float _tickDelay;
    [SerializeField] float _damagePerTick;
    [SerializeField] float _lifeTime;
    [SerializeField] float _stopDamageAfter;

    private float _timeSinceLastTick = 0;
    private float _stopTime = 0;
    private Damage _damage;
    private List<IHitable> _targets = new List<IHitable>();

    [Header("Visuals")]
    [SerializeField] float _timeToGrow;
    private Vector3 _startScale;
    // Start is called before the first frame update
    void Start()
    {
        _startScale = transform.localScale;
        _damage = new Damage(_damagePerTick);
        StartCoroutine(GrowVisual());
        StartCoroutine(DestroyAfter());
    }

    // Update is called once per frame
    void Update()
    {
        _timeSinceLastTick += Time.deltaTime;
        _stopTime += Time.deltaTime;

        if (_stopTime >= _stopDamageAfter) return;
        if (_timeSinceLastTick <= _tickDelay) return;

        _timeSinceLastTick = 0;
        
        Collider[] colliders = Physics.OverlapSphere(transform.transform.position, _effectRadius);  

        foreach (Collider collider in colliders)
        {
            var target = collider.GetComponent<IHitable>();
            if (target != null && !_targets.Contains(target))
            {
                _targets.Add(target);
            }
        }

        DamageTargets();
        _targets.Clear();
    }

    void DamageTargets()
    {
        foreach (var target in _targets)
        {
            target.HandleHit(_damage);
        }
    }

    IEnumerator GrowVisual() 
    {
        float timer = 0.0f;

        while (timer <= _timeToGrow)
        {
            float scale = Mathf.Lerp(0, 1, timer / _timeToGrow);
            transform.localScale = Vector3.Lerp(Vector3.zero, _startScale, scale);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(_lifeTime);

        float timer = 0.0f;

        while (timer <= _timeToGrow)
        {
            float scale = Mathf.Lerp(0, 1, timer / _timeToGrow);
            transform.localScale = Vector3.Lerp(_startScale, Vector3.zero, scale);

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, _effectRadius);
    }
}
