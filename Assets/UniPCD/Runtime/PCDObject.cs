using UnityEngine;

namespace UniPCD
{
  [CreateAssetMenu(fileName = "PCD", menuName = "UniPCD/PCDObject", order = 1)]
  public class PCDObject : ScriptableObject
  {
    [SerializeField]
    public PCD pcd;
  }
}
