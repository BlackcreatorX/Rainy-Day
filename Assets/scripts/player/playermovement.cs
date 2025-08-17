using System.Collections;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpDuration = 0.5f;
    [SerializeField] private float groundCheckRadius = 0.2f;
    //[SerializeField] private int wallJumpThroughLayer = 8;
    [SerializeField] private LayerMask wallJumpThroughLayer;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    private Vector2 movement;
    private Vector2 lastMovement; 
    private bool isJumping = false;
    private Color originalColor;

    // <- This method initializes components and sets up the Rigidbody
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        if (rb != null)
        {
            rb.gravityScale = 0f;
        }
    }

    // <- This method checks for player input each frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        movement = new Vector2(horizontalInput, verticalInput).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            lastMovement = movement;
            StartCoroutine(JumpAnimation());
        }
    }

    // <- This method applies physics-based movement
    void FixedUpdate()
    {
        if (rb != null)
        {
            if (isJumping)
            {
                rb.linearVelocity = lastMovement * moveSpeed;
            }
            else
            {
                rb.linearVelocity = movement * moveSpeed;
            }
        }
    }

    // <- This coroutine handles the jump state, visuals, and collision ignoring
    IEnumerator JumpAnimation()
    {
        isJumping = true;

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "playerButJumping";
            spriteRenderer.color = Color.magenta;
        }

        Physics2D.IgnoreLayerCollision(gameObject.layer, wallJumpThroughLayer, true);

        yield return new WaitForSeconds(jumpDuration);

        HandleLanding();

        Physics2D.IgnoreLayerCollision(gameObject.layer, wallJumpThroughLayer, false);

        isJumping = false;

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "player";
            spriteRenderer.color = originalColor;
        }
    }

    // <- This method handles the logic for when the player lands after a jump
    private void HandleLanding()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, groundCheckRadius, ~0);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject && !collider.isTrigger)
            {
                Vector2 closestPoint = collider.ClosestPoint(transform.position);
                Vector2 direction = (Vector2)transform.position - closestPoint;
                
                if (direction.sqrMagnitude < groundCheckRadius * groundCheckRadius)
                {
                    Vector2 safePosition = closestPoint + direction.normalized * groundCheckRadius;
                    transform.position = new Vector3(safePosition.x, safePosition.y, transform.position.z);
                    break; 
                }
            }
        }
    }

    // <- This method draws a helpful debug sphere in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }
}
