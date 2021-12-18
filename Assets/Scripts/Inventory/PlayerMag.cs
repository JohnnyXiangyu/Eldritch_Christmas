using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMag : MonoBehaviour
{
    [SerializeField] float throwCoolDown = 0.5f;
    [SerializeField] float avoidanceRadius = 0.5f;
    [SerializeField] Player playerController = null;

    InputActions input;

    // inventory management
    Dictionary<string, GameObject> throwablePrefabs = new Dictionary<string, GameObject>();
    List<string> ammoNames = new List<string>();

    // ref
    Camera mainCam;
    PlayerInventory playerInventory;

    // to handle scroll movement
    float ammoSelection = 0;
    float coolDown = 0;

    private void Awake()
    {
        input = new InputActions();
        
        input.Gameplay.Throw.performed += context => ThrowItem(Mouse.current.position.ReadValue());

        input.Gameplay.ChangeAmmo.performed += context => ammoSelection = context.ReadValue<Vector2>().y;
    }

    private void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        if (playerController == null)
        {
            playerController = GetComponent<Player>();
        }
        playerInventory = GetComponent<PlayerInventory>();
    }

    public void AddItem(PickableAmmo newAmmo)
    {
            ammoNames.Add(newAmmo.itemName);
            throwablePrefabs.Add(newAmmo.itemName, newAmmo.throwablePrefab);
    }

    private void ThrowItem(Vector2 target)
    {
        if (ammoNames.Count == 0 || coolDown > 0)
            return;

        if (ammoNames.Count > 0)
        {
            var newAmmo = ammoNames[0];
            ammoNames.RemoveAt(0);

            // launch
            Vector3 direction = mainCam.ScreenToWorldPoint(target) - transform.position;
            direction.z = 0;
            if (direction.magnitude <= 0)
                return;
            direction.Normalize();

            if (!playerController.TryThrow(direction))
                return;

            GameObject projectile = Instantiate(throwablePrefabs[newAmmo], transform.position + direction * avoidanceRadius, Quaternion.identity);
            projectile.GetComponent<LaunchableAmmo>().Launch(direction);

            // start timer
            coolDown = throwCoolDown;

            // update inventory
            playerInventory.RemoveOne(newAmmo);
        }
    }

    private void Update()
    {
        if (ammoNames.Count == 0)
            return;

        if (coolDown > 0)
            coolDown -= Time.deltaTime;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
