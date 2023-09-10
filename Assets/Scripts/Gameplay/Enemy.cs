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
    private int randomNumber = 5;
    private bool isFind = true;
    private bool isCheckCollision = true;
    private bool isBuild = false;
    private IState currentState;
    private Vector3 currentTarget;
    private Brige brige;

    public bool IsBuild { get => isBuild; }

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
        if (currentState is BuildState && brige.IsLock)
        {
            transform.position = new(transform.position.x, transform.position.y, transform.position.z - 8f);
        }
        currentState?.OnExecute(this);
        Control();

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

        //Debug.Log(random);
        if (bricks.Count > randomNumber && !isBuild)
        {
            isBuild = true;
            ChangeState(new BuildState());
        }
    }
    public override void NextStage()
    {
        if (currentStage < stages.Count - 1)
        {
            brige.NextStep(color);
            stages[currentStage].RemoveBrigeByIsLock();
            isBuild = false;
            isFind = true;
            isCheckCollision = true;
            Debug.Log("trc" + currentStage);
            base.NextStage();
            Debug.Log("sau" + currentStage);
            //if (currentStage < stages.Count)
            SelectBrige();
            ChangeState(new CollectState());
        }
        else
        {
            brige.NextStep(color);
            ChangeState(new WinState());
        }

    }
    public void SelectBrige()
    {
        if (stages[currentStage].GetLengthBrige() <= 0)
        {
            ChangeState(new FailState());
        }
        else
        {
            indexBrige = stages[currentStage].RandomBrige();
            brige = stages[currentStage].GetBriges(indexBrige);
        }
    }
    public void BuildBrige(ref int i)
    {
        if (!brige.IsLock)
        {
            isFind = false;
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
        else
        {
            SelectBrige();
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
    public void ChangeState(IState newState)
    {
        currentState?.OnExit(this);
        currentState = newState;
        currentState?.OnEnter(this);
    }
    public void Moving()
    {
        isBuild = false;
        isCheckCollision = true;
        isFind = true;
        randomNumber = Random.Range(5, 9);
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
    public void Fail()
    {
        ChangeAnim(Constants.IdleAnim);
        ChangeState(null);
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
                if (currentState is CollectState)
                {
                    isFind = true;
                }
            }
        }
        if (other.CompareTag(Constants.PlayerTag) && isCheckCollision && !isBuild)
        {
            if (bricks.Count < other.GetComponent<Character>().GetLengthBrick())
            {
                ChangeState(new FallState());
            }
        }
    }
}
