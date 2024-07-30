using UnityEditor;
using UnityEngine;

namespace Editor
{
    public sealed class PalletBoxFiller : EditorWindow
    {
        public GameObject boxPrefab; // Assign your box prefab in the inspector
        public GameObject palletPrefab; // Assign your pallet prefab in the inspector
        public Vector3 boxDimensions = new( 1f, 1f, 1f ); // Dimensions of the box
        public Vector3 palletDimensions = new( 1f, 1f, 1f ); // Dimensions of the pallet
        public float maxHeight = 1.8f; // Maximum height limit for filling

        [MenuItem("Tools/Pallet Box Filler")]
        public static void ShowWindow()
        {
            GetWindow<PalletBoxFiller>("Fill Pallet With Boxes");
        }

        void OnGUI()
        {
            GUILayout.Label( "Settings", EditorStyles.boldLabel );

            boxPrefab = (GameObject) EditorGUILayout.ObjectField( "Box Prefab", boxPrefab, typeof( GameObject ), false );
            palletPrefab = (GameObject) EditorGUILayout.ObjectField( "Pallet Prefab", palletPrefab, typeof( GameObject ), false );
            boxDimensions = EditorGUILayout.Vector3Field( "Box Dimensions", boxDimensions );
            palletDimensions = EditorGUILayout.Vector3Field( "Pallet Dimensions", palletDimensions );
            maxHeight = EditorGUILayout.FloatField( "Maximum Height", maxHeight );

            if (GUILayout.Button( "Fill Pallet" ))
            {
                FillPallet();
            }
        }
        void FillPallet()
        {
            if (boxPrefab == null || palletPrefab == null)
            {
                Debug.LogError( "Box Prefab or Pallet Prefab is not assigned." );
                return;
            }

            GameObject pallet = Instantiate( palletPrefab, Vector3.zero, Quaternion.identity );
            
            Vector3 palletSize = palletDimensions;
            Vector3 boxSize = boxDimensions;

            // Calculate how many boxes fit along each dimension
            int boxesAlongX = Mathf.FloorToInt( palletSize.x / boxSize.x );
            int boxesAlongY = Mathf.FloorToInt( maxHeight / boxSize.y );
            int boxesAlongZ = Mathf.FloorToInt( palletSize.z / boxSize.z );

            // Adjust Y dimension if exceeding maxHeight
            if (boxSize.y * boxesAlongY > maxHeight)
            {
                boxesAlongY = Mathf.FloorToInt( maxHeight / boxSize.y );
            }
            
            // Calculate padding
            float paddingX = (palletSize.x - (boxesAlongX * boxSize.x)) / 2f;
            float paddingZ = (palletSize.z - (boxesAlongZ * boxSize.z)) / 2f;

            // Corner of pallet
            float xStart = pallet.transform.position.x - (palletSize.x / 2) + paddingX + (boxSize.x / 2);
            float yStart = pallet.transform.position.y + palletSize.y + boxSize.y / 2;
            float zStart = pallet.transform.position.z - (palletSize.z / 2) + paddingZ + (boxSize.z / 2);

            for (int x = 0; x < boxesAlongX; x++)
            {
                for (int y = 0; y < boxesAlongY; y++)
                {
                    for (int z = 0; z < boxesAlongZ; z++)
                    {
                        Vector3 pos = new Vector3(
                            xStart + x * boxSize.x,
                            yStart + y * boxSize.y,
                            zStart + z * boxSize.z
                        );

                        // Instantiate the box at the calculated position
                        GameObject box = Instantiate(boxPrefab, pos, Quaternion.identity);
                        box.transform.parent = pallet.transform; // Parent to pallet for organization
                        Debug.Log("Box Made");
                    }
                }
            }

            Debug.Log("Pallet filled with boxes.");
        }
    }
}
