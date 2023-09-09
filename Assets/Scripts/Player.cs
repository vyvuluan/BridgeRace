using UnityEngine;
public class Player : Character
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private LayerMask brickBrigeLayer;
    [SerializeField] private LayerMask wallLayer;
    private bool isControl = true;
    private Vector3 moveVector;
    private RaycastHit hit;
    private RaycastHit hitWall;
    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
    private void Start()
    {
        stages[currentStage].SetActiveBrick(color, true);
    }
    private void Update()
    {
        if (isControl)
        {
            Control();
            BuildBrige();
        }
    }

    public override void Control()
    {
        Vector3 direction = Vector3.zero;
        moveVector = Vector3.zero;
        moveVector.x = joystick.Horizontal * moveSpeed * Time.deltaTime;
        moveVector.z = joystick.Vertical * moveSpeed * Time.deltaTime;
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            direction = Vector3.RotateTowards(transform.forward, moveVector, moveSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(direction);

            ChangeAnim(Constants.RunAnim);
        }
        else if (joystick.Horizontal == 0 || joystick.Vertical == 0)
        {
            ChangeAnim(Constants.IdleAnim);
        }
        Ray ray = new(transform.position, new Vector3(joystick.Horizontal, 0, joystick.Vertical));
        if (Physics.Raycast(ray, out hitWall, 1f, wallLayer))
        {
            moveSpeed = 0f;
            Debug.Log(hitWall.collider.name);
        }
        else moveSpeed = 5f;
        Debug.DrawRay(transform.position, new Vector3(joystick.Horizontal, 0, joystick.Vertical), UnityEngine.Color.red);
        Debug.Log("speed " + moveSpeed);
        rb.MovePosition(rb.position + moveVector);
        //transform.position += moveVector;
    }
    public void BuildBrige()
    {
        Ray ray = new(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, brickBrigeLayer))
        {
            BrickBrige brickBrige = hit.collider.GetComponent<BrickBrige>();
            if ((brickBrige.Color != color || brickBrige.Color == BrickColor.Grey) && bricks.Count > 0)
            {
                Destroy(bricks[^1].gameObject);
                bricks.RemoveAt(bricks.Count - 1);
                brickBrige.Color = color;
                brickBrige.SetMaterial(GameManager.Instance.GetMaterial(color));
            }
        }
    }
    public void SetJoystick(FloatingJoystick floatingJoystick)
    {
        this.joystick = floatingJoystick;
    }
    public void Falling()
    {
        isControl = false;
        ChangeAnim(Constants.FallAnim);
        RemoveAllBrick();
        Invoke(nameof(Respawn), 3f);
    }
    public void Respawn()
    {
        ChangeAnim(Constants.RespawnAnim);
        Invoke(nameof(ResetIdle), 1f);
    }
    public void ResetIdle()
    {
        ChangeAnim(Constants.IdleAnim);
        isControl = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.BrickTag) && isControl)
        {
            Brick brick = other.GetComponent<Brick>();
            if (brick.Color == color || brick.Color == BrickColor.Grey)
            {
                base.AddBrick(brick);
                other.GetComponent<Brick>().OnDespawn();
            }
        }
        if (other.CompareTag(Constants.PlayerTag) && isControl)
        {
            if (bricks.Count < other.GetComponent<Character>().GetLengthBrick())
            {
                Falling();
            }
        }
    }
}
