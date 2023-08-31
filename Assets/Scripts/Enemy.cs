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
        Debug.Log(gameObject.name + ": " + currentState);
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
        currentState?.OnExecute(this);
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
        agent.destination = currentTarget;
        ChangeAnim(Constants.RunAnim);
    }
    public void Falling(Transform rival)
    {
        isFind = false;
        Vector3 direction = (rival.transform.position - transform.position).normalized;
        ChangeAnim(Constants.FallAnim);
        agent.enabled = false;
        //ApplyKnockback(direction);
    }
    public void Respawn()
    {
        isFind = true;
        agent.enabled = true;
        ChangeAnim(Constants.RespawnAnim);
        Invoke(nameof(ResetIdle), 1f);
    }
    public void ResetIdle()
    {
        ChangeAnim(Constants.IdleAnim);
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.PlayerTag))
        {
            Debug.Log("luan");
            Falling(collision.transform);
            ChangeState(new FallState());
        }
    }


}
