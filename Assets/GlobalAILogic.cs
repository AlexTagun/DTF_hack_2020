using UnityEngine;

public class GlobalAILogic : MonoBehaviour
{
    void Awake() {
        _childAIs = FindObjectsOfType<ChildAI>();
        _AIZoneColliders = GetComponents<Collider2D>();

        foreach (ChildAI theChildAI in _childAIs)
            getNextRandomPointForAI(theChildAI);
    }

    Vector2 getRandomPointInAIZone() {
        int theRandomColliderIndex = Random.Range(0, _AIZoneColliders.Length);
        Bounds theColliderBounds = _AIZoneColliders[theRandomColliderIndex].bounds;

        return new Vector2(
            Random.Range(theColliderBounds.min.x, theColliderBounds.max.x),
            Random.Range(theColliderBounds.min.y, theColliderBounds.max.y));
    }

    void getNextRandomPointForAI(ChildAI inAI) {
        inAI.moveToPoint(getRandomPointInAIZone(), ()=>{
            getNextRandomPointForAI(inAI);
        });
    }

    private ChildAI[] _childAIs = null;
    private Collider2D[] _AIZoneColliders = null;
}
