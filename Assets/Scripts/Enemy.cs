using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private LayerMask brickLayer;
    [SerializeField] private LayerMask brickBrigeLayer;
    [SerializeField] private NavMeshAgent agent;
    private int indexBrige;
    private bool isFind = true;
    private bool isCheckCollision = true;
    private bool isBuild = false;
    private IState currentState;
    private Vector3 currentTarget;
    private Brige brige;
    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10.0f);
    }
    private void Start()
    {
        SelectBrige();
        stages[currentStage].SetActiveBrick(color, true);


    }
    private void Update()
    {
        currentState?.OnExecute(this);
        Control();
        Debug.Log(currentTarget);
    }
    public void SelectBrige()
    {
        indexBrige = stages[currentStage].RandomBrige();
        brige = stages[currentStage].GetBriges(indexBrige);
    }
    public override void Control()
    {
        if (isFind)
        {
            isFind = false;
            List<Collider> colliders = Physics.OverlapSphere(transform.position, 15.0f, brickLayer)
                .Where(n => n.GetComponent<Brick>().Color == color || n.GetComponent<Brick>().Color == BrickColor.Grey)
                .ToList();

            if (colliders.Count > 0)
            {
                currentTarget = colliders
                    .OrderBy(col => Vector3.Distance(transform.position, col.transform.position))
                    .First()
                    .transform.position;

                ChangeState(new CollectState());
            }

        }
        if (bricks.Count > 5 && !isBuild)
        {
            isBuild = true;
            ChangeState(new BuildState());
        }
    }
    public void BuildBrige(ref int i)
    {
        isFind = false;
        isCheckCollision = false;

        agent.SetDestination(brige.GetBrickByIndex(i).transform.position);
        float distanceToDestination = agent.remainingDistance;

        if (distanceToDestination < 0.1f)
        {
            if (brige.GetBrickByIndex(i).Color == BrickColor.Grey || brige.GetBrickByIndex(i).Color != color)
            {
                Destroy(bricks[^1].gameObject);
                bricks.RemoveAt(bricks.Count - 1);
                brige.GetBrickByIndex(i).Color = color;
                brige.GetBrickByIndex(i).SetMaterial(GameManager.Instance.GetMaterial(color));
            }
            i++;
        }

        if (bricks.Count <= 0)
        {
            isFind = true;
            isCheckCollision = true;
            isBuild = false;
            ChangeState(new CollectState());
        }
    }
    public int CheckNextStage(int i)
    {
        if (i >= brige.GetCountBrickBrige())
        {
            return brige.CheckCompleteBuild(color) ? 1 : 0;
        }
        return 2;
    }

    public override void NextStage()
    {
        base.NextStage();
        brige.NextStep();
        isBuild = false;
        isFind = true;
        isCheckCollision = true;
        SelectBrige();
        //Invoke(nameof(WaitNextState), 1f);
        ChangeState(new CollectState());
    }
    public void ChangeState(IState newState)
    {
        currentState?.OnExit(this);
        currentState = newState;
        currentState?.OnEnter(this);
    }
    public void Moving()
    {
        agent.enabled = true;
        ChangeAnim(Constants.RunAnim);
        agent.destination = currentTarget;
    }
    public void Falling()
    {
        isCheckCollision = false;
        isFind = false;
        ChangeAnim(Constants.FallAnim);
        agent.enabled = false;
        RemoveAllBrick();
    }
    public void Respawn()
    {
        ChangeAnim(Constants.RespawnAnim);
        isCheckCollision = true;
        isFind = true;
        ChangeState(new CollectState());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.BrickTag))
        {
            Brick brick = other.GetComponent<Brick>();
            if ((brick.Color == color || brick.Color == BrickColor.Grey) && isCheckCollision)
            {
                AddBrick(brick);
                other.GetComponent<Brick>().OnDespawn();
                isFind = true;
            }
        }
        if (other.CompareTag(Constants.PlayerTag) && isCheckCollision)
        {
            if (bricks.Count < other.GetComponent<Character>().GetLengthBrick())
            {
                ChangeState(new FallState());
            }
        }
    }
}
