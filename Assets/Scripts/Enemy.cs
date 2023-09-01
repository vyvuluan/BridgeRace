using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private LayerMask brickLayer;
    private bool isFind = true;
    private bool isFalling = false;
    private IState currentState;
    private Vector3 currentTarget;
    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10.0f);
    }
    private void Update()
    {
        //if (bricks.Count >= 5)
        //{
        //    rb.isKinematic = false;
        //}
        //else
        //{
        //    rb.isKinematic = true;
        //}
        currentState?.OnExecute(this);
        if (isFind)
        {
            isFind = false;
            List<Collider> colliders = Physics.OverlapSphere(transform.position, 10.0f, brickLayer)
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
    }
    public override void Control()
    {

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
        isFalling = true;
        isFind = false;
        ChangeAnim(Constants.FallAnim);
        agent.enabled = false;
        RemoveAllBrick();
    }
    public void Respawn()
    {
        ChangeAnim(Constants.RespawnAnim);
        isFalling = false;
        isFind = true;
        ChangeState(new CollectState());
    }
    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.BrickTag))
        {
            Brick brick = other.GetComponent<Brick>();
            if ((brick.Color == color || brick.Color == BrickColor.Grey) && !isFalling)
            {
                base.AddBrick(brick);
                other.GetComponent<Brick>().OnDespawn();
                isFind = true;
            }
        }
        if (other.CompareTag(Constants.PlayerTag) && !isFalling)
        {
            if (bricks.Count < other.GetComponent<Character>().GetLengthBrick())
            {
                ChangeState(new FallState());
            }
        }
    }
}
