using System.Numerics;

namespace YaEngine.Core
{
    public static class TransformExtensions
    {
        public static Matrix4x4 GetWorldMatrix(this Transform transform)
        {
            var parentModel = transform.Parent?.GetWorldMatrix() ?? Matrix4x4.Identity;
            var rotation = Matrix4x4.CreateFromQuaternion(transform.Rotation);
            var scale = Matrix4x4.CreateScale(transform.Scale);
            var translation = Matrix4x4.CreateTranslation(transform.Position);
            return rotation * scale * translation * parentModel;
        }

        public static Matrix4x4 GetLocalMatrix(this Transform transform)
        {
            var worldMatrix = transform.GetWorldMatrix();
            if (Matrix4x4.Invert(worldMatrix, out var result)) return result;
            
            return Matrix4x4.Identity;
        }

        public static Vector3 GetWorldPosition(this Transform transform)
        {
            if (transform.Parent == null) return transform.Position;

            var worldMatrix = transform.GetWorldMatrix();
            return worldMatrix.Translation;
        }

        public static void SetWorldPosition(this Transform transform, Vector3 position)
        {
            if (transform.Parent == null)
            {
                transform.Position = position;
                return;
            }

            var localMatrix = transform.Parent.GetLocalMatrix();
            transform.Position = Vector3.Transform(position, localMatrix);
        }
        
        public static void SetWorldRotation(this Transform transform, Quaternion rotation)
        {
            if (transform.Parent == null)
            {
                transform.Rotation = rotation;
                return;
            }

            var localMatrix = transform.Parent.GetLocalMatrix();
            transform.Rotation = rotation * Quaternion.CreateFromRotationMatrix(localMatrix);
        }

        public static void SetWorldScale(this Transform transform, Vector3 scale)
        {
            if (transform.Parent == null)
            {
                transform.Scale = scale;
                return;
            }

            var localMatrix = transform.Parent.GetLocalMatrix();
            transform.Scale = scale * localMatrix.GetScale();
        }

        public static void SetWorldTransform(this Transform transform, Matrix4x4 matrix)
        {
            var position = matrix.Translation;
            var rotation = Quaternion.CreateFromRotationMatrix(matrix);
            var scale = matrix.GetScale();
            transform.SetWorldTransform(position, rotation, scale);
        }

        public static void SetWorldTransform(this Transform transform, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            if (transform.Parent == null)
            {
                transform.Position = position;
                transform.Rotation = rotation;
                transform.Scale = scale;
                return;
            }
            
            var localMatrix = transform.Parent.GetLocalMatrix();
            transform.Position = Vector3.Transform(position, localMatrix);
            transform.Rotation = rotation * Quaternion.CreateFromRotationMatrix(localMatrix);
            transform.Scale = scale * localMatrix.GetScale();
        }

        public static void SetPositionRotation(this Transform transform, Matrix4x4 matrix)
        {
            var position = matrix.Translation;
            var rotation = Quaternion.CreateFromRotationMatrix(matrix);
            transform.SetPositionRotation(position, rotation);
        }

        public static void SetPositionRotation(this Transform transform, Vector3 position, Quaternion rotation)
        {
            if (transform.Parent == null)
            {
                transform.Position = position;
                transform.Rotation = rotation;
                return;
            }
            
            var localMatrix = transform.Parent.GetLocalMatrix();
            transform.Position = Vector3.Transform(position, localMatrix);
            transform.Rotation = rotation * Quaternion.CreateFromRotationMatrix(localMatrix);
        }

        public static Vector3 GetWorldForward(this Transform transform)
        {
            return Vector3.Normalize(Vector3.Transform(Vector3.UnitZ, transform.Rotation));
        }

        public static Vector3 GetWorldRight(this Transform transform)
        {
            return Vector3.Normalize(Vector3.Transform(Vector3.UnitX, transform.GetWorldMatrix()));
        }
        
        public static void SetLocalTransform(this Transform transform, Matrix4x4 matrix)
        {
            var position = matrix.Translation;
            var rotation = Quaternion.CreateFromRotationMatrix(matrix);
            var scale = matrix.GetScale();
            transform.SetLocalTransform(position, rotation, scale);
        }

        public static void SetLocalTransform(this Transform transform, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            transform.Position = position;
            transform.Rotation = rotation;
            transform.Scale = scale;
        }
    }
}