using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace GenshinModelViewer.Core
{
    public partial class PMXLoaderScript
    {
        public static PMXFormat.Header GetHeader(string file_path)
        {
            PMXLoaderScript loader = new();
            return loader.GetHeader_(file_path);
        }

        public static PMXFormat.Header GetHeader(Stream stream)
        {
            PMXLoaderScript loader = new();
            return loader.GetHeader_(stream);
        }

        public static PMXFormat Import(string file_path)
        {
            PMXLoaderScript loader = new();
            return loader.Import_(file_path);
        }

        public static PMXFormat Import(Stream stream)
        {
            PMXLoaderScript loader = new();
            return loader.Import_(stream);
        }

        private PMXLoaderScript()
        {
        }

        private PMXFormat.Header GetHeader_(string file_path)
        {
            using FileStream stream = new(file_path, FileMode.Open, FileAccess.Read);
            return GetHeader_(stream);
        }

        private PMXFormat.Header GetHeader_(Stream stream)
        {
            PMXFormat.Header result;
            using BinaryReader bin = new(stream);
            bin.BaseStream.Seek(0, SeekOrigin.Begin);

            filePath = null;
            binaryReader = bin;
            result = ReadHeader();
            return result;
        }

        private PMXFormat Import_(string file_path)
        {
            using FileStream stream = new(file_path, FileMode.Open, FileAccess.Read);

            filePath = file_path;
            return Import_(stream);
        }

        private PMXFormat Import_(Stream stream)
        {
            using BinaryReader bin = new(stream);
            bin.BaseStream.Seek(0, SeekOrigin.Begin);

            binaryReader = bin;
            Read();
            return format;
        }

        private PMXFormat Read()
        {
            format = new();
            format.meta_header = CreateMetaHeader();
            format.header = ReadHeader();
            format.vertex_list = ReadVertexList();
            format.face_vertex_list = ReadFaceVertexList();
            format.texture_list = ReadTextureList();
            format.material_list = ReadMaterialList();
            format.bone_list = ReadBoneList();
            format.morph_list = ReadMorphList();
            format.display_frame_list = ReadDisplayFrameList();
            format.rigidbody_list = ReadRigidbodyList();
            format.rigidbody_joint_list = ReadRigidbodyJointList();
            return format;
        }

        private PMXFormat.MetaHeader CreateMetaHeader()
        {
            PMXFormat.MetaHeader result = new();
            result.path = filePath;
            result.name = Path.GetFileNameWithoutExtension(filePath); // .pmdを抜かす
            result.folder = Path.GetDirectoryName(filePath); // PMDが格納されているフォルダ
            return result;
        }

        private PMXFormat.Header ReadHeader()
        {
            PMXFormat.Header result = new();
            result.magic = binaryReader.ReadBytes(4);
            if (Encoding.ASCII.GetString(result.magic) != "PMX ")
            {
                throw new FormatException();
            }
            result.version = binaryReader.ReadSingle();
            binaryReader.ReadByte();
            result.encodeMethod = (PMXFormat.Header.StringCode)binaryReader.ReadByte();
            result.additionalUV = binaryReader.ReadByte();
            result.vertexIndexSize = (PMXFormat.Header.IndexSize)binaryReader.ReadByte();
            result.textureIndexSize = (PMXFormat.Header.IndexSize)binaryReader.ReadByte();
            result.materialIndexSize = (PMXFormat.Header.IndexSize)binaryReader.ReadByte();
            result.boneIndexSize = (PMXFormat.Header.IndexSize)binaryReader.ReadByte();
            result.morphIndexSize = (PMXFormat.Header.IndexSize)binaryReader.ReadByte();
            result.rigidbodyIndexSize = (PMXFormat.Header.IndexSize)binaryReader.ReadByte();

            stringCode = result.encodeMethod;
            result.model_name = ReadString();
            result.model_english_name = ReadString();
            result.comment = ReadString();
            result.english_comment = ReadString();

            return result;
        }

        private PMXFormat.VertexList ReadVertexList()
        {
            PMXFormat.VertexList result = new();
            uint vert_count = binaryReader.ReadUInt32();
            result.vertex = new PMXFormat.Vertex[vert_count];
            for (uint i = 0, i_max = (uint)result.vertex.Length; i < i_max; ++i)
            {
                result.vertex[i] = ReadVertex();
            }
            return result;
        }

        private PMXFormat.Vertex ReadVertex()
        {
            PMXFormat.Vertex result = new();
            result.pos = ReadSinglesToPoint3D(binaryReader);
            result.normal_vec = ReadSinglesToPoint3D(binaryReader);
            result.uv = ReadSinglesToPoint(binaryReader);
            result.add_uv = new Point4D[format.header.additionalUV];
            for (int i = 0; i < format.header.additionalUV; i++)
            {
                result.add_uv[i] = ReadSinglesToPoint4D(binaryReader);
            }
            PMXFormat.Vertex.WeightMethod weight_method = (PMXFormat.Vertex.WeightMethod)binaryReader.ReadByte();
            switch (weight_method)
            {
                case PMXFormat.Vertex.WeightMethod.BDEF1:
                    result.bone_weight = ReadBoneWeightBDEF1();
                    break;
                case PMXFormat.Vertex.WeightMethod.BDEF2:
                    result.bone_weight = ReadBoneWeightBDEF2();
                    break;
                case PMXFormat.Vertex.WeightMethod.BDEF4:
                    result.bone_weight = ReadBoneWeightBDEF4();
                    break;
                case PMXFormat.Vertex.WeightMethod.SDEF:
                    result.bone_weight = ReadBoneWeightSDEF();
                    break;
                case PMXFormat.Vertex.WeightMethod.QDEF:
                    result.bone_weight = ReadBoneWeightQDEF();
                    break;
                default:
                    result.bone_weight = null;
                    throw new FormatException();
            }
            result.edge_magnification = binaryReader.ReadSingle();
            return result;
        }

        private PMXFormat.BoneWeight ReadBoneWeightBDEF1()
        {
            PMXFormat.BDEF1 result = new();
            result.bone1_ref = CastIntRead(binaryReader, format.header.boneIndexSize);
            return result;
        }

        private PMXFormat.BoneWeight ReadBoneWeightBDEF2()
        {
            PMXFormat.BDEF2 result = new();
            result.bone1_ref = CastIntRead(binaryReader, format.header.boneIndexSize);
            result.bone2_ref = CastIntRead(binaryReader, format.header.boneIndexSize);
            result.bone1_weight = binaryReader.ReadSingle();
            return result;
        }

        private PMXFormat.BoneWeight ReadBoneWeightBDEF4()
        {
            PMXFormat.BDEF4 result = new();
            result.bone1_ref = CastIntRead(binaryReader, format.header.boneIndexSize);
            result.bone2_ref = CastIntRead(binaryReader, format.header.boneIndexSize);
            result.bone3_ref = CastIntRead(binaryReader, format.header.boneIndexSize);
            result.bone4_ref = CastIntRead(binaryReader, format.header.boneIndexSize);
            result.bone1_weight = binaryReader.ReadSingle();
            result.bone2_weight = binaryReader.ReadSingle();
            result.bone3_weight = binaryReader.ReadSingle();
            result.bone4_weight = binaryReader.ReadSingle();
            return result;
        }

        private PMXFormat.BoneWeight ReadBoneWeightSDEF()
        {
            PMXFormat.SDEF result = new();
            result.bone1_ref = CastIntRead(binaryReader, format.header.boneIndexSize);
            result.bone2_ref = CastIntRead(binaryReader, format.header.boneIndexSize);
            result.bone1_weight = binaryReader.ReadSingle();
            result.c_value = ReadSinglesToPoint3D(binaryReader);
            result.r0_value = ReadSinglesToPoint3D(binaryReader);
            result.r1_value = ReadSinglesToPoint3D(binaryReader);
            return result;
        }

        private PMXFormat.BoneWeight ReadBoneWeightQDEF()
        {
            PMXFormat.BDEF4 result = new();
            result.bone1_ref = CastIntRead(binaryReader, format.header.boneIndexSize);
            result.bone2_ref = CastIntRead(binaryReader, format.header.boneIndexSize);
            result.bone3_ref = CastIntRead(binaryReader, format.header.boneIndexSize);
            result.bone4_ref = CastIntRead(binaryReader, format.header.boneIndexSize);
            result.bone1_weight = binaryReader.ReadSingle();
            result.bone2_weight = binaryReader.ReadSingle();
            result.bone3_weight = binaryReader.ReadSingle();
            result.bone4_weight = binaryReader.ReadSingle();
            return result;
        }

        private PMXFormat.FaceVertexList ReadFaceVertexList()
        {
            PMXFormat.FaceVertexList result = new();
            uint face_vert_count = binaryReader.ReadUInt32();
            result.face_vert_index = new uint[face_vert_count];
            for (uint i = 0, i_max = (uint)result.face_vert_index.Length; i < i_max; ++i)
            {
                result.face_vert_index[i] = CastIntRead(binaryReader, format.header.vertexIndexSize);
            }
            return result;
        }

        private PMXFormat.TextureList ReadTextureList()
        {
            PMXFormat.TextureList result = new();
            uint texture_file_count = binaryReader.ReadUInt32();
            result.texture_file = new string[texture_file_count];
            for (uint i = 0, i_max = (uint)result.texture_file.Length; i < i_max; ++i)
            {
                result.texture_file[i] = ReadString();
                //"./"開始なら削除する
                if (('.' == result.texture_file[i][0]) && (1 == result.texture_file[i].IndexOfAny(new[] { '/', '\\' }, 1, 1)))
                {
                    result.texture_file[i] = result.texture_file[i].Substring(2);
                }
            }
            return result;
        }

        private PMXFormat.MaterialList ReadMaterialList()
        {
            PMXFormat.MaterialList result = new();
            uint material_count = binaryReader.ReadUInt32();
            result.material = new PMXFormat.Material[material_count];
            for (uint i = 0, i_max = (uint)result.material.Length; i < i_max; ++i)
            {
                result.material[i] = ReadMaterial();
            }
            return result;
        }

        private PMXFormat.Material ReadMaterial()
        {
            PMXFormat.Material result = new()
            {
                name = ReadString(),
                english_name = ReadString(),
                diffuse_color = ReadSinglesToColor(binaryReader), // dr, dg, db, da // 減衰色
                specular_color = ReadSinglesToColor(binaryReader, 1), // sr, sg, sb // 光沢色
                specularity = binaryReader.ReadSingle(),
                ambient_color = ReadSinglesToColor(binaryReader, 1), // mr, mg, mb // 環境色(ambient)
                flag = (PMXFormat.Material.Flag)binaryReader.ReadByte(),
                edge_color = ReadSinglesToColor(binaryReader), // r, g, b, a
                edge_size = binaryReader.ReadSingle(),
                usually_texture_index = CastIntRead(binaryReader, format.header.textureIndexSize),
                sphere_texture_index = CastIntRead(binaryReader, format.header.textureIndexSize),
                sphere_mode = (PMXFormat.Material.SphereMode)binaryReader.ReadByte(),
                common_toon = binaryReader.ReadByte()
            };
            PMXFormat.Header.IndexSize texture_index_size = ((result.common_toon == 0) ? format.header.textureIndexSize : PMXFormat.Header.IndexSize.Byte1);
            result.toon_texture_index = CastIntRead(binaryReader, texture_index_size);
            result.memo = ReadString();
            result.face_vert_count = binaryReader.ReadUInt32(); // 面頂点数 // インデックスに変換する場合は、材質0から順に加算
            return result;
        }

        private PMXFormat.BoneList ReadBoneList()
        {
            PMXFormat.BoneList result = new();
            uint bone_count = binaryReader.ReadUInt32();
            result.bone = new PMXFormat.Bone[bone_count];
            for (uint i = 0, i_max = (uint)result.bone.Length; i < i_max; ++i)
            {
                result.bone[i] = ReadBone();
            }
            return result;
        }

        private PMXFormat.Bone ReadBone()
        {
            PMXFormat.Bone result = new();
            result.bone_name = ReadString();
            result.bone_english_name = ReadString();
            result.bone_position = ReadSinglesToPoint3D(binaryReader);
            result.parent_bone_index = CastIntRead(binaryReader, format.header.boneIndexSize);
            result.transform_level = binaryReader.ReadInt32();
            result.bone_flag = (PMXFormat.Bone.Flag)binaryReader.ReadUInt16();

            if ((result.bone_flag & PMXFormat.Bone.Flag.Connection) == 0)
            {
                // 座標オフセットで指定
                result.position_offset = ReadSinglesToPoint3D(binaryReader);
            }
            else
            {
                // ボーンで指定
                result.connection_index = CastIntRead(binaryReader, format.header.boneIndexSize);
            }
            if ((result.bone_flag & (PMXFormat.Bone.Flag.AddRotation | PMXFormat.Bone.Flag.AddMove)) != 0)
            {
                result.additional_parent_index = CastIntRead(binaryReader, format.header.boneIndexSize);
                result.additional_rate = binaryReader.ReadSingle();
            }
            if ((result.bone_flag & PMXFormat.Bone.Flag.FixedAxis) != 0)
            {
                result.axis_vector = ReadSinglesToPoint3D(binaryReader);
            }
            if ((result.bone_flag & PMXFormat.Bone.Flag.LocalAxis) != 0)
            {
                result.x_axis_vector = ReadSinglesToPoint3D(binaryReader);
                result.z_axis_vector = ReadSinglesToPoint3D(binaryReader);
            }
            if ((result.bone_flag & PMXFormat.Bone.Flag.ExternalParentTransform) != 0)
            {
                result.key_value = binaryReader.ReadUInt32();
            }
            if ((result.bone_flag & PMXFormat.Bone.Flag.IkFlag) != 0)
            {
                result.ik_data = ReadIkData();
            }
            return result;
        }

        private PMXFormat.IK_Data ReadIkData()
        {
            PMXFormat.IK_Data result = new();
            result.ik_bone_index = CastIntRead(binaryReader, format.header.boneIndexSize);
            result.iterations = binaryReader.ReadUInt32();
            result.limit_angle = binaryReader.ReadSingle();
            uint ik_link_count = binaryReader.ReadUInt32();
            result.ik_link = new PMXFormat.IK_Link[ik_link_count];
            for (uint i = 0, i_max = (uint)result.ik_link.Length; i < i_max; ++i)
            {
                result.ik_link[i] = ReadIkLink();
            }
            return result;
        }

        private PMXFormat.IK_Link ReadIkLink()
        {
            PMXFormat.IK_Link result = new();
            result.target_bone_index = CastIntRead(binaryReader, format.header.boneIndexSize);
            result.angle_limit = binaryReader.ReadByte();
            if (result.angle_limit == 1)
            {
                result.lower_limit = ReadSinglesToPoint3D(binaryReader);
                result.upper_limit = ReadSinglesToPoint3D(binaryReader);
            }
            return result;
        }

        private PMXFormat.MorphList ReadMorphList()
        {
            PMXFormat.MorphList result = new();
            uint morph_count = binaryReader.ReadUInt32();
            result.morph_data = new PMXFormat.MorphData[morph_count];
            for (uint i = 0, i_max = (uint)result.morph_data.Length; i < i_max; ++i)
            {
                result.morph_data[i] = ReadMorphData();
            }
            return result;
        }

        private PMXFormat.MorphData ReadMorphData()
        {
            PMXFormat.MorphData result = new();
            result.morph_name = ReadString();
            result.morph_english_name = ReadString();
            result.handle_panel = (PMXFormat.MorphData.Panel)binaryReader.ReadByte();
            result.morph_type = (PMXFormat.MorphData.MorphType)binaryReader.ReadByte();
            uint morph_offset_count = binaryReader.ReadUInt32();
            result.morph_offset = new PMXFormat.MorphOffset[morph_offset_count];
            for (uint i = 0, i_max = (uint)result.morph_offset.Length; i < i_max; ++i)
            {
                switch (result.morph_type)
                {
                    case PMXFormat.MorphData.MorphType.Group:
                    case PMXFormat.MorphData.MorphType.Flip:
                        result.morph_offset[i] = ReadGroupMorphOffset();
                        break;
                    case PMXFormat.MorphData.MorphType.Vertex:
                        result.morph_offset[i] = ReadVertexMorphOffset();
                        break;
                    case PMXFormat.MorphData.MorphType.Bone:
                        result.morph_offset[i] = ReadBoneMorphOffset();
                        break;
                    case PMXFormat.MorphData.MorphType.Uv:
                    case PMXFormat.MorphData.MorphType.Adduv1:
                    case PMXFormat.MorphData.MorphType.Adduv2:
                    case PMXFormat.MorphData.MorphType.Adduv3:
                    case PMXFormat.MorphData.MorphType.Adduv4:
                        result.morph_offset[i] = ReadUVMorphOffset();
                        break;
                    case PMXFormat.MorphData.MorphType.Material:
                        result.morph_offset[i] = ReadMaterialMorphOffset();
                        break;
                    case PMXFormat.MorphData.MorphType.Impulse:
                        result.morph_offset[i] = ReadImpulseMorphOffset();
                        break;
                    default:
                        throw new System.FormatException();
                }
            }
            return result;
        }
        private PMXFormat.MorphOffset ReadGroupMorphOffset()
        {
            PMXFormat.GroupMorphOffset result = new();
            result.morph_index = CastIntRead(binaryReader, format.header.morphIndexSize);
            result.morph_rate = binaryReader.ReadSingle();
            return result;
        }
        private PMXFormat.MorphOffset ReadVertexMorphOffset()
        {
            PMXFormat.VertexMorphOffset result = new();
            result.vertex_index = CastIntRead(binaryReader, format.header.vertexIndexSize);
            result.position_offset = ReadSinglesToPoint3D(binaryReader);
            return result;
        }
        private PMXFormat.MorphOffset ReadBoneMorphOffset()
        {
            PMXFormat.BoneMorphOffset result = new();
            result.bone_index = CastIntRead(binaryReader, format.header.boneIndexSize);
            result.move_value = ReadSinglesToPoint3D(binaryReader);
            result.rotate_value = ReadSinglesToQuaternion(binaryReader);
            return result;
        }
        private PMXFormat.MorphOffset ReadUVMorphOffset()
        {
            PMXFormat.UVMorphOffset result = new();
            result.vertex_index = CastIntRead(binaryReader, format.header.vertexIndexSize);
            result.uv_offset = ReadSinglesToPoint4D(binaryReader);
            return result;
        }
        private PMXFormat.MorphOffset ReadMaterialMorphOffset()
        {
            PMXFormat.MaterialMorphOffset result = new();
            result.material_index = CastIntRead(binaryReader, format.header.materialIndexSize);
            result.offset_method = (PMXFormat.MaterialMorphOffset.OffsetMethod)binaryReader.ReadByte();
            result.diffuse = ReadSinglesToColor(binaryReader);
            result.specular = ReadSinglesToColor(binaryReader, 1);
            result.specularity = binaryReader.ReadSingle();
            result.ambient = ReadSinglesToColor(binaryReader, 1);
            result.edge_color = ReadSinglesToColor(binaryReader);
            result.edge_size = binaryReader.ReadSingle();
            result.texture_coefficient = ReadSinglesToColor(binaryReader);
            result.sphere_texture_coefficient = ReadSinglesToColor(binaryReader);
            result.toon_texture_coefficient = ReadSinglesToColor(binaryReader);
            return result;
        }
        private PMXFormat.MorphOffset ReadImpulseMorphOffset()
        {
            PMXFormat.ImpulseMorphOffset result = new();
            result.rigidbody_index = CastIntRead(binaryReader, format.header.morphIndexSize);
            result.local_flag = binaryReader.ReadByte();
            result.move_velocity = ReadSinglesToPoint3D(binaryReader);
            result.rotation_torque = ReadSinglesToPoint3D(binaryReader);
            return result;
        }

        private PMXFormat.DisplayFrameList ReadDisplayFrameList()
        {
            PMXFormat.DisplayFrameList result = new();
            uint display_frame_count = binaryReader.ReadUInt32();
            result.display_frame = new PMXFormat.DisplayFrame[display_frame_count];
            for (uint i = 0, i_max = (uint)result.display_frame.Length; i < i_max; ++i)
            {
                result.display_frame[i] = ReadDisplayFrame();
            }
            return result;
        }


        private PMXFormat.DisplayFrame ReadDisplayFrame()
        {
            PMXFormat.DisplayFrame result = new();
            result.display_name = ReadString();
            result.display_english_name = ReadString();
            result.special_frame_flag = binaryReader.ReadByte();
            uint display_element_count = binaryReader.ReadUInt32();
            result.display_element = new PMXFormat.DisplayElement[display_element_count];
            for (uint i = 0, i_max = (uint)result.display_element.Length; i < i_max; ++i)
            {
                result.display_element[i] = ReadDisplayElement();
            }
            return result;
        }

        private PMXFormat.DisplayElement ReadDisplayElement()
        {
            PMXFormat.DisplayElement result = new();
            result.element_target = binaryReader.ReadByte();
            PMXFormat.Header.IndexSize element_target_index_size = (result.element_target == 0) ? format.header.boneIndexSize : format.header.morphIndexSize;
            result.element_target_index = CastIntRead(binaryReader, element_target_index_size);
            return result;
        }

        private PMXFormat.RigidbodyList ReadRigidbodyList()
        {
            PMXFormat.RigidbodyList result = new();
            uint rigidbody_count = binaryReader.ReadUInt32();
            result.rigidbody = new PMXFormat.Rigidbody[rigidbody_count];
            for (uint i = 0, i_max = (uint)result.rigidbody.Length; i < i_max; ++i)
            {
                result.rigidbody[i] = ReadRigidbody();
            }
            return result;
        }

        private PMXFormat.Rigidbody ReadRigidbody()
        {
            PMXFormat.Rigidbody result = new();
            result.name = ReadString();
            result.english_name = ReadString();
            result.rel_bone_index = CastIntRead(binaryReader, format.header.boneIndexSize);
            result.group_index = binaryReader.ReadByte();
            result.ignore_collision_group = binaryReader.ReadUInt16();
            result.shape_type = (PMXFormat.Rigidbody.ShapeType)binaryReader.ReadByte();
            result.shape_size = ReadSinglesToPoint3D(binaryReader);
            result.collider_position = ReadSinglesToPoint3D(binaryReader);
            result.collider_rotation = ReadSinglesToPoint3D(binaryReader);
            result.weight = binaryReader.ReadSingle();
            result.position_dim = binaryReader.ReadSingle();
            result.rotation_dim = binaryReader.ReadSingle();
            result.recoil = binaryReader.ReadSingle();
            result.friction = binaryReader.ReadSingle();
            result.operation_type = (PMXFormat.Rigidbody.OperationType)binaryReader.ReadByte();
            return result;
        }

        private PMXFormat.RigidbodyJointList ReadRigidbodyJointList()
        {
            PMXFormat.RigidbodyJointList result = new();
            uint joint_count = binaryReader.ReadUInt32();
            result.joint = new PMXFormat.Joint[joint_count];
            for (uint i = 0, i_max = (uint)result.joint.Length; i < i_max; ++i)
            {
                result.joint[i] = ReadJoint();
            }
            return result;
        }

        private PMXFormat.Joint ReadJoint()
        {
            PMXFormat.Joint result = new();
            result.name = ReadString();
            result.english_name = ReadString();
            result.operation_type = (PMXFormat.Joint.OperationType)binaryReader.ReadByte();
            switch (result.operation_type)
            {
                case PMXFormat.Joint.OperationType.Spring6DOF:
                    result.rigidbody_a = CastIntRead(binaryReader, format.header.rigidbodyIndexSize);
                    result.rigidbody_b = CastIntRead(binaryReader, format.header.rigidbodyIndexSize);
                    result.position = ReadSinglesToPoint3D(binaryReader);
                    result.rotation = ReadSinglesToPoint3D(binaryReader);
                    result.constrain_pos_lower = ReadSinglesToPoint3D(binaryReader);
                    result.constrain_pos_upper = ReadSinglesToPoint3D(binaryReader);
                    result.constrain_rot_lower = ReadSinglesToPoint3D(binaryReader);
                    result.constrain_rot_upper = ReadSinglesToPoint3D(binaryReader);
                    result.spring_position = ReadSinglesToPoint3D(binaryReader);
                    result.spring_rotation = ReadSinglesToPoint3D(binaryReader);
                    break;
                default:
                    //empty.
                    break;
            }
            return result;
        }

        private string ReadString()
        {
            string result;
            int stringLength = binaryReader.ReadInt32();
            byte[] buf = binaryReader.ReadBytes(stringLength);
            switch (stringCode)
            {
                case PMXFormat.Header.StringCode.Utf16le:
                    result = Encoding.Unicode.GetString(buf);
                    break;
                case PMXFormat.Header.StringCode.Utf8:
                    result = Encoding.UTF8.GetString(buf);
                    break;
                default:
                    throw new InvalidOperationException();
            }
            return result;
        }

        private uint CastIntRead(BinaryReader _, PMXFormat.Header.IndexSize index_size)
        {
            uint result;
            switch (index_size)
            {
                case PMXFormat.Header.IndexSize.Byte1:
                    result = binaryReader.ReadByte();
                    if (byte.MaxValue == result)
                    {
                        result = uint.MaxValue;
                    }
                    break;
                case PMXFormat.Header.IndexSize.Byte2:
                    result = binaryReader.ReadUInt16();
                    if (ushort.MaxValue == result)
                    {
                        result = uint.MaxValue;
                    }
                    break;
                case PMXFormat.Header.IndexSize.Byte4:
                    result = binaryReader.ReadUInt32();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }

        private static Point4D ReadSinglesToPoint4D(BinaryReader binaryReader)
        {
            const int count = 4;
            float[] result = new float[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = binaryReader.ReadSingle();
                if (float.IsNaN(result[i])) result[i] = 0.0f; // 非数値なら回避
            }
            return new Point4D(result[0], result[1], result[2], result[3]);
        }

        private static Point3D ReadSinglesToPoint3D(BinaryReader binaryReader)
        {
            const int count = 3;
            float[] result = new float[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = binaryReader.ReadSingle();
                if (float.IsNaN(result[i])) result[i] = 0f; // 非数値なら回避
            }
            return new Point3D(result[0], result[1], result[2]);
        }

        private static Point ReadSinglesToPoint(BinaryReader binary_reader_)
        {
            const int count = 2;
            float[] result = new float[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = binary_reader_.ReadSingle();
                if (float.IsNaN(result[i])) result[i] = 0f; // 非数値なら回避
            }
            return new Point(result[0], result[1]);
        }

        private static Color ReadSinglesToColor(BinaryReader binary_reader_)
        {
            const int count = 4;
            float[] result = new float[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = binary_reader_.ReadSingle();
            }
            return RGBA2ARGB(result[0], result[1], result[2], result[3]);
        }

        private static Color ReadSinglesToColor(BinaryReader binary_reader_, float fix_alpha)
        {
            const int count = 3;
            float[] result = new float[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = binary_reader_.ReadSingle();
            }
            return RGBA2ARGB(result[0], result[1], result[2], fix_alpha);
        }

        private Quaternion ReadSinglesToQuaternion(BinaryReader binary_reader_)
        {
            const int count = 4;
            float[] result = new float[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = binary_reader_.ReadSingle();
                if (float.IsNaN(result[i])) result[i] = 0.0f; // 非数値なら回避
            }
            return new Quaternion(result[0], result[1], result[2], result[3]);
        }

        private static Color RGBA2ARGB(float r, float g, float b, float a)
        {
            var rr = Float2Byte(r);
            var gg = Float2Byte(g);
            var bb = Float2Byte(b);
            var aa = Float2Byte(a);

            return Color.FromArgb(aa, rr, gg, bb);
        }

        private static byte Float2Byte(float f)
        {
            return (byte)(f * 255);
        }

        string filePath;
        BinaryReader binaryReader;
        PMXFormat format;
        PMXFormat.Header.StringCode stringCode;
    }
}
