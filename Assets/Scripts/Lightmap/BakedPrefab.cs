using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FoxWork
{
    public class BakedPrefab : MonoBehaviour
    {
        /// <summary>
        /// Store lightmaps and lightmaps infos and lights related data in prefab and assign them back when instantiated
        /// </summary>
        /// <remarks>
        /// 1. When prefab instantiated the scene must have LightmapData file.
        /// Also scene needed to be baked (even with nothing in it) and with similar parameters
        /// (i.e: can't mix non-directional light baked prefab in a directional light baked scene).
        /// 2. Sets the baked lights parameters alreadyLightmapped wich is not documented in Unity 2017 script reference !
        /// UPDATE NOTES: Unty 2018 migration: Replaced deprecated PrefabUtility methods call with new equivalents ones
        /// </remarks>
        /// Author: Renaud Carpentiers

        struct PrefabLightmapIndex
        {
            public string id;
            public int lightmapStartIndex;
        }

        [System.Serializable]
        struct RendererInfo
        {
            public Renderer renderer;
            public int lightmapIndex;
            public Vector4 lightmapScaleOffset;
        }

        private static List<PrefabLightmapIndex> s_lightmapIndexes = new List<PrefabLightmapIndex>();
        private static bool isConnectedToSceneMngr = false;
        private static LightmapData[] s_originalLM;

        [SerializeField]
        private RendererInfo[] _rendererInfos;

        [SerializeField]
        private Texture2D[] _lightmaps;

        [SerializeField]
        private Light[] _bakedLights;


        public static void InitIndexes(Scene scene)
        {
            s_lightmapIndexes = new List<PrefabLightmapIndex>();
        }


        private void Awake()
        {
            // Is connected to scene manager
            if (!isConnectedToSceneMngr)
            {
                // Need to reload lightmaps and indexes when scene changed or reloaded
                isConnectedToSceneMngr = true;
                SceneManager.sceneUnloaded += InitIndexes;
            }

            int startMapsIndex;
            PrefabLightmapIndex pli;
            int index = s_lightmapIndexes.FindIndex(x => x.id == gameObject.name);
            if (index < 0)
            {
                Debug.Log("Guid (" + gameObject.name + ") not found -> new index");
                startMapsIndex = LightmapSettings.lightmaps.Length;

                LightmapData[] mergedMaps = new LightmapData[startMapsIndex + _lightmaps.Length];

                LightmapSettings.lightmaps.CopyTo(mergedMaps, 0);

                for (int i = 0; i < _lightmaps.Length; i++)
                {
                    mergedMaps[i + startMapsIndex] = new LightmapData();
                    mergedMaps[i + startMapsIndex].lightmapColor = _lightmaps[i];
                }

                LightmapSettings.lightmaps = mergedMaps;

                pli = new PrefabLightmapIndex();
                pli.lightmapStartIndex = startMapsIndex;
                pli.id = gameObject.name;
                s_lightmapIndexes.Add(pli);
            }
            else
            {
                Debug.Log("Prefab already exist at index " + index);
                pli = s_lightmapIndexes[index];
                startMapsIndex = pli.lightmapStartIndex;
            }

            for (int i = 0; i < _rendererInfos.Length; i++)
            {
                RendererInfo ri = _rendererInfos[i];
                ri.renderer.lightmapIndex = ri.lightmapIndex + startMapsIndex;
                ri.renderer.lightmapScaleOffset = ri.lightmapScaleOffset;
            }

            BakedLightSettingUp(this);
        }

        private static void BakedLightSettingUp(BakedPrefab instance)
        {
            foreach (Light light in instance._bakedLights)
            {
                LightBakingOutput lbo = light.bakingOutput;
                lbo.isBaked = true;
                light.bakingOutput = lbo;

            }

        }

        private void Reset()
        {
#if UNITY_EDITOR
            RefreshDatas();
#endif
        }


        private void RefreshDatas()
        {
            List<RendererInfo> ris = new List<RendererInfo>();
            List<Texture2D> lightmaps = new List<Texture2D>();
            List<Light> lights = new List<Light>();

            Texture2D[] lmArray = new Texture2D[LightmapSettings.lightmaps.Length];

            // Refresh lightmaps and renderer infos
            Renderer[] renderers = GetComponentsInChildren<Renderer>(false);
            foreach (Renderer r in renderers)
            {
                if (r.lightmapIndex < 0)
                    continue; // ignore renderer without lightmap


                lmArray[r.lightmapIndex] = LightmapSettings.lightmaps[r.lightmapIndex].lightmapColor;

                RendererInfo ri = new RendererInfo();
                ri.renderer = r;
                ri.lightmapIndex = r.lightmapIndex;
                ri.lightmapScaleOffset = r.lightmapScaleOffset;
                ris.Add(ri);
            }

            _rendererInfos = ris.ToArray();
            _lightmaps = new Texture2D[lmArray.Length];
            lmArray.CopyTo(_lightmaps, 0);

            // Refresh baked lights list
            foreach (Light l in GetComponentsInChildren<Light>(true))
            {
                if (l.bakingOutput.lightmapBakeType == LightmapBakeType.Baked)
                    lights.Add(l);
            }

            _bakedLights = lights.ToArray();
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Lightmaps/Update Baked Prefabs")]
        private static void RefreshAllPrefabs()
        {
            foreach (BakedPrefab bp in GameObject.FindObjectsOfType<BakedPrefab>())
            {
                // 1. Find the asset object (prefab) related to the actual gameobject
                GameObject asset = PrefabUtility.GetCorrespondingObjectFromOriginalSource(bp.gameObject);
                if (asset == null) return; // No prefab related to this object found

                // 2. Revert current object to the asset state (To avoid unwanted modification from an instance)
                PrefabUtility.RevertPrefabInstance(bp.gameObject, InteractionMode.AutomatedAction);

                // 3. Update object with current lightmaps data
                bp.Reset();

                // 4. Save / Override related asset with current updated object
                PrefabUtility.ApplyPrefabInstance(bp.gameObject, InteractionMode.AutomatedAction);
            }

        }
#endif
    }
}
