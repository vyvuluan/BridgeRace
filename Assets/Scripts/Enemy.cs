using DG.Tweening;
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
    private IState currentState;
    private Vector3 currentTarget;
    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10.0f);
    }
    private void Update()
    {
        if (bricks.Count >= 5)
        {
            rb.isKinematic = false;
        }
        else
        {
            rb.isKinematic = true;
        }
        currentState?.OnExecute(this);
        if (isFind)
        {
            isFind = false;
            List<Collider> colliders = Physics.OverlapSphere(transform.position, 10.0f, brickLayer).ToList();
            colliders = colliders.Where(n => n.GetComponent<Brick>().Color == color).ToList();
            currentTarget = colliders[0].transform.position;
            float distance = Vector3.Distance(transform.position, colliders[0].transform.position);
            for (int i = 1; i < colliders.Count; i++)
            {
                float temp = Vector3.Distance(transform.position, colliders[i].transform.position);
                if (distance > temp)
                {
                    currentTarget = colliders[i].transform.position;
                    distance = temp;
                }
            }
            ChangeState(new CollectState());


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
        ChangeAnim(Constants.RunAnim);
        agent.destination = currentTarget;
    }
    public void Falling()
    {
        isFind = false;
        ChangeAnim(Constants.FallAnim);
        agent.enabled = false;
        foreach (var brick in bricks)
        {
            Vector3 temp = brick.transform.position;
            temp.x += Random.Range(0f, 1.5f);
            temp.z += Random.Range(0f, 1.5f);
            brick.transform.DOMove(temp, 0.3f).OnComplete(() =>
            {
                temp.y = 2.5f;
                brick.transform.DOMove(temp, 0.4f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    brick.transform.SetParent(null);
                    brick.SetMaterial(GameManager.Instance.GetMaterial(BrickColor.Grey));
                    brick.Color = BrickColor.Grey;
                });
            });
        }
        bricks.Clear();
    }
    public void Respawn()
    {
        ChangeAnim(Constants.RespawnAnim);
        Invoke(nameof(ResetIdle), 1f);
    }
    public void ResetIdle()
    {
        ChangeAnim(Constants.IdleAnim);
        isFind = true;
        agent.enabled = true;
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
            if (brick.Color == color)
            {
                base.AddBrick(brick);
                other.GetComponent<Brick>().OnDespawn();
                isFind = true;
            }
        }
        if (other.CompareTag(Constants.PlayerTag))
        {
            if (bricks.Count < other.GetComponent<Character>().GetLengthBrick())
            {
                ChangeState(new FallState());
            }
        }
    }
}
