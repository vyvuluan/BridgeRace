using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private BrickColor color;
    [SerializeField] private LayerMask brickLayer;
    [SerializeField] private List<Brick> targets;
    private bool isFind = false;
    private IState currentState;
    private Vector3 currentTarget;

    public BrickColor Color { get => color; set => color = value; }

    private void Start()
    {
        targets = base.GetStageCurrent().FindBrickByColor(color);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10.0f);
    }
    private void Update()
    {
        if (!isFind)
        {
            isFind = true;
            List<Collider> colliders = Physics.OverlapSphere(transform.position, 10.0f, brickLayer).ToList();
            colliders = colliders.Where(n => n.GetComponent<Brick>().Color == color).ToList();
            currentTarget = colliders[0].transform.position;
            Debug.Log(colliders[0].transform.position);
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
        //Debug.Log(currentTarget);
        agent.destination = currentTarget;
        ChangeAnim(Constants.RunAnim);
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
                other.GetComponent<Brick>().OnDespawn();
                isFind = false;
            }
        }
    }

}
