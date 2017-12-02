using UnityEngine;

namespace Helpers
{
    public class ManagersHelper : MonoBehaviour
    {
        private void Start()
        {
            FieldManager = GetComponent<FieldManager>();
            MonstersManager = GetComponent<MonstersManager>();
            TowerManager = GetComponent<TowerManager>();
        }

        public static FieldManager FieldManager { private set; get; }
        public static MonstersManager MonstersManager { private set; get; }
        public static TowerManager TowerManager { private set; get; }
    }
}