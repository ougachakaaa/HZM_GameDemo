using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum VoxelBehaviorType
{
    EmptyVoxel =0, 
    OpenTile =1,
    Obstacle =2,
    EnemySpawn = 3,
}
[CreateAssetMenu(fileName = "Voxel Behavior",menuName = "Map Element/Voxel Behavior")]
public class VoxelBehavior : ScriptableObject
{
    public enum Executor
    {
        MapManager,
        EnemySpawner,
    }
    public Executor executor;
    public VoxelBehaviorType targetType;
    public VoxelBehaviorType originType;
    Map map;
    public Transform targetPrefab;

    //On generating,trigger translucent effect;
    public Transform targetMesh;
    public Transform targetCollider;
    public Color effectColor = new Color(0.1f,0.1f,0.1f,0.1f);

    public byte TargetTypeIndex { get => (byte)targetType; }
    public byte OriginalType { get => (byte)originType; }
    public bool isEnabled;
    public bool isLooping;
    public bool hasAvailableTile { get; private set; }
    [SerializeField]float _executeDuration;
    public float ExecuteDuration { 
        get { return _executeDuration > effectLastTime ? _executeDuration : effectLastTime + 0.1f; } 
    }
    //hide
    public int flashTimes;
    public float effectLastTime;
    [Range(0, 1f)]
    public float density;

    public float executeTimer{ get; private set; }
    int currentVoxelCount;
    public int CurrentVoxelCount 
    { 
        get { return currentVoxelCount; }
        private set { currentVoxelCount = value < 0 ? 0 : value; }
    }
    public int maxVoxelCount { get ; private set; }

    public void InitializeBehavior(Map _map)
    {
        map = _map;
        CheckCount();
        executeTimer = 0f;
        hasAvailableTile = true;
    }
    public void IncreaseCount(int increasement =1)
    {
        currentVoxelCount += increasement;
    }
    public bool CheckTimer()
    {
        if (executeTimer > ExecuteDuration)
        {
            executeTimer = 0f;
            return true;
        }
        else
        {
            executeTimer += Time.deltaTime;
            return false;
        }
    }
    public bool CheckCount()
    {
        currentVoxelCount = map.CheckVoxelCountOfType(TargetTypeIndex);
        maxVoxelCount = Mathf.FloorToInt(map.voxelCount * density);
        return currentVoxelCount < maxVoxelCount-1;
    }
    public void TargetTypeCountCheck()
    {

        if (targetType == VoxelBehaviorType.OpenTile)
        {
            hasAvailableTile = true;
        }
        else
        {
            int openTileCount = 0;
            foreach (byte i in map.voxelMap)
            {
                if (i == (byte)VoxelBehaviorType.OpenTile)
                {
                    openTileCount++;
                }
            }
            hasAvailableTile = openTileCount > map.minimunOpenTileCount;
        }
    }
    

}
