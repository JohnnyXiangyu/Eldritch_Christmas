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
    Dictionary<string, int> magazine = new Dictionary<string, int>();
    Dictionary<string, GameObject> throwablePrefabs = new Dictionary<string, GameObject>();
    List<string> ammoNames = new List<string>();
    int currentAmmo = 0;

    // ref
    Camera mainCam;

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
    }

    public void AddItem(PickableAmmo newAmmo)
    {
        if (!magazine.ContainsKey(newAmmo.itemName))
        {
            ammoNames.Add(newAmmo.itemName);
            magazine.Add(newAmmo.itemName, 0);
            throwablePrefabs.Add(newAmmo.itemName, newAmmo.throwablePrefab);
        }

        magazine[newAmmo.itemName]++;
    }

    private void ThrowItem(Vector2 target)
    {
        if (ammoNames.Count == 0 || coolDown > 0)
            return;

        if (magazine.ContainsKey(ammoNames[currentAmmo]) && magazine[ammoNames[currentAmmo]] > 0)
        {
            // launch
            Vector3 direction = mainCam.ScreenToWorldPoint(target) - transform.position;
            direction.z = 0;
            if (direction.magnitude <= 0)
                return;
            direction.Normalize();

            if (!playerController.TryThrow(direction))
                return;

            GameObject projectile = Instantiate(throwablePrefabs[ammoNames[currentAmmo]], transform.position + direction * avoidanceRadius, Quaternion.identity);
            projectile.GetComponent<LaunchableAmmo>().Launch(direction);

            // TODO: update player direction

            // start timer
            coolDown = throwCoolDown;
        }
    }

    private void Update()
    {
        if (ammoNames.Count == 0)
            return;

        if (ammoSelection < 0)
        {
            // prev
            currentAmmo--;
            if (currentAmmo < 0)
                currentAmmo = ammoNames.Count - 1;
        }
        else if (ammoSelection > 0)
        {
            // next
            currentAmmo++;
            if (currentAmmo > ammoNames.Count - 1)
                currentAmmo = 0;
        }

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
