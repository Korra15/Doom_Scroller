using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(SpriteRenderer))]
public class Entity : MonoBehaviour
{
    const string HITBOX_LAYER = "Hitbox";
    const string HURTBOX_LAYER = "Hurtbox";

    // -- (( PROTECTED DATA )) ------------------- >>
    protected SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();
    protected Dictionary<Entity, Collider2D> externalEntityColliders = new Dictionary<Entity, Collider2D>();

    // -- (( SERIALIZED FIELDS )) ---------------- >>
    [Header("Health")]
    [SerializeField] int _maxHealth = 10;
    [SerializeField] int _currHealth = 10;

    [Header("Damage")]
    [SerializeField] int _attackDamage = 1;
    [SerializeField, Range(0, 1)] float _damageReduction = 1f; // 0 = no damage, 1 = full damage

    [Header("Stagger")]
    [SerializeField] int _maxStagger = 10;
    [SerializeField] int _currStagger = 10;
    [SerializeField] float _staggerTime = 0.25f;

    [Header("Hitbox")]
    [SerializeField] Collider2D _hitboxCollider;
    [SerializeField] Collider2D _hurtboxCollider;


    // -- (( PUBLIC PROPERTIES )) ---------------- >>
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int CurrentHealth { get => _currHealth; set => _currHealth = value; }
    public int AttackDamage { get => _attackDamage; set => _attackDamage = value; }
    public float DamageReduction { get => _damageReduction; set => _damageReduction = value; }
    public int MaxStagger { get => _maxStagger; set => _maxStagger = value; }
    public int CurrentStagger { get => _currStagger; set => _currStagger = value; }
    protected float StaggerTime { get => _staggerTime; set => _staggerTime = value; }

    // TODO : Move player specific data to Player class
    [HideInInspector] public bool IsBlocking = false; // Checks whether or not the player is currently blocking.
    [HideInInspector] public bool IsParrying = false; // Check if the player currenly parrying.

    // TODO : Implement basic health and stagger bars
    /*
    public Image HealthBar;
    public Image StaggerBar;
    */

    #region ==================== [[ BASE UNITY METHODS ]] ==================== //
    public void Awake()
    {
        GenerateCollider(ref _hitboxCollider, "Hitbox", HITBOX_LAYER, typeof(BoxCollider2D));
        GenerateCollider(ref _hurtboxCollider, "Hurtbox", HURTBOX_LAYER, typeof(PolygonCollider2D));
    }

