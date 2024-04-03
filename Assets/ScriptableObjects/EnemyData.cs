using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject {
    [Space(5), Header("Chasing and Attacking"), Space(5)]
    public float chaseDistanceThreshold;
    public float attackDistanceThreshold;

    [Space(5), Header("Line of Sight"), Space(5)]
    public float sightDistance;
    public float sightAngle;
}
