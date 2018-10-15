using UnityEngine;

public abstract class BaseTurret : SelectableObject
{
    #region VARIABLES

    private StateMachine stateMachine = new StateMachine();
    private LayerMask searchLayer;
    private Transform rotatingPart;
    private readonly string targetTag = "Enemy";

    protected Ray ray;
    protected RaycastHit raycastHit;
    protected Transform firePoint;
    protected float fireRate;
    protected float fireCountdown;
    protected float turrentAttackRadius;
    protected float rotationSpeed;

    #endregion VARIABLES

    #region PROPERTIES

    public Unit CurrentTarget
    {
        get;
        private set;
    }
    public bool IsCurrentTargetValid
    {
        get
        {
            return CurrentTarget != null && CurrentTarget.gameObject.activeSelf == true;
        }
    }

    #endregion PROPERTIES

    protected abstract void Attack();
    protected abstract void StopAttack();

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        rotatingPart = transform.Find("RotatingPart");
        firePoint = rotatingPart.Find("FirePoint");
        searchLayer = LayerMask.GetMask("Target");
        SelectableRadius = turrentAttackRadius;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        stateMachine.ChangeState
            (
            new SearchState
            (gameObject, 
            searchLayer, 
            turrentAttackRadius, 
            targetTag, 
            OnTargetFound
            ));
    }

    private void Update()
    {
        stateMachine.ExecuteState();
    }

    private Unit GetNearestUnit(Collider[] allTargets)
    {
        //Debug.LogWarning("Searching target!");

        if (allTargets.Length > 0)
        {
            int closestIndex = 0;
            float nearestDistance = Vector3.SqrMagnitude(transform.position - allTargets[0].transform.position);

            for (int i = 1; i < allTargets.Length; i++)
            {
                float distance = Vector3.SqrMagnitude(transform.position - allTargets[i].transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    closestIndex = i;
                }
            }
            return allTargets[closestIndex].GetComponent<Unit>();
        }

        return null;
    }

    private void OnTargetFound(SearchResult searchResult)
    {
        //Debug.LogError("Target found!");

        var allTargets = searchResult.AllHitObjectWithRequiredTags;
        var allTargetsArray = allTargets.ToArray();

        CurrentTarget = GetNearestUnit(allTargetsArray);

            stateMachine.ChangeState( new TargetingState
                (
                CurrentTarget.transform,
                rotatingPart,
                rotationSpeed,
                OnTargetInSight
                ));
    }

    private void OnTargetInSight()
    {
        //Debug.LogError("Target in sight!");

        ray = new Ray(rotatingPart.position, rotatingPart.forward * turrentAttackRadius);

        Debug.DrawRay(rotatingPart.position, rotatingPart.forward * turrentAttackRadius, Color.white);

        if (Physics.Raycast(ray, out raycastHit, turrentAttackRadius))
        {
            if (raycastHit.collider.CompareTag(targetTag))
            {
                if (IsCurrentTargetValid)
                {
                    Debug.DrawRay(rotatingPart.position, rotatingPart.forward * turrentAttackRadius, Color.red);
                    Attack();
                    return;
                }
            }
            else
            {
                CurrentTarget = null;
                StopAttack();

                stateMachine.ChangeState(new SearchState
                       (
                       gameObject,
                       searchLayer,
                       turrentAttackRadius,
                       targetTag,
                       OnTargetFound
                       ));
            }
        }
        else
        {
            CurrentTarget = null;
            StopAttack();

            stateMachine.ChangeState(new SearchState
                   (
                   gameObject,
                   searchLayer,
                   turrentAttackRadius,
                   targetTag,
                   OnTargetFound
                   ));
        }
    }
}
