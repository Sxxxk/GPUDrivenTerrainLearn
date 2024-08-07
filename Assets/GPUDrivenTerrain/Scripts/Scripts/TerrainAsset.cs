using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPUDrivenTerrainLearn{

    //为Asset页面右键Create页面增加标签项
    //能够右键直接Create这个ScriptableObject
    //ScriptableObject是一个数据配置存储基类，一个大容量的数据容器
    [CreateAssetMenu(menuName = "GPUDrivenTerrainLearn/TerrainAsset")]
    public class TerrainAsset : ScriptableObject
    {
        public const uint MAX_NODE_ID = 34124; //5x5+10x10+20x20+40x40+80x80+160x160 - 1
        public const int MAX_LOD = 5;

        /// <summary>
        /// MAX LOD下，世界由5x5个区块组成
        /// </summary>
        public const int MAX_LOD_NODE_COUNT = 5;

        //配置地形相关的属性
        [SerializeField]
        private Vector3 _worldSize = new Vector3(10240,2048,10240);
        //反照率（基本颜色）
        [SerializeField]
        private Texture2D _albedoMap;
        //高程
        [SerializeField]
        private Texture2D _heightMap;
        //法线
        [SerializeField]
        private Texture2D _normalMap;

        [SerializeField]
        private Texture2D[] _minMaxHeightMaps;
        //四叉树map?
        [SerializeField]
        private Texture2D[] _quadTreeMaps;
        //地形计算着色器，主要负责视锥裁剪，四叉树构建（节点的评价与维护
        [SerializeField]
        private ComputeShader _terrainCompute;

        private RenderTexture _quadTreeMap;
        private RenderTexture _minMaxHeightMap;

        private Material _boundsDebugMaterial;


        public Vector3 worldSize{
            get{
                return _worldSize;
            }
        }
        
        public Texture2D albedoMap{
            get{
                return _albedoMap;
            }
        }

        public Texture2D heightMap{
            get{
                return _heightMap;
            }
        }

        public Texture2D normalMap{
            get{
                return _normalMap;
            }
        }

        public RenderTexture quadTreeMap{
            get{
                if(!_quadTreeMap){
                    _quadTreeMap = TextureUtility.CreateRenderTextureWithMipTextures(_quadTreeMaps,RenderTextureFormat.R16);
                }
                return _quadTreeMap;
            }
        }

        public RenderTexture minMaxHeightMap{
            get{
                if(!_minMaxHeightMap){
                    _minMaxHeightMap = TextureUtility.CreateRenderTextureWithMipTextures(_minMaxHeightMaps,RenderTextureFormat.RG32);
                }
                return _minMaxHeightMap;
            }
        }

        public Material boundsDebugMaterial{
            get{
                if(!_boundsDebugMaterial){
                    _boundsDebugMaterial = new Material(Shader.Find("GPUTerrainLearn/BoundsDebug"));
                }
                return _boundsDebugMaterial;
            }
        }

        public ComputeShader computeShader{
            get{
                return _terrainCompute;
            }
        }

        private static Mesh _patchMesh;

        public static Mesh patchMesh{
            get{
                if(!_patchMesh){
                    _patchMesh = MeshUtility.CreatePlaneMesh(16);
                }
                return _patchMesh;
            }
        }


        private static Mesh _unitCubeMesh;

        public static Mesh unitCubeMesh{
            get{
                if(!_unitCubeMesh){
                    _unitCubeMesh = MeshUtility.CreateCube(1);
                }
                return _unitCubeMesh;
            }
        }
    }
}
