using UnityEditor;
using UnityEngine;

namespace Editor
{
    public sealed class PalletStackGenerator : EditorWindow
    {
        public GameObject palletPrefab; // Assign your pallet prefab in the inspector
        public int stackHeight = 10;
        public float rotationVariance = 5f; // Maximum rotation around the Y axis
        public float translationVariance = 0.1f; // Maximum translation on X and Y axes

        public Vector3 palletDimensions = new( 1f, 0.14f, 1.22f );

        [MenuItem( "Tools/Pallet Stack Generator" )]
        public static void ShowWindow()
        {
            GetWindow<PalletStackGenerator>( "Create Pallet Stack" );
        }

        void OnGUI()
        {
            GUILayout.Label( "Stack Pallets Settings", EditorStyles.boldLabel );

            palletPrefab = (GameObject) EditorGUILayout.ObjectField( "Pallet Prefab", palletPrefab, typeof( GameObject ), false );
            stackHeight = EditorGUILayout.IntField( "Stack Height", stackHeight );
            rotationVariance = EditorGUILayout.FloatField( "Rotation Variance (degrees)", rotationVariance );
            translationVariance = EditorGUILayout.FloatField( "Translation Variance (units)", translationVariance );

            if (GUILayout.Button( "Generate Pallet Stack" ))
            {
                GeneratePalletStack();
            }
        }
        void GeneratePalletStack()
        {
            if (palletPrefab == null)
            {
                Debug.LogError( "Pallet Prefab is not assigned." );
                return;
            }

            GameObject stack = Instantiate( new GameObject( "Pallet Stack" ) );

            for ( int i = 0; i < stackHeight; i++ )
            {
                GameObject pallet = Instantiate( palletPrefab, GetStackPosition( i ), GetStackRotation( i ) );
                pallet.transform.parent = stack.transform;
            }

            Debug.Log( "Pallet stack generated." );
        }
        Vector3 GetStackPosition( int index )
        {
            // Stack height increment
            float yOffset = index * (palletDimensions.y * 0.9f); // Slight gap between pallets
            float xOffset = Random.Range( -translationVariance, translationVariance );
            float zOffset = Random.Range( -translationVariance, translationVariance );
            return new Vector3( xOffset, yOffset, zOffset );
        }
        Quaternion GetStackRotation( int index )
        {
            // Random rotation around the Y axis
            float yRotation = Random.Range( -rotationVariance, rotationVariance );
            return Quaternion.Euler( 0, yRotation, 0 );
        }
    }
}
