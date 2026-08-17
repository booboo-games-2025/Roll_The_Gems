using System.Collections.Generic;
using UnityEngine;

public class BuildingSetup : MonoBehaviour
{
    [SerializeField] private List<Transform> _buildingPositionRefs;

    public int GetNumberOfBuildings() => _buildingPositionRefs.Count;

    public List<Transform> GetBuildingPositionRefs() => _buildingPositionRefs;
}