    protected virtual void FixedUpdate()
    {
        // Assign the current physics shape to the hurtbox collider
        if (_hurtboxCollider != null && _hurtboxCollider is PolygonCollider2D)
        {
            AssignCurrentPhysicsShapeToCollider((PolygonCollider2D)_hurtboxCollider);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // IMPORTANT: Collisions are disabled between Hurtbox/Hurtbox and Hitbox/Hitbox. 
        // This way only when a Hurtbox and Hitbox collide with each other will this function run.

        // << TRACK ENTERING HURTBOX COLLISIONS >>
        // if the hurtbox is colliding with itself, ignore it.
        if (other.transform == this.transform) return;
        if (other.transform.parent == this.transform) return;

        // if the other object is not attached to an entity, ignore it.
        if (other.GetComponentInParent<Entity>() == null) return;

        // if the other object is not a hurtbox, ignore it.
        if (other.gameObject.layer != LayerMask.NameToLayer(HURTBOX_LAYER)) return;

        // check if the other entity is already in the list of external entity colliders
        Entity otherEntity = other.GetComponentInParent<Entity>();
        if (!externalEntityColliders.ContainsKey(otherEntity))
        {
            // if the other entity is not in the list, add it.
            externalEntityColliders[otherEntity] = other;

            // apply damage from other entity to this entity
            TakeDamage(otherEntity.AttackDamage);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        // << TRACK EXITING HURTBOX COLLISIONS >>
        // if the hurtbox is colliding with itself, ignore it.
        if (other.transform == this.transform) return;
        if (other.transform.parent == this.transform) return;

        // if the other object is not attached to an entity, ignore it.
        if (other.GetComponentInParent<Entity>() == null) return;

        // if the other object is not a hurtbox, ignore it.
        if (other.gameObject.layer != LayerMask.NameToLayer(HURTBOX_LAYER)) return;

        // check if the other entity is already in the list of external entity colliders
        Entity otherEntity = other.GetComponentInParent<Entity>();
        if (externalEntityColliders.ContainsKey(otherEntity))
        {
            // if the other entity is in the list, remove it.
            externalEntityColliders.Remove(otherEntity);
        }
    }
    #endregion

    #region ==================== [[ PUBLIC METHODS ]] ==================== //
    public void TakeDamage(int damage)
    {
        CurrentHealth -= (int)(damage * DamageReduction);
        Debug.Log($"{name} took {damage} damage. Current Health: {CurrentHealth}");
    }
    #endregion

    #region ==================== [[ PRIVATE HELPER METHODS ]] ==================== //
    /// <summary>
    /// Generates a collider of the specified type if it does not exist.
    /// </summary>
    /// <param name="collider">
    ///     The collider variable to check and generate if it does not exist.
    /// </param>
    /// <param name="name">
    ///     The name of the collider object.
    /// </param>
    /// <param name="layerName">
    ///     The name of the layer to assign to the collider object.
    /// </param>
    /// <param name="colliderType">
    ///     The type of collider to generate.
    /// </param>
    void GenerateCollider(ref Collider2D collider, string name, string layerName, Type colliderType)
    {
        if (collider == null)
        {
            GameObject colliderObject = new GameObject(name);
            colliderObject.layer = LayerMask.NameToLayer(layerName);
            colliderObject.transform.SetParent(transform);
            colliderObject.transform.localPosition = Vector2.zero;
            colliderObject.transform.localScale = Vector2.one;

            collider = (Collider2D)colliderObject.AddComponent(colliderType);
            collider.isTrigger = true;
        } else {
            print("Generate Collider doing nothing " + collider);
        }
    }

    /// <summary>
    /// Assign the current physics shape of the sprite to the specified collider.
    ///     Unity doesn't like handling colliders outside of the update loop, so this function needs to be called in the LateUpdate() method.
    /// [ Originally created by Gene Park. ]
    /// </summary>
    /// <param name="collider">
    ///     The collider to assign the physics shape to.
    /// </param>
    void AssignCurrentPhysicsShapeToCollider(PolygonCollider2D collider)
    {
        if (collider == null) return; //If collider is null do nothing.
        // print("COLLIDER != NULL");

        // Get the sprite for the current state
        Sprite currentSprite = spriteRenderer.sprite;
        if (currentSprite == null) return;
        // print("CURRENTSPRITE != NULL");

        // Get the number of paths in the physics shape
        int pathCount = currentSprite.GetPhysicsShapeCount();
        
        //This wasn't an issue before, but it is now for some reason. Not sure why exactly.
        if (pathCount == 0) {
            collider.enabled = false;
            return;
        } else {
            collider.enabled = true;
        }

        // Pre-allocate a list to hold the points
        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < pathCount; i++)
        {
            // Clear the list to reuse it
            points.Clear();

            // Get the physics shape for the current path
            currentSprite.GetPhysicsShape(i, points);

            // Set the path to the collider
            collider.SetPath(i, points);
        }
    }
    #endregion
}


#if UNITY_EDITOR
[CustomEditor(typeof(Entity))]
public class EntityCustomEditor : Editor
{
    SerializedObject _serializedObject;
    Entity _script;
    private void OnEnable()
    {
        _serializedObject = new SerializedObject(target);
        _script = (Entity)target;

        // Call the Awake method of the script to generate the colliders
        _script.Awake();
    }

    public override void OnInspectorGUI()
    {
        _serializedObject.Update();

        EditorGUI.BeginChangeCheck();

        base.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck())
        {
            _serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif