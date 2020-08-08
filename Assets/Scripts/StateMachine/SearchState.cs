using System;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : IState
{
    private GameObject owner;
    private LayerMask searchLayer;
    private float searchRadius;
    private string targetTag;
    private Action<SearchResult> searchResultCallback;

    public bool IsSearchComplete { get; private set; }

    public SearchState(
        GameObject owner, 
        LayerMask searchLayer,
        float searchRadius, 
        string targetTag,
        Action<SearchResult> searchResultCallback)
    {
        this.owner = owner;
        this.searchLayer = searchLayer;
        this.searchRadius = searchRadius;
        this.targetTag = targetTag;
        this.searchResultCallback = searchResultCallback;
    }

    public void EnterState()
    {
        IsSearchComplete = false;
    }

    public void ExecuteState()
    {
        if (!IsSearchComplete)
        {
            FindTarget();
        }           
    }

    public void ExitState()
    {
        //Debug.LogWarning(owner.name + " Exited from search state!");
    }

    private void FindTarget()
    {           
        var hitObjects = Physics.OverlapSphere(owner.transform.position, searchRadius, searchLayer);

        var allHitObjectWithRequiredTags = new Stack<Collider>();

        foreach (var target in hitObjects)
        {
            if (target.CompareTag(targetTag))
            {
                allHitObjectWithRequiredTags.Push(target);
            }
        }

        if(allHitObjectWithRequiredTags.Count > 0)
        {
            //Debug.LogError("Seach done: " + owner.name + " " + allHitObjectWithRequiredTags.Count);
            var searchResult = new SearchResult(hitObjects, allHitObjectWithRequiredTags);
            searchResultCallback(searchResult);
            IsSearchComplete = true;
        }                                        
    }
}

public class SearchResult
{
    public Collider[] AllHitObjectsInSearchRadius;
    public Stack<Collider> AllHitObjectWithRequiredTags;

    public SearchResult(Collider[] allHitObjectsInSearchRadius, Stack<Collider> allHitObjectWithRequiredTags)
    {
        AllHitObjectsInSearchRadius = allHitObjectsInSearchRadius;
        AllHitObjectWithRequiredTags = allHitObjectWithRequiredTags;
    }
}
