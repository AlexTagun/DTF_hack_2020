using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ChildAI : MonoBehaviour
{
    public bool pauseAI = false;

    public void OnDrawGizmos() {
        //Debug.DrawLine(theOriginalCurrentTargetPoint_ + new Vector3(-1f, 0f, 0f), theOriginalCurrentTargetPoint_ + new Vector3(1f, 0f, 0f), Color.green);
        //Debug.DrawLine(theOriginalCurrentTargetPoint_ + new Vector3(0f, -1f, 0f), theOriginalCurrentTargetPoint_ + new Vector3(0f, 1f, 0f), Color.green);
        //
        //Debug.DrawLine(theCurrentTargetPoint_ + new Vector3(-1f, 0f, 0f), theCurrentTargetPoint_ + new Vector3(1f, 0f, 0f), Color.red);
        //Debug.DrawLine(theCurrentTargetPoint_ + new Vector3(0f, -1f, 0f), theCurrentTargetPoint_ + new Vector3(0f, 1f, 0f), Color.red);
    }


    public void moveToPoint(Vector2 inPoint, System.Action inOnEndCallback = null) {
        List<MovingScheduleElement> theSchedule = buildMovingScheduleToPoint(inPoint);
        executeMovingSchedule(theSchedule, inOnEndCallback);
    }

    //Vector3 theOriginalCurrentTargetPoint_ = Vector3.zero;
    //Vector3 theCurrentTargetPoint_ = Vector3.zero;

    private List<MovingScheduleElement> buildMovingScheduleToPoint(Vector2 inPoint) {
        List<MovingScheduleElement> theResult = new List<MovingScheduleElement>();

        Vector2 theCurrentPointOnFloor = getPointOnFloor(transform.position);
        Vector2 theTargetPointOnFloor = getPointOnFloor(inPoint);

        //theOriginalCurrentTargetPoint_ = inPoint;
        //theCurrentTargetPoint_ = theTargetPointOnFloor;

        int theIterationsGuard = 10;
        while (true) {
            FloorDirection theDirection = getFloorDirectionToAchievePosition(theCurrentPointOnFloor.y, theTargetPointOnFloor.y);
            if (FloorDirection.None == theDirection)
                break;

            Stairs theRightNearestStairs = getNearestStairs(theCurrentPointOnFloor, true, theDirection);
            Stairs theLeftNearestStairs = getNearestStairs(theCurrentPointOnFloor, false, theDirection);

            if (!theRightNearestStairs && !theLeftNearestStairs)
                throw new System.Exception("Cannot find any stairs");

            MovingScheduleElement theStairsScheduleElement = new MovingScheduleElement();
            theStairsScheduleElement.movingDirection = theRightNearestStairs ? MovingDirection.Right : MovingDirection.Left;
            theStairsScheduleElement.movingTarget = MovingTarget.MoveUntilStairs;
            theStairsScheduleElement.targetStairs = theRightNearestStairs ? theRightNearestStairs : theLeftNearestStairs;
            theResult.Add(theStairsScheduleElement);

            theCurrentPointOnFloor = getPointOnFloor(theStairsScheduleElement.targetStairs.endPointPosition);

            if (--theIterationsGuard < 0)
                throw new System.Exception("Too many pathfind iterationis");
        }

        MovingScheduleElement theScheduleElement = new MovingScheduleElement();
        theScheduleElement.movingDirection = (theTargetPointOnFloor.x > theCurrentPointOnFloor.x) ? MovingDirection.Right : MovingDirection.Left;
        theScheduleElement.movingTarget = MovingTarget.MoveUntilPoint;
        theScheduleElement.targetPoint = theTargetPointOnFloor;
        theResult.Add(theScheduleElement);

        //Debug.Log("PATH REQUEST {{{");
        //foreach (MovingScheduleElement theElement in theResult)
        //    Debug.Log("[" + theElement.movingDirection + "|" + theElement.movingTarget + "|" + theElement.targetPoint + "|" + theElement.targetStairs + "]");
        //Debug.Log("}}}");

        return theResult;
    }

    private void executeMovingSchedule(List<MovingScheduleElement> inSchedule, System.Action inOnEndCallback) {
        if (null != movingCoroutine)
            StopCoroutine(movingCoroutine);
        movingCoroutine = executeMovingScheduleCoroutine(inSchedule, inOnEndCallback);
        StartCoroutine(movingCoroutine);
    }
    private IEnumerator movingCoroutine = null;

    private IEnumerator executeMovingScheduleCoroutine(List<MovingScheduleElement> inSchedule, System.Action inOnEndCallback) {
        float theMinimumDistance = 2f;

        List<MovingScheduleElement> theSchedule = new List<MovingScheduleElement>(inSchedule);

        Boxed<MovingDirection> theStairsMovingDirection = null;

        System.Action theGetNextScheduleElement = () => {
            theStairsMovingDirection = null;
            theSchedule.RemoveAt(0);
        };

        while (theSchedule.Count > 0) {
            if (pauseAI) yield return null;

            MovingScheduleElement theCurrentScheduleElement = theSchedule[0];

            MovingDirection theActualMovingDirection = theCurrentScheduleElement.movingDirection;

            switch (theCurrentScheduleElement.movingTarget) {
                case MovingTarget.MoveUntilStairs:
                    if (null == theStairsMovingDirection) {
                        if (movementComponent.stairsThatMayBeUsed == theCurrentScheduleElement.targetStairs) {
                            theStairsMovingDirection = new Boxed<MovingDirection>(
                                movementComponent.stairsThatMayBeUsed.isRightOriented ? MovingDirection.Right : MovingDirection.Left);
                            movementComponent.useStairs();

                            theActualMovingDirection = theStairsMovingDirection.value;
                        }
                    } else {
                        theActualMovingDirection = theStairsMovingDirection.value;
                        if (!movementComponent.isUsingStairs)
                            theGetNextScheduleElement();
                    }
                    break;

                case MovingTarget.MoveUntilPoint:
                    float theDistance = (new Vector2(transform.position.x, transform.position.y) -
                        theCurrentScheduleElement.targetPoint).magnitude;
                    if (theDistance < theMinimumDistance)
                        theGetNextScheduleElement();
                    break;
            }

            switch (theActualMovingDirection) {
                case MovingDirection.Left:    movementComponent.moveLeft();   break;
                case MovingDirection.Right:   movementComponent.moveRight();  break;
            }

            yield return null;
        }

        Debug.Log(">>> NEXT 1");

        if (null != inOnEndCallback)
            inOnEndCallback();
    }

    public enum MovingDirection { Left, Right }
    public enum MovingTarget { MoveUntilStairs, MoveUntilPoint }
    public struct MovingScheduleElement
    {
        public MovingDirection movingDirection;
        public MovingTarget movingTarget;
        public Vector2 targetPoint;
        public Stairs targetStairs;
    }

    Stairs getNearestStairs(Vector2 inPointToFindFrom, bool inFindRight, FloorDirection inDirection) {
        float theCastingRadius = 1f;
        float theCastingDistance = 100f;

        float theCastingVectorSign = inFindRight ? 1f : -1f;
        Vector2 theCastingDistanceVector = Vector2.right * theCastingDistance * theCastingVectorSign;
        RaycastHit2D[] theHits = Physics2D.CircleCastAll(
            inPointToFindFrom, theCastingRadius, theCastingDistanceVector);
        Boxed<RaycastHit2D> theNearestHit = getNearestHitByType<Stairs>(inPointToFindFrom, theHits,
            (Stairs inStairs)=>
        {
            return (FloorDirection.Down == inDirection && !inStairs.isForMovingUp) ||
                (FloorDirection.Up == inDirection && inStairs.isForMovingUp);
        });
        return (null != theNearestHit) ? theNearestHit.value.collider.GetComponent<Stairs>() : null;
    }

    Vector2 getPointOnFloor(Vector2 inPointOverPointOnFloor) {
        float theCastingRadius = 1f;
        float theCastingDistance = 100f;

        Vector2 theCastingDistanceVector = Vector2.down * theCastingDistance;
        RaycastHit2D[] theHits = Physics2D.CircleCastAll(
            inPointOverPointOnFloor, theCastingRadius, theCastingDistanceVector);
        Boxed<RaycastHit2D> theNearestBorderHit = getNearestHitByType<Border>(inPointOverPointOnFloor, theHits);
        return theNearestBorderHit.value.centroid;
    }

    private enum FloorDirection { None, Up, Down }
    private static float kFloorHeight = 6f;
    private FloorDirection getFloorDirectionToAchievePosition(float inCurrentY, float inTargetY) {
        float theDeltaY = inTargetY - inCurrentY;

        return Mathf.Abs(theDeltaY) < kFloorHeight ? FloorDirection.None :
            theDeltaY > 0f ? FloorDirection.Up : FloorDirection.Down;
    }

    private class Boxed<Type> {
        public Boxed(Type inValue) { value = inValue; }
        public Type value;
    }
    private Boxed<RaycastHit2D> getNearestHitByType<Type>(Vector2 inPointToFindFrom, RaycastHit2D[] inHits, System.Func<Type, bool> inAcceptingFilter = null) where Type : Component {
        Boxed<RaycastHit2D> theResult = null;
        float theMinimumDistance = float.MaxValue;

        foreach (RaycastHit2D theHit in inHits) {
            Type theComponent = theHit.collider.GetComponent<Type>();
            if (theComponent && (null == inAcceptingFilter || inAcceptingFilter(theComponent))) {
                Vector2 thePosition2D = new Vector2(theComponent.transform.position.x, theComponent.transform.position.y);
                Vector3 theDelta = inPointToFindFrom - thePosition2D;
                float theDistance = new Vector2(theDelta.x, theDelta.y).magnitude;
                if (theDistance < theMinimumDistance) {
                    theResult = new Boxed<RaycastHit2D>(theHit);
                    theMinimumDistance = theDistance;
                }
            }
        }

        return theResult;
    }


    private ChildMoveComponent movementComponent => GetComponent<ChildMoveComponent>();


    //bool theButtonWasPressed = false;
    //private void FixedUpdate() {
    //    if (Input.GetMouseButton(0)) {
    //        if (!theButtonWasPressed) {
    //            theButtonWasPressed = true;
    //            moveToPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    //        }
    //    } else {
    //        theButtonWasPressed = false;
    //    }
    //}
}
