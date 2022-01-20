using HelixToolkit.Wpf;
using MMDExtensions;
using MMDExtensions.PMX;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace QuickLook.Plugin.HelixViewer
{
    public partial class HelixViewerPanel : UserControl
    {
        private readonly PMXProvider loader = new();

        public string ModelPath
        {
            get => (string)GetValue(ModelPathProperty);
            set => SetValue(ModelPathProperty, value);
        }
        public static readonly DependencyProperty ModelPathProperty = DependencyProperty.Register("ModelPath", typeof(string), typeof(HelixViewerPanel), new PropertyMetadata(string.Empty, OnPathChanged));

        private PMXFormat format;

        public HelixViewerPanel()
        {
            InitializeComponent();
        }

        public HelixViewerPanel(string modelPath) : this()
        {
            ModelPath = modelPath;
        }

        private static void OnPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HelixViewerPanel self)
            {
                self.LoadModel(e.NewValue as string);
            }
        }

        public void LoadModel(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }

            loader.Load(path);

            format = loader.GetPMXFormat(path);
            MeshCreationInfo creation_info = CreateMeshCreationInfoSingle();

            var mats = format.material_list.material.Length;

            var models = new Model3DGroup();

            for (int i = 0, i_max = creation_info.value.Length; i < i_max; ++i)
            {
                try
                {
                    //format_.face_vertex_list.face_vert_indexを[start](含む)から[start+count](含まず)迄取り出し
                    int[] indices = creation_info.value[i].plane_indices.Select(x => (int)creation_info.reassign_dictionary[x]) //頂点リアサインインデックス変換
                                                                    .ToArray();
                    var mesh = new MeshGeometry3D()
                    {
                        Positions = new Point3DCollection(format.vertex_list.vertex.Select(x => x.pos)),
                        TextureCoordinates = new PointCollection(format.vertex_list.vertex.Select(x => x.uv)),
                    };

                    indices.ToList().ForEach(x => mesh.TriangleIndices.Add(x));

                    var textureIndex = format.material_list.material[i].usually_texture_index;

                    Material material;

                    if (textureIndex == uint.MaxValue)
                    {
                        material = new DiffuseMaterial(new SolidColorBrush(Color.FromRgb(160, 160, 160)));
                    }
                    else
                    {
                        // Texture
                        ImageSource bitmapImage = loader.GetTexture(format.meta_header.folder, format.texture_list.texture_file[textureIndex]);

                        var colors_brush = new ImageBrush
                        {
                            ImageSource = bitmapImage,
                        };
                        material = new DiffuseMaterial(colors_brush);
                    }

                    GeometryModel3D model = new(mesh, material);
                    model.BackMaterial = material;
                    models.Children.Add(model);
                }
                catch
                {
                }
            }
            PreviewModel.Content = models;
        }

        MeshCreationInfo CreateMeshCreationInfoSingle()
        {
            MeshCreationInfo result = new();
            // 全マテリアルを設定
            result.value = CreateMeshCreationInfoPacks();
            // 全頂点を設定
            result.all_vertices = Enumerable.Range(0, format.vertex_list.vertex.Length).Select(x => (uint)x).ToArray();
            // 頂点リアサインインデックス用辞書作成
            result.reassign_dictionary = new Dictionary<uint, uint>(result.all_vertices.Length);
            for (uint i = 0, i_max = (uint)result.all_vertices.Length; i < i_max; ++i)
            {
                result.reassign_dictionary[i] = i;
            }
            return result;
        }
        MeshCreationInfo.Pack[] CreateMeshCreationInfoPacks()
        {
            uint plane_start = 0;
            // マテリアル単位のMeshCreationInfo.Packを作成する
            return Enumerable.Range(0, format.material_list.material.Length).Select(x =>
            {
                MeshCreationInfo.Pack pack = new();
                pack.material_index = (uint)x;
                uint plane_count = format.material_list.material[x].face_vert_count;
                pack.plane_indices = format.face_vertex_list.face_vert_index.Skip((int)plane_start).Take((int)plane_count).ToArray();
                pack.vertices = pack.plane_indices.Distinct().ToArray();
                plane_start += plane_count;
                return pack;
            }).ToArray();
        }
    }
}

public class MeshCreationInfo
{
    public class Pack
    {
        public uint material_index; // マテリアル
        public uint[] plane_indices; // 面
        public uint[] vertices; // 頂点
    }

    public Pack[] value;
    public uint[] all_vertices; // 総頂点
    public Dictionary<uint, uint> reassign_dictionary; // 頂点リアサインインデックス用辞書
}
