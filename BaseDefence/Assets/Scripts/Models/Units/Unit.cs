using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    private GameObject nearDethEffectObject;
    private bool isNearDeath = false;
    private StateMachine stateMachine = new StateMachine();
    private LayerMask searchLayer;
    private NavMeshAgent navMeshAgent;
    protected float baseSpeed;
    protected float baseAcceleration;
    protected string deathEffect;
    protected string nearDeathEffect = "NearDeathSmokeEffect";

    protected float health;
    protected float rewardEnergy;

    protected Transform mainTarget;
    private readonly string mainTargetTag = "Goal";

    public bool IsActive
    {
        get
        {
            return
                gameObject.activeSelf;
        }
    }

    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = baseSpeed;
        navMeshAgent.acceleration = baseAcceleration;
        searchLayer = LayerMask.GetMask("Target");
    }

    protected virtual void OnEnable()
    {
        stateMachine.ChangeState(new SearchState(gameObject, searchLayer, 100f, mainTargetTag, TargetFound));
    }

    private void OnDisable()
    {
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        isNearDeath = false;      
    }

    private void Update()
    {
        stateMachine.ExecuteState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(mainTargetTag))
        {
            OnNearDeathEffect();

            ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
        }
    }

    private void SetDestination(Vector3 targetPosition)
    {
        navMeshAgent.SetDestination(targetPosition);
    }

    private void TargetFound(SearchResult searchResult)
    {
        var targets = searchResult.AllHitObjectWithRequiredTags;

        foreach (var target in targets)
        {
            if (target.CompareTag(mainTargetTag))
            {
                SetDestination(target.transform.position);
            }
        }
    }

    private void OnNearDeathEffect()
    {
        if (nearDethEffectObject != null)
        {
            nearDethEffectObject.transform.SetParent(null);
            ObjectPoolManager.Instance.ReturnObjectToPool(nearDethEffectObject);
        }
        else
        {
            nearDethEffectObject = ObjectPoolManager.Instance.GetObjectFromPool(nearDeathEffect, transform.position, Quaternion.identity);
            nearDethEffectObject.transform.SetParent(transform);
        }     
    }

    private void OnDeath()
    {
        PlayerStats.Instance.AddEnergy(rewardEnergy);

        ObjectPoolManager.Instance.GetObjectFromPool(deathEffect, transform.position, Quaternion.identity);
        ObjectPoolManager.Instance.ReturnObjectToPool(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if(health < 50f && isNearDeath == false)
        {
            isNearDeath = true;
            OnNearDeathEffect();
        }
        else if(health <= 0)
        {
            OnNearDeathEffect();
            OnDeath();
        }
    }
}
