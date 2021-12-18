using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnableEnemy : MonoBehaviour
{
    [SerializeField] string enemyType = "";

    public string GetEnemyType()
    {
        return enemyType;
    }
}
