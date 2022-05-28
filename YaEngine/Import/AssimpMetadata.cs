using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Silk.NET.Assimp;

namespace YaEngine.Import
{
    public class AssimpMetadata
    {
        public readonly Dictionary<string, object> Data = new();
        
        public unsafe AssimpMetadata(Metadata* pMetadata)
        {
            if (pMetadata == null) return;
            
            for (var i = 0; i < pMetadata->MNumProperties; ++i)
            {
                var key = pMetadata->MKeys[i].AsString;
                var metadata = pMetadata->MValues[i];
                Data[key] = metadata.MType switch
                {
                    MetadataType.Bool => Unsafe.Read<bool>(metadata.MData),
                    MetadataType.Int32 => Unsafe.Read<int>(metadata.MData),
                    MetadataType.Uint64 => Unsafe.Read<ulong>(metadata.MData),
                    MetadataType.Float => Unsafe.Read<float>(metadata.MData),
                    MetadataType.Double => Unsafe.Read<double>(metadata.MData),
                    MetadataType.Aistring => Unsafe.Read<AssimpString>(metadata.MData).AsString,
                    MetadataType.Aivector3D => Unsafe.Read<Vector3>(metadata.MData),
                    MetadataType.Aimetadata => new AssimpMetadata((Metadata*) metadata.MData),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default;
            if (!Data.TryGetValue(key, out var obj)) return false;
            if (obj is not T tValue) return false;

            value = tValue;
            return true;
        }

        public Matrix4x4 GetValidRootTransformation(float importScaleFactor)
        {
            if (!TryGet<int>("CoordAxis", out var coordAxis)) coordAxis = 0;
            if (!TryGet<int>("UpAxis", out var upAxis)) upAxis = 1;
            if (!TryGet<int>("FrontAxis", out var frontAxis)) frontAxis = 2;
            
            if (!TryGet<int>("CoordAxisSign", out var coordAxisSign)) coordAxisSign = 1;
            if (!TryGet<int>("UpAxisSign", out var upAxisSign)) upAxisSign = 1;
            if (!TryGet<int>("FrontAxisSign", out var frontAxisSign)) frontAxisSign = 1;
            
            if (!TryGet<double>("UnitScaleFactor", out var scaleFactor)) scaleFactor = 1;

            var scale = (float) (scaleFactor * importScaleFactor);
            
            var coord = new float[3];
            coord[coordAxis] = coordAxisSign * scale;
            
            var up = new float[3];
            up[upAxis] = upAxisSign * scale;

            var front = new float[3];
            front[frontAxis] = frontAxisSign * scale;

            return new Matrix4x4(coord[0],  coord[1],   coord[2],   0,
                                 up[0],     up[1],      up[2],      0,
                                 front[0],  front[1],   front[2],   0,
                                 0,         0,          0,          1);
        }
    }
}